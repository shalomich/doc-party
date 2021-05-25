using DocParty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.NotificationHandlers.UserHandlers
{
    public class UserHandlerData<T> : HandlerData<T>
    {
        public User User {set;get;}
    }
}
