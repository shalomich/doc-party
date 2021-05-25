using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Email
{
    class EmailMessageCreator
    {
        private EmailMessageBuilder Builder { set; get; }
        public EmailMessageCreator(EmailMessageBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public EmailMessage Create()
        {
            Builder.SetText();
            Builder.SetSubject();
            return Builder.Build();
        }
    }
}
