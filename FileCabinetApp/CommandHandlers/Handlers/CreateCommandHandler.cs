using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for create command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "CREATE";
        private readonly IInputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// create command handler calls service methods.</param>
        /// <param name="inputValidator">A validator which will be used for input validation.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService, IInputValidator inputValidator)
            : base(fileCabinetService)
        {
            this.inputValidator = inputValidator;
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
            InputConverter converter = new InputConverter();

            RecordParameters recordParameters = inputHandler.ReadRecordParameters(this.inputValidator, converter);

            try
            {
                this.FileCabinetService.CreateRecord(recordParameters);
                Console.WriteLine($"Record #{this.FileCabinetService.GetStat().Item2} is created.");
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
