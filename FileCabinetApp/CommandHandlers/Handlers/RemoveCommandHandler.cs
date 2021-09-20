using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for remove command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "REMOVE";

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// remove command handler calls service methods.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
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
                bool isDeleted = this.FileCabinetService.Remove(id);
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
