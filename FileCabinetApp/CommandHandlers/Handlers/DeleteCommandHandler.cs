﻿using System;
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
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "DELETE";

        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Delete(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Delete(string parameters)
        {
            if (parameters is null)
            {
                Console.WriteLine("Wrong parameters.");
            }

            InputHandler inputHandler = new InputHandler();
            InputConverter converter = new InputConverter();

            if (!inputHandler.TryReadDeleteCommandParameters(parameters, out Tuple<string, string> propertyValue))
            {
                Console.WriteLine("Incorrect command parameters syntax. Please, correct your input.");
                return;
            }

            string name = propertyValue.Item1;
            string value = propertyValue.Item2;

            var deletedRecordsIds = this.FileCabinetService.Delete(name, value);
            string idsString = string.Empty;

            if (deletedRecordsIds.Count < 1)
            {
                Console.WriteLine("There is no records with such parameters.");
            }
            else
            {
                for (int i = 0; i < deletedRecordsIds.Count - 1; i++)
                {
                    idsString += $"#{deletedRecordsIds[i]}, ";
                }

                idsString += $"#{deletedRecordsIds[^1]}";

                Console.WriteLine($"Records {deletedRecordsIds} are deleted.");
            }
        }
    }
}
