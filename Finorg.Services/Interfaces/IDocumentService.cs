using Telegram.Bot.Types.InputFiles;

namespace Finorg.Services.Interfaces
{
    public interface IDocumentService
    {
        InputOnlineFile CreateExtract(decimal allEarnings, decimal allDebts);
    }
}
