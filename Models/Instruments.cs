using System.ComponentModel.DataAnnotations;

namespace EternalBAND.Models;

public class Instruments
{
    public int Id { get; set; }
    [Display(Name = "Enstrüman")]
    public string Instrument { get; set; }
    [Display(Name = "Enstrüman Kısa ")]
    public string? InstrumentShort { get; set; }
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}