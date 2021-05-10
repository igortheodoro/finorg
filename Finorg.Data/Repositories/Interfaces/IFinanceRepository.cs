using Finorg.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Finorg.Data.Repositories.Interfaces
{
    public interface IFinanceRepository
    {   
        Task<List<Finance>> GetWithChatIdCurrentMonth(long chatId);
        decimal GetEarningsWithChatIdCurrentMonth(long chatId);
        decimal GetDebtsWithChatIdCurrentMonth(long chatId);
        Task Register(Finance finance);
        Task Update(Finance finance);
        Task RemoveRange(List<Finance> finances);
    }
}
