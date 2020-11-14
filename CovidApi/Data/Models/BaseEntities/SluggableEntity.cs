using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CovidApi.Models.BaseEntities
{
    public abstract class SluggableEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SlugId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
    }
}
