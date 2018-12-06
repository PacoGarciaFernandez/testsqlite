using System;
using System.Collections;

namespace biodanza {

  public class ItuneList {

    public int TrackId { get; set; }
        

    public override string ToString() {
      return string.Format("Track ID: {0}", TrackId);
    }

    public ItuneList Copy() {
      return MemberwiseClone() as ItuneList;
    }

    protected bool Equals(ItuneList other) {
            return TrackId== other.TrackId;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((PlayList)obj);
    }

    public override int GetHashCode() {
      unchecked {
        var hashCode = TrackId;
        return hashCode;
      }
    }
  }

}
