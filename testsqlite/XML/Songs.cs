using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biodanza
{
    class Song
          {
        public Int32    TrackID            { get; set; }
        public string   Name               { get; set; }
        public string   Genere             { get; set; }
        public string   Kind               { get; set; }
        public Int32    Size               { get; set; }
        public Int32    TotalTime          { get; set; }
        public DateTime DateAdded          { get; set; }
        public DateTime DateModified       { get; set; }
        public Int32    BitRate            { get; set; }
        public Int32    SampleRate         { get; set; }
        public string   PersistentID       { get; set; }
        public string   TrackType          { get; set; }
        public string   Location           { get; set; }
        public Int32    FileFolderCount    { get; set; }
        public Int32    LibraryFolderCount { get; set; }


    }
}
