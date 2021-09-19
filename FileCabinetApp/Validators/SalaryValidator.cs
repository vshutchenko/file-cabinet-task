using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class SalaryValidator : IRecordValidator
    {
        private decimal minValue;
        private decimal maxValue;

        public SalaryValidator(decimal minValue, decimal maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            if ((recordParameters.Salary < minValue) || (recordParameters.Salary > maxValue))
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is not between {minValue} and {maxValue}.");
            }
        }
    }
}
