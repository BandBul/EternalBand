using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    public List<string> AllPhotos =>
    new List<string>()
    {
            PhotoPath,
            PhotoPath2,
            PhotoPath3,
            PhotoPath4,
            PhotoPath5,
    };

    public string? GetTopPhoto()
    {
        return AllPhotos.FirstOrDefault(photo => photo != null);
    }
}