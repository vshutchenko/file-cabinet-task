﻿using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "EDIT";

        public EditCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
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
                this.Edit(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Edit(string parameters)
        {
            RecordParameters recordParameters;
            InputValidator validator = new InputValidator(fileCabinetService);
            Tuple<bool, string, int> parametersConversionResult = InputConverter.IntConverter(parameters);
            Tuple<bool, string> recordIdValidationResult;
            int id;
            if (!parametersConversionResult.Item1)
            {
                Console.WriteLine(parametersConversionResult.Item2);
                return;
            }
            else
            {
                id = parametersConversionResult.Item3;
                recordIdValidationResult = validator.IdValidator(id);
                if (!recordIdValidationResult.Item1)
                {
                    Console.WriteLine(recordIdValidationResult.Item2);
                    return;
                }
            }

            Console.Write("First name: ");
            var firstName = Program.ReadInput(InputConverter.StringConverter, validator.FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = Program.ReadInput(InputConverter.StringConverter, validator.LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = Program.ReadInput(InputConverter.DateTimeConverter, validator.DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = Program.ReadInput(InputConverter.CharConverter, validator.GenderValidator);
            Console.Write("Experience: ");
            var experience = Program.ReadInput(InputConverter.ShortConverter, validator.ExperienceValidator);
            Console.Write("Salary: ");
            var salary = Program.ReadInput(InputConverter.DecimalConverter, validator.SalaryValidator);

            recordParameters = new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);

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
