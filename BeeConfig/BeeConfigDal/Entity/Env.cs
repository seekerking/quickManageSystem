using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeConfigDal.Entity
{
    [Table("EnvTB")]
    public class Env
    {
       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string EnvId { get; set; }
        [StringLength(50)]
        public string EnvDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Status { get; set; }
    }
}
