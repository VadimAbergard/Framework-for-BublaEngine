using Box2DX.Collision;
using Bubla;
using MyFramework.framefork.physics;
using MyFramework.utils;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FrameworkEngine.framefork.world
{
    public class TiledMap
    {
        private static List<Sprite> sprites = new List<Sprite>();
        private static List<Collider> colliders = new List<Collider>();
        public TiledMap() { } // for lua script

        public void load(string nameTmxFile, float _size)
        {
            float size = _size + 30;

            XmlDocument xDoc = new XmlDocument();
            string pathXmlBubla = $"assets\\{nameTmxFile}.bubla";
            //string pathXml = $"assets\\{nameTmxFile}.tmx";

            //try
            //Console.WriteLine(File.ReadAllText(pathXmlBubla));
            //{
            xDoc.Load(pathXmlBubla);
                //xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXmlBubla))));
            //}
            //catch (FileNotFoundException e) { xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXml)))); }

            int widthMap = int.Parse(xDoc.DocumentElement.GetAttribute("width"));
            int heightMap = int.Parse(xDoc.DocumentElement.GetAttribute("height"));
            int tilewidth = int.Parse(xDoc.DocumentElement.GetAttribute("tilewidth"));
            int tileheight = int.Parse(xDoc.DocumentElement.GetAttribute("tileheight"));

            TsxFileInfo tsxFileInfo = null;
            List<Texture> textures = new List<Texture>();


            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                if(xnode.Name.Equals("tileset"))
                {
                    int firstgid = int.Parse(xnode.GetAttribute("firstgid"));
                    string source = xnode.GetAttribute("source").Replace(".tsx", "");

                    tsxFileInfo = readTsxFile(source);
                    int column = -1;
                    int row = 0;
                    
                    for(int i = 0; i < tsxFileInfo.tileCount;i++)
                    {
                        //Console.WriteLine($"{tsxFileInfo.tileWidth * row}, {tsxFileInfo.tileHeight * column}, {tsxFileInfo.tileWidth}, {tsxFileInfo.tileHeight})");
                        
                        try
                        {
                            
                            textures.Add(new Texture(tsxFileInfo.imageSource, new IntRect(tsxFileInfo.tileWidth * column, tsxFileInfo.tileHeight * row, tsxFileInfo.tileWidth, tsxFileInfo.tileHeight)));
                            //Console.WriteLine($"map loading: load texture {i + 1}/{tsxFileInfo.tileCount} sourse:{tsxFileInfo.imageSource}");
                        }
                        catch {
                            i--;
                            row++;
                            column = 0;
                            //Console.WriteLine($"map loading: load texture {i + 1}/{tsxFileInfo.tileCount} sourse:{tsxFileInfo.imageSource}       error load!!!!!!");
                            continue; 
                        }

                        column++;
                        if(column == tsxFileInfo.columns)
                        {
                            row++;
                            column = 0;
                        }
                    }
                }
                if (xnode.Name.Equals("objectgroup"))
                {
                    foreach (XmlNode childnodeObjectgroup in xnode.ChildNodes)
                    {
                        Objects obj = new Objects();
                        obj.x = float.Parse(childnodeObjectgroup.Attributes["x"].InnerText, CultureInfo.InvariantCulture);
                        obj.y = float.Parse(childnodeObjectgroup.Attributes["y"].InnerText, CultureInfo.InvariantCulture);
                        obj.width = float.Parse(childnodeObjectgroup.Attributes["width"].InnerText, CultureInfo.InvariantCulture);
                        obj.height = float.Parse(childnodeObjectgroup.Attributes["height"].InnerText, CultureInfo.InvariantCulture);

                        colliders.Add(new Collider(new Vector2f(size + obj.width * 1.5f, size + obj.height * 1.5f ),
                            new Vector2f(obj.x * 2f + obj.width / 2 + size, obj.y * 2 + obj.height * 1.5f + size), 0, true, 0, "square", false));
                    }
                }
                if (xnode.Name.Equals("layer"))
                {
                    foreach (XmlNode childnodeLayer in xnode.ChildNodes)
                    {
                        if(childnodeLayer.Name.Equals("data"))
                        {
                            string[] intChars = childnodeLayer.InnerText.Split(',');
                            int[] idTiles = new int[intChars.Length];
                            int i = 0;
                            foreach(string number in intChars)
                            {
                                idTiles[i++] = int.Parse(number);
                            }

                            int column = 0;
                            int row = 0;
                            int countProgress = 0;
                            int t = 0;
                            foreach (int id in idTiles)
                            {
                                if (id == 0)
                                {
                                    //sprites.Add(new Sprite());
                                    column++;
                                    countProgress++;
                                    if (column == widthMap)
                                    {
                                        row++;
                                        column = 0;
                                    }
                                    continue;
                                }
                                Sprite sprite = new Sprite();
                                try
                                {
                                    sprite.Texture = textures[id];
                                    sprite.Position = new Vector2f(column * (tilewidth + size), row * (tileheight + size));
                                    sprite.Scale = new Vector2f((tilewidth + size) / sprite.Texture.Size.X, (tileheight + size) / sprite.Texture.Size.Y);
                                    if (xnode.HasAttribute("opacity")) sprite.Color = new SFML.Graphics.Color(255, 255, 255, (byte)(float.Parse(xnode.GetAttribute("opacity"), CultureInfo.InvariantCulture) * 255));
                                }
                                catch { }
                                try
                                {
                                    //if(t++ < 6000)
                                    for (int k = 0; k < tsxFileInfo.tiles.Count; k++)
                                    {
                                        if(tsxFileInfo.tiles[k].id == id - 1) {
                                            for (int j = 0; j < tsxFileInfo.tiles[k].objects.Count; j++)
                                            {
                                                Objects obj = tsxFileInfo.tiles[k].objects[j];

                                                    Console.WriteLine(new Vector2f(size + obj.width, size + obj.height).ToString());
                                                    Console.WriteLine(new Vector2f(obj.x + obj.width * 2 + column * (tilewidth + size), obj.y + obj.height * 2 + row * (tileheight + size)).ToString());

                                                    colliders.Add(new Collider(new Vector2f(size + obj.width, size + obj.height),
                                                        new Vector2f(obj.x + obj.width * 2 + column * (tilewidth + size), obj.y + obj.height * 2 + row * (tileheight + size)), 0, true, 0, "square", false));
                                            }
                                        }
                                    }
                                }
                                catch (ArgumentOutOfRangeException ignored) { }

                                sprites.Add(sprite);
                                //Console.WriteLine($"load map {countProgress}/{tsxFileInfo.tileCount}    id:{id}");

                                column++;
                                countProgress++;
                                if (column == widthMap)
                                {
                                    row++;
                                    column = 0;
                                }
                            }
                        }
                    }
                }
            }

        }

        public static List<Sprite> GetSpites()
        {
            return sprites;
        }

        public static List<Collider> GetColliders()
        {
            return colliders;
        }

        private TsxFileInfo readTsxFile(string nameTsxFile) {
            TsxFileInfo tsxFileInfo = new TsxFileInfo();

            XmlDocument xDoc = new XmlDocument();
            string pathXmlBubla = $"assets\\{nameTsxFile}.bubla";
            //string pathXml = $"assets\\{nameTsxFile}.tsx";

            //try
            //{
                xDoc.Load(pathXmlBubla);
                //xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXmlBubla))));
            //}
            //catch (FileNotFoundException e) { xDoc.Load(new StringReader(Encryption.DencrypText(File.ReadAllText(pathXml)))); }


            XmlElement xRoot = xDoc.DocumentElement;
            tsxFileInfo.tileWidth = int.Parse(xRoot.GetAttribute("tilewidth"));
            tsxFileInfo.tileHeight = int.Parse(xRoot.GetAttribute("tileheight"));
            tsxFileInfo.tileCount = int.Parse(xRoot.GetAttribute("tilecount"));
            tsxFileInfo.columns = int.Parse(xRoot.GetAttribute("columns"));
            foreach (XmlElement xnode in xRoot)
            {
                if(xnode.Name.Equals("image"))
                {
                    tsxFileInfo.imageWidth = int.Parse(xnode.GetAttribute("width"));
                    tsxFileInfo.imageHeight = int.Parse(xnode.GetAttribute("height"));

                    string imageSourse = xnode.GetAttribute("source");
                    char[] imageSourseChars = imageSourse.ToCharArray();
                    string finalSourse = "assets\\";

                    for(int i = imageSourseChars.Length - 1; 0 < i;i--)
                    {
                        if (imageSourseChars[i] == '/')
                        {
                            i++;
                            for(int k = i; k < imageSourseChars.Length;k++)
                            {
                                if(imageSourseChars[k] == '.')
                                {
                                    finalSourse += ".bubla";
                                    break;
                                }
                                finalSourse += imageSourseChars[k];
                            }
                            break;
                        }
                    }

                    tsxFileInfo.imageSource = finalSourse;
                }
                else if(xnode.Name.Equals("tile"))
                {
                    tsxFileInfo.addTile(new Tile(int.Parse(xnode.GetAttribute("id"))));
                    foreach(XmlNode childnodeTile in xnode.ChildNodes)
                    {
                        if (childnodeTile.Name.Equals("objectgroup"))
                        {
                            foreach (XmlNode childnodeTileObjects in childnodeTile.ChildNodes)
                            {
                                //try
                                //{
                                    Objects obj = new Objects();
                                    obj.x = float.Parse(childnodeTileObjects.Attributes["x"].InnerText, CultureInfo.InvariantCulture);
                                    obj.y = float.Parse(childnodeTileObjects.Attributes["y"].InnerText, CultureInfo.InvariantCulture);
                                    obj.width = float.Parse(childnodeTileObjects.Attributes["width"].InnerText, CultureInfo.InvariantCulture);
                                    obj.height = float.Parse(childnodeTileObjects.Attributes["height"].InnerText, CultureInfo.InvariantCulture);
                                    tsxFileInfo.tiles[tsxFileInfo.tiles.Count - 1].addObject(obj);
                                //} catch { }
                            }
                        }
                    }
                }
            }

            return tsxFileInfo;
        }
    }
}
class TsxFileInfo
{
    public int tileWidth { get; set; }
    public int tileHeight { get; set; }
    public int tileCount { get; set; }
    public int columns { get; set; }
    public string imageSource { get; set; }
    public int imageWidth { get; set; }
    public int imageHeight { get; set; }

    private List<Tile> tile = new List<Tile>();
    public List<Tile> tiles { get { return tile; } }

    public void addTile(Tile tile)
    {
        this.tile.Add(tile);
    }

}

class Tile
{
    public Tile(int id)
    {
        this.id = id;
    }
    public int id { get; set; }

    private List<Objects> _object = new List<Objects>();
    public List<Objects> objects { get { return _object; } }

    public void addObject(Objects obj)
    {
        this._object.Add(obj);
    }
}

class Objects
{
    public float width { get; set; }
    public float height { get; set; }
    public float x { get; set; }
    public float y { get; set; }

}