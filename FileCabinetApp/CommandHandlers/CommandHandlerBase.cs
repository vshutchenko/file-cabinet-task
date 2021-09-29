using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents a base class for command handlers.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private readonly string[] commandsList = new string[] { "help", "export", "create", "delete", "find", "import", "insert", "list", "purge", "stat", "exit" };
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
                this.PrintSimilarCommands(request.Command);
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

        private void PrintSimilarCommands(string command)
        {
            List<string> similarCommands = new List<string>();

            foreach (var cmd in this.commandsList)
            {
                if (cmd.StartsWith(command, StringComparison.InvariantCultureIgnoreCase)
                    || cmd.Contains(command, StringComparison.InvariantCultureIgnoreCase))
                {
                    similarCommands.Add(cmd);
                }
            }

            if (similarCommands.Count == 0)
            {
                var commandChars = command.ToCharArray();
                Array.Sort(commandChars);

                foreach (var cmd in this.commandsList)
                {
                    var cmdChars = cmd.ToCharArray();
                    Array.Sort(cmdChars);

                    if (cmdChars.SequenceEqual(commandChars))
                    {
                        similarCommands.Add(cmd);
                    }
                }
            }

            if (similarCommands.Count != 0)
            {
                Console.WriteLine("The most similar commands are:");
                foreach (var cmd in similarCommands)
                {
                    Console.WriteLine($"\t{cmd}");
                }
            }
        }
    }
}
