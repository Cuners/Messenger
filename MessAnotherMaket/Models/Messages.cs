//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessAnotherMaket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Messages()
        {
            this.Vlozhenia = new HashSet<Vlozhenia>();
        }
    
        public int Id { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<System.DateTime> Deleted_at { get; set; }
        public Nullable<int> Razgovor_id { get; set; }
        public Nullable<int> Sender_id { get; set; }
        public Nullable<int> Type_id { get; set; }
        public Nullable<int> Receiver_id { get; set; }
    
        public virtual Razgovor Razgovor { get; set; }
        public virtual Type_messages Type_messages { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vlozhenia> Vlozhenia { get; set; }
    }
}
