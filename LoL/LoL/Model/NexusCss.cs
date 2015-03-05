using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LoL.Model
{
    class CssItem
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class NexusCss
    {
        private BitmapImage _allChampsImage;
        private BitmapImage _allRanksImage;

       // private string[] _champs28;
        private Dictionary<String, CssItem> _items = new Dictionary<String, CssItem>();
        private Dictionary<String, CssItem> _ranks = new Dictionary<String, CssItem>();

        public void Load()
        {
            try
            {
                Regex regex = new Regex(@"//([-\w\.]+)+(:\d+)?(/([-\w/_\.]*(\?\S+)?)?)?");
                Regex rchampname = new Regex(@"\.([a-z-]+) ");
                Regex rpos = new Regex(@"(?:\d*\.)?\d+");
                using (WebClient client = new WebClient())
                {
                    _items.Clear();

                    String []champs28 = client.DownloadString("http://media-copper.cursecdn.com/avatars/sprites/champions-lol-28/champions-lol-28.css").Split("\n".ToCharArray());
                    Match m = regex.Match(champs28[champs28.Length - 3]);
                    if (m.Success)
                    {
                        String val = "http:" + m.Value;
                        _allChampsImage = Utils.LoadImageFromURL(val);

                    }
                    for (int i = 0; i < champs28.Length - 4; i++)
                    {

                        CssItem ci = new CssItem();

                        MatchCollection mc = rpos.Matches(champs28[i]);
                        if (mc.Count > 2)
                        {
                            ci.X = int.Parse(mc[1].Value);
                            ci.Y = int.Parse(mc[2].Value);
                        }

                        Match m1 = rchampname.Match(champs28[i]);
                        if (m1.Success)
                        {
                            _items.Add(m1.Value.TrimStart(".".ToArray()).TrimEnd(" ".ToCharArray()), ci);
                        }
                    }


                    _ranks.Clear();

                    _ranks.Add("bronze-i", new CssItem() { X = 0, Y = 0 });
                    _ranks.Add("bronze-ii", new CssItem() { X = 0, Y = 78 });
                    _ranks.Add("bronze-iii", new CssItem() { X = 0, Y = 156 });
                    _ranks.Add("bronze-iv", new CssItem() { X = 0, Y = 234 });
                    _ranks.Add("bronze-v", new CssItem() { X = 0, Y = 312 });

                    _ranks.Add("challenger-i", new CssItem() { X = 0, Y = 390 });

                    _ranks.Add("master-i", new CssItem() { X = 78, Y = 156 });

                    _ranks.Add("diamond-i", new CssItem() { X = 0, Y = 468 });
                    _ranks.Add("diamond-ii", new CssItem() { X = 0, Y = 546 });
                    _ranks.Add("diamond-iii", new CssItem() { X = 0, Y = 624 });
                    _ranks.Add("diamond-iv", new CssItem() { X = 0, Y = 720 });
                    _ranks.Add("diamond-v", new CssItem() { X = 0, Y = 780 });

                    _ranks.Add("gold-i", new CssItem() { X = 0, Y = 858 });
                    _ranks.Add("gold-ii", new CssItem() { X = 0, Y = 936 });
                    _ranks.Add("gold-iii", new CssItem() { X = 0, Y = 1014 });
                    _ranks.Add("gold-iv", new CssItem() { X = 0, Y = 1092 });
                    _ranks.Add("gold-v", new CssItem() { X = 0, Y = 1170 });

                    _ranks.Add("platinum-i", new CssItem() { X = 0, Y = 1248 });
                    _ranks.Add("platinum-ii", new CssItem() { X = 0, Y = 1326 });
                    _ranks.Add("platinum-iii", new CssItem() { X = 0, Y = 1404 });
                    _ranks.Add("platinum-iv", new CssItem() { X = 0, Y = 1482 });
                    _ranks.Add("platinum-v", new CssItem() { X = 0, Y = 1560 });

                    _ranks.Add("silver-i", new CssItem() { X = 0, Y = 1638 });
                    _ranks.Add("silver-ii", new CssItem() { X = 0, Y = 1716 });
                    _ranks.Add("silver-iii", new CssItem() { X = 0, Y = 1794 });
                    _ranks.Add("silver-iv", new CssItem() { X = 0, Y = 1872 });
                    _ranks.Add("silver-v", new CssItem() { X = 78, Y = 0 });

                    _ranks.Add("unranked", new CssItem() { X = 78, Y = 78 });

                    _allRanksImage = Utils.LoadImageFromURL("http://static-noxia.cursecdn.com/1-0-5533-25805/Skins/Noxia/images/LoL/ranks/28/lol_ranks_28.png");
                }
            }
            catch(Exception ex)
            {
                int t = 4;
                t = 4;
            }
        }

        public BitmapImage GetChampImage(String champName)
        {
            CssItem ci = _items[champName];
            int stride = 28 * (_allChampsImage.Format.BitsPerPixel / 8);
            byte[] data = new byte[stride * 28];
            Int32Rect r = new Int32Rect()
            {
                X = ci.X,
                Y = ci.Y,
                Width = 28,
                Height = 28
            };
            _allChampsImage.CopyPixels(r, data, stride, 0);

            return CreateBitmapImage(data, stride, _allChampsImage.Format);
        }



        public BitmapImage GetRankImage(String rankName)
        {
            CssItem ci = _ranks[rankName];
            int stride = 28 * (_allRanksImage.Format.BitsPerPixel / 8);
            byte[] data = new byte[stride * 28];
            Int32Rect r = new Int32Rect()
            {
                X = ci.X,
                Y = ci.Y,
                Width = 28,
                Height = 28
            };
            _allRanksImage.CopyPixels(r, data, stride, 0);

            return CreateBitmapImage(data, stride, _allRanksImage.Format);
        }


        private BitmapImage CreateBitmapImage(byte[] data, int stride, PixelFormat format)
        {
            var bitmapSource = BitmapSource.Create(28, 28, 96, 96, format, null, data, stride);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bi = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            bi.BeginInit();
            bi.StreamSource = new MemoryStream(memoryStream.ToArray());
            bi.EndInit();

            memoryStream.Close();

            return bi; 
        }
    }
}
