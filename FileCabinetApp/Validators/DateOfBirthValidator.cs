using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            if ((recordParameters.DateOfBirth < this.from) || (this.to > DateTime.Now))
            {
                throw new ArgumentOutOfRangeException(nameof(recordParameters.DateOfBirth), $"{nameof(recordParameters.DateOfBirth)} is not in range between {this.from} and {this.to}.");
            }
        }
    }
}
