using System;
using System.IO.Ports;
using System.Threading;

namespace IdentixSocketReader
{
    class Program
    {
        private static SerialPort serialPort = null;

        static void Main(string[] args)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialReceivedData);
            }

            OpenSerialPort();
            StartInventory();

            Thread.Sleep(5000);

            StopInventory();
            CloseSerialPort();
        }

        private static void SerialReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            string data = ((SerialPort)sender).ReadExisting();
            Console.WriteLine(data);
        }

        private static void OpenSerialPort()
        {
            try
            {
                if (!serialPort.IsOpen)
                    serialPort.Open();

                serialPort.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open serial port connection. Exception: {ex.Message}");                
            }            
        }

        private static void CloseSerialPort()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        private static void StartInventory()
        {
            if (!serialPort.IsOpen) return;
            serialPort.Write("1\n");
        }

        private static void StopInventory()
        {
            if (!serialPort.IsOpen) return;
            serialPort.Write("0\n");
        }
    }
}