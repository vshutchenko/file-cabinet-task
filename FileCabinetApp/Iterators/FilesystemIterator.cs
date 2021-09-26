using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Supports iteration over a file with records.
    /// </summary>
    public class FilesystemIterator : IEnumerator<FileCabinetRecord>, IEnumerable<FileCabinetRecord>
    {
        private const int MaxStringLength = 120;
        private IList<int> offsets;
        private int currentPosition = -1;
        private FileStream fileStream;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="fileStream">A stream to file with records.</param>
        /// <param name="offsets">Pinters to start positions of records.</param>
        public FilesystemIterator(FileStream fileStream, IList<int> offsets)
        {
            this.fileStream = fileStream;
            this.offsets = offsets;
        }

        /// <summary>
        /// Gets current record.
        /// </summary>
        /// <value>Current record.</value>
        public FileCabinetRecord Current
        {
            get
            {
                this.fileStream.Seek(this.offsets[this.currentPosition], SeekOrigin.Begin);
                FileCabinetRecord record;
                BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
                record = this.ReadRecord(reader);
                return record;
            }
        }

        /// <summary>
        /// Gets current object.
        /// </summary>
        /// <value>Current object.</value>
        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        /// <summary>
        /// Moves pointer to next record.
        /// </summary>
        /// <returns>True if one more record exists, false if there is no more records.</returns>
        public bool MoveNext()
        {
            this.currentPosition++;
            return this.currentPosition < this.offsets.Count;
        }

        /// <summary>
        /// Sets pointer to a start position.
        /// </summary>
        public void Reset()
        {
            this.currentPosition = -1;
        }

        /// <summary>
        /// Free managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Enumerator over the file with records.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Enumerator over the file with records.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Free managed resources.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.Reset();
            }

            this.isDisposed = true;
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
