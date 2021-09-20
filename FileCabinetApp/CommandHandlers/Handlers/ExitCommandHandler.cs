using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for exit command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string Command = "EXIT";
        private Action<bool> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="action">Delegate which will be used for exiting the application.</param>
        public ExitCommandHandler(Action<bool> action)
        {
            this.action = action;
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
