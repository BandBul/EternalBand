
using System.ComponentModel.DataAnnotations;

namespace EternalBAND.DomainObjects
{
    public class SystemInfo : IEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Display(Name ="Tip")]
        public string Type { get; set; }
        [Display(Name = "Değer")]
        public string? Value { get; set; }
        [Display(Name = "Açıklama")]
        public string Desc { get; set; }
    }
}