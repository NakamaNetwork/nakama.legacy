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
    
    public partial class TeamSocket
    {
        public int TeamId { get; set; }
        public SocketType SocketType { get; set; }
        public byte Level { get; set; }
    
        public virtual Team Team { get; set; }
    }
}
