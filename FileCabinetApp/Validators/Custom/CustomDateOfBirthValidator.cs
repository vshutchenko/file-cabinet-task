using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Custom
{
    public class CustomDateOfBirthValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            DateTime minDate = new DateTime(1900, 1, 1);
            DateTime maxDate = DateTime.Now;

            if ((recordParameters.DateOfBirth < minDate) || (maxDate > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between {minDate} and {maxDate}.");
            }
        }
    }
}
