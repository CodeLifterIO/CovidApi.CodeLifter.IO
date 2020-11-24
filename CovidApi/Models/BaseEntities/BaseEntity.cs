using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CovidApi.Models.BaseEntities
{
    public interface IBaseEntity
    {   
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    } 
}
