using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "PURGE";

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// purge command handler calls service methods.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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
                this.Purge(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Purge(string parameters)
        {
            Tuple<int, int> numberOfRecords = this.FileCabinetService.Purge();
            Console.WriteLine($"Data file processing is completed: {numberOfRecords.Item1} of {numberOfRecords.Item2} records were purged.");
        }
    }
}
