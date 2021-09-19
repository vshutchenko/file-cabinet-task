using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public interface IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records); 
    }
}
