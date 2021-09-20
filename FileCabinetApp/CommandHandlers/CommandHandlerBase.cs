using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents a base class for command handlers.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>
        /// Calls next command handler in the chain of handlers.
        /// </summary>
        /// <param name="request">A command with parameters.</param>
        public virtual void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                return;
            }

            if (this.nextHandler != null)
            {
                this.nextHandler.Handle(request);
            }
            else
            {
                this.PrintMissedCommandInfo(request.Command);
            }
        }

        /// <summary>
        /// Sets next command handler in the chain of handlers.
        /// </summary>
        /// <param name="handler">Next handler in the chain.</param>
        /// <returns>Next command handler.</returns>
        public ICommandHandler SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
            return this.nextHandler;
        }

        private void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
