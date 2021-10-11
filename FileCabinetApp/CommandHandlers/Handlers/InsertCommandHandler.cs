using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for insert command.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "INSERT";
        private readonly IInputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">A reference to service class is needed because
        /// create command handler calls service methods.</param>
        /// <param name="inputValidator">A validator which will be used for input validation.</param>
        public InsertCommandHandler(IFileCabinetService service, IInputValidator inputValidator)
            : base(service)
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
                this.Insert(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Insert(string parameters)
        {
            InputHandler inputHandler = new InputHandler();
            InputConverter converter = new InputConverter();

            inputHandler.TryReadInsertCommandParameters(parameters, out var fieldsValues);
            string[] fields = fieldsValues.Item1;
            string[] values = fieldsValues.Item2;

            var record = inputHandler.ReadRecordParameters(values, fields, this.inputValidator, converter);

            try
            {
                this.FileCabinetService.Insert(record);
                Console.WriteLine($"Record #{record.Id} is inserted.");
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
