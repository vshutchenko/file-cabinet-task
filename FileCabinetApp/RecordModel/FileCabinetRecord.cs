using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordModel
{
    /// <summary>
    /// This class represents a record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets the record id.
        /// </summary>
        /// <value>The record id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of person.
        /// </summary>
        /// <value>The first name of person.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of person.
        /// </summary>
        /// <value>The last name of person.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the person's date of birth.
        /// </summary>
        /// <value>The person's date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        /// <value>The gender of the person. 'M' - male, 'F' - female.</value>
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets the experience of person.
        /// </summary>
        /// <value>The person's experience in work.</value>
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets the salary of the person.
        /// </summary>
        /// <value>The salary of the person.</value>
        public decimal Salary { get; set; }
    }
}
