using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TreasureGuide.Web.Models.TeamModels;

namespace TreasureGuide.Web.Helpers.Validators
{
    /// <summary>
    /// {0} = Maximum, {1} = Offending Slot
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NotTooManyUnitsAttribute : ValidationAttribute
    {
        private int _maximum;
        private int _offending;

        public NotTooManyUnitsAttribute(int maximum) : base("There are too many units in slot {1}. The maximum is {0}.")
        {
            _maximum = maximum;
        }


        public override bool IsValid(object value)
        {
            var team = value as IEnumerable<TeamUnitEditorModel>;
            if (team == null)
            {
                return true;
            }
            var groups = team.GroupBy(x => x.Position);
            var offending = groups.FirstOrDefault(x => x.Count() > _maximum);
            if (offending != null)
            {
                _offending = offending.Key + 1;
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, _maximum, _offending);
        }
    }
}
