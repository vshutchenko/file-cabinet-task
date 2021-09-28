using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for help command.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const string Command = "HELP";

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "edit", "edits the record with specified id", "The 'edit' command edits the record with specified id." },
            new string[] { "export", "exports stored records in the CSV or XML file", "The 'export' command exports stored records in the CSV or XML file." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "delete", "deletes a record with specified parameters", "The 'delete' command deletes a record with specified parameters." },
            new string[] { "find", "finds records, recieves name of property and text to search", "The 'find' command finds records, recieves name of property and text to search." },
            new string[] { "import", "imports stored records in the CSV or XML file", "The 'import' command imports stored records in the CSV or XML file." },
            new string[] { "insert", "creates a new record with specified parameters", "The 'insert' command creates a new record with specified parameters." },
            new string[] { "list", "prints the list of records", "The 'list' command prints the list of records." },
            new string[] { "purge", "defragments the file", "The 'purge' command defragments the file." },
            new string[] { "remove", "removes the record with specified id", "The 'remove' command removes the record with specified id." },
            new string[] { "stat", "prints number of records stored in the service", "The 'stat' command prints number of records stored in the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>
        /// Handles the command or calls next command handler.
        /// </summary>
        /// <param name="request">A command with parameters.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.PrintHelp(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                int index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
