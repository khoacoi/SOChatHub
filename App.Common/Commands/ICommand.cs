using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Commands
{
    /// <summary>
    /// An domain command interface
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid();

        /// <summary>
        /// Validations the results.
        /// </summary>
        /// <returns></returns>
        ICollection<ValidationResult> ValidationResults();
    }
}
