using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeLifter.Covid19.Data.Models.BaseEntities
{
    public abstract class SluggableEntity : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SlugId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
    }
}
