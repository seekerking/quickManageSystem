using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeConfigDal.Entity
{
    [Table("AppTB")]
   public class App
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string AppId { get; set; }

        [StringLength(50)]
        public string AppName { get; set; }
        [StringLength(100)]
        public string AppDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid Secret { get; set; }

        public int Status { get; set; }
    }
}
