﻿using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "EDIT";
        private IInputValidator inputValidator;

        public EditCommandHandler(IFileCabinetService fileCabinetService, IInputValidator inputValidator)
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
                this.Edit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Edit(string parameters)
        {
            InputHandler inputHandler = new InputHandler();
            InputConverter converter = new InputConverter();
            Tuple<bool, string, int> parametersConversionResult = converter.IntConverter(parameters);
            int id;
            if (!parametersConversionResult.Item1)
            {
                Console.WriteLine(parametersConversionResult.Item2);
                return;
            }
            else
            {
                id = parametersConversionResult.Item3;
            }

            RecordParameters recordParameters = inputHandler.ReadRecordParameters(this.inputValidator);

            try
            {
                this.fileCabinetService.EditRecord(id, recordParameters);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (Exception ex) when (
                    ex is ArgumentException
                    || ex is ArgumentNullException
                    || ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}
