using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validating of <see cref="FileCabinetRecord.FirstName"/> property.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimum input length.</param>
        /// <param name="maxLength">Maximum input length.</param>
        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.FirstName"/> property.
        /// </summary>
        /// <param name="recordParameters">Parameter object of <see cref="FileCabinetRecord"/> class.</param>
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
