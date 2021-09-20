using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.Handlers;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// This class contains information about available commands and processes console commands.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shutchenko";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static bool isRunning = true;
        private static bool isCustomRulesEnabled;
        private static bool isFileSystemServiceEnabled;
        private static IInputValidator inputValidator;

        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// This method runs an instance of FileCabinetMemoryService and processes console commands.
        /// </summary>
        /// <param name="args">Console command parameters.</param>
        public static void Main(string[] args)
        {
            HandleCommandLineParameters(args);
            string validationRulesHint;
            IRecordValidator validator;
            FileStream fileStream = null;

            if (isCustomRulesEnabled)
            {
                validationRulesHint = "Using custom validation rules.";
                inputValidator = new CustomInputValidator();
                validator = new ValidatorBuilder().CreateCustom();
            }
            else
            {
                validationRulesHint = "Using default validation rules.";
                inputValidator = new DefaultInputValidator();
                validator = new ValidatorBuilder().CreateDefault();
            }

            if (isFileSystemServiceEnabled)
            {
                validationRulesHint += $"{Environment.NewLine}Using file storage.";
                fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFilesystemService(fileStream, validator);
            }
            else
            {
                validationRulesHint += $"{Environment.NewLine}Using memory storage.";
                fileCabinetService = new FileCabinetMemoryService(validator);
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
                const int parametersIndex = 1;

                var command = inputs[commandIndex];
                string parameters;
                if (inputs.Length != 2)
                {
                    parameters = string.Empty;
                }
                else
                {
                    parameters = inputs[parametersIndex];
                }

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var commandHandler = CreateCommandHandler();
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Prints collection of records.
        /// </summary>
        /// <param name="records">Records to print.</param>
        public static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            string recordString;
            foreach (var record in records)
            {
                recordString = $"#{record.Id}, " +
                    $"{record.FirstName}, " +
                    $"{record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                    $"{record.Gender}, " +
                    $"{record.Experience}, " +
                    $"{record.Salary}$";
                Console.WriteLine(recordString);
            }
        }

        private static ICommandHandler CreateCommandHandler()
        {
            static void Exit(bool b) => isRunning = b;

            var helpCommandHandler = new HelpCommandHandler();
            var createCommandHandler = new CreateCommandHandler(fileCabinetService, inputValidator);
            var editCommandHandler = new EditCommandHandler(fileCabinetService, inputValidator);
            var exitCommandHandler = new ExitCommandHandler(Exit);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var findCommandHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var listCommandHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            var removeCommandHandler = new RemoveCommandHandler(fileCabinetService);
            var statCommandHandler = new StatCommandHandler(fileCabinetService);

            helpCommandHandler.SetNext(createCommandHandler).SetNext(createCommandHandler).SetNext(editCommandHandler).
                SetNext(exitCommandHandler).SetNext(exportCommandHandler).SetNext(findCommandHandler).
                SetNext(importCommandHandler).SetNext(listCommandHandler).SetNext(purgeCommandHandler).
                SetNext(removeCommandHandler).SetNext(statCommandHandler);

            return helpCommandHandler;
        }

        private static void HandleCommandLineParameters(string[] arguments)
        {
            if (arguments is null)
            {
                return;
            }

            InputHandler inputHandler = new InputHandler();
            Dictionary<string, string> parameters = inputHandler.ReadCommandLineParameters(arguments);
            string validationRules;
            string storage;

            if (parameters.TryGetValue("-V", out validationRules) || parameters.TryGetValue("--VALIDATION-RULES", out validationRules))
            {
                if (validationRules.Equals("DEFAULT", StringComparison.InvariantCultureIgnoreCase))
                {
                    isCustomRulesEnabled = false;
                }
                else if (validationRules.Equals("CUSTOM", StringComparison.InvariantCultureIgnoreCase))
                {
                    isCustomRulesEnabled = true;
                }
            }

            if (parameters.TryGetValue("-S", out storage) || parameters.TryGetValue("--STORAGE", out storage))
            {
                if (storage.Equals("MEMORY", StringComparison.InvariantCultureIgnoreCase))
                {
                    isFileSystemServiceEnabled = false;
                }
                else if (storage.Equals("FILE", StringComparison.InvariantCultureIgnoreCase))
                {
                    isFileSystemServiceEnabled = true;
                }
            }
        }
    }
}
