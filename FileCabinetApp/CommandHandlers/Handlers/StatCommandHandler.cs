using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "STAT";

        public StatCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Stat(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.Item2} record(s). Deleted records: {recordsCount.Item1}.");
        }
    }
}
