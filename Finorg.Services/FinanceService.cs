using Finorg.Data.Repositories.Interfaces;
using Finorg.Models;
using Finorg.Services.Interfaces;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;

namespace Finorg.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IFinanceRepository _financeRepository;
        private readonly IDocumentService _documentService;
        private long _chatId;

        public FinanceService(IFinanceRepository financeRepository, IDocumentService documentService)
        {
            _financeRepository = financeRepository;
            _documentService = documentService;
        }

        public async Task<dynamic> ManipulateMessage(string message, long chatId)
        {
            _chatId = chatId;

            if (message.StartsWith("-"))
            {
                return await CreateDebt(message);
            }
            else if (message.StartsWith("+"))
            {
                return await CreateEarnings(message);
            }
            else if (message.Contains("/extrato"))
            {
                return CreateUserExtract();
            }
            else if (message.Contains("ajuda") || message.Contains("help") || message.Contains("start"))
            {
                return StartMessage();
            }

            return "Digite /ajuda para conhecer os comandos.";
        }

        private async Task<string> CreateDebt(string message)
        {
            var valueParsed = ReplateStringToCleanDecimal(message);

            if (!string.IsNullOrEmpty(valueParsed))
            {
                var decimalValue = decimal.Parse(valueParsed, NumberFormatInfo.InvariantInfo);

                var financeModel = BindToModel(-decimalValue);

                await _financeRepository.Register(financeModel);

                var debtsCurrentMonth = _financeRepository
                    .GetDebtsWithChatIdCurrentMonth(financeModel.ChatId);

                return $"{decimalValue} reais debitados. \nGastos do mês: {debtsCurrentMonth} reais.";
            }

            return $"Para adicionar dívidas tente esse formato, por exemplo: -{(new Random().NextDouble() * 2 + 5).ToString().Substring(0, 4)}";
        }

        private async Task<string> CreateEarnings(string message)
        {
            var valueParsed = ReplateStringToCleanDecimal(message);

            if (!string.IsNullOrEmpty(valueParsed))
            {
                var decimalValue = decimal.Parse(valueParsed, NumberFormatInfo.InvariantInfo);

                var financeModel = BindToModel(decimalValue);

                await _financeRepository.Register(financeModel);

                var earningsCurrentMonth = _financeRepository
                    .GetEarningsWithChatIdCurrentMonth(financeModel.ChatId);

                return $"{decimalValue} reais recebidos. \nGanhos do mês: {earningsCurrentMonth} reais.";
            }

            return $"Para adicionar ganhos tente esse formato, por exemplo: +{(new Random().NextDouble() * 2 + 5).ToString().Substring(0, 4)}";
        }

        private InputOnlineFile CreateUserExtract()
        {
            var allEarnings = _financeRepository.GetEarningsWithChatIdCurrentMonth(_chatId);
            var allDebts = Math.Abs(_financeRepository.GetDebtsWithChatIdCurrentMonth(_chatId));

            var doc = _documentService.CreateExtract(allEarnings, allDebts);
            doc.FileName = $"extrato_{DateTime.Now:MM_yyyy}.pdf";

            return doc;
        }

        private string StartMessage()
        {
            var randomNumberString = (new Random().NextDouble() * 2 + 5).ToString().Substring(0, 4);

            return "*Comandos para manipular dívidas:*" +
            $"\r\n[+{randomNumberString}](tg://bot_command?command=5&bot=FinOrg) > Adiciona um *ganho*" +
            $"\r\n[-{randomNumberString}](tg://bot_command?command=-5&bot=FinOrg) > Adiciona um *gasto*" +
            "\n\r\n\r\n*Comando para exibir extrato:*" +
            "\r\n/extrato > exibe o extrato do movimento financeiro mensal";
        }

        private Finance BindToModel(decimal value)
        {
            return new Finance()
            {
                ChatId = _chatId,
                Transaction = value,
                RegisterDate = DateTime.Now
            };
        }

        private string ReplateStringToCleanDecimal(string value)
        {
            var extractedValue = Regex.Match(value, @"[0-9]+([.?,?][0-9]*)?|[.?,?][0-9]+").Value;
            return extractedValue.Replace(",", ".");
        }
    }
}