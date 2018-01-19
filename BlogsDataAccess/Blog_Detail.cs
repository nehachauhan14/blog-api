//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlogsDataAccess
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class Blog_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Blog_Detail()
        {
            this.reaction_info = new HashSet<reaction_info>();
        }
    
        public int BID { get; set; }
        public string Title { get; set; }
        public string Blog_Content { get; set; }
        public System.DateTime DateOfUpdation { get; set; }
        public Nullable<int> UID { get; set; }
    
        [JsonIgnore]
        public virtual User_Details User_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<reaction_info> reaction_info { get; set; }
    }
}
