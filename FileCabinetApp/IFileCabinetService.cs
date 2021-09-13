using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp
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
        /// This method returns number of stored records.
        /// </summary>
        /// <returns>Number of stored records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// This method returns array of stored records.
        /// </summary>
        /// <returns>Array of stored records.</returns>
        public int GetStat();

        /// <summary>
        /// This method performs searching records by first name.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <returns>The array of records with matched first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// This method performs searching records by last name.
        /// </summary>
        /// <param name="lastName">The person's last name.</param>
        /// <returns>The array of records with matched last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// This method performs searching in records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The person's date of birth.</param>
        /// <returns>The array of records with matched date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

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

        public void Restore(FileCabinetServiceSnapshot serviceSnapshot);
    }
}
