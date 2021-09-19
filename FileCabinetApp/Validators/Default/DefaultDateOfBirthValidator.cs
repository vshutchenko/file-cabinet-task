using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultDateOfBirthValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;

            if ((recordParameters.DateOfBirth < minDate) || (maxDate > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between {minDate} and {maxDate}.");
            }
        }
    }
}
