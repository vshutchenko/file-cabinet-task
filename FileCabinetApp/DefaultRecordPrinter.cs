using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class DefaultRecordPrinter : IRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            string recordString;
            foreach (var record in records)
            {
                recordString = $"#{record.Id}, " +
                    $"{record.FirstName}, " +
                    $"{record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                    $"{record.Gender}, " +
                    $"{record.Experience}, " +
                    $"{record.Salary}$";
                Console.WriteLine(recordString);
            }
        }
    }
}
