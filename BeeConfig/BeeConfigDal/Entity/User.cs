using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeConfigDal.Entity
{
    [Table("UserTB")]
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
       public  string UserId { get; set; }
        [StringLength(200)]
        public string Pwd { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Status { get; set; }
    }
}
