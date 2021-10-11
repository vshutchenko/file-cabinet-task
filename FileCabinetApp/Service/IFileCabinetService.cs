using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// This interface provides methods to work with records.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// This method makes snaphot of object.
        /// </summary>
        /// <returns>The instnace of <see cref="FileCabinetServiceSnapshot"/> class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// This method returns collection of stored records.
        /// </summary>
        /// <returns>Collection of stored records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords();

        /// <summary>
        /// This method returns number of deleted and stored records.
        /// </summary>
        /// <returns>Number of deleted and stored records.</returns>
        public Tuple<int, int> GetStat();

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        /// <returns>Id of the created record.</returns>
        public int CreateRecord(RecordParameters recordParameters);

        /// <summary>
        /// This method creates a new record.
        /// </summary>
        /// <param name="id">The edited record's id.</param>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        public void EditRecord(int id, RecordParameters recordParameters);

        /// <summary>
        /// This method restores state of object from snapshot.
        /// </summary>
        /// <param name="serviceSnapshot">The snapshot of service.</param>
        public void Restore(FileCabinetServiceSnapshot serviceSnapshot);

        /// <summary>
        /// This method removes record with specified id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>True if record was removed, false if record doesn't exist.</returns>
        public bool Remove(int id);

        /// <summary>
        /// Deletes record which has property with specified value.
        /// </summary>
        /// <param name="property">Name of property.</param>
        /// <param name="value">Value of property.</param>
        /// <returns>Ids of deleted records.</returns>
        public IList<int> Delete(string property, string value);

        /// <summary>
        /// This method removes empty records with from file.
        /// </summary>
        /// <returns>Number of purged records.</returns>
        public Tuple<int, int> Purge();

        /// <summary>
        /// Add record to storage.
        /// </summary>
        /// <param name="record">Record to add.</param>
        /// <returns>Id of inserted record.</returns>
        public int Insert(FileCabinetRecord record);

        /// <summary>
        /// Updates record in storage.
        /// </summary>
        /// <param name="propertiesToSearchNames">Names of properties to search.</param>
        /// <param name="propertiesToUpdateNames">Names of properties to update.</param>
        /// <param name="valuesToSearch">Values to search.</param>
        /// <param name="newValues">New value of properties to update.</param>
        /// <param name="allFieldsMatch">True if record properties should match all values, false if one or more properties should match.</param>
        public void Update(IList<string> propertiesToSearchNames, IList<string> propertiesToUpdateNames, IList<string> valuesToSearch, IList<string> newValues, bool allFieldsMatch = true);

        /// <summary>
        /// Selects all records from storage.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public IEnumerable<FileCabinetRecord> SelectAll();

        /// <summary>
        /// Selects records matching search parameters.
        /// </summary>
        /// <param name="propertiesNames">Names of properties to search.</param>
        /// <param name="values">Values to search.</param>
        /// <param name="allFieldsMatch">True if record properties should match all values, false if one or more properties should match.</param>
        /// <returns>Collection of records.</returns>
        public IEnumerable<FileCabinetRecord> Select(IList<string> propertiesNames, IList<string> values, bool allFieldsMatch = true);
    }
}
