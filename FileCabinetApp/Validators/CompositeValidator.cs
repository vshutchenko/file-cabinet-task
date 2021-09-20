using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public abstract class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = new List<IRecordValidator>(validators);
        }

        public void ValidateParameters(RecordParameters recordParameters)
        {
            foreach (var validator in validators)
            {
                validator.ValidateParameters(recordParameters);
            }
        }
    }
}
