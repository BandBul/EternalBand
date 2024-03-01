using EternalBAND.Common;
using System.ComponentModel;

namespace EternalBAND.DomainObjects;

public class PostTypes : IEntity
{
    public int? Id { get; set; }
    [DisplayName("Tip")]
    public string SearchName { get; set; }
    [DisplayName("Tip Kısa")]
    public string? Type { get; set; }
    [DisplayName("Aktif")]
    public bool Active { get; set; }
    [DisplayName("Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }
    public PostTypeName TargetGroup { get; set; }
    public string TargetGroupName { get; set; }
}