using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validating of <see cref="FileCabinetRecord.Gender"/> property.
    /// </summary>
    public class GenderValidator : IRecordValidator
    {
        private char[] allowedChars;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderValidator"/> class.
        /// </summary>
        /// <param name="allowedChars">Allowed characters to specify he gender.</param>
        public GenderValidator(char[] allowedChars)
        {
            this.allowedChars = allowedChars;
        }

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Gender"/> property.
        /// </summary>
        /// <param name="recordParameters">Parameter object of <see cref="FileCabinetRecord"/> class.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (Array.IndexOf(this.allowedChars, recordParameters.Gender) == -1)
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to one of allowed values: " + string.Join(',', this.allowedChars));
            }
        }
    }
}
