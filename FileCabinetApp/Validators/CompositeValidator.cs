using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.RecordModel;

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
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(recordParameters);
            }
        }
    }
}
