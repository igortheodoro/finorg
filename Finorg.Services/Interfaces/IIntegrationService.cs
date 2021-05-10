using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Args;

namespace Finorg.Services.Interfaces
{
    public interface IIntegrationService
    {
        void OnReceiveMessage(object sender, MessageEventArgs e);
    }
}
