using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "LIST";
        private Action<IEnumerable<FileCabinetRecord>> print;

        public ListCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> print)
            : base(fileCabinetService)
        {
            this.print = print;
        }

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
            ReadOnlyCollection<FileCabinetRecord> records = this.fileCabinetService.GetRecords();
            this.print(records);
        }
    }
}
