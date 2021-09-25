using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for find command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "FIND";
        private Action<IEnumerable<FileCabinetRecord>> print;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// find command handler calls service methods.</param>
        /// <param name="print">Delegate which contains a method for printing records.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> print)
            : base(fileCabinetService)
        {
            this.print = print;
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
                this.Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Find(string parameters)
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

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            IRecordIterator iterator = null;

            if (string.Equals(propertyName, nameof(FileCabinetRecord.FirstName), StringComparison.InvariantCultureIgnoreCase))
            {
                iterator = this.FileCabinetService.FindByFirstName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.LastName), StringComparison.InvariantCultureIgnoreCase))
            {
                iterator = this.FileCabinetService.FindByLastName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.DateOfBirth), StringComparison.InvariantCultureIgnoreCase))
            {
                if (DateTime.TryParse(textToSearch, out DateTime dateOfBirth))
                {
                    iterator = this.FileCabinetService.FindByDateOfBirth(dateOfBirth);
                }
            }

            if (iterator != null)
            {
                while (iterator.HasMore())
                {
                    records.Add(iterator.GetNext());
                }
            }

            this.print(records);
        }
    }
}
