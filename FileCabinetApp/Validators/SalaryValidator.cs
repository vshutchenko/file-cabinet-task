using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validating of <see cref="FileCabinetRecord.Salary"/> property.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private decimal minValue;
        private decimal maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimum input value.</param>
        /// <param name="maxValue">Maximum input value.</param>
        public SalaryValidator(decimal minValue, decimal maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Validate value of <see cref="FileCabinetRecord.Salary"/> property.
        /// </summary>
        /// <param name="recordParameters">Parameter object of <see cref="FileCabinetRecord"/> class.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if ((recordParameters.Salary < this.minValue) || (recordParameters.Salary > this.maxValue))
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is not between {this.minValue} and {this.maxValue}.");
            }
        }
    }
}
