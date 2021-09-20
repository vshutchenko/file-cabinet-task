using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
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
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
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
            long deletedRecords = (this.fileStream.Length / RecordSize) - records.Count;
            long recordsCount = this.fileStream.Length / RecordSize;
            this.fileStream.SetLength(0);
            this.fileStream.Seek(0, SeekOrigin.Begin);

            for (int i = 0; i < records.Count; i++)
            {
                recordParameters = new RecordParameters(
                    records[i].FirstName,
                    records[i].LastName,
                    records[i].DateOfBirth,
                    records[i].Gender,
                    records[i].Experience,
                    records[i].Salary);
                WriteRecord(records[i].Id, recordParameters, writer);
            }

            writer.Close();

            return new Tuple<int, int>((int)deletedRecords, (int)recordsCount);
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

                if (record.Id <= this.GetStat().Item2)
                {
                    this.EditRecord(record.Id, recordParameters);
                }
                else
                {
                    BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);
                    writer.Seek(0, SeekOrigin.End);
                    WriteRecord(record.Id, recordParameters, writer);
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

            WriteRecord(id, recordParameters, writer);
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
                if (TryReadRecord(reader, out FileCabinetRecord record) && (record.Id == id))
                {
                    this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
                    WriteRecord(id, recordParameters, writer);
                    break;
                }
            }

            reader.Close();
            writer.Close();
        }

        /// <summary>
        /// This method performs searching in records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The person's date of birth.</param>
        /// <returns>The array of records with matched date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            int dateOfBirthOffset = 246;
            int recordStartOffset = -dateOfBirthOffset - (sizeof(int) * 3);

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(dateOfBirthOffset, SeekOrigin.Current);

                int year = reader.ReadInt32();
                int month = reader.ReadInt32();
                int day = reader.ReadInt32();
                DateTime currentRecordDateOfBirth = new DateTime(year, month, day);

                if (currentRecordDateOfBirth == dateOfBirth)
                {
                    this.fileStream.Seek(recordStartOffset, SeekOrigin.Current);

                    if (TryReadRecord(reader, out FileCabinetRecord record))
                    {
                        records.Add(record);
                    }
                }
                else
                {
                    this.fileStream.Seek(RecordSize + recordStartOffset, SeekOrigin.Current);
                }
            }

            reader.Close();

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// This method performs searching records by first name.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <returns>The array of records with matched first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            int firstNameOffset = 6;
            int recordStartOffset = -firstNameOffset - MaxStringLength;
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(firstNameOffset, SeekOrigin.Current);
                string currentRecordFirstName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');

                if (currentRecordFirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(recordStartOffset, SeekOrigin.Current);

                    if (TryReadRecord(reader, out FileCabinetRecord record))
                    {
                        records.Add(record);
                    }
                }
                else
                {
                    this.fileStream.Seek(RecordSize + recordStartOffset, SeekOrigin.Current);
                }
            }

            reader.Close();

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// This method performs searching records by last name.
        /// </summary>
        /// <param name="lastName">The person's last name.</param>
        /// <returns>The array of records with matched last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            int lastNameOffset = 126;
            int recordStartOffset = -lastNameOffset - MaxStringLength;
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(lastNameOffset, SeekOrigin.Current);
                string currentRecordLastName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
                if (currentRecordLastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(recordStartOffset, SeekOrigin.Current);

                    if (TryReadRecord(reader, out FileCabinetRecord record))
                    {
                        records.Add(record);
                    }
                }
                else
                {
                    this.fileStream.Seek(RecordSize + recordStartOffset, SeekOrigin.Current);
                }
            }

            reader.Close();

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// This method returns collection of stored records.
        /// </summary>
        /// <returns>Collection of stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                if (TryReadRecord(reader, out FileCabinetRecord record))
                {
                    records.Add(record);
                }
            }

            reader.Close();

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// This method returns number of deleted and stored records.
        /// </summary>
        /// <returns>Number of deleted and stored records.</returns>
        public Tuple<int, int> GetStat()
        {
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            int recordsCount = (int)(this.fileStream.Length / RecordSize);
            int deletedRecordsCount = 0;

            if (recordsCount > int.MaxValue)
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

            recordsCount -= deletedRecordsCount;

            return new Tuple<int, int>(deletedRecordsCount, recordsCount);
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
                if (TryReadRecord(reader, out FileCabinetRecord record))
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
                if (TryReadRecord(reader, out FileCabinetRecord record) && (record.Id == id))
                {
                    this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
                    writer.Write(DeletedBitFlag);
                    return true;
                }
            }

            reader.Close();
            writer.Close();

            return false;
        }

        private static bool TryReadRecord(BinaryReader reader, out FileCabinetRecord record)
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

        private static void WriteRecord(int id, RecordParameters recordParameters, BinaryWriter writer)
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
