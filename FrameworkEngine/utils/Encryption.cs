using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFramework.utils
{
    public class Encryption
    {
        private static string[] randomSymbol = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j",
            "k", "l", "z", "x", "c", "v", "b", "n", "m"};

        public static void Encod(string encodingFile, string endFile)
        {
            FileStream fsRead = new FileStream(encodingFile, FileMode.Open);

            string textRead = "";
            int data = 0;
            int scipSymbol = 0;
            bool scip = endFile.EndsWith(".xml");
            Random random = new Random();
            while ((data = fsRead.ReadByte()) != -1)
            {
                if ((char)(byte)data == ' ') data = '`';
                textRead += (char)(byte)data + randomSymbol[random.Next(randomSymbol.Length)];
            }
            fsRead.Close();


            FileStream fsWrite = new FileStream(endFile, FileMode.Create);

            byte[] bytesWrite = Encoding.UTF8.GetBytes(textRead);
            string finalText = "";
            for (int i = 0; i < bytesWrite.Length; i++)
            {
                finalText += bytesWrite[i];
                fsWrite.WriteByte(bytesWrite[i]);
            }
            fsWrite.Close();
        }

        public static void Dencryp(string dencodingFile, string endFile)
        {
            FileStream fsRead = new FileStream(dencodingFile, FileMode.Open);

            string textRead = "";
            int data = 0;
            int scipSymbol = 0;
            bool spaceSymbol = false;
            bool scip = endFile.EndsWith(".xml");
            while ((data = fsRead.ReadByte()) != -1)
            {
                if (spaceSymbol)
                {
                    textRead += (char)data;
                    Console.Write((char)data);
                    Thread.Sleep(10);
                }
                spaceSymbol = !spaceSymbol;
            }
            fsRead.Close();
            textRead = textRead.Replace("`", " ");

            FileStream fsWrite = new FileStream(endFile, FileMode.Create);

            byte[] bytesWrite = Encoding.UTF8.GetBytes(textRead);
            string finalText = "";
            for (int i = 0; i < bytesWrite.Length; i++)
            {
                finalText += bytesWrite[i];
                fsWrite.WriteByte(bytesWrite[i]);
            }
            fsWrite.Close();
        }

        public static string DencrypText(string text)
        {
            Console.OutputEncoding = Encoding.UTF8;
            char[] chars = text.ToCharArray();

            string textRead = "";
            bool spaceSymbol = false;
            foreach(char _char in chars) {
                if (!spaceSymbol)
                {
                    textRead += (char)_char;
                }
                spaceSymbol = !spaceSymbol;
            }
            textRead = textRead.Replace("`", " ");
            return textRead;
        }

        public static string EncodText(string text)
        {
            char[] chars = text.ToCharArray();
            string finalText = "";
            Random random = new Random();
            for(int i = 0; i < chars.Length ;i++)
            {
                if (chars[i] == ' ') chars[i] = '`';
                finalText += chars[i] + randomSymbol[random.Next(randomSymbol.Length)];
            }

            return finalText;
        }
    }
}
