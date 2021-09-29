using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "UPDATE";
        private readonly IInputValidator inputValidator;

        public UpdateCommandHandler(IFileCabinetService service, IInputValidator inputValidator)
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
                this.Update(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Update(string parameters)
        {
            InputHandler inputHandler = new InputHandler();
            InputConverter converter = new InputConverter();

            if (!inputHandler.TryReadUpdateCommandParameters(parameters, out var fieldsValues, out Tuple<string[], string[]> propertyValueToSearch, out bool allFieldsEnabled))
            {
                Console.WriteLine("Incorrect command syntax.");
                return;
            }

            string[] fields = fieldsValues.Item1;
            string[] newValues = fieldsValues.Item2;

            string[] fieldsToSearch = propertyValueToSearch.Item1;
            string[] valuesToSearch = propertyValueToSearch.Item2;

            try
            {
                this.FileCabinetService.Update(fieldsToSearch, fields, valuesToSearch, newValues, allFieldsEnabled);
                Console.WriteLine($"Records were updated.");
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
