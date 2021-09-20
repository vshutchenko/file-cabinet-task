using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class implements custom parameters validation.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(1900, 1, 1), DateTime.Now),
                new GenderValidator(new[] { 'F', 'M', 'f', 'm' }),
                new ExperienceValidator(5, 40),
                new SalaryValidator(300, 1000),
            })
        {
        }
    }
}
