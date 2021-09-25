using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    public class FilesystemIterator : IRecordIterator
    {
        private IList<int> offsets;
        private int currentPosition = -1;
        private const int MaxStringLength = 120;
        private FileStream fileStream;

        public FilesystemIterator(FileStream fileStream, IList<int> offsets)
        {
            this.fileStream = fileStream;
            this.offsets = offsets;
        }

        private enum Offset
        {
            Reserved = 0,
            Id = 2,
            FirstName = 6,
            LastName = 126,
            Year = 246,
            Month = 250,
            Day = 254,
            Gender = 258,
            Experience = 260,
            Salary = 262,
        }

        public FileCabinetRecord GetNext()
        {
            currentPosition++;
            this.fileStream.Seek(offsets[currentPosition], SeekOrigin.Begin);
            FileCabinetRecord record;
            BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            record = ReadRecord(reader);
            return record;
        }

        public bool HasMore()
        {
            return currentPosition + 1 < offsets.Count;
        }

        private FileCabinetRecord ReadRecord(BinaryReader reader)
        {
            short reservedBytes = reader.ReadInt16();
            int id = reader.ReadInt32();
            string firstName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            string lastName = Encoding.Unicode.GetString(reader.ReadBytes(MaxStringLength)).Trim('\0');
            int year = reader.ReadInt32();
            int month = reader.ReadInt32();
            int day = reader.ReadInt32();
            char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
            short experience = reader.ReadInt16();
            decimal salary = reader.ReadDecimal();

            var record = new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = new DateTime(year, month, day),
                Gender = gender,
                Experience = experience,
                Salary = salary,
            };

            return record;
        }
    }
}
