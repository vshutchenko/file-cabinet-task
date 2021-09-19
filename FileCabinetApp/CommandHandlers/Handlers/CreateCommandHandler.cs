using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.InputHandlers;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "CREATE";

        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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
                this.Create(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Create(string parameters)
        {
            InputValidator validator = new InputValidator(fileCabinetService);
            RecordParameters recordParameters;

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
                this.fileCabinetService.CreateRecord(recordParameters);
                Console.WriteLine($"Record #{this.fileCabinetService.GetStat().Item2} is created.");
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
