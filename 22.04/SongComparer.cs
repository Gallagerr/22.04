using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _22._04
{
  public class SongComparer : IComparer<Song>
  {
    public int Compare(Song x, Song y)
    {
      if (x.Rating > y.Rating)
      {
        return 1;
      }
      else if (x.Rating == y.Rating)
      {
        return -1;
      }
      return 0;
    }
  }
}
