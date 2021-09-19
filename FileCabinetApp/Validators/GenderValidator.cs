using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class GenderValidator : IRecordValidator
    {
        private char[] allowedChars;

        public GenderValidator(char[] allowedChars)
        {
            this.allowedChars = allowedChars;
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (Array.IndexOf(allowedChars, recordParameters.Gender) == -1)
            {
                throw new ArgumentException($"{nameof(recordParameters.Gender)} is not equals to one of allowed values: " + string.Join(',', allowedChars));
            }
        }
    }
}
