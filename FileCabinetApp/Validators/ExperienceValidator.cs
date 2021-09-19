using System;
using System.Collections.Generic;
using System.Text;

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

            if ((recordParameters.Experience < minValue) || (recordParameters.Experience > maxValue))
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is not between {minValue} and {maxValue}.");
            }
        }
    }
}
