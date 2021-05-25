using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Email
{
    class ProjectInviteBuilder : EmailMessageBuilder
    {
        private readonly string _projectUrl;
        public ProjectInviteBuilder(string projectUrl)
        {
            _projectUrl = projectUrl;
        }


        public override void SetText()
        {
            Message.Text = $"You are invited to project. Follow the link {_projectUrl}";
        }

        public override void SetSubject()
        {
            Message.Subject = "DocParty";
        }
    }
}
