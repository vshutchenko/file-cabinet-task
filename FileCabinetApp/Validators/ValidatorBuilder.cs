using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Creates a validator which implements <see cref="IRecordValidator"/> interface.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly string firstName = "firstName";
        private readonly string lastName = "lastName";
        private readonly string dateOfBirth = "dateOfBirth";
        private readonly string experience = "experience";
        private readonly string salary = "salary";
        private readonly string gender = "gender";
        private readonly string allowedChars = "allowedChars";
        private readonly string min = "min";
        private readonly string max = "max";
        private readonly string configurationFile = "validation-rules.json";
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public IRecordValidator CreateFromConfiguration(string validationRules)
        {
            IConfiguration config = new ConfigurationBuilder().
                AddJsonFile(this.configurationFile).
                Build();

            DateTime minDate;
            DateTime maxDate;
            decimal minSalary;
            decimal maxSalary;
            short minExperience;
            short maxExperience;
            char[] allowedChars;
            int lastNameMinLength;
            int lastNameMaxLength;
            int firstNameMinLength;
            int firstNameMaxLength;

            IConfigurationSection section = config.GetSection(validationRules);

            try
            {
                firstNameMinLength = int.Parse(section.GetSection(this.firstName).GetSection(this.min).Value);
                firstNameMaxLength = int.Parse(section.GetSection(this.firstName).GetSection(this.max).Value);
                lastNameMinLength = int.Parse(section.GetSection(this.lastName).GetSection(this.min).Value);
                lastNameMaxLength = int.Parse(section.GetSection(this.lastName).GetSection(this.max).Value);
                minDate = DateTime.Parse(section.GetSection(this.dateOfBirth).GetSection(this.min).Value);
                maxDate = DateTime.Parse(section.GetSection(this.dateOfBirth).GetSection(this.max).Value);
                minExperience = short.Parse(section.GetSection(this.experience).GetSection(this.min).Value);
                maxExperience = short.Parse(section.GetSection(this.experience).GetSection(this.max).Value);
                minSalary = decimal.Parse(section.GetSection(this.salary).GetSection(this.min).Value);
                maxSalary = decimal.Parse(section.GetSection(this.salary).GetSection(this.max).Value);
                allowedChars = section.GetSection(this.gender).GetSection(this.allowedChars).Value.ToCharArray();
            }
            catch (Exception ex) when (
                ex is OverflowException
                || ex is FormatException
                || ex is ArgumentNullException)
            {
                Console.WriteLine("Invalid configuration file. " + ex.Message + " Creating default validator.");
                return new ValidatorBuilder().CreateDefault();
            }

            return new ValidatorBuilder().
                    ValidateFirstName(firstNameMinLength, firstNameMaxLength).
                    ValidateLastName(lastNameMinLength, lastNameMaxLength).
                    ValidateDateOfBirth(minDate, maxDate).
                    ValidateGender(allowedChars).
                    ValidateExperience(minExperience, maxExperience).
                    ValidateSalary(minSalary, maxSalary).
                    Create();
        }

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
        public ValidatorBuilder ValidateSalary(decimal minValue, decimal maxValue)
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
