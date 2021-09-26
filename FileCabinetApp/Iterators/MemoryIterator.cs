using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Supports iteration over a collection with records.
    /// </summary>
    public class MemoryIterator : IEnumerator<FileCabinetRecord>, IEnumerable<FileCabinetRecord>
    {
        private IList<FileCabinetRecord> records;
        private int currentPosition;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">Collection of records.</param>
        public MemoryIterator(IList<FileCabinetRecord> records)
        {
            this.records = records;
            this.currentPosition = -1;
        }

        /// <summary>
        /// Gets current record.
        /// </summary>
        /// <value>Current record.</value>
        public FileCabinetRecord Current
        {
            get
            {
                return this.records[this.currentPosition];
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
        /// <returns>Enumerator over the collection of records.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Moves pointer to next record.
        /// </summary>
        /// <returns>True if one more record exists, false if there is no more records.</returns>
        public bool MoveNext()
        {
            this.currentPosition++;
            return this.currentPosition < this.records.Count;
        }

        /// <summary>
        /// Sets pointer to a start position.
        /// </summary>
        public void Reset()
        {
            this.currentPosition = -1;
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        /// <returns>Enumerator over the collection of records.</returns>
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
    }
}
