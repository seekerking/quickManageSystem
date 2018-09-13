using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeeConfigDal.Entity
{
    [Table("PublishTB")]
   public class Publish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string AppId { get; set; }

        public string EnvId { get; set; }

        public long PublishTimeSpan { get; set; }

        public string Data { get; set; }
    }
}
