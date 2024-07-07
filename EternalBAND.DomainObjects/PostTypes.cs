using EternalBAND.Common;
using System.ComponentModel;

namespace EternalBAND.DomainObjects;

public class PostTypes : IEntity
{
    public int? Id { get; set; }
    [DisplayName("Tip")]
    public string FilterText { get; set; }
    [DisplayName("Tip")]
    public string? Type { get; set; }
    [DisplayName("Aktif")]
    public bool Active { get; set; }
    [DisplayName("Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }
    public string PostCreateText { get; set; }
    public string? TypeText { get; set; }
}