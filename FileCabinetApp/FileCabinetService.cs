﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class represents a file-cabinet. It stores records and
    /// allows you create, edit and search them.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        /// <returns>Id of the created record.</returns>
        internal int CreateRecord(RecordParameters recordParameters)
        {
            this.CreateValidator().ValidateParameters(recordParameters);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = recordParameters.FirstName,
                LastName = recordParameters.LastName,
                DateOfBirth = recordParameters.DateOfBirth,
                Gender = recordParameters.Gender,
                Experience = recordParameters.Experience,
                Salary = recordParameters.Salary,
            };

            this.list.Add(record);

            AddInDictionary(this.firstNameDictionary, recordParameters.FirstName, new List<FileCabinetRecord>() { record });
            AddInDictionary(this.lastNameDictionary, recordParameters.LastName, new List<FileCabinetRecord>() { record });
            AddInDictionary(this.dateOfBirthDictionary, recordParameters.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord>() { record });

            return record.Id;
        }

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="id">The edited record's id.</param>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        internal void EditRecord(int id, RecordParameters recordParameters)
        {
            if ((id < 0) || (id > this.list.Count))
            {
                throw new ArgumentException($"There is no record with {nameof(id)}={id}");
            }

            this.CreateValidator().ValidateParameters(recordParameters);

            for (int i = 0; i < this.list.Count; i++)
            {
                if (id == this.list[i].Id)
                {
                    this.firstNameDictionary[this.list[i].FirstName.ToUpperInvariant()].Remove(this.list[i]);
                    this.lastNameDictionary[this.list[i].LastName.ToUpperInvariant()].Remove(this.list[i]);
                    this.dateOfBirthDictionary[this.list[i].DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)].Remove(this.list[i]);

                    this.list[i].FirstName = recordParameters.FirstName;
                    this.list[i].LastName = recordParameters.LastName;
                    this.list[i].DateOfBirth = recordParameters.DateOfBirth;
                    this.list[i].Gender = recordParameters.Gender;
                    this.list[i].Experience = recordParameters.Experience;
                    this.list[i].Salary = recordParameters.Salary;

                    AddInDictionary(this.firstNameDictionary, recordParameters.FirstName, new List<FileCabinetRecord>() { this.list[i] });
                    AddInDictionary(this.lastNameDictionary, recordParameters.LastName, new List<FileCabinetRecord>() { this.list[i] });
                    AddInDictionary(this.dateOfBirthDictionary, recordParameters.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord>() { this.list[i] });
                }
            }
        }

        /// <summary>
        /// The overrides of this method should implements parameters validation.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        protected abstract void ValidateRecordParams(RecordParameters recordParameters);

        protected abstract IRecordValidator CreateValidator();

        private static void AddInDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, string key, List<FileCabinetRecord> records)
        {
            List<FileCabinetRecord> existingValue;

            if (!dictionary.TryGetValue(key.ToUpperInvariant(), out existingValue))
            {
                existingValue = dictionary[key.ToUpperInvariant()] = new List<FileCabinetRecord>();
            }

            existingValue.AddRange(records);
        }
    }
}
