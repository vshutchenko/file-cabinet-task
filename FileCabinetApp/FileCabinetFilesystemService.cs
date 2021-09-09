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
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The stream to file with records.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates new record.
        /// </summary>
        /// <param name="recordParameters">Parameters object for <see cref="FileCabinetRecord"/> class.</param>
        /// <returns>Id of new record.</returns>
        public int CreateRecord(RecordParameters recordParameters)
        {
            if (recordParameters is null)
            {
                throw new ArgumentNullException(nameof(recordParameters), $"{nameof(recordParameters)} is null.");
            }

            int id = (int)(this.fileStream.Length == 0 ? 1 : (this.fileStream.Length / RecordSize) + 1);
            this.fileStream.Seek(0, SeekOrigin.End);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);

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
            if (recordParameters is null)
            {
                throw new ArgumentNullException(nameof(recordParameters), $"{nameof(recordParameters)} is null.");
            }

            this.fileStream.Seek(2, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                int currentRecordId = reader.ReadInt32();
                if (currentRecordId == id)
                {
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

                    break;
                }
                else
                {
                    this.fileStream.Seek(RecordSize - sizeof(int), SeekOrigin.Current);
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
                    this.fileStream.Seek(2, SeekOrigin.Current);

                    FileCabinetRecord record = ReadRecord(reader);
                    records.Add(record);
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
                    this.fileStream.Seek(2, SeekOrigin.Current);

                    FileCabinetRecord record = ReadRecord(reader);
                    records.Add(record);
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
                    this.fileStream.Seek(2, SeekOrigin.Current);

                    FileCabinetRecord record = ReadRecord(reader);
                    records.Add(record);
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
        /// This method returns number of stored records.
        /// </summary>
        /// <returns>Number of stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);

            while (this.fileStream.Position != this.fileStream.Length)
            {
                this.fileStream.Seek(2, SeekOrigin.Current);

                int id = reader.ReadInt32();
                string firstName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
                string lastName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
                int year = reader.ReadInt32();
                int month = reader.ReadInt32();
                int day = reader.ReadInt32();
                char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
                short experience = reader.ReadInt16();
                decimal salary = reader.ReadDecimal();

                FileCabinetRecord record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = new DateTime(year, month, day), Gender = gender, Experience = experience, Salary = salary };
                records.Add(record);
            }

            reader.Close();

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// This method returns array of stored records.
        /// </summary>
        /// <returns>Array of stored records.</returns>
        public int GetStat()
        {
            long recordsCount = this.fileStream.Length / RecordSize;

            if (recordsCount > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"Number of records is bigger than int.MaxValue.");
            }

            return (int)recordsCount;
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
                this.fileStream.Seek(2, SeekOrigin.Current);

                FileCabinetRecord record = ReadRecord(reader);
                records.Add(record);
            }

            reader.Close();

            return new FileCabinetServiceSnapshot(records.ToArray());
        }

        private static FileCabinetRecord ReadRecord(BinaryReader reader)
        {
            int id = reader.ReadInt32();
            string firstName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            string lastName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            int year = reader.ReadInt32();
            int month = reader.ReadInt32();
            int day = reader.ReadInt32();
            char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
            short experience = reader.ReadInt16();
            decimal salary = reader.ReadDecimal();

            FileCabinetRecord record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = new DateTime(year, month, day), Gender = gender, Experience = experience, Salary = salary };

            return record;
        }
    }
}
