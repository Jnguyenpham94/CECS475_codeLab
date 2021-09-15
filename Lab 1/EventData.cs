using System;
using System.Collections.Generic;
using System.Text;
namespace Lab_1
{
    public class EventData : EventArgs
    {
        public string StockName { get; }
        public int InitialValue { get; }
        public int CurrentValue { get; set; }
        public int NumOfChanges { get; set; }
        public DateTime CurrentTime { get; }
        public EventData(string stockName, int initialValue, int currentValue, int
        numOfChanges, DateTime currentTime)
        {
            StockName = stockName;
            InitialValue = initialValue;
            CurrentValue = currentValue;
            CurrentTime = currentTime;
            NumOfChanges = numOfChanges;
        }
    }
}