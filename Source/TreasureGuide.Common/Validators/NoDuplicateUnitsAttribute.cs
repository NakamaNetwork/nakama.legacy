using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TreasureGuide.Common.Models.TeamModels;

namespace TreasureGuide.Common.Validators
{
    /// <summary>
    /// {0} = Offending Slot
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class NoDuplicateUnitsAttribute : ValidationAttribute
    {
        private int _offending;

        public NoDuplicateUnitsAttribute() : base("There are two identical units in slot {0}.")
        {
        }

        public override bool IsValid(object value)
        {
            var team = value as IEnumerable<TeamUnitEditorModel>;
            if (team == null)
            {
                return true;
            }
            var groups = team.GroupBy(x => new { x.Position, x.UnitId, x.Support });
            var offending = groups.FirstOrDefault(x => x.Count() > 1);
            if (offending != null)
            {
                _offending = offending.Key.Position + 1;
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, _offending);
        }
    }
}
