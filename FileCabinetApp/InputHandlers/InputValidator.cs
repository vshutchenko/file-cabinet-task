using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.InputHandlers
{
    public class InputValidator
    {
        private IFileCabinetService fileCabinetService;

        public InputValidator(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        public Tuple<bool, string> IdValidator(int id)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int minValue = 1;
            int maxValue = this.fileCabinetService.GetStat().Item2;

            if ((id < minValue) || (id > maxValue))
            {
                isValid = false;
                validationErrorMessage = $"There is no record with id={id}.";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> ExperienceValidator(short experience)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int minValue = 0;
            int maxValue = short.MaxValue;

            if (Program.IsCustomRulesEnabled)
            {
                maxValue = 100;
            }

            if ((experience < minValue) || (experience > maxValue))
            {
                isValid = false;
                validationErrorMessage = $"Experience must be in range between {minValue} and {maxValue}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> SalaryValidator(decimal salary)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int minValue = 0;

            if (salary < minValue)
            {
                isValid = false;
                validationErrorMessage = $"Incorrect salary value: {salary} < {minValue}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> GenderValidator(char gender)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            char[] validValues;

            if (Program.IsCustomRulesEnabled)
            {
                validValues = new char[] { 'F', 'M' };
            }
            else
            {
                validValues = new char[] { 'F', 'M', 'f', 'm' };
            }

            if (Array.IndexOf(validValues, gender) == -1)
            {
                isValid = false;
                validationErrorMessage = $"Incorrect gender value";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            DateTime minDate;

            if (Program.IsCustomRulesEnabled)
            {
                minDate = new DateTime(1950, 1, 1);
            }
            else
            {
                minDate = new DateTime(1900, 1, 1);
            }

            DateTime maxDate = DateTime.Now;

            if ((dateOfBirth < minDate) || (dateOfBirth > maxDate))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect date. Specify the date between " +
                    $"{minDate.ToString("dd-MMM-yyy", CultureInfo.InvariantCulture)} and " +
                    $"{maxDate.ToString("dd-MMM-yyy", CultureInfo.InvariantCulture)}";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int maxStringLength = 60;
            int minStringLegth = 2;

            if ((firstName.Length < minStringLegth) || (firstName.Length > maxStringLength))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect first name length. Minimal length is {minStringLegth}. Maximum length is {maxStringLength}";
            }

            if (Program.IsCustomRulesEnabled)
            {
                for (int i = 1; i < firstName.Length; i++)
                {
                    if (char.IsUpper(firstName[i]))
                    {
                        isValid = false;
                        validationErrorMessage = $"Only first letter of first name can be capital";
                    }
                }

                if (!char.IsUpper(firstName[0]))
                {
                    isValid = false;
                    validationErrorMessage = $"The first letter of first name must be capital";
                }
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int maxStringLength = 60;
            int minStringLegth = 2;

            if ((lastName.Length < minStringLegth) || (lastName.Length > maxStringLength))
            {
                isValid = false;
                validationErrorMessage = $"Incorrect last name length. Minimal length is {minStringLegth}. Maximum length is {maxStringLength}";
            }

            if (Program.IsCustomRulesEnabled)
            {
                for (int i = 1; i < lastName.Length; i++)
                {
                    if (char.IsUpper(lastName[i]))
                    {
                        isValid = false;
                        validationErrorMessage = $"Only first letter of last name can be capital";
                    }
                }

                if (!char.IsUpper(lastName[0]))
                {
                    isValid = false;
                    validationErrorMessage = $"The first letter of last name must be capital";
                }
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }
    }
}
