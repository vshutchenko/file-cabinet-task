using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators.Custom
{
    public class CustomGenderValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if ((char.ToUpper(recordParameters.Gender, CultureInfo.InvariantCulture) != 'M') && (char.ToUpper(recordParameters.Gender, CultureInfo.InvariantCulture) != 'F'))
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to F or M");
            }
        }
    }
}
