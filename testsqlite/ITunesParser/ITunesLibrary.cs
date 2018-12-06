using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace biodanza {
  public interface IITunesLibrary {
    IEnumerable<Track> Parse(string xmlFileLocation);
  }

  public class ITunesLibrary : IITunesLibrary {

    public IEnumerable<Track> Parse(string fileLocation) {
      var trackElements = LoadTrackElements(fileLocation);
      return trackElements.Select(CreateTrack);
    }

        public IEnumerable<PlayList> ParseList(string fileLocation)
        {
            var playlist = LoadPlayLists(fileLocation);
            var lista = playlist.Select(CreatePlayList);

            return lista;
        }


        private static IEnumerable<XElement> LoadTrackElements(string fileLocation) {
      return from x in XDocument.Load(fileLocation).Descendants("dict").Descendants("dict").Descendants("dict")
                          where x.Descendants("key").Count() > 1
                          select x;
    }

        private static  IEnumerable<XElement> LoadPlayLists(string fileLocation) {
            return from x in XDocument.Load(fileLocation).Descendants("array").Descendants("dict")
                   where x.Descendants("key").Count() > 1 
                   select x;
        }

        //private static IEnumerable<XElement> LoadLists(IEnumerable<List> list)
        //{
        //    return from x in XDocument.Load().Descendants("array").Descendants("dict")//.Descendants("array")//.Descendants("dict").Descendants("key")
        //           where x.Descendants("key").Count() > 1
        //           select x;
        //}


        private Track CreateTrack(XElement trackElement) {
      return new Track {
        TrackId                = Int32.Parse(XElementParser.ParseStringValue(trackElement, "Track ID")),
        Name                   = XElementParser.ParseStringValue(trackElement, "Name"),
        Master                 = XElementParser.ParseBoolean(trackElement, "Master"),
        Artist                 = XElementParser.ParseStringValue(trackElement, "Artist"),
        AlbumArtist            = XElementParser.ParseStringValue(trackElement, "AlbumArtist"),
        Composer               = XElementParser.ParseStringValue(trackElement, "Composer"),
        Album                  = XElementParser.ParseStringValue(trackElement, "Album"),
        Genre                  = XElementParser.ParseStringValue(trackElement, "Genre"),
        Kind                   = XElementParser.ParseStringValue(trackElement, "Kind"),
        Size                   = XElementParser.ParseLongValue(trackElement, "Size"),
        PlayingTime            = TimeConvert.MillisecondsToFormattedMinutesAndSeconds((XElementParser.ParseLongValue(trackElement, "Total Time"))),
        TrackNumber            = XElementParser.ParseNullableIntValue(trackElement, "Track Number"),
        Year                   = XElementParser.ParseNullableIntValue(trackElement, "Year"),
        DateModified           = XElementParser.ParseNullableDateValue(trackElement, "Date Modified"),
        DateAdded              = XElementParser.ParseNullableDateValue(trackElement, "Date Added"),
        BitRate                = XElementParser.ParseNullableIntValue(trackElement, "Bit Rate"),
        SampleRate             = XElementParser.ParseNullableIntValue(trackElement, "Sample Rate"),
        PlayDate               = XElementParser.ParseNullableDateValue(trackElement, "Play Date UTC"),
        PlayCount              = XElementParser.ParseNullableIntValue(trackElement, "Play Count"),
        PartOfCompilation      = XElementParser.ParseBoolean(trackElement, "Compilation"),
        Location               = XElementParser.ParseStringValue(trackElement, "Location"),
        PersistentID           = XElementParser.ParseStringValue(trackElement, "Persistent ID"),
        PlaylistPersistentID   = XElementParser.ParseStringValue(trackElement, "Playlist Persistent ID"),
      };
    }
        private PlayList CreatePlayList(XElement playlistElement)  {
            return new PlayList
            {
                AllItems = XElementParser.ParseBoolean(playlistElement, "All Items"),
                Audiobooks = XElementParser.ParseBoolean(playlistElement, "Audiobooks"),
                Books = XElementParser.ParseBoolean(playlistElement, "Books"),
                DistinguishedKind = XElementParser.ParseNullableIntValue(playlistElement, "Distinguished Kind"),
                Folder = XElementParser.ParseBoolean(playlistElement, "Folder"),
                Master = XElementParser.ParseBoolean(playlistElement, "Master"),
                Movies = XElementParser.ParseBoolean(playlistElement, "Movies"),
                Music = XElementParser.ParseBoolean(playlistElement, "Music"),
                Name = XElementParser.ParseStringValue(playlistElement, "Name"),
                ParentPersistentID = XElementParser.ParseStringValue(playlistElement, "Parent Persistent ID"),
                PlayListID = XElementParser.ParseIntValue(playlistElement, "Playlist ID"),
                PlaylistItems = XElementParser.ParseArray(playlistElement, "Track ID"),
                PlaylistPersistentID = XElementParser.ParseStringValue(playlistElement, "Playlist Persistent ID"),
                PodCasts = XElementParser.ParseBoolean(playlistElement, "PodCasts"),
                TVShows = XElementParser.ParseBoolean(playlistElement, "TV Shows"),
                Visible = XElementParser.ParseBoolean(playlistElement, "Visible"),
                iTunesU = XElementParser.ParseBoolean(playlistElement, "iTunesU"),
         };
      }
    }
}
