using System;
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
        private static FileCabinetService fileCabinetService = new FileCabinetService();

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
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            bool isValidInput;
            RecordParameters recordParameters;

            do
            {
                isValidInput = true;

                try
                {
                    recordParameters = ReadAndParseParams();
                    fileCabinetService.CreateRecord(recordParameters);
                    Console.WriteLine($"Record #{fileCabinetService.GetStat()} is created.");
                }
                catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException
                    || ex is FormatException
                    || ex is OverflowException)
                {
                    Console.WriteLine("Invalid input.");
                    isValidInput = false;
                }
            }
            while (isValidInput != true);
        }

        private static void List(string parameters)
        {
            string record;
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
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
            bool isValidInput = int.TryParse(parameters, out int id);

            if (!isValidInput)
            {
                Console.WriteLine("The entered parameter is not a number.");
                return;
            }

            if ((id < 1) || (id > fileCabinetService.GetStat()))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            do
            {
                isValidInput = true;

                try
                {
                    recordParameters = ReadAndParseParams();
                    fileCabinetService.EditRecord(id, recordParameters);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException
                    || ex is FormatException
                    || ex is OverflowException)
                {
                    Console.WriteLine("Invalid input.");
                    isValidInput = false;
                }
            }
            while (isValidInput != true);
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

            FileCabinetRecord[] records = Array.Empty<FileCabinetRecord>();
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

            for (int i = 0; i < records.Length; i++)
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

        private static RecordParameters ReadAndParseParams()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            DateTime dateOfBirth = DateTime.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Gender: ");
            char gender = char.Parse(Console.ReadLine());
            Console.Write("Experience: ");
            short experience = short.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            RecordParameters recordParameters = new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);
            return recordParameters;
        }
    }
}