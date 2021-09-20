using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Provides methods for introducing of Chain of Responsibility pattern for
    /// command handlers.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next command handler in the chain of handlers.
        /// </summary>
        /// <param name="commandHandler">Next handler in the chain.</param>
        /// <returns>Next command handler.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Calls next command handler in the chain of handlers.
        /// </summary>
        /// <param name="request">A command with parameters.</param>
        public void Handle(AppCommandRequest request);
    }
}
