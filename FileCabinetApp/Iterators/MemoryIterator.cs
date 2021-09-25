using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    public class MemoryIterator : IEnumerator<FileCabinetRecord>, IEnumerable<FileCabinetRecord>
    {
        private IList<FileCabinetRecord> records;
        private int currentPosition;

        public MemoryIterator(IList<FileCabinetRecord> records)
        {
            this.records = records;
            this.currentPosition = -1;
        }

        public FileCabinetRecord Current
        {
            get
            {
                return records[currentPosition];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            this.Reset();
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            currentPosition++;
            return currentPosition < records.Count;
        }

        public void Reset()
        {
            currentPosition = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
