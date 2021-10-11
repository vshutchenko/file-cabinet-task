using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// This class represents a file-cabinet. It stores records in the file and
    /// allows create, edit and search records.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int MaxStringLength = 120;
        private const int RecordSize = (sizeof(short) * 2) + (sizeof(int) * 4) + (MaxStringLength * 2) + sizeof(char) + sizeof(decimal);
        private const short DeletedBitFlag = 0b_0000_0100;

        private FileStream fileStream;
        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The stream to file with records.</param>
        /// <param name="validator">The validator which will be used for parameters validation.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        private int RecordsCount
        {
            get
            {
                return (int)(this.fileStream.Length / RecordSize);
            }
        }

        /// <summary>
        /// This method removes empty records with from file.
        /// </summary>
        /// <returns>Number of purged records.</returns>
        public Tuple<int, int> Purge()
        {
            RecordParameters recordParameters;
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            var records = this.GetRecords();
            List<FileCabinetRecord> recordsList = new List<FileCabinetRecord>();
            foreach (var r in records)
            {
                recordsList.Add(r);
            }

            int recordsBeforePurge = this.RecordsCount;
            this.fileStream.SetLength(0);
            this.fileStream.Seek(0, SeekOrigin.Begin);

            foreach (var record in recordsList)
            {
                recordParameters = new RecordParameters(
                    record.FirstName,
                    record.LastName,
                    record.DateOfBirth,
                    record.Gender,
                    record.Experience,
                    record.Salary);
                this.WriteRecord(record.Id, recordParameters, writer);
            }

            writer.Close();

            return new Tuple<int, int>(recordsBeforePurge - this.RecordsCount, recordsBeforePurge);
        }

        /// <summary>
        /// This method restores state of object from snapshot.
        /// </summary>
        /// <param name="serviceSnapshot">The snapshot of service.</param>
        public void Restore(FileCabinetServiceSnapshot serviceSnapshot)
        {
            if (serviceSnapshot is null)
            {
                throw new ArgumentNullException(nameof(serviceSnapshot), $"{nameof(serviceSnapshot)} is null.");
            }

            foreach (var record in serviceSnapshot.Records)
            {
                RecordParameters recordParameters = new RecordParameters(
                        record.FirstName,
                        record.LastName,
                        record.DateOfBirth,
                        record.Gender,
                        record.Experience,
                        record.Salary);

                if (record.Id <= this.RecordsCount)
                {
                    this.EditRecord(record.Id, recordParameters);
                }
                else
                {
                    BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
                    writer.Seek(0, SeekOrigin.End);
                    this.WriteRecord(record.Id, recordParameters, writer);
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="recordParameters">Parameters object for <see cref="FileCabinetRecord"/> class.</param>
        /// <returns>Id of new record.</returns>
        public int CreateRecord(RecordParameters recordParameters)
        {
            this.validator.ValidateParameters(recordParameters);

            int id = (int)(this.fileStream.Length == 0 ? 1 : (this.fileStream.Length / RecordSize) + 1);
            this.fileStream.Seek(0, SeekOrigin.End);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            this.WriteRecord(id, recordParameters, writer);
            writer.Close();

            return id;
        }

        /// <summary>
        /// This method updates record.
        /// </summary>
        /// <param name="id">Id of edited record.</param>
        /// <param name="recordParameters">Parameters object for <see cref="FileCabinetRecord"/> class.</param>
        public void EditRecord(int id, RecordParameters recordParameters)
        {
            this.validator.ValidateParameters(recordParameters);

            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (this.TryReadRecord(reader, out FileCabinetRecord record) && (record.Id == id))
                {
                    this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
                    this.WriteRecord(id, recordParameters, writer);
                    break;
                }
            }

            reader.Close();
            writer.Close();
        }

        /// <summary>
        /// This method returns collection of stored records.
        /// </summary>
        /// <returns>Collection of stored records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (this.TryReadRecord(reader, out FileCabinetRecord record))
                {
                    yield return record;
                }
            }

            reader.Close();
        }

        /// <summary>
        /// This method returns number of deleted and stored records.
        /// </summary>
        /// <returns>Number of deleted and stored records.</returns>
        public Tuple<int, int> GetStat()
        {
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            int deletedRecordsCount = 0;

            if (this.RecordsCount > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"Number of records is bigger than int.MaxValue.");
            }

            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                short reservedBytes = reader.ReadInt16();
                if ((reservedBytes & DeletedBitFlag) == DeletedBitFlag)
                {
                    deletedRecordsCount++;
                }

                reader.BaseStream.Seek(RecordSize - sizeof(short), SeekOrigin.Current);
            }

            return new Tuple<int, int>(deletedRecordsCount, this.RecordsCount - deletedRecordsCount);
        }

        /// <summary>
        /// This method makes snapshot of object.
        /// </summary>
        /// <returns>The instnace of <see cref="FileCabinetServiceSnapshot"/> class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (this.TryReadRecord(reader, out FileCabinetRecord record))
                {
                    records.Add(record);
                }
            }

            reader.Close();

            return new FileCabinetServiceSnapshot(records.ToArray());
        }

        /// <summary>
        /// This method removes record with specified id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>True if record was removed, false if record doesn't exist.</returns>
        public bool Remove(int id)
        {
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);

            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (this.TryReadRecord(reader, out FileCabinetRecord record) && (record.Id == id))
                {
                    this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
                    writer.Write(DeletedBitFlag);
                    this.fileStream.Seek(-sizeof(short), SeekOrigin.Current);
                    return true;
                }
            }

            reader.Close();
            writer.Close();

            return false;
        }

        /// <inheritdoc/>
        public int Insert(FileCabinetRecord record)
        {
            RecordParameters recordParameters = new RecordParameters(record);
            this.validator.ValidateParameters(recordParameters);

            this.fileStream.Seek(0, SeekOrigin.End);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
            this.WriteRecord(record.Id, recordParameters, writer);
            writer.Close();

            return record.Id;
        }

        /// <inheritdoc/>
        public IList<int> Delete(string property, string value)
        {
            var recordsToDelete = this.FindByTemplate(new[] { property }, new[] { value });
            List<int> deletedRecordsIds = new List<int>();
            foreach (var record in recordsToDelete)
            {
                this.Remove(record.Id);
                deletedRecordsIds.Add(record.Id);
            }

            return deletedRecordsIds;
        }

        /// <inheritdoc/>
        public void Update(IList<string> propertiesToSearchNames, IList<string> propertiesToUpdateNames, IList<string> valuesToSearch, IList<string> newValues, bool allFieldsMatch = true)
        {
            List<FileCabinetRecord> recordsToUpdate = new List<FileCabinetRecord>();
            var records = this.FindByTemplate(propertiesToSearchNames, valuesToSearch, allFieldsMatch);

            Type recordType = typeof(FileCabinetRecord);
            List<PropertyInfo> recordProperties = recordType.GetProperties().ToList();

            foreach (var r in records)
            {
                recordsToUpdate.Add(r);
            }

            for (int i = 0; i < recordsToUpdate.Count; i++)
            {
                FileCabinetRecord template = recordsToUpdate[i];

                for (int j = 0; j < propertiesToUpdateNames.Count; j++)
                {
                    var prop = recordProperties.FirstOrDefault(p => p.Name.Equals(propertiesToUpdateNames[j], StringComparison.InvariantCultureIgnoreCase));
                    if (prop != null)
                    {
                        var conv = TypeDescriptor.GetConverter(prop.PropertyType);
                        prop.SetValue(template, conv.ConvertFromString(newValues[j]));
                    }
                }

                this.EditRecord(template.Id, new RecordParameters(template));
            }
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> Select(IList<string> propertiesNames, IList<string> values, bool allFieldsMatch = true)
        {
            var records = this.FindByTemplate(propertiesNames, values, allFieldsMatch);
            return records;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> SelectAll()
        {
            return this.GetRecords();
        }

        /// <summary>
        /// Search records.
        /// </summary>
        /// <param name="propertiesNames">Names of properties to search.</param>
        /// <param name="values">Values of properties to search.</param>
        /// <param name="allFieldsMatch">True if record properties should match all values, false if one or more properties should match.</param>
        /// <returns>Collection of records.</returns>
        private IEnumerable<FileCabinetRecord> FindByTemplate(IList<string> propertiesNames, IList<string> values, bool allFieldsMatch = true)
        {
            Type recordType = typeof(FileCabinetRecord);
            List<PropertyInfo> recordProperties = recordType.GetProperties().ToList();
            List<PropertyInfo> propertiesToSearch = new List<PropertyInfo>();
            FileCabinetRecord template = new FileCabinetRecord();

            for (int i = 0; i < propertiesNames.Count; i++)
            {
                var prop = recordProperties.FirstOrDefault(p => p.Name.Equals(propertiesNames[i], StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    propertiesToSearch.Add(prop);
                    var conv = TypeDescriptor.GetConverter(prop.PropertyType);
                    prop.SetValue(template, conv.ConvertFromString(values[i]));
                }
            }

            var records = this.GetRecords();
            if (allFieldsMatch)
            {
                bool isMatch = true;
                foreach (var r in records)
                {
                    for (int j = 0; j < propertiesToSearch.Count; j++)
                    {
                        if (!propertiesToSearch[j].GetValue(r).ToString().Equals(propertiesToSearch[j].GetValue(template).ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        yield return r;
                    }

                    isMatch = true;
                }
            }
            else
            {
                foreach (var r in records)
                {
                    for (int j = 0; j < propertiesToSearch.Count; j++)
                    {
                        if (propertiesToSearch[j].GetValue(r).ToString().Equals(propertiesToSearch[j].GetValue(template).ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            yield return r;
                            break;
                        }
                    }
                }
            }
        }

        private bool TryReadRecord(BinaryReader reader, out FileCabinetRecord record)
        {
            short reservedBytes = reader.ReadInt16();

            if ((reservedBytes & DeletedBitFlag) == DeletedBitFlag)
            {
                reader.BaseStream.Seek(RecordSize - sizeof(short), SeekOrigin.Current);
                record = null;
                return false;
            }

            int id = reader.ReadInt32();
            string firstName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            string lastName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            int year = reader.ReadInt32();
            int month = reader.ReadInt32();
            int day = reader.ReadInt32();
            char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
            short experience = reader.ReadInt16();
            decimal salary = reader.ReadDecimal();

            record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = new DateTime(year, month, day), Gender = gender, Experience = experience, Salary = salary };

            return true;
        }

        private void WriteRecord(int id, RecordParameters recordParameters, BinaryWriter writer)
        {
            writer.Write(new byte[2]);
            writer.Write(id);

            char[] firstNameChars = new char[MaxStringLength];
            char[] lastNameChars = new char[MaxStringLength];

            Array.Copy(recordParameters.FirstName.ToCharArray(), firstNameChars, recordParameters.FirstName.Length);
            Array.Copy(recordParameters.LastName.ToCharArray(), lastNameChars, recordParameters.LastName.Length);

            writer.Write(Encoding.Unicode.GetBytes(firstNameChars), 0, MaxStringLength);
            writer.Write(Encoding.Unicode.GetBytes(lastNameChars), 0, MaxStringLength);

            writer.Write(recordParameters.DateOfBirth.Year);
            writer.Write(recordParameters.DateOfBirth.Month);
            writer.Write(recordParameters.DateOfBirth.Day);
            writer.Write(Encoding.Unicode.GetBytes(new char[] { recordParameters.Gender }));
            writer.Write(recordParameters.Experience);
            writer.Write(recordParameters.Salary);
        }
    }
}
