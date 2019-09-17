using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TreasureGuide.Common.Models.TeamModels;

namespace TreasureGuide.Common.Validators
{
    /// <summary>
    /// {0} = Minimum
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class EnoughNonSubsAttribute : ValidationAttribute
    {
        private readonly int _minimum;

        public EnoughNonSubsAttribute(int minimum) : base("You must specify at least {0} non-sub units.")
        {
            _minimum = minimum;
        }

        public override bool IsValid(object value)
        {
            var team = value as IEnumerable<TeamUnitEditorModel>;
            return team?.Count(x => !x.Sub && !x.Support) >= _minimum;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, _minimum);
        }
    }
}
