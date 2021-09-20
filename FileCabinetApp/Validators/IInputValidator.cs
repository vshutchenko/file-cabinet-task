using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Provides methods for validating properties of <see cref="FileCabinetRecord"></see> class.
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Experience"/> property.
        /// </summary>
        /// <param name="experience">Years of experience.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> ExperienceValidator(short experience);

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Salary"/> property.
        /// </summary>
        /// <param name="salary">Salary.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> SalaryValidator(decimal salary);

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Gender"/> property.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> GenderValidator(char gender);

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.DateOfBirth"/> property.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth);

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.FirstName"/> property.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> FirstNameValidator(string firstName);

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.LastName"/> property.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns><see cref="Tuple{Boolean, String}"/> where the first bool value indicates was validation successfull or not.
        /// The second string value contains an error message if validation ended with error.</returns>
        public Tuple<bool, string> LastNameValidator(string lastName);
    }
}
