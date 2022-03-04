﻿using ITServiceApp.MapperProfiles;
using ITServiceApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace ItServiceApp.Test
{
    public class Startup
    {   
       public  void ConfigureServices(IServiceCollection services)
       {
          services.AddScoped<IPaymentService, IyzicoPaymentService>();
          services.AddAutoMapper(options =>
          {
              options.AddProfile(typeof(PaymentProfile));
          });
       }
    }
}
