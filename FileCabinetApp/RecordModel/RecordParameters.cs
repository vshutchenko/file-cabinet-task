using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.RecordModel
{
    /// <summary>
    /// This class represents a parameter object for FileCabinetRecord.
    /// </summary>
    public class RecordParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordParameters"/> class.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <param name="dateOfBirth">The date of birth of the person.</param>
        /// <param name="gender">The gender of the person.</param>
        /// <param name="experience">The person's experience in work.</param>
        /// <param name="salary">The person's salary.</param>
        public RecordParameters(string firstName, string lastName, DateTime dateOfBirth, char gender, short experience, decimal salary)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Experience = experience;
            this.Salary = salary;
        }

        /// <summary>
        /// Gets the first name of person.
        /// </summary>
        /// <value>The first name of person.</value>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name of person.
        /// </summary>
        /// <value>The last name of person.</value>
        public string LastName { get; }

        /// <summary>
        /// Gets the person's date of birth.
        /// </summary>
        /// <value>The person's date of birth.</value>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Gets the gender of the person.
        /// </summary>
        /// <value>The gender of the person. 'M' - male, 'F' - female.</value>
        public char Gender { get; }

        /// <summary>
        /// Gets the experience of person.
        /// </summary>
        /// <value>The person's experience in work.</value>
        public short Experience { get; }

        /// <summary>
        /// Gets the salary of the person.
        /// </summary>
        /// <value>The salary of the person.</value>
        public decimal Salary { get; }

        /// <summary>
        /// Gets string representation of <see cref="RecordParameters"/> class.
        /// </summary>
        /// <returns>String representation of <see cref="RecordParameters"/> class.</returns>
        public override string ToString()
        {
            return $"{nameof(this.FirstName)} = '{this.FirstName}', " +
                $"{nameof(this.LastName)} = '{this.LastName}', " +
                $"{nameof(this.DateOfBirth)} = '{this.DateOfBirth}', " +
                $"{nameof(this.Gender)} = '{this.Gender}', " +
                $"{nameof(this.Experience)} = '{this.Experience}', " +
                $"{nameof(this.Salary)} = '{this.Salary}'";
        }
    }
}
