using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    public class MemoryIterator : IRecordIterator
    {
        private IList<FileCabinetRecord> records;
        private int currentPosition;

        public MemoryIterator(IList<FileCabinetRecord> records)
        {
            this.records = records;
            this.currentPosition = -1;
        }

        public FileCabinetRecord GetNext()
        {
            if (this.HasMore())
            {
                currentPosition++;
                return records[currentPosition];
            }
            else
            {
                return null;
            }
        }

        public bool HasMore()
        {
            return currentPosition + 1 < records.Count;
        }
    }
}
