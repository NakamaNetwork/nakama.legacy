//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TreasureGuide.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScheduledEvent
    {
        public int StageId { get; set; }
        public bool Global { get; set; }
        public System.DateTimeOffset StartDate { get; set; }
        public System.DateTimeOffset EndDate { get; set; }
        public bool Source { get; set; }
    
        public virtual Stage Stage { get; set; }
    }
}
