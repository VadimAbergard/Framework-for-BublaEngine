using MyFramework.utils;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bubla
{
    public class Save
    {
        private static string saveFile = null;

        public static void Init()
        {
            string nameGame = Game.GetTitle().Replace(' ', '_');
            string pathLaba1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + nameGame;
            if(!Directory.Exists(pathLaba1)) Directory.CreateDirectory(pathLaba1);
            saveFile = $"{pathLaba1}\\save.bubla";
            try
            {
                FileStream fileStream = new FileStream(saveFile, FileMode.CreateNew);
                fileStream.Close();
            }
            catch { }
        }

        public void Write(string key, string value)
        {
            SWrite(key, value);
        }

        public static void SWrite(string key, string value)
        {

            FileStream fileStream = new FileStream(saveFile, FileMode.Open);

            // read file
            string textRead = "";
            int data = 0;
            while ((data = fileStream.ReadByte()) != -1)
            {
                textRead += (char)data;
            }
            string textWrite = "";
            if (textRead != "")
            {
                textRead = Encryption.DencrypText(textRead);
                // split text
                textRead = textRead.Remove(textRead.Length - 1);
                string[] textSplit = textRead.Split(';');
                string[] args = new string[textSplit.Length * 2];
                int i = 0;
                foreach (string text in textSplit)
                {
                    string[] splitText = text.Split(' ');
                    args[i++] += splitText[0];
                    args[i++] += splitText[1];
                }
                bool hasKey = false;
                for (int k = 0; k < args.Length; k += 2)
                {
                    if (args[k].Equals(key))
                    {
                        args[k + 1] = value;
                        hasKey = true;
                        break;
                    }
                }
                // write
                bool isKey = true;
                foreach (string text in args)
                {
                    textWrite += text + (isKey ? " " : ";");
                    isKey = !isKey;
                }
                if (!hasKey) textWrite += $"{key} {value};";
            }
            else textWrite = $"{key} {value};";
            textWrite = Encryption.EncodText(textWrite);
            fileStream.SetLength(0);
            fileStream.Write(Encoding.UTF8.GetBytes(textWrite), 0, textWrite.Length);
            fileStream.Close();
        }

        public void Delete(string key)
        {
            FileStream fileStream = new FileStream(saveFile, FileMode.Open);

            // read file
            string textRead = "";
            int data = 0;
            while ((data = fileStream.ReadByte()) != -1)
            {
                textRead += (char)data;
            }
            string textWrite = "";
            if (textRead == "")
            {
                fileStream.Close();
                return;
            }
            textRead = Encryption.DencrypText(textRead);
            // split text
            textRead = textRead.Remove(textRead.Length - 1);
            string[] textSplit = textRead.Split(';');
            string[] args = new string[textSplit.Length * 2];
            int i = 0;
            foreach (string text in textSplit)
            {
                string[] splitText = text.Split(' ');
                args[i++] += splitText[0];
                args[i++] += splitText[1];
            }
            for (int k = 0; k < args.Length; k += 2)
            {
                if (args[k].Equals(key))
                {
                    args[k] = null;
                    args[k + 1] = null;
                    break;
                }
            }
            // write
            bool isKey = true;
            foreach (string text in args)
            {
                if (text == null) continue;
                textWrite += text + (isKey ? " " : ";");
                isKey = !isKey;
            }
            textWrite = Encryption.EncodText(textWrite);
            fileStream.SetLength(0);
            fileStream.Write(Encoding.UTF8.GetBytes(textWrite), 0, textWrite.Length);
            fileStream.Close();
        }
        
        public string Read(string key)
        {
            return SRead(key);
        }

        public static string SRead(string key)
        {
            FileStream fileStream = new FileStream(saveFile, FileMode.Open);

            // read file
            string textRead = "";
            int data = 0;
            bool ignoreRead = true;
            while ((data = fileStream.ReadByte()) != -1)
            {
                ignoreRead = !ignoreRead;
                if (ignoreRead) continue;
                textRead += (char)data;
            }
            if (textRead == "")
            {
                fileStream.Close();
                return null;
            }
            textRead = textRead.Replace("`", " ");
            // split text
            textRead = textRead.Remove(textRead.Length - 1);
            string[] textSplit = textRead.Split(';');
            string[] args = new string[textSplit.Length * 2];
            int i = 0;
            foreach (string text in textSplit)
            {
                string[] splitText = text.Split(' ');
                args[i++] += splitText[0];
                args[i++] += splitText[1];
            }
            for (int k = 0; k < args.Length; k += 2)
            {
                if (args[k].Equals(key))
                {
                    fileStream.Close();
                    return args[k + 1];
                }
            }
            fileStream.Close();
            return null;
        }
    }
}
