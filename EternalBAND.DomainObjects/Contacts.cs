using System.ComponentModel.DataAnnotations;

namespace EternalBAND.DomainObjects;

public class Contacts : IEntity
{
    public int Id { get; set; }
    [Display(Name = "Mail Adresi")]
    public string Mail { get; set; }
    [Display(Name = "Telefon")]
    public string Phone { get; set; }
    [Display(Name = "Ad Soyad")]
    public string NameSurname { get; set; }
    [Display(Name = "Başlık")]
    public string Title { get; set; }
    [Display(Name = "Mesaj")]
    public string Message { get; set; }
    [Display(Name = "Eklenme Tarihi")]
    public DateTime AddedDate { get; set; }

    [Display(Name = "İşlem Yapıldı")] 
    public bool? IsDone { get; set; } = false;

}