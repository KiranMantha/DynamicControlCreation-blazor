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
                        _Template.Parameters = new List<TemplateParameter>();
                    }
                }
            }
            else
            {
                _Template.Parameters = new List<TemplateParameter>();
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

        public Task<TemplateParameter> GetParameter(int TID, int ID = 0)
        {
            TemplateParameter _TemplateParameter = new TemplateParameter();
            if (ID != 0)
            {
                using (var ctx = new DynamicParameterContext())
                {
                    _TemplateParameter = ctx.TemplateParameters.Where(m => (m.TemplateParameterID == ID && m.TemplateID == TID)).FirstOrDefault();
                    _TemplateParameter.ParameterValues = ctx.TemplateParameterValues.Where(m => (m.TemplateParameterID == ID)).ToList();
                    _TemplateParameter.ParameterValues.Sort(delegate (TemplateParameterValues x, TemplateParameterValues y)
                    {
                        return x.DisplayIndex.CompareTo(y.DisplayIndex);
                    });
                    _TemplateParameter.ParameterDefaults = ctx.TemplateParameterDefaults.Where(m => (m.TemplateParameterID == ID)).ToList();
                    _TemplateParameter.Values = new List<string>();
                    if (_TemplateParameter.ParameterDefaults.Count > 0)
                    {
                        if (_TemplateParameter.AllowMultiple)
                        {
                            _TemplateParameter.Values = _TemplateParameter.ParameterDefaults.Select(m => m.Value).ToList();
                        }
                        else
                        {
                            _TemplateParameter.Values.Add(_TemplateParameter.ParameterDefaults.Select(m => m.Value).LastOrDefault());
                        }
                    }
                }
            }
            else
            {
                _TemplateParameter.ParameterValues = new List<TemplateParameterValues>();
                _TemplateParameter.ParameterDefaults = new List<TemplateParameterDefaults>();
                _TemplateParameter.Type = (int)TemplateParameterType.NotSet;
            }
            _TemplateParameter.TemplateID = TID;
            return Task.FromResult(_TemplateParameter);
        }

        public Task<bool> SaveTemplateParameter(TemplateParameter _TemplateParameter)
        {
            bool isValid = true;
            using (var ctx = new DynamicParameterContext())
            {
                //Check If New Model Or Existing Model
                if (_TemplateParameter.TemplateParameterID != 0)
                {
                    TemplateParameter temp = ctx.TemplateParameters.Where(m => m.TemplateParameterID == _TemplateParameter.TemplateParameterID).FirstOrDefault();
                    temp.Name = _TemplateParameter.Name;
                    temp.Value = _TemplateParameter.Value;
                    temp.Type = _TemplateParameter.Type;
                    temp.AllowMultiple = _TemplateParameter.AllowMultiple;
                }
                else
                {
                    ctx.TemplateParameters.Add(_TemplateParameter);
                }

                //Add Parameter Values
                if (_TemplateParameter.ParameterValues.Count > 0)
                {
                    //Remove All Template Parameter Values From DB Based On Template Parameter ID
                    var tpv = ctx.TemplateParameterValues.Where(x => x.TemplateParameterID == _TemplateParameter.TemplateParameterID);
                    if (tpv.Any())
                    {
                        ctx.TemplateParameterValues.RemoveRange(tpv);
                    }

                    //Remove Deleted Values On ClientSide
                    _TemplateParameter.ParameterValues.RemoveAll(x => x.Deleted == true);

                    //Add New Template Parameter Values To DB          
                    foreach (TemplateParameterValues t in _TemplateParameter.ParameterValues)
                    {
                        ctx.TemplateParameterValues.Add(t);
                    }
                }

                //Add Parameter Defaults
                if (_TemplateParameter.ParameterDefaults.Count > 0)
                {
                    //Remove All Template Parameter Values From DB Based On Template Parameter ID
                    var tpd = ctx.TemplateParameterDefaults.Where(x => x.TemplateParameterID == _TemplateParameter.TemplateParameterID);
                    if (tpd.Any())
                    {
                        ctx.TemplateParameterDefaults.RemoveRange(tpd);
                    }

                    //Remove Deleted Values On ClientSide
                    _TemplateParameter.ParameterDefaults.RemoveAll(x => x.Deleted == true);

                    //Add New Template Parameter Values To DB
                    foreach (TemplateParameterDefaults t in _TemplateParameter.ParameterDefaults)
                    {
                        if (_TemplateParameter.ParameterValues.Count > 0)
                        {
                            if (_TemplateParameter.ParameterValues.Any(x => x.Value == t.Value))
                                ctx.TemplateParameterDefaults.Add(t);
                            else
                            {
                                isValid = false;
                                //ModelState.AddModelError("", "Some of the default values are not found in available values. please check once.");
                                break;
                            }
                        }
                        else
                        {
                            ctx.TemplateParameterDefaults.Add(t);
                        }
                    }
                }

                //Save Changes To DB
                if (isValid)
                {
                    ctx.SaveChanges();
                }
            }
            return Task.FromResult(isValid);
        }
    }
}