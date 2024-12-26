using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace EternalBAND.DomainObjects;

public class Blogs : IEntity
{
    public int Id { get; set; }
    [DisplayName("Başlık")]
    [Required(ErrorMessage = "Başlık boş bırakılamaz.")]
    public string Title { get; set; }

    [DisplayName("Fotoğraf 1")]
    public string? PhotoPath { get; set; }

    [DisplayName("Fotoğraf 2")]
    public string? PhotoPath2 { get; set; }

    [DisplayName("Fotoğraf 3")]
    public string? PhotoPath3 { get; set; }

    [DisplayName("Fotoğraf 4")]
    public string? PhotoPath4 { get; set; }

    [DisplayName("Fotoğraf 5")]
    public string? PhotoPath5 { get; set; }

    [DisplayName("HTML Metin")]
    [Required(ErrorMessage = "Açıklama boş bırakılamaz.")]
    public string? HtmlText { get; set; }

    [DisplayName("Etiketler(virgül ile ayırın)")]
    public string? Tags { get; set; }

    [DisplayName("Link")]
    public string? SeoLink { get; set; }

    [DisplayName("Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }

    [DisplayName("Özet Metin")]
    [Required(ErrorMessage = "Özet metini boş bırakılamaz")]
    public string SummaryText { get; set; }

    public Guid? Guid { get; set; }
    public List<string> AllPhotos =>
    new List<string>()
    {
            PhotoPath,
            PhotoPath2,
            PhotoPath3,
            PhotoPath4,
            PhotoPath5,
    };

    public string PhotoFolder => $"{Guid}";

    public string? GetTopPhoto()
    {
        return AllPhotos.FirstOrDefault(photo => photo != null);
    }

    public string GetShortSummaryText(int size)
    {
        return SummaryText.Length > size ? SummaryText.Substring(0, size) : SummaryText;
    }

    public void SetPhoto(List<string> photos)
    {
        if(photos == null || !photos.Any())
        {
            return;
        }
        //TODO enhanced this, if count is 0 we are doing unnecessary 5 conditional check
        PhotoPath = photos.Count >= 1 ? photos[0] : null;
        PhotoPath2 = photos.Count >= 2 ? photos[1] : null;
        PhotoPath3 = photos.Count >= 3 ? photos[2] : null;
        PhotoPath4 = photos.Count >= 4 ? photos[3] : null;
        PhotoPath5 = photos.Count >= 5 ? photos[4] : null;
    }
}