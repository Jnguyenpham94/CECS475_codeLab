using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Lab_1
{
    public class Stock
    {
        public event EventHandler<EventData> StockEvent;
        public event EventHandler<EventData> StockEventData;
        private readonly Thread thread;
        public string StockName { get; set; }
        public int InitialValue { get; set; }
        public int CurrentValue { get; set; }
        public int MaxChange { get; set; }
        public int Threshold { get; set; }
        public int NumChanges { get; set; }

        /// <summary>
        /// Stock class that contains all the information and changes of the stock
        /// </summary>
        /// <param name="name">Stock name</param>
        /// <param name="startingValue">Starting stock value</param>
        /// <param name="maxChange">The max value change of the stock</param>
        /// <param name="threshold">The range for the stock</param>
        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            StockName = name;
            InitialValue = startingValue;
            MaxChange = maxChange;
            Threshold = threshold;
            thread = new Thread(new ThreadStart(Activate));
            thread.Start();
        }


        // Change the stock's value every 500 milliseconds
        private void Activate()
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(500);
                ChangeStockValue();
                i++;
            }
        }
        // Change the stock value and invoke event to notify stock brokers when the threshold is reach
        private void ChangeStockValue()
        {
            // Generate a random number to within a range that stock can change every time unit and add it to the current stock's value
            Random rand = new Random();
            CurrentValue += rand.Next(-MaxChange, MaxChange);
            // Increase the number of changes in value by 1
            NumChanges++;
            // Check if the threshold is reached
            if (Math.Abs(CurrentValue - InitialValue) >= Threshold)
            {
                // Raise the events
                Parallel.Invoke(() => StockEvent?.Invoke(this, new EventData(StockName, InitialValue, CurrentValue, NumChanges, DateTime.Now)),
                () => StockEventData?.Invoke(this, new EventData(StockName, InitialValue, CurrentValue, NumChanges, DateTime.Now)));
            }
        }
    }
}
}