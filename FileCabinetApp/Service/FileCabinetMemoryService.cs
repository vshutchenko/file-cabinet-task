﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// This class represents a file-cabinet. It stores records and
    /// allows you create, edit and search them.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">The validator which will be used for parameters validation.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// This method removes empty records with from file.
        /// </summary>
        /// <returns>Number of purged records.</returns>
        public Tuple<int, int> Purge()
        {
            return new Tuple<int, int>(0, this.list.Count);
        }

        /// <summary>
        /// This method restores records from service snapshot.
        /// </summary>
        /// <param name="serviceSnapshot">The snapshot of <see cref="IFileCabinetService"/> class.</param>
        public void Restore(FileCabinetServiceSnapshot serviceSnapshot)
        {
            if (serviceSnapshot is null)
            {
                throw new ArgumentNullException(nameof(serviceSnapshot), $"{nameof(serviceSnapshot)} is null.");
            }

            foreach (var record in serviceSnapshot.Records)
            {
                if (record.Id <= this.list.Count)
                {
                    RecordParameters recordParameters = new RecordParameters(
                        record.FirstName,
                        record.LastName,
                        record.DateOfBirth,
                        record.Gender,
                        record.Experience,
                        record.Salary);
                    this.EditRecord(record.Id, recordParameters);
                }
                else
                {
                    this.list.Add(record);
                    AddInDictionary(this.firstNameDictionary, record.FirstName, new List<FileCabinetRecord>() { record });
                    AddInDictionary(this.lastNameDictionary, record.LastName, new List<FileCabinetRecord>() { record });
                    AddInDictionary(this.dateOfBirthDictionary, record.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord>() { record });
                }
            }
        }

        /// <summary>
        /// This method makes snapshot of object.
        /// </summary>
        /// <returns>The instnace of <see cref="FileCabinetServiceSnapshot"/> class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot(this.list.ToArray());
            return serviceSnapshot;
        }

        /// <summary>
        /// This method returns collection of stored records.
        /// </summary>
        /// <returns>Collection of stored records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return this.list;
        }

        /// <summary>
        /// This method returns number of deleted and stored records.
        /// </summary>
        /// <returns>Number of deleted and stored records.</returns>
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(0, this.list.Count);
        }

        /// <summary>
        /// This method performs searching records by first name.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <returns>The array of records with matched first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"{nameof(firstName)} is null");
            }

            List<FileCabinetRecord> records;
            bool isValueExists = this.firstNameDictionary.TryGetValue(firstName.ToUpperInvariant(), out records);

            if (isValueExists)
            {
                return new MemoryIterator(records);
            }
            else
            {
                return new MemoryIterator(new List<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// This method performs searching records by last name.
        /// </summary>
        /// <param name="lastName">The person's last name.</param>
        /// <returns>The array of records with matched last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"{nameof(lastName)} is null");
            }

            List<FileCabinetRecord> records;
            bool isValueExists = this.lastNameDictionary.TryGetValue(lastName.ToUpperInvariant(), out records);

            if (isValueExists)
            {
                return new MemoryIterator(records);
            }
            else
            {
                return new MemoryIterator(new List<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// This method performs searching in records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The person's date of birth.</param>
        /// <returns>The array of records with matched date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> records;
            bool isValueExists = this.dateOfBirthDictionary.TryGetValue(dateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), out records);

            if (isValueExists)
            {
                return new MemoryIterator(records);
            }
            else
            {
                return new MemoryIterator(new List<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        /// <returns>Id of the created record.</returns>
        public int CreateRecord(RecordParameters recordParameters)
        {
            this.validator.ValidateParameters(recordParameters);

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
        /// This method updates record.
        /// </summary>
        /// <param name="id">Id of edited record.</param>
        /// <param name="recordParameters">Parameters object for <see cref="FileCabinetRecord"/> class.</param>
        public void EditRecord(int id, RecordParameters recordParameters)
        {
            this.validator.ValidateParameters(recordParameters);

            if (id < 0)
            {
                throw new ArgumentException($"There is no record with {nameof(id)}={id}");
            }

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
        /// This method removes record with specified id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>True if record was removed, false if record doesn't exist.</returns>
        public bool Remove(int id)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == id)
                {
                    this.firstNameDictionary[this.list[i].FirstName.ToUpperInvariant()].Remove(this.list[i]);
                    this.lastNameDictionary[this.list[i].LastName.ToUpperInvariant()].Remove(this.list[i]);
                    this.dateOfBirthDictionary[this.list[i].DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)].Remove(this.list[i]);
                    this.list.RemoveAt(i);
                    return true;
                }
            }

            return false;
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

        public int Insert(FileCabinetRecord record)
        {
            RecordParameters recordParameters = new RecordParameters(record);
            this.validator.ValidateParameters(recordParameters);
            this.list.Add(record);

            AddInDictionary(this.firstNameDictionary, recordParameters.FirstName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            AddInDictionary(this.lastNameDictionary, recordParameters.LastName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            AddInDictionary(this.dateOfBirthDictionary, recordParameters.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });

            return record.Id;
        }

        public IList<int> Delete(string property, string value)
        {
            var recordsToDelete = this.FindByTemplate(new[] { property }, new[] { value });
            List<int> deletedRecordsIds = new List<int>();
            foreach (var record in recordsToDelete)
            {
                this.Remove(record.Id);
                deletedRecordsIds.Add(record.Id);
            }

            return deletedRecordsIds;
        }

        public IEnumerable<FileCabinetRecord> FindByTemplate(IList<string> propertiesNames, IList<string> values, bool allFieldsMatch = true)
        {
            Type recordType = typeof(FileCabinetRecord);
            List<PropertyInfo> recordProperties = recordType.GetProperties().ToList();
            List<PropertyInfo> propertiesToSearch = new List<PropertyInfo>();
            FileCabinetRecord template = new FileCabinetRecord();

            for (int i = 0; i < propertiesNames.Count; i++)
            {
                var prop = recordProperties.FirstOrDefault(p => p.Name.Equals(propertiesNames[i], StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    propertiesToSearch.Add(prop);
                    var conv = TypeDescriptor.GetConverter(prop.PropertyType);
                    prop.SetValue(template, conv.ConvertFromString(values[i]));
                }
            }

            var records = this.GetRecords();
            if (allFieldsMatch)
            {
                foreach (var r in records)
                {
                    for (int j = 0; j < propertiesToSearch.Count; j++)
                    {
                        if (propertiesToSearch[j].GetValue(r).ToString().Equals(propertiesToSearch[j].GetValue(template).ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            yield return r;
                        }
                    }
                }
            }
            else
            {
                bool isMatch = false;
                foreach (var r in records)
                {
                    for (int j = 0; j < propertiesToSearch.Count; j++)
                    {
                        if (propertiesToSearch[j].GetValue(r).ToString().Equals(propertiesToSearch[j].GetValue(template).ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            isMatch = true;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        yield return r;
                    }

                    isMatch = false;
                }
            }
        }

        public void Update(IList<string> propertiesToSearchNames, IList<string> propertiesToUpdateNames, IList<string> valuesToSearch, IList<string> newValues, bool allFieldsMatch = true)
        {
            var recordsInFile = this.GetRecords();
            List<FileCabinetRecord> recordsToUpdate = new List<FileCabinetRecord>();
            var records = this.FindByTemplate(propertiesToSearchNames, valuesToSearch);

            Type recordType = typeof(FileCabinetRecord);
            List<PropertyInfo> recordProperties = recordType.GetProperties().ToList();

            foreach (var r in records)
            {
                recordsToUpdate.Add(r);
            }

            for (int i = 0; i < recordsToUpdate.Count; i++)
            {
                FileCabinetRecord template = recordsToUpdate[i];

                for (int j = 0; j < propertiesToUpdateNames.Count; j++)
                {
                    var prop = recordProperties.FirstOrDefault(p => p.Name.Equals(propertiesToUpdateNames[j], StringComparison.InvariantCultureIgnoreCase));
                    if (prop != null)
                    {
                        var conv = TypeDescriptor.GetConverter(prop.PropertyType);
                        prop.SetValue(template, conv.ConvertFromString(newValues[j]));
                    }
                }

                this.EditRecord(template.Id, new RecordParameters(template));
            }
        }
    }
}
