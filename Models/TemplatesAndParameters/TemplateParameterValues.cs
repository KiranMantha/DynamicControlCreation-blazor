using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DynamicControlCreation_blazor.Models.TemplatesAndParameters
{
    public class TemplateParameterValues
    {
        public TemplateParameterValues()
        {
            Deleted = false;
            DisplayIndex = 0;
        }
        [Key]
        public int ID { get; set; }
        public int TemplateParameterID { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public int DisplayIndex { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool Deleted { get; set; }
    }
}