using System;
namespace CodeLifter.Covid19.Data.Interfaces
{
    public interface INamedEntity
    {
        string Name { get; set; }
        string Slug { get; set; }
    }
}
