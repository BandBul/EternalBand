using System.ComponentModel.DataAnnotations;

namespace EternalBAND.Models;

public class Posts
{
    public int Id { get; set; }
    [Display(Name = "Başlık")]
    public string Title { get; set; }
    [Display(Name = "Açıklama")]
    public string HTMLText { get; set; }
    [Display(Name = "İlan Tipi")]
    public PostTypes? PostTypes { get; set; }
    [Display(Name = "İlan Tipi")]
    public int PostTypesId { get; set; }
    [Display(Name = "Enstrüman")]
    public Instruments? Instruments { get; set; }
    [Display(Name = "Enstrüman")]
    public int? InstrumentsId { get; set; }
    [Display(Name = "Fotoğraf 1")]
    public string? Photo1 { get; set; }
    [Display(Name = "Fotoğraf 2")]
    public string? Photo2 { get; set; } [Display(Name = "Fotoğraf 3")]
    public string? Photo3 { get; set; } [Display(Name = "Fotoğraf 4")]
    public string? Photo4 { get; set; } [Display(Name = "Fotoğraf 5")]
    public string? Photo5 { get; set; }
    [Display(Name = "Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }
    [Display(Name = "Ekleyen Kullanıcı ")]
    public Users? AddedByUser { get; set; }
    
    public string? AddedByUserId { get; set; }
    [Display(Name = "Yönetici Kabul Tarihi ")]
    public DateTime? AdminConfirmationDate { get; set; }
    public bool AdminConfirmation { get; set; } = false;
    [Display(Name = "Kabul Eden Yönetici")]
    public Users? AdminConfirmationUser { get; set; }
    public string? AdminConfirmationUserId { get; set; }
    public string? SeoLink { get; set; }
    public Guid? Guid { get; set; }
    [Display(Name = "Şehir")]
    public int CityId { get; set; }
}