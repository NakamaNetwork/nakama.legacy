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
    
    public partial class TeamVideo
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string VideoLink { get; set; }
        public bool Deleted { get; set; }
        public System.DateTimeOffset SubmittedDate { get; set; }
        public string UserId { get; set; }
    
        public virtual Team Team { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
