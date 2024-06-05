using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Xml;

namespace Bubla
{
    public class Text
    {
        private static Dictionary<string, Text> texts = new Dictionary<string, Text>();

        private SFML.Graphics.Text sfmlText;
        private string name;
        private string text;
        private Vector2f position;
        private uint size;

        private bool active;
        private bool veryUpLayer;

        public Text()
        { }

            public Text(string name, Vector2f position, uint size, string text) {
            this.name = name;
            this.size = size;
            this.text = text;
            this.position = position;
            this.active = true;

            SFML.Graphics.Text textInGame = new SFML.Graphics.Text();
            try
            {
                textInGame.Font = new Font("assets\\font.ttf");
            } catch { textInGame.Font = new Font("assets\\font.bubla"); }
            textInGame.DisplayedString = text;
            textInGame.CharacterSize = size;
            textInGame.Position = position;
            sfmlText = textInGame;
            texts.Add(name, this);
        }

        public void CopyCreate(string name)
        {
            new Text(name, position, size, text);
        }

        public void Delete(string name)
        {
            texts.Remove(name);
        }

        public static void Clear()
        {
            foreach (KeyValuePair<string, Text> listTexts in texts)
            {
                Text text = listTexts.Value;
                text.sfmlText.Dispose();
            }
            texts.Clear();
        }

        public Text Get(string name)
        {
            try
            {
                return texts[name];
            }
            catch { return null; }
        }

        public SFML.Graphics.Text SfmlText
        {
            get { return sfmlText; }
        }

        public string Name
        {
            get { return name; }
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public void SetColor(byte r, byte g, byte b) {
            sfmlText.FillColor = new Color(r, g, b);
        }

        public Position GetPosition()
        {
            return new Position(sfmlText.Position.X, sfmlText.Position.Y);
        }

        public int GetVisibility()
        {
            return sfmlText.FillColor.A;
        }

        public void SetVisibility(int alpha)
        {
            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;
            sfmlText.FillColor = new Color(sfmlText.FillColor.R, sfmlText.FillColor.G, sfmlText.FillColor.B, (byte)alpha);
        }

        public string TextFull
        {
            get { return text; }
            set {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                string textUtf8 = Encoding.UTF8.GetString(bytes);
                Console.OutputEncoding = Encoding.UTF8;
                string textFinal = "";
                string check = "";
                foreach(char _char in textUtf8.ToCharArray())
                {
                    check = Utf8ToRussianSmallWords(_char);
                    if(check == "") check = Utf8ToRussianBigWords(_char);
                    if(check == "") textFinal += _char;
                    else textFinal += check;
                }
                sfmlText.DisplayedString = textFinal;
                text = textFinal;
            }
        }

        public Vector2f Position
        {
            get { return position; }
            set { position = value; }
        }

        public void AddPosition(float x, float y)
        {
            sfmlText.Position = new Vector2f(sfmlText.Position.X + x, sfmlText.Position.Y + y);
        }

        public void SetPosition(float x, float y)
        {
            sfmlText.Position = new Vector2f(x, y);
        }

        public void SetSize(int size)
        {
            sfmlText.CharacterSize = (uint)size;
        }

        public string GetText()
        {
            return TextFull;
        }

        public void SetText(string text)
        {
            TextFull = text.Replace("\\n", "\n");
        }

        public void SetVeryUpLayer(bool value)
        {
            veryUpLayer = value;
        }

        public bool GetVeryUpLayer()
        {
            return veryUpLayer;
        }

        public uint Size
        {
            get { return size; }
            set { size = value; }
        }

        public static Dictionary<string, Text> GetTexts()
        {
            return texts;
        }

        private string Utf8ToRussianSmallWords(char _char)
        {
            switch (_char)
            {
                case 'à':
                    return "а";

                case 'á':
                    return "б";

                case 'â':
                    return "в";

                case 'ã':
                    return "г";

                case 'ä':
                    return "д";

                case 'å':
                    return "е";

                case '¸':
                    return "ё";

                case 'æ':
                    return "ж";

                case 'ç':
                    return "з";

                case 'è':
                    return "и";

                case 'é':
                    return "й";

                case 'ê':
                    return "к";

                case 'ë':
                    return "л";

                case 'ì':
                    return "м";

                case 'í':
                    return "н";

                case 'î':
                    return "о";

                case 'ï':
                    return "п";

                case 'ð':
                    return "р";

                case 'ñ':
                    return "с";

                case 'ò':
                    return "т";

                case 'ó':
                    return "у";

                case 'ô':
                    return "ф";

                case 'õ':
                    return "х";

                case 'ö':
                    return "ц";

                case '÷':
                    return "ч";

                case 'ø':
                    return "ш";

                case 'ù':
                    return "щ";

                case 'ü':
                    return "ъ";

                case 'û':
                    return "ы";

                case 'ú':
                    return "ь";

                case 'ý':
                    return "э";

                case 'þ':
                    return "ю";

                case 'ÿ':
                    return "я";

                case '�':
                    return "?";

                default:
                    return "";

            }
        }

        private string Utf8ToRussianBigWords(char _char)
        {
            switch (_char)
            {
                case 'À':
                    return "А";
                case 'Á':
                    return "Б";
                case 'Â':
                    return "В";
                case 'Ã':
                    return "Г";
                case 'Ä':
                    return "Д";
                case 'Å':
                    return "Е";
                case '¨':
                    return "Ё";
                case 'Æ':
                    return "Ж";
                case 'Ç':
                    return "З";
                case 'È':
                    return "И";
                case 'É':
                    return "Й";
                case 'Ê':
                    return "К";
                case 'Ë':
                    return "Л";
                case 'Ì':
                    return "М";
                case 'Í':
                    return "Н";
                case 'Î':
                    return "О";
                case 'Ï':
                    return "П";

                case 'Ð':
                    return "Р";

                case 'Ñ':
                    return "С";

                case 'Ò':
                    return "Т";

                case 'Ó':
                    return "У";

                case 'Ô':
                    return "Ф";

                case 'Õ':
                    return "Х";

                case 'Ö':
                    return "Ц";

                case '×':
                    return "Ч";

                case 'Ø':
                    return "Ш";

                case 'Ù':
                    return "Щ";

                case 'Ü':
                    return "Ъ";

                case 'Û':
                    return "Ы";

                case 'Ú':
                    return "Ь";

                case 'Ý':
                    return "Э";

                case 'Þ':
                    return "Ю";

                case 'ß':
                    return "Я";
                default:
                    return "";
            }
        }
    }
}
