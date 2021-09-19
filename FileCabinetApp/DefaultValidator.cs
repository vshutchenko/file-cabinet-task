using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements default parameters validation.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// This method implements default parameters validation.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters is null)
            {
                throw new ArgumentNullException(nameof(recordParameters), $"{nameof(recordParameters)} is null.");
            }

            this.ValidateFirstName(recordParameters);
            this.ValidateLastName(recordParameters);
            this.ValidateDateOfBirth(recordParameters);
            this.ValidateGender(recordParameters);
            this.ValidateExperience(recordParameters);
            this.ValidateSalary(recordParameters);
        }

        private void ValidateFirstName(RecordParameters recordParameters)
        {
            int minLength = 2;
            int maxLength = 60;

            if (string.IsNullOrWhiteSpace(recordParameters.FirstName))
            {
                throw new ArgumentNullException(nameof(recordParameters.FirstName), $"{nameof(recordParameters.FirstName)} is null or whitespace");
            }

            if ((recordParameters.FirstName.Length < minLength) || (recordParameters.FirstName.Length > maxLength))
            {
                throw new ArgumentException($"{nameof(recordParameters.FirstName)} length is not between {minLength} and {maxLength}.");
            }
        }

        private void ValidateLastName(RecordParameters recordParameters)
        {
            int minLength = 2;
            int maxLength = 60;

            if (string.IsNullOrWhiteSpace(recordParameters.LastName))
            {
                throw new ArgumentNullException(nameof(recordParameters.LastName), $"{nameof(recordParameters.LastName)} is null or whitespace");
            }

            if ((recordParameters.LastName.Length < minLength) || (recordParameters.LastName.Length > maxLength))
            {
                throw new ArgumentException($"{nameof(recordParameters.LastName)} length is not between {minLength} and {maxLength}.");
            }
        }

        private void ValidateDateOfBirth(RecordParameters recordParameters)
        {
            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;

            if ((recordParameters.DateOfBirth < minDate) || (maxDate > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between {minDate} and {maxDate}.");
            }
        }

        private void ValidateGender(RecordParameters recordParameters)
        {
            if ((recordParameters.Gender != 'M') && (recordParameters.Gender != 'F'))
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to F or M.");
            }
        }

        private void ValidateExperience(RecordParameters recordParameters)
        {
            if (recordParameters.Experience < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is less than zero.");
            }
        }

        private void ValidateSalary(RecordParameters recordParameters)
        {
            if (recordParameters.Salary < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is less than zero");
            }
        }
    }
}
