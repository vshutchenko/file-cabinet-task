using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    public class FilesystemIterator : IEnumerator<FileCabinetRecord>, IEnumerable<FileCabinetRecord>
    {
        private IList<int> offsets;
        private int currentPosition = -1;
        private const int MaxStringLength = 120;
        private FileStream fileStream;

        public FileCabinetRecord Current
        {
            get
            {
                this.fileStream.Seek(offsets[currentPosition], SeekOrigin.Begin);
                FileCabinetRecord record;
                BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
                record = ReadRecord(reader);
                return record;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public FilesystemIterator(FileStream fileStream, IList<int> offsets)
        {
            this.fileStream = fileStream;
            this.offsets = offsets;
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

        public bool MoveNext()
        {
            currentPosition++;
            return currentPosition < offsets.Count;
        }

        public void Reset()
        {
            currentPosition = -1;
        }

        public void Dispose()
        {
            this.Reset();
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
