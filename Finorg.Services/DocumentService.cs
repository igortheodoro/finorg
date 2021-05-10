using System;
using Telegram.Bot.Types.InputFiles;
using Finorg.Dto.Fiis;
using Finorg.Dto.Stocks;
using Spire.Xls;
using System.IO;
using System.Linq;
using Finorg.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace Finorg.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRequestService _requestService;
        private readonly IConfiguration _configuration;

        public DocumentService(IRequestService requestService, IConfiguration configuration)
        {
            _requestService = requestService;
            _configuration = configuration;
        }

        public InputOnlineFile CreateExtract(decimal allEarnings, decimal allDebts)
        {
            var workbook = new Workbook();
            workbook.CreateEmptySheet();

            var sheet = workbook.Worksheets[0];

            if (allEarnings == 0 && allDebts == 0)
            {
                sheet.Range[1, 1].Text = "Não é possível gerar extratos enquanto não houver movimentação financeira.";
                var streamToReturn = new MemoryStream();
                workbook.SaveToStream(streamToReturn, FileFormat.PDF);

                streamToReturn.Flush();

                return streamToReturn;
            }

            sheet.PageSetup.LeftHeader = $"Relatório das movimentações {DateTime.Now:MM/yyyy}";

            sheet.Range[2, 1].Value = " Ganhos";
            sheet.Range[2, 2].Value = " Gastos";

            sheet.Range[3, 1].NumberValue = (double)allEarnings;
            sheet.Range[3, 2].NumberValue = (double)allDebts;

            sheet.Range[3, 1, 3, 2].Style.Font.Color = Color.White;
            sheet.Range[3, 1, 3, 2].Style.NumberFormat = @"""R$""0.00";

            var chart = sheet.Charts.Add(ExcelChartType.Pie3D);
            chart.SeriesDataFromRange = false;
            chart.PlotArea.Fill.Visible = false;
            chart.ChartTitle = "Soma das movimentações mensais";

            var cs = chart.Series.Add();
            cs.CategoryLabels = sheet.Range[2, 1, 2, 2];
            cs.Values = sheet.Range[3, 1, 3, 2];
            cs.DataPoints.DefaultDataPoint.DataLabels.HasValue = true;

            var profit = allEarnings - allDebts;
            var spendPerDay = allDebts / DateTime.Now.Day;
            var spendPerDayString = spendPerDay.ToString().Substring(0, spendPerDay.ToString().IndexOf(',') + 3);

            var summaryOfMonthly = "A média dos gastos diários foi de " +
                $"{spendPerDayString} reais." +
                " Com base na movimentação financeira desse mês, " +
                $"{(profit >= 0 ? $"restou {profit} reais." : $"faltou {profit} reais para abater suas dívidas.")}";


            var shape = chart.TextBoxes.AddTextBox(21, 1, 400, 3985);
            shape.Top = 3560;
            shape.HAlignment = CommentHAlignType.Center;
            shape.VAlignment = CommentVAlignType.Center;
            shape.Text = summaryOfMonthly;

            var secondParagraph = "";

            if (profit < 10)
            {
                secondParagraph = "Poupando um pouco mais, você pode pesquisar sobre essas ações e fundos imobiliários que separamos para você:";
            }
            else
            {
                secondParagraph = "Separamos algumas ações e fundos imobiliários, com base no dinheiro que restou, para você pesquisar um pouco mais:";
            }

            sheet.Range[23, 1, 23, 10].Merge();
            sheet.Range[23, 1].Text = secondParagraph;
            sheet.Range[23, 1].Style.Font.IsBold = true;
            sheet.Range[23, 1].Style.ShrinkToFit = true;

            var restToCreateRecommendations = profit > 10 ? (double)profit : 10;

            CreateRecommendationsFii(sheet, restToCreateRecommendations);
            CreateRecommendationsStocks(sheet, restToCreateRecommendations);

            var stream = new MemoryStream();
            workbook.SaveToStream(stream, FileFormat.PDF);

            stream.Flush();

            return stream;
        }

        private void CreateRecommendationsFii(Worksheet sheet, double restValue)
        {
            var rnd = new Random();

            var fiis = _requestService.Get<ListFiiDto>(_configuration["StockExchangeApi:Url"], _configuration["StockExchangeApi:Fiis"]).Fiis;
            fiis = fiis
                .Where(f => f.LastPrice <= restValue && f.LastPrice > 0)
                .OrderBy(f => rnd.Next())
                .ToList();

            var seqIndex = 1;
            var line = 26;

            for (int l = 1; l <= 2; l++)
            {
                for (int i = 1; i <= 10; i += 3)
                {
                    var columnText = i + 1;

                    var secondLine = line + 1;
                    var thirdLine = line + 2;
                    var fourthLine = line + 3;
                    var fifthLine = line + 4;

                    sheet.Range[line, i, line, columnText].Merge();
                    sheet.Range[line, i].Text = "Fundo imobiliário";
                    sheet.Range[line, i, line, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[line, i, line, columnText].Style.Color = Color.Azure;
                    sheet.Range[line, i].Style.Font.IsBold = true;

                    sheet.Range[secondLine, i].Text = "Símbolo:";
                    sheet.Range[secondLine, i, secondLine, columnText].BorderAround();
                    sheet.Range[secondLine, columnText].Text = fiis.ElementAtOrDefault(seqIndex).Symbol;

                    sheet.Range[thirdLine, i].Text = "Preço:";
                    sheet.Range[thirdLine, i, thirdLine, columnText].BorderAround();
                    sheet.Range[thirdLine, i, thirdLine, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[thirdLine, i, thirdLine, columnText].Style.Color = Color.Azure;
                    sheet.Range[thirdLine, columnText].Text = fiis.ElementAtOrDefault(seqIndex).LastPrice.ToString();

                    sheet.Range[fourthLine, i].Text = "Alta:";
                    sheet.Range[fourthLine, columnText].Text = fiis.ElementAtOrDefault(seqIndex).High.ToString();

                    sheet.Range[fifthLine, i].Text = "Baixa:";
                    sheet.Range[fifthLine, i, fifthLine, columnText].BorderAround();
                    sheet.Range[fifthLine, i, fifthLine, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[fifthLine, i, fifthLine, columnText].Style.Color = Color.Azure;
                    sheet.Range[fifthLine, columnText].Text = fiis.ElementAtOrDefault(seqIndex).Low.ToString();

                    sheet.Range[line, i, fifthLine, columnText].BorderAround();

                    seqIndex++;
                }

                line += 7;
            }
        }

        private void CreateRecommendationsStocks(Worksheet sheet, double restValue)
        {
            var rnd = new Random();

            var stocks = _requestService.Get<ListStockDto>(_configuration["StockExchangeApi:Url"], _configuration["StockExchangeApi:Stocks"]).Stocks;
            stocks = stocks
                .Where(s => s.LastPrice <= restValue && s.LastPrice > 0)
                .OrderBy(s => rnd.Next())
                .ToList();

            var seqIndex = 1;
            var line = 40;

            for (int l = 1; l <= 2; l++)
            {
                for (int i = 1; i <= 10; i += 3)
                {
                    var columnText = i + 1;

                    var secondLine = line + 1;
                    var thirdLine = line + 2;
                    var fourthLine = line + 3;
                    var fifthLine = line + 4;
                    var sixthLine = line + 5;

                    sheet.Range[line, i].Text = "Ação";
                    sheet.Range[line, i, line, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[line, i, line, columnText].Style.Color = Color.Azure;
                    sheet.Range[line, i].Style.Font.IsBold = true;

                    sheet.Range[secondLine, i].Text = "Símbolo:";
                    sheet.Range[secondLine, i, secondLine, columnText].BorderAround();
                    sheet.Range[secondLine, columnText].Text = stocks.ElementAtOrDefault(seqIndex).Symbol;

                    sheet.Range[thirdLine, i].Text = "Preço:";
                    sheet.Range[thirdLine, i, thirdLine, columnText].BorderAround();
                    sheet.Range[thirdLine, i, thirdLine, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[thirdLine, i, thirdLine, columnText].Style.Color = Color.Azure;
                    sheet.Range[thirdLine, columnText].Text = stocks.ElementAtOrDefault(seqIndex).LastPrice.ToString();

                    sheet.Range[fourthLine, i].Text = "Alta:";
                    sheet.Range[fourthLine, columnText].Text = stocks.ElementAtOrDefault(seqIndex).High.ToString();

                    sheet.Range[fifthLine, i].Text = "Baixa:";
                    sheet.Range[fifthLine, i, fifthLine, columnText].BorderAround();
                    sheet.Range[fifthLine, i, fifthLine, columnText].Style.FillPattern = ExcelPatternType.Solid;
                    sheet.Range[fifthLine, i, fifthLine, columnText].Style.Color = Color.Azure;
                    sheet.Range[fifthLine, columnText].Text = stocks.ElementAtOrDefault(seqIndex).Low.ToString();

                    sheet.Range[sixthLine, i].Text = "Setor:";
                    sheet.Range[sixthLine, columnText].Text = stocks.ElementAtOrDefault(seqIndex).Sector;
                    sheet.Range[sixthLine, columnText].Style.ShrinkToFit = true;

                    sheet.Range[line, i, sixthLine, columnText].BorderAround();

                    seqIndex++;
                }

                line += 8;
            }
        }
    }
}
