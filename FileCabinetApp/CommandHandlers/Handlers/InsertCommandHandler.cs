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
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "INSERT";
        private readonly IInputValidator inputValidator;

        public InsertCommandHandler(IFileCabinetService service, IInputValidator inputValidator)
            : base(service)
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

            var record = inputHandler.ReadRecordParameters(values, fields, inputValidator, converter);

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
