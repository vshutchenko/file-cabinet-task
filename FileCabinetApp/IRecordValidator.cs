using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This interface provides a method for parameters validation.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// This method implements parameters validation.
        /// </summary>
        /// <param name="recordParameters">The parameter object for FileCabinetRecord.</param>
        public void ValidateParameters(RecordParameters recordParameters);
    }
}
