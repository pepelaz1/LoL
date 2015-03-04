using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LoL.Model
{
    public class NexusCss
    {
        private BitmapImage _allChampsImage;
        private string[] _champs28;
        public void Load()
        {
            Regex regex = new Regex(@"//([-\w\.]+)+(:\d+)?(/([-\w/_\.]*(\?\S+)?)?)?");            
            using (WebClient client = new WebClient())
            {
                _champs28 = client.DownloadString("http://media-copper.cursecdn.com/avatars/sprites/champions-lol-28/champions-lol-28.css").Split("\n".ToCharArray());
                Match m = regex.Match(_champs28[_champs28.Length-3]);
                if (m.Success)
                {
                    String val = "http:" + m.Value;
                    _allChampsImage = Utils.LoadImageFromURL(val);

                }
                //_allChampsImage = Utils.
            }
        }

        public BitmapImage GetChampImage(String champName)
        {
            return new BitmapImage();
        }
    }
}
