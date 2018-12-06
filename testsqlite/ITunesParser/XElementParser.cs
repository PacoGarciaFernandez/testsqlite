using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace biodanza {
  public static class XElementParser {
    public static bool ParseBoolean(XElement track, string keyValue) {
      return (from keyNode in track.Descendants("key")
        where keyNode.Value == keyValue
        select (keyNode.NextNode as XElement).Name).FirstOrDefault() == "true";
    }

    public static string ParseStringValue(XElement track, string keyValue) {
      return (from key in track.Descendants("key")
        where key.Value == keyValue
        select (key.NextNode as XElement).Value).FirstOrDefault();
    }

    public static long ParseLongValue(XElement track, string keyValue) {
      var stringValue = ParseStringValue(track, keyValue);
      return long.Parse(stringValue);
    }

    public static int? ParseNullableIntValue(XElement track, string keyValue) {
      var stringValue = ParseStringValue(track, keyValue);
      return String.IsNullOrEmpty(stringValue) ? (int?)null : int.Parse(stringValue);
    }

    public static int ParseIntValue(XElement track, string keyValue)  {
        var stringValue = ParseStringValue(track, keyValue);
        return int.Parse(stringValue);
    }
    public static DateTime? ParseNullableDateValue(XElement track, string keyValue) {
            var stringValue = ParseStringValue(track, keyValue);
            return String.IsNullOrEmpty(stringValue) ? (DateTime?)null : DateTime.SpecifyKind(DateTime.Parse(stringValue, CultureInfo.InvariantCulture), DateTimeKind.Utc).ToLocalTime();
    }
    public static List<Int32> ParseArray(XElement track, string keyValue)   {
            return (from key in track.Descendants("array").Descendants("dict").Descendants("key") 
                    where key.Value== keyValue
                    select Int32.Parse((key.NextNode as XElement).Value)).ToList<Int32>();
        }


    }
}