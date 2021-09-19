using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.Handlers;

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

        public static bool IsCustomRulesEnabled { get; private set; }

        public static bool IsFileSystemServiceEnabled { get; private set; }

        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// This method runs an instance of FileCabinetMemoryService and processes console commands.
        /// </summary>
        /// <param name="args">Console command parameters.</param>
        public static void Main(string[] args)
        {
            ReadCommandLineParameters(args);
            string validationRulesHint = IsCustomRulesEnabled ? "Using custom validation rules." : "Using default validation rules.";
            IRecordValidator validator;
            FileStream fileStream = null;

            if (IsCustomRulesEnabled)
            {
                validationRulesHint = "Using custom validation rules.";
                validator = new CustomValidator();
            }
            else
            {
                validationRulesHint = "Using default validation rules.";
                validator = new DefaultValidator();
            }

            if (IsFileSystemServiceEnabled)
            {
                fileStream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFilesystemService(fileStream);
            }
            else
            {
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

        private static ICommandHandler CreateCommandHandler()
        {
            static void Exit(bool b) => isRunning = b;
            var recordPrinter = new DefaultRecordPrinter();

            var helpCommandHandler = new HelpCommandHandler();
            var createCommandHandler = new CreateCommandHandler(fileCabinetService);
            var editCommandHandler = new EditCommandHandler(fileCabinetService);
            var exitCommandHandler = new ExitCommandHandler(Exit);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var findCommandHandler = new FindCommandHandler(fileCabinetService, recordPrinter);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var listCommandHandler = new ListCommandHandler(fileCabinetService, recordPrinter);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            var removeCommandHandler = new RemoveCommandHandler(fileCabinetService);
            var statCommandHandler = new StatCommandHandler(fileCabinetService);

            helpCommandHandler.SetNext(createCommandHandler).SetNext(createCommandHandler).SetNext(editCommandHandler).
                SetNext(exitCommandHandler).SetNext(exportCommandHandler).SetNext(findCommandHandler).
                SetNext(importCommandHandler).SetNext(listCommandHandler).SetNext(purgeCommandHandler).
                SetNext(removeCommandHandler).SetNext(statCommandHandler);

            return helpCommandHandler;
        }

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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
                                IsCustomRulesEnabled = true;
                            }
                            else if (parameters[i + 1] == "DEFAULT")
                            {
                                IsCustomRulesEnabled = false;
                            }
                        }

                        break;
                    case "-S":
                        if (i + 1 < parameters.Length)
                        {
                            if (parameters[i + 1] == "FILE")
                            {
                                IsFileSystemServiceEnabled = true;
                            }
                            else if (parameters[i + 1] == "MEMORY")
                            {
                                IsCustomRulesEnabled = false;
                            }
                        }

                        break;
                    case "--VALIDATION-RULES=DEFAULT":
                        IsCustomRulesEnabled = false;
                        break;
                    case "--VALIDATION-RULES=CUSTOM":
                        IsCustomRulesEnabled = true;
                        break;
                    case "--STORAGE=MEMORY":
                        IsFileSystemServiceEnabled = false;
                        break;
                    case "--STORAGE=FILE":
                        IsFileSystemServiceEnabled = true;
                        break;
                }
            }
        }
    }
}
