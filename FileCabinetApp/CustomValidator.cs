using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements custom parameters validation.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// This method implements custom parameters validation.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters is null)
            {
                throw new ArgumentNullException(nameof(recordParameters), $"{nameof(recordParameters)} is null.");
            }

            if (string.IsNullOrWhiteSpace(recordParameters.FirstName))
            {
                throw new ArgumentNullException(nameof(recordParameters.FirstName), $"{nameof(recordParameters.FirstName)} is null or whitespace");
            }

            if ((recordParameters.FirstName.Length < 2) || (recordParameters.FirstName.Length > 60))
            {
                throw new ArgumentException($"{nameof(recordParameters.FirstName)} length is not between 2 and 60");
            }

            if (string.IsNullOrWhiteSpace(recordParameters.LastName))
            {
                throw new ArgumentNullException(nameof(recordParameters.LastName), $"{nameof(recordParameters.LastName)} is null or whitespace");
            }

            if ((recordParameters.LastName.Length < 2) || (recordParameters.LastName.Length > 60))
            {
                throw new ArgumentException($"{nameof(recordParameters.LastName)} length is not between 2 and 60");
            }

            if ((recordParameters.DateOfBirth < new DateTime(1900, 1, 1)) || (recordParameters.DateOfBirth > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between 01-Jan-1950 and current date");
            }

            if (!char.IsUpper(recordParameters.FirstName[0]))
            {
                throw new ArgumentException($"{nameof(recordParameters.FirstName)} must start with a capital letter.");
            }

            for (int i = 1; i < recordParameters.FirstName.Length; i++)
            {
                if (char.IsUpper(recordParameters.FirstName[i]))
                {
                    throw new ArgumentException($"{nameof(recordParameters.FirstName)} contains a capital letter at index {i}.");
                }
            }

            if (!char.IsUpper(recordParameters.LastName[0]))
            {
                throw new ArgumentException($"{nameof(recordParameters.LastName)} must start with a capital letter.");
            }

            for (int i = 1; i < recordParameters.LastName.Length; i++)
            {
                if (char.IsUpper(recordParameters.LastName[i]))
                {
                    throw new ArgumentException($"{nameof(recordParameters.LastName)} contains a capital letter at index {i}.");
                }
            }

            if ((char.ToUpper(recordParameters.Gender, CultureInfo.InvariantCulture) != 'M') && (char.ToUpper(recordParameters.Gender, CultureInfo.InvariantCulture) != 'F'))
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to F or M");
            }

            if ((recordParameters.Experience < 0) || (recordParameters.Experience > 100))
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is bigger than 100 less than 0");
            }

            if (recordParameters.Salary < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is less than zero");
            }
        }
    }
}
