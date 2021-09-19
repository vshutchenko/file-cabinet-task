using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class implements custom parameters validation.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// This method implements custom parameters validation.
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
            new DateOfBirthValidator(new DateTime(1900, 1, 1), DateTime.Now).ValidateParameters(recordParameters);
            new GenderValidator(new[] { 'F', 'M', 'f', 'm' }).ValidateParameters(recordParameters);
            new ExperienceValidator(5, 40).ValidateParameters(recordParameters);
            new SalaryValidator(300, 1000).ValidateParameters(recordParameters);
        }
    }
}
