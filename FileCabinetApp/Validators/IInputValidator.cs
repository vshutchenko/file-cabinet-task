using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    public interface IInputValidator
    {
        public Tuple<bool, string> ExperienceValidator(short experience);

        public Tuple<bool, string> SalaryValidator(decimal salary);

        public Tuple<bool, string> GenderValidator(char gender);

        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth);

        public Tuple<bool, string> FirstNameValidator(string firstName);

        public Tuple<bool, string> LastNameValidator(string lastName);
    }
}
