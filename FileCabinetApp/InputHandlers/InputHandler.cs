﻿using System;
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
        private static readonly char Comma = ',';
        private static readonly char Space = ' ';
        private static readonly char SingleQuote = '\'';
        private static readonly string Values = "values";
        private static readonly string Where = "where";
        private static readonly string Set = "set";
        private static readonly string And = "and";
        private static readonly string Or = "or";

        private const char OpenBrace = '(';
        private const char CloseBrace = ')';

        private const string Id = "ID";
        private const string FirstName = "FIRSTNAME";
        private const string LastName = "LASTNAME";
        private const string DateOfBirth = "DATEOFBIRTH";
        private const string Gender = "GENDER";
        private const string Experience = "EXPERIENCE";
        private const string Salary = "SALARY";

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

        public bool TryReadUpdateCommandParameters(string parameters, out Tuple<string[], string[]> propertyValue, out Tuple<string[], string[]> propertyValueToSearch, out bool allFieldsMatch)
        {
            int maxFieldsToSearch = int.MaxValue;
            propertyValue = new Tuple<string[], string[]>(Array.Empty<string>(), Array.Empty<string>());
            propertyValueToSearch = new Tuple<string[], string[]>(Array.Empty<string>(), Array.Empty<string>());
            allFieldsMatch = false;

            if (parameters.Contains(Or) && !parameters.Contains(And))
            {
                allFieldsMatch = false;
            }
            else if (!parameters.Contains(Or) && parameters.Contains(And))
            {
                allFieldsMatch = true;
            }
            else if (parameters.Contains(Or) && parameters.Contains(And))
            {
                return false;
            }
            else
            {
                maxFieldsToSearch = 1;
            }

            var arguments = parameters.Split(new[] { Set, Where }, StringSplitOptions.RemoveEmptyEntries);

            if (arguments.Length != 2)
            {
                return false;
            }

            if (TryGetParams(arguments[0], out string[] fields, out string[] newValues)
                && TryGetParams(arguments[1], out string[] fieldsToSearch, out string[] valuesToSearch)
                && fields.Length == newValues.Length
                && fieldsToSearch.Length == valuesToSearch.Length
                && fieldsToSearch.Length <= maxFieldsToSearch)
            {
                propertyValue = new Tuple<string[], string[]>(fields, newValues);
                propertyValueToSearch = new Tuple<string[], string[]>(fieldsToSearch, valuesToSearch);

                return true;
            }
            else
            {
                return false;
            }

            bool TryGetParams(string input, out string[] parameters, out string[] values)
            {
                input = input.Trim();
                string[] separators = new string[] { Comma.ToString(), Or, And };
                parameters = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                values = new string[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var pair = parameters[i].Split(Equivalent);
                    if (pair.Length != 2)
                    {
                        return false;
                    }
                    else
                    {
                        parameters[i] = pair[0].Trim(new[] { Space, SingleQuote });
                        values[i] = pair[1].Trim(new[] { Space, SingleQuote });
                    }
                }

                return true;
            }
        }

        public bool TryReadInsertCommandParameters(string parameters, out Tuple<string[], string[]> propertyValue)
        {
            var arguments = parameters.Split(Values, 2);

            if (arguments.Length != 2)
            {
                propertyValue = new Tuple<string[], string[]>(Array.Empty<string>(), Array.Empty<string>());
                return false;
            }

            if (TryGetParams(arguments[0], out string[] fields)
                && TryGetParams(arguments[1], out string[] values)
                && fields.Length == values.Length)
            {
                propertyValue = new Tuple<string[], string[]>(fields, values);
                return true;
            }
            else
            {
                propertyValue = new Tuple<string[], string[]>(Array.Empty<string>(), Array.Empty<string>());
                return false;
            }

            bool TryGetParams(string input, out string[] parameters)
            {
                input = input.Trim();
                int openBraceindex = input.IndexOf(OpenBrace);
                int closeBraceindex = input.IndexOf(CloseBrace);
                if ((openBraceindex != 0) || (closeBraceindex != input.Length - 1))
                {
                    parameters = Array.Empty<string>();
                    return false;
                }

                input = input.Substring(openBraceindex + 1, closeBraceindex - openBraceindex - 1);
                parameters = input.Split(Comma);
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = parameters[i].Trim(Space, SingleQuote);
                }

                return true;
            }
        }

        public bool TryReadDeleteCommandParameters(string parameters, out Tuple<string, string> propertyValue)
        {
            var arguments = parameters.Split(Where, StringSplitOptions.RemoveEmptyEntries);

            if (arguments.Length != 1)
            {
                propertyValue = new Tuple<string, string>(string.Empty, string.Empty);
                return false;
            }

            var propertyValuePair = arguments[0].Split(Equivalent);

            if (propertyValuePair.Length != 2)
            {
                propertyValue = new Tuple<string, string>(string.Empty, string.Empty);
                return false;
            }

            string property = propertyValuePair[0].Trim(new[] { Space, SingleQuote });
            string value = propertyValuePair[1].Trim(new[] { Space, SingleQuote });

            propertyValue = new Tuple<string, string>(property, value);
            return true;
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

        public FileCabinetRecord ReadRecordParameters(string[] values, string[] fields, IInputValidator inputValidator, InputConverter converter)
        {
            FileCabinetRecord record = new FileCabinetRecord();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Equals(FirstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.FirstName = ReadInput(values[i], converter.StringConverter, inputValidator.FirstNameValidator);
                }
                else if (fields[i].Equals(LastName, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.LastName = ReadInput(values[i], converter.StringConverter, inputValidator.LastNameValidator);
                }
                else if (fields[i].Equals(DateOfBirth, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.DateOfBirth = ReadInput(values[i], converter.DateTimeConverter, inputValidator.DateOfBirthValidator);
                }
                else if (fields[i].Equals(Gender, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.Gender = ReadInput(values[i], converter.CharConverter, inputValidator.GenderValidator);
                }
                else if (fields[i].Equals(Experience, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.Experience = ReadInput(values[i], converter.ShortConverter, inputValidator.ExperienceValidator);
                }
                else if (fields[i].Equals(Salary, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.Salary = ReadInput(values[i], converter.DecimalConverter, inputValidator.SalaryValidator);
                }
                else if (fields[i].Equals(Id, StringComparison.InvariantCultureIgnoreCase))
                {
                    record.Id = ReadInput(values[i], converter.IntConverter, inputValidator.IdValidator);
                }
                else
                {
                    Console.WriteLine($"There is no '{fields[i]}' property");
                }
            }

            return record;
        }

        private T ReadInput<T>(string input, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            T value;

            var conversionResult = converter(input);

            if (!conversionResult.Item1)
            {
                Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
            }

            value = conversionResult.Item3;

            var validationResult = validator(value);
            if (!validationResult.Item1)
            {
                Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
            }

            return value;
        }
    }
}
