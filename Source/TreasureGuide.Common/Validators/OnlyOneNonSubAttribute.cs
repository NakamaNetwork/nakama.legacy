using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NakamaNetwork.Entities.Interfaces;

namespace TreasureGuide.Common.Validators
{
    /// <summary>
    /// {0} = Offending Slot
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class OnlyOneNonSubAttribute : ValidationAttribute
    {
        private int _offending;

        public OnlyOneNonSubAttribute() : base("There are non-sub units in slot {0}.")
        {
        }

        public override bool IsValid(object value)
        {
            var team = value as IEnumerable<ISubItem>;
            if (team == null)
            {
                return true;
            }
            var groups = team.Where(x => !x.Sub).GroupBy(x => x.Position);
            var offending = groups.FirstOrDefault(x => x.Count() > 1);
            if (offending != null)
            {
                _offending = offending.Key + 1;
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
