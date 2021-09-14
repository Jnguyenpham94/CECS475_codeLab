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
        private static Mutex _locker = new Mutex();

        // Path of the textfile to save the stock's information when the threshold is reached
        private readonly string _docPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));

        public static ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
        readonly string docPath = @"C:\Users\nguye\Documents\GitHub\CECS475_codeLab\Lab 1\Lab3_output.txt";

        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) + "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

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
            stocks.StockEvent += Notify;
            stocks.StockEventData += WriteToFile;

        }
        private void Notify(string stockName, int initialValue, int currentValue, int numOfChanges, DateTime currentTime)
        {
            Console.WriteLine(BrokerName.PadRight(20) + stockName.PadRight(20) + initialValue.ToString().PadRight(20) + currentValue.ToString().PadRight(20) + numOfChanges.ToString().PadRight(20) + currentTime);
        }

        // Output to a textfile the broker's name and the stock's name, initial value, current value, number of changes in value, and time when the threshold is reached
        private void WriteToFile(object sender, EventData e)
        {
            try
            {
                // Wait for the resource to be free
                lock (_locker)
                {
                    using (FileStream file = new FileStream(Path.Combine(_docPath, "output.txt"), FileMode.Append, FileAccess.Write, FileShare.Read))
                    using (StreamWriter outputFile = new StreamWriter(file))
                    {
                        outputFile.WriteLine(BrokerName.PadRight(20) + e.StockName.PadRight(20) + e.InitialValue.ToString().PadRight(20) + e.CurrentValue.ToString().PadRight(20) + e.NumOfChanges.ToString().PadRight(20) + e.CurrentTime);
                    }
                }
            }
            catch (IOException) { }
        }


        /// <summary>
        ///     The eventhandler that raises the event of a change
        /// </summary>
        /// <param name="sender">The sender that indicated a change</param>
        /// <param name="e">Event arguments</param>
        void EventHandler(Object sender, EventArgs e)
        {
            try
            {
                Stock newStock = (Stock)sender;
                //string statement;
                Console.WriteLine(titles);
            }
            finally
            {//file processing HERE

            }
        }
    }
}
