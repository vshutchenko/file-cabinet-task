using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for stat command.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "STAT";

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// stat command handler calls service methods.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
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
                this.Stat(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.Item2} record(s). Deleted records: {recordsCount.Item1}.");
        }
    }
}
