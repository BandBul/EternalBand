using EternalBAND.Common;
using EternalBAND.DomainObjects.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;

namespace EternalBAND.DomainObjects;

public class Posts : IEntity
{
    public int Id { get; set; }

    [Display(Name = "Başlık")]
    [Required(ErrorMessage = "Başlık boş bırakılamaz.")]
    public string? Title { get; set; }

    [Display(Name = "Açıklama")]
    [Required(ErrorMessage = "Açıklama boş bırakılamaz.")]
    public string? HTMLText { get; set; }

    [Display(Name = "İlan Tipi")]
    public PostTypes? PostTypes { get; set; }

    [Display(Name = "İlan Tipi")]
    [Required(ErrorMessage = "Lütfen İlan Tipi seçiniz.")]
    public int? PostTypesId { get; set; }

    [Display(Name = "Enstrüman")]
    public Instruments? Instruments { get; set; }

    [Display(Name = "Enstrüman")]
    [Required(ErrorMessage = $"Lütfen Enstrüman seçiniz.")]
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
    [Required(ErrorMessage = "Lütfen Şehir seçiniz.")]
    public int? CityId { get; set; }
    public PostStatus Status { get; set; }

    public virtual IList<MessageBox>? MessageBoxes { get; set; }

    public List<string> AllPhotos =>
        new List<string>()
        {
            Photo1,
            Photo2,
            Photo3,
            Photo4,
            Photo5,
        };

    public string? GetTopPhoto()
    {
        return AllPhotos.FirstOrDefault(photo => photo != null);
    }

    public void SetPhoto(List<string> photos)
    {
        Photo1 = photos.Count >= 1 ? photos[0] : null;
        Photo2 = photos.Count >= 2 ? photos[1] : null;
        Photo3 = photos.Count >= 3 ? photos[2] : null;
        Photo4 = photos.Count >= 4 ? photos[3] : null;
        Photo5 = photos.Count >= 5 ? photos[4] : null;
    }
}