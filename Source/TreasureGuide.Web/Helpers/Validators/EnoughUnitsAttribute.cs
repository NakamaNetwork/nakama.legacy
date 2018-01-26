using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TreasureGuide.Web.Models.TeamModels;

namespace TreasureGuide.Web.Helpers.Validators
{
    /// <summary>
    /// {0} = Minimum
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EnoughUnitsAttribute : ValidationAttribute
    {
        private readonly int _minimum;

        public EnoughUnitsAttribute(int minimum) : base("You must specify at least {0} units.")
        {
            
            _minimum = minimum;
        }

        public override bool IsValid(object value)
        {
            var team = value as TeamEditorModel;
            return ((team?.TeamUnits?.Count() ?? 0) + (team?.TeamGenericSlots?.Count() ?? 0)) >= _minimum;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, _minimum);
        }
    }
}
