using System.ComponentModel;

namespace EternalBAND.DomainObjects;

public class PostTypes : IEntity
{
    public int Id { get; set; }
    [DisplayName("Tip")]
    public string Type { get; set; }
    [DisplayName("Tip Kısa")]
    public string? TypeShort { get; set; }
    [DisplayName("Aktif")]
    public bool Active { get; set; }
    [DisplayName("Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }
}