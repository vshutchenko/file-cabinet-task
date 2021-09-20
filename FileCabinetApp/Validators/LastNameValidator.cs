using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (string.IsNullOrWhiteSpace(recordParameters.LastName))
            {
                throw new ArgumentNullException(nameof(recordParameters.LastName), $"{nameof(recordParameters.LastName)} is null or whitespace");
            }

            if ((recordParameters.LastName.Length < this.minLength) || (recordParameters.LastName.Length > this.maxLength))
            {
                throw new ArgumentException($"{nameof(recordParameters.LastName)} length is not between {this.minLength} and {this.maxLength}.");
            }
        }
    }
}
