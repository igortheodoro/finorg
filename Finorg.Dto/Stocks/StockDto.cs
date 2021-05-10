using System;
using System.Collections.Generic;
using System.Text;

namespace Finorg.Dto.Stocks
{
    public class StockDto
    {
        public double Change { get; set; }
        public double ClosingPrice { get; set; }
        public double Eps { get; set; }
        public double High { get; set; }
        public double LastPrice { get; set; }
        public double LastYearHigh { get; set; }
        public double LastYearLow { get; set; }
        public double Low { get; set; }
        public long MarketCap { get; set; }
        public string Name { get; set; }
        public double PriceOpen { get; set; }
        public long Shares { get; set; }
        public string Symbol { get; set; }
        public int Volume { get; set; }
        public int VolumeAvg { get; set; }
        public string Sector { get; set; }
        public string SubSector { get; set; }
        public string Segment { get; set; }
    }
}
