using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class PlayListRecord
    {
        Dictionary<string,Action<string>> functions;

        public PlayListRecord()
        {
            functions = new Dictionary<string,Action<string>>();
            functions.Add("Name", ExtractName);
            functions.Add("Master", ExtractMaster);
        }
        public void ExtractName(string line)
        {
            string token1 = "<key>Name</key><string>";
            string token2 = "</string>";
            string Name = line.Substring(token1.Length, line.Length-line.IndexOf(token2));
        }
        public void ExtractMaster(string line)
        {
            string token1 = "<key>Master</key>";
            
            string Master = line.Substring(token1.Length+1,4);
        }
        public void ExtractPlayListID(string line)
        {
            string token1 = "<key>Playlist ID</key><integer>";
            string token2 = "</integer>";
            string PlayListID = line.Substring(token1.Length, line.Length - line.IndexOf(token2));
        }
        public void ExtractPlayListPersistetID(string line)
        {
            string token1 = "<key>Playlist Persistent ID</key><string>";
            string token2 = "</string>";
            string PlayListPersistentID = line.Substring(token1.Length, line.Length - line.IndexOf(token2));
        }

        public void ExtractVisible(string line)
        {
            string token1 = "<key>Visible</key>";

            string Visible = line.Substring(token1.Length + 1, 4);
        }
        
        public void ExtractAllItems(string line)
        {
            string token1 = "<key>All Items</key>";
            string AllItems = line.Substring(token1.Length + 1, 4);
        }
        
        public void ExtractTrackID(string line)
        {
            string token1 = "<key>Track ID</key><integer>";
            string token2 = "</integer>";
            string TrackID = line.Substring(token1.Length, line.Length - line.IndexOf(token2));
        }








    }
}
