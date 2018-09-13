using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeeConfigDal.Entity
{
    [Table("ConfigTB")]
   public class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string ConfigId { get; set; }
        [StringLength(2000)]
        public string ConfigValue { get; set; }
        [StringLength(50)]
        public string EnvId { get; set; }
        [StringLength(50)]
        public string AppId { get; set; }
        [StringLength(200)]
        public string ConfigDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Status { get; set; }
        public long LastTimespan { get; set; }
    }
}
