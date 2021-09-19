using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class implements default parameters validation.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// This method implements default parameters validation.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            if (recordParameters is null)
            {
                throw new ArgumentNullException(nameof(recordParameters), $"{nameof(recordParameters)} is null.");
            }

            new FirstNameValidator(2, 60).ValidateParameters(recordParameters);
            new LastNameValidator(2, 60).ValidateParameters(recordParameters);
            new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now).ValidateParameters(recordParameters);
            new GenderValidator(new[] { 'F', 'M' }).ValidateParameters(recordParameters);
            new ExperienceValidator(0, 10).ValidateParameters(recordParameters);
            new SalaryValidator(1000, 10000).ValidateParameters(recordParameters);
        }
    }
}
