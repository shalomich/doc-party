using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Email
{
    abstract class EmailMessageBuilder
    {
        private EmailMessage _message;
        protected EmailMessage Message
        {
            get
            {
                if (_message == null)
                    _message = new EmailMessage();
                return _message;
            }
        }
        public abstract void SetSubject();
        public abstract void SetText();
        public EmailMessage Build()
        {
            EmailMessage buildedMessage = _message;
            _message = null;
            return buildedMessage;
        }

    }
}
