using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// This class contains information about available commands and processes console commands.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shutchenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static IFileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());
        private static bool isCustomRulesEnabled;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "edit", "edits the record with specified id", "The 'edit' command edits the record with specified id." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "find", "finds records, recieves name of property and text to search", "The 'find' command finds records, recieves name of property and text to search." },
            new string[] { "list", "prints the list of records", "The 'list' command prints the list of records." },
            new string[] { "stat", "prints number of records stored in the service", "The 'stat' command prints number of records stored in the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// This method runs an instance of FileCabinetService and processes console commands.
        /// </summary>
        /// <param name="args">Console command parameters.</param>
        public static void Main(string[] args)
        {
            ReadCommandLineParameters(args);
            string validationRulesHint = isCustomRulesEnabled ? "Using custom validation rules." : "Using default validation rules.";

            if (isCustomRulesEnabled)
            {
                validationRulesHint = "Using custom validation rules.";
                fileCabinetService = new FileCabinetService(new CustomValidator());
            }
            else
            {
                validationRulesHint = "Using default validation rules.";
                fileCabinetService = new FileCabinetService(new DefaultValidator());
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(validationRulesHint);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            RecordParameters recordParameters;

            Console.Write("First name: ");
            var firstName = ReadInput(StringConverter, FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = ReadInput(StringConverter, LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = ReadInput(CharConverter, GenderValidator);
            Console.Write("Experience: ");
            var experience = ReadInput(ShortConverter, ExperienceValidator);
            Console.Write("Salary: ");
            var salary = ReadInput(DecimalConverter, SalaryValidator);

            recordParameters = new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);

            try
            {
                fileCabinetService.CreateRecord(recordParameters);
                Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
            }
            catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid input.");
            }
        }

        private static void List(string parameters)
        {
            string record;
            ReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.GetRecords();

            for (int i = 0; i < fileCabinetService.GetStat(); i++)
            {
                record = $"#{records[i].Id}, " +
                    $"{records[i].FirstName}, " +
                    $"{records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                    $"{records[i].Gender}, " +
                    $"{records[i].Experience}, " +
                    $"{records[i].Salary}$";
                Console.WriteLine(record);
            }
        }

        private static void Edit(string parameters)
        {
            RecordParameters recordParameters;
            Tuple<bool, string, int> parametersConversionResult = IntConverter(parameters);
            Tuple<bool, string> recordIdValidationResult;
            int id;
            if (!parametersConversionResult.Item1)
            {
                Console.WriteLine(parametersConversionResult.Item2);
                return;
            }
            else
            {
                id = parametersConversionResult.Item3;
                recordIdValidationResult = IdValidator(id);
                if (!recordIdValidationResult.Item1)
                {
                    Console.WriteLine(recordIdValidationResult.Item2);
                    return;
                }
            }

            Console.Write("First name: ");
            var firstName = ReadInput(StringConverter, FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = ReadInput(StringConverter, LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = ReadInput(CharConverter, GenderValidator);
            Console.Write("Experience: ");
            var experience = ReadInput(ShortConverter, ExperienceValidator);
            Console.Write("Salary: ");
            var salary = ReadInput(DecimalConverter, SalaryValidator);

            recordParameters = new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);

            try
            {
                fileCabinetService.EditRecord(id, recordParameters);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid input.");
            }
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Wrong parameters. Please specify name of property and text to search.");
                return;
            }

            string[] paramArray = parameters.Split(' ', 2);
            string propertyName = paramArray[0];
            string textToSearch = paramArray[1];

            if ((textToSearch.Length < 2) || (textToSearch[0] != '"') || (textToSearch[^1] != '"'))
            {
                Console.WriteLine("Wrong parameters.");
                return;
            }

            textToSearch = textToSearch[1..^1];

            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            string record;

            if (string.Equals(propertyName, nameof(FileCabinetRecord.FirstName), StringComparison.InvariantCultureIgnoreCase))
            {
                records = fileCabinetService.FindByFirstName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.LastName), StringComparison.InvariantCultureIgnoreCase))
            {
                records = fileCabinetService.FindByLastName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.DateOfBirth), StringComparison.InvariantCultureIgnoreCase))
            {
                if (DateTime.TryParse(textToSearch, out DateTime dateOfBirth))
                {
                    records = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                }
            }

            for (int i = 0; i < records.Count; i++)
            {
                record = $"#{records[i].Id}, " +
                $"{records[i].FirstName}, " +
                $"{records[i].LastName}, " +
                $"{records[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                $"{records[i].Gender}, " +
                $"{records[i].Experience}, " +
                $"{records[i].Salary}$";
                Console.WriteLine(record);
            }
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

        private static Tuple<bool, string, int> IntConverter(string stringToConvert)
        {
            Tuple<bool, string, int> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = int.TryParse(stringToConvert, out int convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number.";
            }

            conversionResult = new Tuple<bool, string, int>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        private static Tuple<bool, string, string> StringConverter(string stringToConvert)
        {
            bool isConverted = true;
            string conversionErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(stringToConvert) || string.IsNullOrEmpty(stringToConvert))
            {
                isConverted = false;
                conversionErrorMessage = "Input string is null, empty or whitespace";
            }

            Tuple<bool, string, string> conversionResult = new Tuple<bool, string, string>(isConverted, conversionErrorMessage, stringToConvert);
            return conversionResult;
        }

        private static Tuple<bool, string, DateTime> DateTimeConverter(string stringToConvert)
        {
            Tuple<bool, string, DateTime> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = DateTime.TryParse(stringToConvert, out DateTime date);

            if (!isConverted)
            {
                conversionErrorMessage = "Incorrect date format";
            }

            conversionResult = new Tuple<bool, string, DateTime>(isConverted, conversionErrorMessage, date);

            return conversionResult;
        }

        private static Tuple<bool, string, char> CharConverter(string stringToConvert)
        {
            Tuple<bool, string, char> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = char.TryParse(stringToConvert, out char convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a single character";
            }

            conversionResult = new Tuple<bool, string, char>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        private static Tuple<bool, string, short> ShortConverter(string stringToConvert)
        {
            Tuple<bool, string, short> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = short.TryParse(stringToConvert, out short convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number or a too big value";
            }

            conversionResult = new Tuple<bool, string, short>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string stringToConvert)
        {
            Tuple<bool, string, decimal> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = decimal.TryParse(stringToConvert, out decimal convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number";
            }

            conversionResult = new Tuple<bool, string, decimal>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        private static Tuple<bool, string> IdValidator(int id)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int minValue = 1;
            int maxValue = fileCabinetService.GetStat();

            if ((id < minValue) || (id > maxValue))
            {
                isValid = false;
                validationErrorMessage = $"There is no record with id={id}.";
            }

            return new Tuple<bool, string>(isValid, validationErrorMessage);
        }

        private static Tuple<bool, string> ExperienceValidator(short experience)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            int minValue = 0;
            int maxValue = short.MaxValue;

            if (isCustomRulesEnabled)
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

        private static Tuple<bool, string> SalaryValidator(decimal salary)
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

        private static Tuple<bool, string> GenderValidator(char gender)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            char[] validValues;

            if (isCustomRulesEnabled)
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

        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool isValid = true;
            string validationErrorMessage = string.Empty;
            DateTime minDate;

            if (isCustomRulesEnabled)
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

        private static Tuple<bool, string> FirstNameValidator(string firstName)
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

            if (isCustomRulesEnabled)
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

        private static Tuple<bool, string> LastNameValidator(string lastName)
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

            if (isCustomRulesEnabled)
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

        private static void ReadCommandLineParameters(string[] parameters)
        {
            if (parameters is null)
            {
                return;
            }

            for (int i = 0; i < parameters.Length; i += 2)
            {
                switch (parameters[i].ToUpperInvariant())
                {
                    case "-V":
                        if (i + 1 < parameters.Length)
                        {
                            if (parameters[i + 1] == "CUSTOM")
                            {
                                isCustomRulesEnabled = true;
                            }
                            else if (parameters[i + 1] == "DEFAULT")
                            {
                                isCustomRulesEnabled = false;
                            }
                        }

                        break;
                    case "--VALIDATION-RULES=DEFAULT":
                        isCustomRulesEnabled = false;
                        break;
                    case "--VALIDATION-RULES=CUSTOM":
                        isCustomRulesEnabled = true;
                        break;
                }
            }
        }
    }
}
