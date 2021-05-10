using Finorg.Data.Repositories.Interfaces;
using Finorg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finorg.Data.Repositories
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly ApplicationContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FinanceRepository(ApplicationContext context, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<List<Finance>> GetWithChatIdCurrentMonth(long chatId)
        {
            var currentData = DateTime.Now;

            return await _context.Finances
                .Where(f => f.ChatId == chatId && f.RegisterDate.Month == currentData.Month && f.RegisterDate.Year == currentData.Year)
                .AsNoTracking()
                .ToListAsync();
        }

        public decimal GetEarningsWithChatIdCurrentMonth(long chatId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var currentData = DateTime.Now;

                return context.Finances
                     .AsNoTracking()
                     .Where(f => f.ChatId == chatId
                        && f.RegisterDate.Month == currentData.Month
                        && f.RegisterDate.Year == currentData.Year
                        && f.Transaction > 0)
                    .Sum(f => f.Transaction);
            }
        }

        public decimal GetDebtsWithChatIdCurrentMonth(long chatId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var currentData = DateTime.Now;

                return context.Finances
                     .AsNoTracking()
                     .Where(f => f.ChatId == chatId
                        && f.RegisterDate.Month == currentData.Month
                        && f.RegisterDate.Year == currentData.Year
                        && f.Transaction < 0)
                    .Sum(f => f.Transaction);
            }
        }

        public async Task Register(Finance finance)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                context.Finances.Add(finance);
                await context.SaveChangesAsync();
            }
        }

        public async Task Update(Finance finance)
        {
            _context.Entry(finance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(List<Finance> finances)
        {
            _context.Finances.RemoveRange(finances);
            await _context.SaveChangesAsync();
        }
    }
}