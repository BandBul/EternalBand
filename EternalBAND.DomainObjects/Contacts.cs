using System.ComponentModel.DataAnnotations;

namespace EternalBAND.DomainObjects;

public class Contacts : IEntity
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Mail adresi boş bırakılamaz.")]
    [Display(Name = "Mail Adresi")]
    public string Mail { get; set; }
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "Ad Soyad boş bırakılamaz.")]
    [Display(Name = "Ad Soyad")]
    public string NameSurname { get; set; }
    [Required(ErrorMessage = "Başlık boş bırakılamaz.")]
    [Display(Name = "Başlık")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Mesaj boş bırakılamaz.")]
    [Display(Name = "Mesaj")]
    public string Message { get; set; }
    [Display(Name = "Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }

    [Display(Name = "İşlem Yapıldı")] 
    public bool? IsDone { get; set; } = false;

}