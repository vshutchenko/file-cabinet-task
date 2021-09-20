using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validating of <see cref="FileCabinetRecord.LastName"/> property.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">Minimum input length.</param>
        /// <param name="maxLength">Maximum input length.</param>
        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.LastName"/> property.
        /// </summary>
        /// <param name="recordParameters">Parameter object of <see cref="FileCabinetRecord"/> class.</param>
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
