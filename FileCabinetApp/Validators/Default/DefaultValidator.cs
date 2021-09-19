using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.Default
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

            new DefaultFirstNameValidator().ValidateParameters(recordParameters);
            new DefaultLastNameValidator().ValidateParameters(recordParameters);
            new DefaultDateOfBirthValidator().ValidateParameters(recordParameters);
            new DefaultGenderValidator().ValidateParameters(recordParameters);
            new DefaultExperienceValidator().ValidateParameters(recordParameters);
            new DefaultSalaryValidator().ValidateParameters(recordParameters);
        }
    }
}
