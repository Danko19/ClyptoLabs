using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Crypto_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = File.ReadAllBytes("key");
            var sBlock = new SBlock(key);

            foreach (var k in sBlock.GetBytes())
            {
                var keyChar = Console.ReadKey(true).KeyChar;
                var b = Convert.ToByte(keyChar);
                var e = b ^ k;
                Console.Write(Convert.ToChar(e));
            }
        }
    }
}
