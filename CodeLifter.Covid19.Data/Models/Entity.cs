using System.Reflection;
using Newtonsoft.Json;

namespace CodeLifter.Covid19.Data.Models
{
    public abstract class Entity
    {
        [JsonIgnore]
        public int Id { get; set; }
        public static void Insert(Entity entity)
        {
            if (entity.Id != 0)
            {
                Update(entity);
                return;
            }

            using (var context = new CovidContext())
            {
                context.Add(entity);
                context.SaveChanges();
            }
        }

        public static void Update(Entity entity)
        {
            if(entity.Id == 0)
            {
                Insert(entity);
                return;
            }

            using (var context = new CovidContext())
            {
                context.Update(entity);
                context.SaveChanges();
            }
        }

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
                    if(property.Name != "Id" || copyId == true)
                    {
                        property.SetValue(this, property.GetValue(entity, null), null);
                    }
                }
            }
            return this;
        }
    }
}