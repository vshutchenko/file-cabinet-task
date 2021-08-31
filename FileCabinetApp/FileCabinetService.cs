using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class represents a file-cabinet. It stores records and
    /// allows you create, edit and search them.
    /// </summary>
    public class FileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <param name="dateOfBirth">The date of birth of the person.</param>
        /// <param name="gender">The gender of the person.</param>
        /// <param name="experience">The person's experience in work.</param>
        /// <param name="salary">The person's salary.</param>
        /// <returns>Id of the created record.</returns>
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

            AddInDictionary(this.firstNameDictionary, firstName, new List<FileCabinetRecord>() { record });
            AddInDictionary(this.lastNameDictionary, lastName, new List<FileCabinetRecord>() { record });
            AddInDictionary(this.dateOfBirthDictionary, dateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord>() { record });

            return record.Id;
        }

        /// <summary>
        /// This method returns number of stored records.
        /// </summary>
        /// <returns>Number of stored records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// This method returns array of stored records.
        /// </summary>
        /// <returns>Array of stored records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="id">The edited record's id.</param>
        /// <param name="firstName">The edited first name of the person.</param>
        /// <param name="lastName">The edited last name of the person.</param>
        /// <param name="dateOfBirth">The edited date of birth of the person.</param>
        /// <param name="gender">The edited gender of the person.</param>
        /// <param name="experience">The edited person's experience in work.</param>
        /// <param name="salary">The edited person's salary.</param>
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
                    this.firstNameDictionary[this.list[i].FirstName.ToUpperInvariant()].Remove(this.list[i]);
                    this.lastNameDictionary[this.list[i].LastName.ToUpperInvariant()].Remove(this.list[i]);
                    this.dateOfBirthDictionary[this.list[i].DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)].Remove(this.list[i]);

                    this.list[i].FirstName = firstName;
                    this.list[i].LastName = lastName;
                    this.list[i].DateOfBirth = dateOfBirth;
                    this.list[i].Gender = gender;
                    this.list[i].Experience = experience;
                    this.list[i].Salary = salary;

                    AddInDictionary(this.firstNameDictionary, firstName, new List<FileCabinetRecord>() { this.list[i] });
                    AddInDictionary(this.lastNameDictionary, lastName, new List<FileCabinetRecord>() { this.list[i] });
                    AddInDictionary(this.dateOfBirthDictionary, dateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord>() { this.list[i] });
                }
            }
        }

        /// <summary>
        /// This method performs searching records by first name.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <returns>The array of records with matched first name.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"{nameof(firstName)} is null");
            }

            List<FileCabinetRecord> records;
            this.firstNameDictionary.TryGetValue(firstName.ToUpperInvariant(), out records);

            return records is null ? Array.Empty<FileCabinetRecord>() : records.ToArray();
        }

        /// <summary>
        /// This method performs searching records by last name.
        /// </summary>
        /// <param name="lastName">The person's last name.</param>
        /// <returns>The array of records with matched last name.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"{nameof(lastName)} is null");
            }

            List<FileCabinetRecord> records;
            this.lastNameDictionary.TryGetValue(lastName.ToUpperInvariant(), out records);

            return records is null ? Array.Empty<FileCabinetRecord>() : records.ToArray();
        }

        /// <summary>
        /// This method performs searching in records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The person's date of birth.</param>
        /// <returns>The array of records with matched date of birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> records;
            this.dateOfBirthDictionary.TryGetValue(dateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), out records);

            return records is null ? Array.Empty<FileCabinetRecord>() : records.ToArray();
        }

        private static void AddInDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string key, List<FileCabinetRecord> records)
        {
            List<FileCabinetRecord> existingValue;

            if (!dictionary.TryGetValue(key.ToUpperInvariant(), out existingValue))
            {
                existingValue = dictionary[key.ToUpperInvariant()] = new List<FileCabinetRecord>();
            }

            existingValue.AddRange(records);
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
