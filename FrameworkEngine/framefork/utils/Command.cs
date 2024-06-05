using System;
using System.IO;
using System.Security;
using System.Text;

namespace Bubla
{
    public class Command
    {
        public void Exit()
        {
            Game.GetWindow().Close();
        }
        public int GetMillisecond()
        {
            return DateTime.Now.Millisecond;
        }

        public int GetSecond()
        {
            return DateTime.Now.Second;
        }

        public int GetMinute()
        {
            return DateTime.Now.Minute;
        }

        public int GetHour()
        {
            return DateTime.Now.Hour;
        }

        public int GetDay()
        {
            return DateTime.Now.Day;
        }

        public int GetMonth()
        {
            return DateTime.Now.Month;
        }

        public int GetYear()
        {
            return DateTime.Now.Year;
        }

        // work with files

        public string ReadFile(string path)
        {
            FileStream fsRead = new FileStream(path, FileMode.Open);
             
            string textRead = "";
            int data = 0;
            while ((data = fsRead.ReadByte()) != -1)
            {
                textRead += (char)(byte)data;
            }
            fsRead.Close();

            return textRead;
        }

        public void WriteFile(string path, string text)
        {
            FileStream fsWrite = new FileStream(path, FileMode.Create);

            byte[] bytesWrite = Encoding.UTF8.GetBytes(text);
            string finalText = "";
            for (int i = 0; i < bytesWrite.Length; i++)
            {
                finalText += bytesWrite[i];
                fsWrite.WriteByte(bytesWrite[i]);
            }
            fsWrite.Close();
        }

        public void OpenUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
    }
}
