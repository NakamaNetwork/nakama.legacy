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
    
    public partial class StageUnitAttackPattern
    {
        public int Id { get; set; }
        public int StageUnitId { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
    
        public virtual StageUnit StageUnit { get; set; }
    }
}
