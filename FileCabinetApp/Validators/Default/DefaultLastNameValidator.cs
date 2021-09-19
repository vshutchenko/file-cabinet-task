using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultLastNameValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
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
    }
}
