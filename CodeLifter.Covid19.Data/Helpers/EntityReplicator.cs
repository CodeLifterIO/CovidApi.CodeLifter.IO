using CodeLifter.Covid19.Data.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeLifter.Covid19.Data.Helpers
{
    /// <summary>
    /// TODO - Complete this in HELPER
    /// </summary>
    public static class EntityReplicator
    {
        //public static Entity ShallowCopy(Entity entity, Entity sourceEntity)
        //{
        //    return entity.ShallowCopy(sourceEntity);
        //}

        //public static Entity ShallowCopy(Entity entity, bool copyId = false)
        //{
        //    foreach (PropertyInfo property in entity.GetType().GetProperties())
        //    {
        //        if (property.CanWrite)
        //        {
        //            if (property.Name != "Id" || copyId == true)
        //            {
        //                property.SetValue(this, property.GetValue(entity, null), null);
        //            }
        //        }
        //    }
        //    return this;
        //}
    }
}
