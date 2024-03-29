﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class provides extension methods for <see cref="ValidatorBuilder"></see> class.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Creates validator with default parameters.
        /// </summary>
        /// <param name="validator">Validator to build.</param>
        /// <returns>The validator with default parameters.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder validator)
        {
            return validator.
                    ValidateFirstName(2, 60).
                    ValidateLastName(2, 60).
                    ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now).
                    ValidateGender(new char[] { 'F', 'M' }).
                    ValidateExperience(0, 50).
                    ValidateSalary(1000, 10000).
                    Create();
        }

        /// <summary>
        /// Creates validator with custom parameters.
        /// </summary>
        /// <param name="validator">Validator to build.</param>
        /// <returns>The validator with custom parameters.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder validator)
        {
            return validator.
                    ValidateFirstName(2, 60).
                    ValidateLastName(2, 60).
                    ValidateDateOfBirth(new DateTime(1900, 1, 1), DateTime.Now).
                    ValidateGender(new char[] { 'F', 'M', 'f', 'm' }).
                    ValidateExperience(5, 40).
                    ValidateSalary(300, 1000).
                    Create();
        }
    }
}
