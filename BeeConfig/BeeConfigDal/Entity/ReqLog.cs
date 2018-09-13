using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeeConfigDal.Entity
{
    [Table("ReqLogsTB")]
    public class ReqLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string ClientIp { get; set; }
        [StringLength(50)]
        public string AppId { get; set; }
        [StringLength(50)]
        public string AppEnv { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int ReqTimes { get; set; }
        public DateTime LastConfigDate { get; set; }
    }
}
