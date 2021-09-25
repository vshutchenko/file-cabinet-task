using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Iterators
{
    public interface IRecordIterator
    {
        public FileCabinetRecord GetNext();

        public bool HasMore();
    }
}
