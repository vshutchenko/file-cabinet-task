using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

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
