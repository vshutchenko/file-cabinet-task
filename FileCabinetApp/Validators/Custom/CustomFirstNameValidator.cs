using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Custom
{
    public class CustomFirstNameValidator : IRecordValidator
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
        }
    }
}
