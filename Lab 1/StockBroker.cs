using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Lab_1
{
    public class StockBroker
    {
        public string BrokerName { get; set; }

        public List<Stock> stocks = new List<Stock>();
        // Path of the textfile to save the stock's information when the threshold is reached
        private readonly string _docPath =
        Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));

        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) + "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

        public Mutex _locker = new Mutex();

        /// <summary>
        ///     The stockbroker object
        /// </summary>
        /// <param name="brokerName">The stockbroker's name</param>
        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
        }


        public void AddStock(Stock stock)
        {
            // Add this stock to the list of stocks controlled by the stock broker
            stocks.Add(stock);
            // Subscribe to the events
            //stock.StockEvent += Notify;
            stock.StockEventData += WriteToFile;
        }
        private void Notify(string stockName, int initialValue, int currentValue, int
        numOfChanges, DateTime currentTime)
        {
            Console.WriteLine(BrokerName.PadRight(20) + stockName.PadRight(20) +
            initialValue.ToString().PadRight(20) + currentValue.ToString().PadRight(20) +
            numOfChanges.ToString().PadRight(20) + currentTime);
        }
        // Output to a textfile the broker's name and the stock's name, initial value, current value, number of changes in value, and time when the threshold is reached
        private void WriteToFile(object sender, EventData e)
        {
            try
            {
                // Wait for the resource to be free
                lock (_locker)
                {
                    using (FileStream file = new FileStream(Path.Combine(_docPath,
                    "output.txt"), FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (StreamWriter outputFile = new StreamWriter(file))
                    {
                        outputFile.WriteLine(BrokerName.PadRight(20) +
                        e.StockName.PadRight(20) + e.InitialValue.ToString().PadRight(20) +
                        e.CurrentValue.ToString().PadRight(20) + e.NumOfChanges.ToString().PadRight(20) +  e.CurrentTime);
                    }
                }
            }
            catch (IOException) { }
        }

    } 
}
