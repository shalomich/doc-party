using DocParty.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Components
{
    public class Footer : ViewComponent
    {
        private const string FeedbackConfigPath = "Feedback";
        private IConfigurationSection FeedbackConfigSection { get; }
        public Footer(IConfiguration configuration)
        {
            FeedbackConfigSection = configuration.GetSection(FeedbackConfigPath);
        }

        public IViewComponentResult Invoke()
        {
            var info = new FooterInfo
            {
                Email = FeedbackConfigSection["Email"],
                PhoneNumber = FeedbackConfigSection["PhoneNumber"]
            };
            return View(info); 
        }
    }
}
