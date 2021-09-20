using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Validators;

namespace FileCabinetApp.InputHandlers
{
    /// <summary>
    /// Provides methods for reading command line input.
    /// </summary>
    public class InputHandler
    {
        private static readonly string Dash = "-";
        private static readonly string DoubleDash = "--";
        private static readonly string Equivalent = "=";

        /// <summary>
        /// Reads command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns><see cref="Dictionary{String, String}"/> which contains pairs: parameter - value.</returns>
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

        /// <summary>
        /// Creates a parameter object for <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="inputValidator">A validator which will be used to validate input.</param>
        /// <param name="converter">A converter which will be used to convert input values.</param>
        /// <returns>A parameter object for <see cref="FileCabinetRecord"/> class.</returns>
        public RecordParameters ReadRecordParameters(IInputValidator inputValidator, InputConverter converter)
        {
            Console.Write("First name: ");
            var firstName = ReadInput(converter.StringConverter, inputValidator.FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = ReadInput(converter.StringConverter, inputValidator.LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(converter.DateTimeConverter, inputValidator.DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = ReadInput(converter.CharConverter, inputValidator.GenderValidator);
            Console.Write("Experience: ");
            var experience = ReadInput(converter.ShortConverter, inputValidator.ExperienceValidator);
            Console.Write("Salary: ");
            var salary = ReadInput(converter.DecimalConverter, inputValidator.SalaryValidator);

            return new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
