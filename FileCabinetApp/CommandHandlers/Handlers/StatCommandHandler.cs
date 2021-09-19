using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class StatCommandHandler : CommandHandlerBase
    {
        private const string Command = "STAT";
        private IFileCabinetService fileCabinetService;

        public StatCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
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
