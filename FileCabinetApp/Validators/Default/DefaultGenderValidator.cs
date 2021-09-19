using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultGenderValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if ((recordParameters.Gender != 'M') && (recordParameters.Gender != 'F'))
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to F or M.");
            }
        }
    }
}
