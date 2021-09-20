using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
#pragma warning disable SA1401 // Fields should be private
        protected IFileCabinetService fileCabinetService;
#pragma warning restore SA1401 // Fields should be private

        internal ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }
    }
}
