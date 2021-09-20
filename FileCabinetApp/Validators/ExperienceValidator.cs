using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validating of <see cref="FileCabinetRecord.Experience"/> property.
    /// </summary>
    public class ExperienceValidator : IRecordValidator
    {
        private int minValue;
        private int maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperienceValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimum input value.</param>
        /// <param name="maxValue">Maximum input value.</param>
        public ExperienceValidator(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Experience"/> property.
        /// </summary>
        /// <param name="recordParameters">Parameter object of <see cref="FileCabinetRecord"/> class.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters.Experience > short.MaxValue)
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} value is bigger than short.MaxValue: {short.MaxValue}.");
            }

            if ((recordParameters.Experience < this.minValue) || (recordParameters.Experience > this.maxValue))
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is not between {this.minValue} and {this.maxValue}.");
            }
        }
    }
}
