﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITServiceApp.Extensions
{
    [ModelBinder(BinderType =typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions:DataSourceLoadOptionsBase
    {
    }
    public class DataSourceLoadOptionsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var loadoptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadoptions,
                key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
            bindingContext.Result = ModelBindingResult.Success(loadoptions);
            return Task.CompletedTask;
        }
    }
}
