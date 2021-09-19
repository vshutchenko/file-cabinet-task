using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultExperienceValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters.Experience < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Experience)} is less than zero.");
            }
        }
    }
}
