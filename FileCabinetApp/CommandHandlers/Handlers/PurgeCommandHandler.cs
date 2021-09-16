﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class PurgeCommandHandler : CommandHandlerBase
    {
        private const string Command = "PURGE";

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
            Tuple<int, int> numberOfRecords = Program.fileCabinetService.Purge();
            Console.WriteLine($"Data file processing is completed: {numberOfRecords.Item1} of {numberOfRecords.Item2} records were purged.");
        }
    }
}
