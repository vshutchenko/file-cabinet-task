using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class writes records into CSV format.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The instance of the <see cref="TextWriter"/> which will be used for writing records.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
            this.writer.WriteLine("Id,First Name,Last Name,Date of Birth,Gender,Experience,Salary");
        }

        /// <summary>
        /// Writes one record using an instance of <see cref="TextWriter"/> class.
        /// </summary>
        /// <param name="record">The instance of the <see cref="FileCabinetRecord"/> class.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record), $"{nameof(record)} is null.");
            }

            this.writer.WriteLine($"{record.Id}," +
                $"{record.FirstName}," +
                $"{record.LastName}," +
                $"{record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}," +
                $"{record.Gender}," +
                $"{record.Experience}," +
                $"{record.Salary}");
        }
    }
}
