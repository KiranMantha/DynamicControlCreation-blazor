using DynamicControlCreation_blazor.Models.TemplatesAndParameters;
using Microsoft.EntityFrameworkCore;

namespace DynamicControlCreation_blazor.DBModel
{
    public class DynamicParameterContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DynamicParameters.db");
        }

        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<TemplateParameters> TemplateParameters { get; set; }
        public virtual DbSet<TemplateParameterValues> TemplateParameterValues { get; set; }
        public virtual DbSet<TemplateParameterDefaults> TemplateParameterDefaults { get; set; }
    }
}