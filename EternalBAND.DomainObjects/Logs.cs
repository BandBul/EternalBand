using System;
using System.ComponentModel.DataAnnotations;

namespace EternalBAND.DomainObjects
{
    public class Logs : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Tip")]
        public string Type { get; set; }
        [Display(Name = "Değer")]
        public string Value { get; set; }
        [Display(Name = "Yer")]
        public string PageOrMethod { get; set; }
        [Display(Name = "Tarih")]
        public DateTime Date { get; set; }
    }
}