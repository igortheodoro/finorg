using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;

namespace Finorg.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<dynamic> ManipulateMessage(string message, long chatId);
    }
}
