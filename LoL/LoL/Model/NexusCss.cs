using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
        private string[] _champs28;
        private Dictionary<String, CssItem> _items = new Dictionary<String, CssItem>();

        public void Load()
        {
            try
            {
                Regex regex = new Regex(@"//([-\w\.]+)+(:\d+)?(/([-\w/_\.]*(\?\S+)?)?)?");
                Regex rchampname = new Regex(@"\.([a-z]+) ");
                Regex rpos = new Regex(@"(?:\d*\.)?\d+");
                using (WebClient client = new WebClient())
                {
                    _items.Clear();

                    _champs28 = client.DownloadString("http://media-copper.cursecdn.com/avatars/sprites/champions-lol-28/champions-lol-28.css").Split("\n".ToCharArray());
                    Match m = regex.Match(_champs28[_champs28.Length - 3]);
                    if (m.Success)
                    {
                        String val = "http:" + m.Value;
                        _allChampsImage = Utils.LoadImageFromURL(val);

                    }
                    for (int i = 0; i < _champs28.Length - 4; i++)
                    {

                        CssItem ci = new CssItem();

                        MatchCollection mc = rpos.Matches(_champs28[i]);
                        if (mc.Count > 2)
                        {
                            ci.X = int.Parse(mc[1].Value);
                            ci.Y = int.Parse(mc[2].Value);
                        }

                        Match m1 = rchampname.Match(_champs28[i]);
                        if (m1.Success)
                        {
                            _items.Add(m1.Value.TrimStart(".".ToArray()).TrimEnd(" ".ToCharArray()), ci);
                        }
                    }
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

            var bitmapSource = BitmapSource.Create(28, 28, 96, 96, _allChampsImage.Format, null, data, stride);
            
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        
        }
    }
}
