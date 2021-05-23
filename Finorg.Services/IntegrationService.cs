using Finorg.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Finorg.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly ITelegramBotClient _telegramCliente;
        private readonly IFinanceService _dataService;
        private static AutoResetEvent _semaphore;

        public IntegrationService(IFinanceService dataService, IConfiguration configuration)
        {
            _telegramCliente = new TelegramBotClient(configuration["TelegramKey"]);
            _telegramCliente.StartReceiving();
            _dataService = dataService;
            _semaphore = new AutoResetEvent(false);
        }

        public void Start()
        {
            _telegramCliente.OnMessage += OnReceiveMessage;
            _semaphore.WaitOne();
        }

        private async void OnReceiveMessage(object sender, MessageEventArgs e)
        {
            var name = e.Message.Chat.Username ?? e.Message.Chat.FirstName;
            var chatId = e.Message.Chat.Id;

            try
            {
                if (e.Message.Type == MessageType.Text)
                {
                    var message = e.Message.Text.ToLower();

                    var messageToReturn = await _dataService.ManipulateMessage(message, chatId);

                    if (messageToReturn.GetType() == typeof(InputOnlineFile))
                    {
                        await _telegramCliente.SendDocumentAsync(chatId, messageToReturn, $"Extrato do mês {DateTime.Now:MM/yyyy}", ParseMode.Default);
                    }
                    else
                    {
                        await _telegramCliente.SendTextMessageAsync(chatId, messageToReturn, ParseMode.Markdown);
                    }
                }
                else
                {
                    await _telegramCliente.SendTextMessageAsync(chatId,
                        $"{name}, ainda não consigo interpretar esse tipo de mídia.\r\n\n\n" +
                        "Digite /ajuda para conhecer os comandos.");
                }
            }
            catch (Exception ex)
            {
                await _telegramCliente.SendTextMessageAsync(chatId,
                        $"{name}, houve um problema {ex.Message}.\r\n" +
                        "Digite /ajuda para conhecer os comandos.");
            }
        }
    }
}
