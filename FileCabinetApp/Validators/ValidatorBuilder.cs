using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateGender(char[] allowedChars)
        {
            this.validators.Add(new GenderValidator(allowedChars));
            return this;
        }

        public ValidatorBuilder ValidateExperience(int minValue, int maxValue)
        {
            this.validators.Add(new ExperienceValidator(minValue, maxValue));
            return this;
        }

        public ValidatorBuilder ValidateSalary(int minValue, int maxValue)
        {
            this.validators.Add(new SalaryValidator(minValue, maxValue));
            return this;
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
