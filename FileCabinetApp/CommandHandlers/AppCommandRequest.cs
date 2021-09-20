using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// This class represents command line request to service.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">String representation of the command.</param>
        /// <param name="parameters">Command parameters separated with space.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets string representation of the command.
        /// </summary>
        /// <value>String representation of the command.</value>
        public string Command { get; }

        /// <summary>
        /// Gets string representation of parameters.
        /// </summary>
        /// <value>Command parameters separated with space.</value>
        public string Parameters { get; }
    }
}
