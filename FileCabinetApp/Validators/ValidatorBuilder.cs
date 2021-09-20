using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Creates a validator which implements <see cref="IRecordValidator"/> interface.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.FirstName"/> property.
        /// </summary>
        /// <param name="minLength">Minimum first name length.</param>
        /// <param name="maxLength">Maximum first name length.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.LastName"/> property.
        /// </summary>
        /// <param name="minLength">Minimum last name length.</param>
        /// <param name="maxLength">Maximum last name length.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.DateOfBirth"/> property.
        /// </summary>
        /// <param name="from">Minimum date.</param>
        /// <param name="to">Maximum date.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.Gender"/> property.
        /// </summary>
        /// <param name="allowedChars">Array of allowed characters to specify the gender.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateGender(char[] allowedChars)
        {
            this.validators.Add(new GenderValidator(allowedChars));
            return this;
        }

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.Experience"/> property.
        /// </summary>
        /// <param name="minValue">Minimum experience value.</param>
        /// <param name="maxValue">Maximum experience value.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateExperience(int minValue, int maxValue)
        {
            this.validators.Add(new ExperienceValidator(minValue, maxValue));
            return this;
        }

        /// <summary>
        /// Sets validator with parameters parameters for <see cref="FileCabinetRecord.Salary"/> property.
        /// </summary>
        /// <param name="minValue">Minimum salary value.</param>
        /// <param name="maxValue">Maximum salary value.</param>
        /// <returns>Reference on current builder.</returns>
        public ValidatorBuilder ValidateSalary(int minValue, int maxValue)
        {
            this.validators.Add(new SalaryValidator(minValue, maxValue));
            return this;
        }

        /// <summary>
        /// Creates validator.
        /// </summary>
        /// <returns>The validator which implements <see cref="IRecordValidator"/> interface.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
