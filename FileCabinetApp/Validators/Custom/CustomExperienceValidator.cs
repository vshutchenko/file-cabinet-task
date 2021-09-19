using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Custom
{
    public class CustomExperienceValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            int minValue = 0;
            int maxValue = short.MaxValue;

            if ((recordParameters.Experience < minValue) || (recordParameters.Experience > maxValue))
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is bigger than {maxValue} or less than {minValue}.");
            }
        }
    }
}
