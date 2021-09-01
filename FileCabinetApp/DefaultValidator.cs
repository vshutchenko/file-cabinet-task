using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
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

            if ((recordParameters.DateOfBirth < new DateTime(1950, 1, 1)) || (recordParameters.DateOfBirth > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between 01-Jan-1950 and current date");
            }

            if ((recordParameters.Gender != 'M') && (recordParameters.Gender != 'F'))
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to F or M");
            }

            if (recordParameters.Experience < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is less than zero");
            }

            if (recordParameters.Salary < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is less than zero");
            }
        }
    }
}
