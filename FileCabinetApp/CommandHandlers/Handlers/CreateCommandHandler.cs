using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "CREATE";
        private readonly IInputValidator inputValidator;

        public CreateCommandHandler(IFileCabinetService fileCabinetService, IInputValidator inputValidator)
            : base(fileCabinetService)
        {
            this.inputValidator = inputValidator;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Create(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Create(string parameters)
        {
            InputHandler inputHandler = new InputHandler();
            RecordParameters recordParameters = inputHandler.ReadRecordParameters(this.inputValidator);

            try
            {
                this.fileCabinetService.CreateRecord(recordParameters);
                Console.WriteLine($"Record #{this.fileCabinetService.GetStat().Item2} is created.");
            }
            catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
