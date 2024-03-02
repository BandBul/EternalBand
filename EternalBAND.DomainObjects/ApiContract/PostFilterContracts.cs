using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EternalBAND.DomainObjects;

public class PostFilterContracts
{
    public int PageID { get; set; } = 1;
    public string Type { get; set; } ="";
    public int CityId { get; set; } = 0;
    public string Instrument { get; set; } = "";
}