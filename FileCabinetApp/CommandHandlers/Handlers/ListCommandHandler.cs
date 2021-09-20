using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for list command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "LIST";
        private Action<IEnumerable<FileCabinetRecord>> print;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// list command handler calls service methods.</param>
        /// <param name="print">Delegate which contains a method for printing records.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> print)
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
                this.List(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.FileCabinetService.GetRecords();
            this.print(records);
        }
    }
}
