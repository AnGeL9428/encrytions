using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // text with bytes
            byte[] originalData = Encoding.UTF8.GetBytes("Example");
            
            // (encode)
            string encodedData = ChineseEncoder.ChineseEncode(originalData);

            Console.WriteLine($"Encoded Data: {encodedData}");

            // (decode)
            byte[] decodedData = ChineseEncoder.ChineseDecode(encodedData);
            string originalString = Encoding.UTF8.GetString(decodedData);

            Console.WriteLine($"Decoded Data: {originalString}");


            Console.ReadKey();
        }
    }
}
