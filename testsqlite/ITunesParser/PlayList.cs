using System;
using System.Collections;
using System.Collections.Generic;

namespace biodanza {

  public class PlayList {

    public string Name                 { get; set; }
    public bool   Master               { get; set; }
    public int    PlayListID           { get; set; }
    public string PlaylistPersistentID { get; set; }
    public string ParentPersistentID   { get; set; }
    public bool   Visible              { get; set; }
    public bool   AllItems             { get; set; }
    public bool   Movies               { get; set; }
    public bool   Music                { get; set; }
    public bool   TVShows              { get; set; }
    public bool   PodCasts             { get; set; }
    public bool   iTunesU              { get; set; }
    public bool   Audiobooks           { get; set; }
    public bool   Books                { get; set; }
    public bool   Folder               { get; set; }
    public int?   DistinguishedKind    { get; set; }
    public List<Int32> PlaylistItems   { get; set; }

    


    public override string ToString() {
      return string.Format("Name: {0} - PlayListID: {1} - Folder: {2} - {3}", Name, PlayListID, Folder, Folder? "Parent: " + ParentPersistentID:"");
    }

    public PlayList Copy() {
      return MemberwiseClone() as PlayList;
    }

    protected bool Equals(PlayList other) {
      return string.Equals(Name, other.Name) && 
                   Master == other.Master && 
                   PlayListID == other.PlayListID &&
                   PlaylistPersistentID == other.PlaylistPersistentID &&
                   ParentPersistentID == other.ParentPersistentID &&
                   Folder == other.Folder;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((PlayList)obj);
    }

    public override int GetHashCode() {
      unchecked {
        var hashCode = PlayListID;
        hashCode = (hashCode * 397) ^ Name                .GetHashCode();
        hashCode = (hashCode * 397) ^ Master              .GetHashCode();
        hashCode = (hashCode * 397) ^ PlayListID          .GetHashCode();
        hashCode = (hashCode * 397) ^ PlaylistPersistentID.GetHashCode();
        hashCode = (hashCode * 397) ^ ParentPersistentID  .GetHashCode();
        hashCode = (hashCode * 397) ^ Visible             .GetHashCode();
        hashCode = (hashCode * 397) ^ AllItems            .GetHashCode();
        hashCode = (hashCode * 397) ^ DistinguishedKind   .GetHashCode();
        hashCode = (hashCode * 397) ^ Movies              .GetHashCode();
        hashCode = (hashCode * 397) ^ TVShows             .GetHashCode();
        hashCode = (hashCode * 397) ^ PodCasts            .GetHashCode();
        hashCode = (hashCode * 397) ^ iTunesU             .GetHashCode();
        hashCode = (hashCode * 397) ^ Audiobooks          .GetHashCode();
        hashCode = (hashCode * 397) ^ Books               .GetHashCode();
        hashCode = (hashCode * 397) ^ Folder              .GetHashCode();
        hashCode = (hashCode * 397) ^ PlaylistItems       .GetHashCode();
        return hashCode;
      }
    }
  }

}
