using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubla
{
    public class OtherFile
    {
        public string OpenFileDialog(bool save)
        {
            DeleteFiles();
            StartProgrammSql();
            string text = ReadFile();
            if (save)
            {
                if (!Directory.Exists("files")) Directory.CreateDirectory("files");
                string finalNameFile = AppDomain.CurrentDomain.BaseDirectory + "files\\" + new FileInfo(text).Name;
                //
                if (File.Exists(finalNameFile)) File.Delete(finalNameFile);
                File.Copy(text, finalNameFile);
            }
            DeleteFiles();
            return text;
        }

        public string[] GetFiles()
        {
            string files = "";

            DirectoryInfo d = new DirectoryInfo("files");
            foreach (FileInfo file in d.GetFiles())
            {
                files += file.FullName + ";";
            }

            string[] values = files.Split(';');
            string[] _values = new string[values.Length - 1];
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
            return _values;
        }

        public string ConvertFromPathInNameFile(string path)
        {
            string[] file = path.Split('\\');
            return file[file.Length - 1];
        }

        private static string ReadFile()
        {
            string text = null;
            if (!File.Exists("lib\\window\\write.txt")) return text;
            text = File.ReadAllText("lib\\window\\write.txt");
            return text;
        }

        private static void StartProgrammSql()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.Arguments = @"/C cd " + AppDomain.CurrentDomain.BaseDirectory + "/lib/window & OpenDialog.exe";
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.CreateNoWindow = true;
            //myProcess.StartInfo.Arguments = textWrite;
            myProcess.Start();
            myProcess.WaitForExit();
        }

        private static void DeleteFiles()
        {
            if (File.Exists("lib\\window\\write.txt")) File.Delete("lib\\window\\write.txt");
        }
    }
}
