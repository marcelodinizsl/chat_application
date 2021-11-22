using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_application.Infra.CrossCutting.Bus
{
    public interface IMessageBroker
    {
        void Insert(string message);
    }
}
