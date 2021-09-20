using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public class CustomInputValidator : IInputValidator
    {
        private int maxStringLength = 60;
        private int minStringLength = 2;
        private DateTime from = new DateTime(1900, 1, 1);
        private DateTime to = DateTime.Now;
        private char[] allowedChars = new[] { 'F', 'M', 'f', 'm' };
        private int minExperienceValue = 5;
        private int maxExperienceValue = 40;
        private int minSalaryValue = 100;
        private int maxSalaryValue = 10000;

        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if ((dateOfBirth < this.from) || (dateOfBirth > this.to))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect date. Specify the date between " +
                    $"{this.from.ToString("dd-MMM-yyy", CultureInfo.InvariantCulture)} and " +
                    $"{this.to.ToString("dd-MMM-yyy", CultureInfo.InvariantCulture)}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> ExperienceValidator(short experience)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if ((experience < this.minExperienceValue) || (experience > this.maxExperienceValue))
            {
                isValid = false;
                validationErrorMessage = $"Experience value must be in range between {this.minExperienceValue} and {this.maxExperienceValue}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if ((firstName.Length < this.minStringLength) || (firstName.Length > this.maxStringLength))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect first name length. Minimal length is {this.minStringLength}. Maximum length is {this.maxStringLength}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> GenderValidator(char gender)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if (Array.IndexOf(this.allowedChars, gender) == -1)
            {
                isValid = false;
                validationErrorMessage = $"{nameof(gender)} is not equals to one of allowed chars: " + string.Join(',', this.allowedChars);
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if ((lastName.Length < this.minStringLength) || (lastName.Length > this.maxStringLength))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect last name length. Minimal length is {this.minStringLength}. Maximum length is {this.maxStringLength}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> SalaryValidator(decimal salary)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;

            if ((salary < this.minSalaryValue) || (salary > this.maxSalaryValue))
            {
                isValid = false;
                validationErrorMessage = $"Salary value must be in range between {this.minSalaryValue} and {this.maxSalaryValue}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }
    }
}
