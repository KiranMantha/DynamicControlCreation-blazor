using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicControlCreation_blazor.DBModel;
using DynamicControlCreation_blazor.Models.TemplatesAndParameters;

namespace DynamicControlCreation_blazor.Data
{
    public class TemplateService
    {
        public Task<List<Template>> GetTemplates()
        {
            List<Template> lstTemplates = new List<Template>();
            using (var ctx = new DynamicParameterContext())
            {
                lstTemplates = ctx.Templates.ToList();
            }
            return Task.FromResult(lstTemplates);
        }

        public Task<Template> GetTemplate(int id = 0)
        {
            Template _Template = new Template();
            if (id != 0)
            {
                using (var ctx = new DynamicParameterContext())
                {
                    _Template = ctx.Templates.Where(m => m.TemplateID == id).FirstOrDefault();
                    var TemplateParameters = ctx.TemplateParameters.Where(m => m.TemplateID == id);
                    if (TemplateParameters.Any())
                    {
                        _Template.Parameters = TemplateParameters.ToList();
                    }
                    else
                    {
                        _Template.Parameters = new List<TemplateParameters>();
                    }
                }
            }
            else
            {
                _Template.Parameters = new List<TemplateParameters>();
            }
            return Task.FromResult(_Template);
        }

        public Task<bool> SaveTemplate(Template _Template)
        {
            using (var ctx = new DynamicParameterContext())
            {
                if (_Template.TemplateID != 0)
                {
                    Template temp = ctx.Templates.Where(m => m.TemplateID == _Template.TemplateID).FirstOrDefault();
                    temp.Name = _Template.Name;
                }
                else
                {
                    ctx.Templates.Add(_Template);
                }
                ctx.SaveChanges();
            }
            return Task.FromResult(true);
        }

        public Task<TemplateParameters> GetParameter(int TID, int ID = 0)
        {
            TemplateParameters _TemplateParameter = new TemplateParameters();
            if (ID != 0)
            {
                using (var ctx = new DynamicParameterContext())
                {
                    _TemplateParameter = ctx.TemplateParameters.Where(m => (m.TemplateParameterID == ID && m.TemplateID == TID)).FirstOrDefault();
                    _TemplateParameter.ParameterValues = ctx.TemplateParameterValues.Where(m => (m.TemplateParameterID == ID)).ToList();
                    _TemplateParameter.ParameterDefaults = ctx.TemplateParameterDefaults.Where(m => (m.TemplateParameterID == ID)).ToList();
                    _TemplateParameter.Types = (TemplateParameterType)_TemplateParameter.Type;
                }
            }
            _TemplateParameter.TemplateID = TID;
            return Task.FromResult(_TemplateParameter);
        }
    }
}