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
    
    public partial class TeamUnit
    {
        public int TeamId { get; set; }
        public int UnitId { get; set; }
        public byte Position { get; set; }
        public Nullable<byte> SpecialLevel { get; set; }
        public bool Sub { get; set; }
    
        public virtual Unit Unit { get; set; }
        public virtual Team Team { get; set; }
    }
}
