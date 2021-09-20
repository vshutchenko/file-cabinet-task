using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class performs validation with collection of validators.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Collection of validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = new List<IRecordValidator>(validators);
        }

        /// <summary>
        /// Validates record parameters using collection of validators.
        /// </summary>
        /// <param name="recordParameters">A parameter object for <see cref="FileCabinetRecord"/> class.</param>
        public void ValidateParameters(RecordParameters recordParameters)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(recordParameters);
            }
        }
    }
}
