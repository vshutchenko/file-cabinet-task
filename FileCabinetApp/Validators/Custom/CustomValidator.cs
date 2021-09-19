using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators.Custom
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

            new CustomFirstNameValidator().ValidateParameters(recordParameters);
            new CustomLastNameValidator().ValidateParameters(recordParameters);
            new CustomDateOfBirthValidator().ValidateParameters(recordParameters);
            new CustomGenderValidator().ValidateParameters(recordParameters);
            new CustomExperienceValidator().ValidateParameters(recordParameters);
            new CustomSalaryValidator().ValidateParameters(recordParameters);
        }
    }
}
