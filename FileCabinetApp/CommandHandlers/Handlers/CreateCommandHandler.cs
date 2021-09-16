using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class CreateCommandHandler : CommandHandlerBase
    {
        private const string Command = "CREATE";

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
            RecordParameters recordParameters;

            Console.Write("First name: ");
            var firstName = Program.ReadInput(InputConverter.StringConverter, InputValidator.FirstNameValidator);
            Console.Write("Last name: ");
            var lastName = Program.ReadInput(InputConverter.StringConverter, InputValidator.LastNameValidator);
            Console.Write("Date of birth: ");
            var dateOfBirth = Program.ReadInput(InputConverter.DateTimeConverter, InputValidator.DateOfBirthValidator);
            Console.Write("Gender: ");
            var gender = Program.ReadInput(InputConverter.CharConverter, InputValidator.GenderValidator);
            Console.Write("Experience: ");
            var experience = Program.ReadInput(InputConverter.ShortConverter, InputValidator.ExperienceValidator);
            Console.Write("Salary: ");
            var salary = Program.ReadInput(InputConverter.DecimalConverter, InputValidator.SalaryValidator);

            recordParameters = new RecordParameters(firstName, lastName, dateOfBirth, gender, experience, salary);

            try
            {
                Program.fileCabinetService.CreateRecord(recordParameters);
                Console.WriteLine($"Record #{Program.fileCabinetService.GetStat()} is created.");
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
