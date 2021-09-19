﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
{
    public class DefaultSalaryValidator : IRecordValidator
    {
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters.Salary < 0)
            {
                throw new ArgumentException($"{nameof(recordParameters.Salary)} is less than zero");
            }
        }
    }
}
