using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Validators;

namespace FileCabinetApp.InputHandlers
{
    public class InputHandler
    {
        private static readonly string Dash = "-";
        private static readonly string DoubleDash = "--";
        private static readonly string Equivalent = "=";

        public Dictionary<string, string> ReadCommandLineParameters(string[] args)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(DoubleDash))
                {
                    string[] currentParameter = args[i].Split(Equivalent, 2);
                    parameters.Add(currentParameter[0].ToUpperInvariant(), currentParameter[^1]);
                }
                else if (args[i].StartsWith(Dash) && (i + 1 < args.Length))
                {
                    parameters.Add(args[i].ToUpperInvariant(), args[i + 1]);
                    i++;
                }
            }

            return parameters;
        }

        public RecordParameters ReadRecordParameters(IInputValidator inputValidator)
        {
            InputConverter converter = new InputConverter();

            Console.Write("First name: ");
            var firstName = Program.ReadInput(converter.StringConverter, inputValidator.FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = Program.ReadInput(converter.StringConverter, inputValidator.LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = Program.ReadInput(converter.DateTimeConverter, inputValidator.DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = Program.ReadInput(converter.CharConverter, inputValidator.GenderValidator);
            Console.Write("Experience: ");
            var experience = Program.ReadInput(converter.ShortConverter, inputValidator.ExperienceValidator);
            Console.Write("Salary: ");
            var salary = Program.ReadInput(converter.DecimalConverter, inputValidator.SalaryValidator);

            return new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);
        }
    }
}
