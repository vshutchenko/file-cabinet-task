using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "SELECT";

        public SelectCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

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
                this.Select(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Select(string parameters)
        {
            InputHandler inputHandler = new InputHandler();
            if (parameters == string.Empty)
            {
                var records = this.FileCabinetService.GetRecords();
                var properties = typeof(FileCabinetRecord).GetProperties();
                string[] propertiesToprint = new string[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    propertiesToprint[i] = properties[i].Name;
                }

                TablePrinter printer = new TablePrinter(records, propertiesToprint);
                printer.PrintTable();
            }
            else if (inputHandler.TryReadSelectCommandParameters(parameters, out Tuple<string[], string[]> propertyValueToSearch, out string[] propertiesToprint, out bool allFieldMatch))
            {
                var records = this.FileCabinetService.Select(propertyValueToSearch.Item1, propertyValueToSearch.Item2, allFieldMatch);
                TablePrinter printer = new TablePrinter(records, propertiesToprint);
                printer.PrintTable();
            }
        }
    }
}
