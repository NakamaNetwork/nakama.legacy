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
    
    public partial class StageAlias
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public System.DateTimeOffset EditedDate { get; set; }
    
        public virtual Stage Stage { get; set; }
    }
}
