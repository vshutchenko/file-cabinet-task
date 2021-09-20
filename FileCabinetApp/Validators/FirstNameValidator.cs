using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (string.IsNullOrWhiteSpace(recordParameters.FirstName))
            {
                throw new ArgumentNullException(nameof(recordParameters.FirstName), $"{nameof(recordParameters.FirstName)} is null or whitespace");
            }

            if ((recordParameters.FirstName.Length < this.minLength) || (recordParameters.FirstName.Length > this.maxLength))
            {
                throw new ArgumentException($"{nameof(recordParameters.FirstName)} length is not between {this.minLength} and {this.maxLength}.");
            }
        }
    }
}
