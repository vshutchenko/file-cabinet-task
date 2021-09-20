using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public class ExperienceValidator : IRecordValidator
    {
        private int minValue;
        private int maxValue;

        public ExperienceValidator(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

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
