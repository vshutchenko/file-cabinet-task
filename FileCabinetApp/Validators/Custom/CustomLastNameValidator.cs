using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Custom
{
    public class CustomLastNameValidator : IRecordValidator
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
        }
    }
}
