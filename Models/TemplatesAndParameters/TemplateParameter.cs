using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicControlCreation_blazor.Models.TemplatesAndParameters
{
    public class TemplateParameter
    {
        public TemplateParameter()
        {
            ParameterValues = new List<TemplateParameterValues>();
            ParameterDefaults = new List<TemplateParameterDefaults>();
            Values = new List<string>();
            AllowMultiple = false;
        }
        [Key]
        public int TemplateParameterID { get; set; }
        public int TemplateID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public bool AllowMultiple { get; set; }
        public List<TemplateParameterValues> ParameterValues { get; set; }
        public List<TemplateParameterDefaults> ParameterDefaults { get; set; }

        [NotMapped]
        public List<string> Values { get; set; }
    }

    public enum TemplateParameterType
    {
        NotSet = 0,
        String = 1,
        Bool = 2,
        Integer = 3
        //Multiple =3,
        //MultiSelect = 4
    }
}