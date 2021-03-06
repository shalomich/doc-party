using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.NotificationHandlers
{
    public class HandlerData<T> : INotification
    {
        public T Data { set; get; }
    }
}
