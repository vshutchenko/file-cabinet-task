using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string Command = "EXIT";
        private Action<bool> action;

        public ExitCommandHandler(Action<bool> action)
        {
            this.action = action;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Exit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.action(false);
        }
    }
}
