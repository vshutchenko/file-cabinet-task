using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// A base class for command handlers classes.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// command handlers may need to call service methods.</param>
        internal ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.FileCabinetService = fileCabinetService;
        }

        /// <summary>
        /// Gets reference to service.
        /// </summary>
        /// <value>Reference to service.</value>
        protected IFileCabinetService FileCabinetService { get; private set; }
    }
}
