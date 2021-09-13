using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            string[] recordString;
            FileCabinetRecord record;

            int id;
            string firstName;
            string lastName;
            DateTime dateOfBirth;
            char gender;
            short experience;
            decimal salary;

            while (!reader.EndOfStream)
            {
                recordString = reader.ReadLine().Split(',');
                id = int.Parse(recordString[0], CultureInfo.InvariantCulture);
                firstName = recordString[1];
                lastName = recordString[2];
                dateOfBirth = DateTime.Parse(recordString[3]);
                gender = char.Parse(recordString[4]);
                experience = short.Parse(recordString[5]);
                salary = decimal.Parse(recordString[6]);

                record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Experience = experience, Gender = gender, Salary = salary };
                records.Add(record);
            }

            return records;
        }
    }
}
