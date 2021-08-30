using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, short experience, decimal salary)
        {
            ValidateRecordParams(firstName, lastName, dateOfBirth, gender, experience, salary);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Experience = experience,
                Salary = salary,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, short experience, decimal salary)
        {
            if ((id < 0) || (id > this.list.Count))
            {
                throw new ArgumentException($"There is no record with {nameof(id)}={id}");
            }

            ValidateRecordParams(firstName, lastName, dateOfBirth, gender, experience, salary);

            for (int i = 0; i < this.list.Count; i++)
            {
                if (id == this.list[i].Id)
                {
                    this.list[i].FirstName = firstName;
                    this.list[i].LastName = lastName;
                    this.list[i].DateOfBirth = dateOfBirth;
                    this.list[i].Gender = gender;
                    this.list[i].Experience = experience;
                    this.list[i].Salary = salary;
                }
            }
        }

        private static void ValidateRecordParams(string firstName, string lastName, DateTime dateOfBirth, char gender, short experience, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), $"{nameof(firstName)} is null or whitespace");
            }

            if ((firstName.Length < 2) || (firstName.Length > 60))
            {
                throw new ArgumentException($"{nameof(firstName)} length is not between 2 and 60");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), $"{nameof(lastName)} is null or whitespace");
            }

            if ((lastName.Length < 2) || (lastName.Length > 60))
            {
                throw new ArgumentException($"{nameof(lastName)} length is not between 2 and 60");
            }

            if ((dateOfBirth < new DateTime(1950, 1, 1)) || (dateOfBirth > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth), $"{nameof(dateOfBirth)} is not in range between 01-Jan-1950 and current date");
            }

            if ((gender != 'M') && (gender != 'F'))
            {
                throw new ArgumentException($"{nameof(gender)} is not equals to F or M");
            }

            if (experience < 0)
            {
                throw new ArgumentException($"{nameof(experience)} is less than zero");
            }

            if (salary < 0)
            {
                throw new ArgumentException($"{nameof(salary)} is less than zero");
            }
        }
    }
}
