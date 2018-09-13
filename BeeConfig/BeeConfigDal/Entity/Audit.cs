using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeConfigDal.Entity
{

    [Table("AuditTB")]
   public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }
       [StringLength(50)]
      public string UserId{ get; set; }
        [StringLength(50)]
        public string AppId{ get; set; }
        [StringLength(50)]
        public string EnvId{ get; set; }
        [StringLength(50)]
        public string ConfigId{ get; set; }
        [StringLength(2000)]
        public string ConfigValue{ get; set; }
      public DateTime CreateDate{ get; set; }
    }
}
