using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "REMOVE";

        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
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
                this.Remove(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Remove(string parameters)
        {
            if (parameters is null)
            {
                Console.WriteLine("Wrong parameters. Please, specify id of record to remove.");
            }

            if (int.TryParse(parameters, out int id))
            {
                bool isDeleted = this.fileCabinetService.Remove(id);
                if (isDeleted)
                {
                    Console.WriteLine($"Record #{id} is removed.");
                }
                else
                {
                    Console.WriteLine($"Record #{id} doesn't exists.");
                }
            }
            else
            {
                Console.WriteLine("Input parameter shoud be an integer value.");
            }
        }
    }
}
