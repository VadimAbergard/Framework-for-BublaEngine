using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Bubla
{
    public class Sql
    {
        private static string host;
        private static string user;
        private static string password;

        private static string[] ecod = new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "a", "s", "d", "f", "g", "h", "j", 
            "k", "l", ";", "\"", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/", ">", "<", "!", "@", "?","#","$","%",
            "^","&","*","(",")","-","=","_","+","1","2","3","4","5","6","7","8","9","0","`","~"};

        public void Init(string _host, string _user, string _password)
        {
            DeleteFiles();
            host = _host;
            user = _user;
            password = _password;
        }

        public string[] ReadTable(string nameTable, int maxColumn) {
            WriteFile(nameTable, 0, maxColumn, null, "Read", null, null, null, 0);
            StartProgrammSql();
            string[] values = ReadFile().Split(',');
            string[] _values = new string[values.Length - 1];
            for(int i = 0;i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
            DeleteFiles();
            return _values;
        }

        public string[] ReadTableLimit(string nameTable, int maxColumn, int limit) {
            WriteFile(nameTable, 0, maxColumn, null, "ReadLimit", null, null, null, limit);
            StartProgrammSql();
            string[] values = ReadFile().Split(',');
            string[] _values = new string[values.Length - 1];
            for(int i = 0;i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
            DeleteFiles();
            return _values;
        }

        public string[] ReadColumn(string nameTable, int column) {
            WriteFile(nameTable, column, 0, null, "ReadColumn", null, null, null, 0);
            StartProgrammSql();
            string[] values = ReadFile().Split(',');
            string[] _values = new string[values.Length - 1];
            for(int i = 0;i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
            DeleteFiles();
            return _values;
        }

        public string[] ReadColumnWhere(string nameTable, string nameColumn, int maxColumn, string equals) {
            WriteFile(nameTable, 0, maxColumn, equals, "ReadColumnWhere", nameColumn, null, null, 0);
            StartProgrammSql();
            string[] values = ReadFile().Split(',');
            string[] _values = new string[values.Length - 1];
            for(int i = 0;i < _values.Length; i++)
            {
                _values[i] = values[i];
            }
            DeleteFiles();
            return _values;
        }

        public void Delete(string nameTable, string nameColumn, string equals) {
            WriteFile(nameTable, 0, 0, equals, "Delete", nameColumn, null, null, 0);
            StartProgrammSql();
            DeleteFiles();
        }

        public void Insert(string nameTable, string[] nameColumns, string[] valuesForInsert) {
            string columns = "";
            string values = "";
            foreach(string name in nameColumns)
            {
                columns += name + ",";
            }
            foreach(string value in valuesForInsert)
            {
                values += value + ",";
            }
            columns = columns.Remove(columns.Length - 1);
            values = values.Remove(values.Length - 1);
            WriteFile(nameTable, 0, 0, null, "Insert", null, columns, values, 0);
            StartProgrammSql();
            DeleteFiles();
        }

        private static void WriteFile(string nameTable, int column, int maxColumn, string where, string type, string nameColumn, string nameColumns, string values, int limit)
        {
            FileStream fileScript = new FileStream("lib\\sql\\load.txt", FileMode.CreateNew);
            string text = $"host={host};user={user};password={password};nameTable={nameTable};column={column};maxColumn={maxColumn};where={(where == null ? " " : where)};type={type};nameColumn={(nameColumn == null ? " " : nameColumn)};" +
                $"nameColumns={(nameColumns == null ? " " : nameColumns)};values={(values == null ? " " : values)};limit={limit};";
            Random random = new Random();
            string finalText = "";
            foreach(char _char in text.ToCharArray()) {
                finalText += _char + ecod[random.Next(ecod.Length)];
            }
            fileScript.Write(Encoding.UTF8.GetBytes(finalText), 0, finalText.Length);
            fileScript.Close();
        }

        private static string ReadFile()
        {
            string text = File.ReadAllText("lib\\sql\\write.txt");
            string finalText = "";
            bool readChar = false;
            foreach(char _char in text.ToCharArray())
            {
                readChar = !readChar;
                if (readChar) finalText += _char;
            }
            return finalText;
        }

        private static void StartProgrammSql()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.Arguments = @"/C cd " + AppDomain.CurrentDomain.BaseDirectory + "/lib/sql & start.bat";
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            myProcess.WaitForExit();
        }

        private static void DeleteFiles()
        {
            if (File.Exists("lib\\sql\\load.txt")) File.Delete("lib\\sql\\load.txt");
            if (File.Exists("lib\\sql\\write.txt")) File.Delete("lib\\sql\\write.txt");
        }
    }
}
