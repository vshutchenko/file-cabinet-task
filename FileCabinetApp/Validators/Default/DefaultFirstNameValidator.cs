using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultFirstNameValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
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
    }
}
