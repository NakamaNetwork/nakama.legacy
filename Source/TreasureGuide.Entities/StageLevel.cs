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
    
    public partial class StageLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StageLevel()
        {
            this.StageUnits = new HashSet<StageUnit>();
        }
    
        public int Id { get; set; }
        public int StageDifficultyId { get; set; }
        public byte Number { get; set; }
        public bool Secret { get; set; }
    
        public virtual StageDifficulty StageDifficulty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StageUnit> StageUnits { get; set; }
    }
}
