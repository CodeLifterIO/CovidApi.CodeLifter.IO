using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CodeLifter.Covid19.Data.Models.BaseEntities
{
    public abstract class Entity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static Entity ShallowCopy(Entity entity, Entity sourceEntity)
        {
            return entity.ShallowCopy(sourceEntity);
        }

        public Entity ShallowCopy(Entity entity, bool copyId = false)
        {
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (property.CanWrite)
                {
                    if (property.Name != "Id" || copyId == true)
                    {
                        property.SetValue(this, property.GetValue(entity, null), null);
                    }
                }
            }
            return this;
        }
    } 
}