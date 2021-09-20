using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
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
