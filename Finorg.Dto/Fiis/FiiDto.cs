using System;
using System.Collections.Generic;
using System.Text;

namespace Finorg.Dto.Fiis
{
    public class FiiDto
    {
        public double Change { get; set; }
        public double ClosingPrice { get; set; }
        public double High { get; set; }
        public double LastPrice { get; set; }
        public double LastYearHigh { get; set; }
        public double LastYearLow { get; set; }
        public double Low { get; set; }
        public string Name { get; set; }
        public double PriceOpen { get; set; }
        public double Shares { get; set; }
        public string Symbol { get; set; }
        public int Volume { get; set; }
        public int VolumeAvg { get; set; }  
    }
}
