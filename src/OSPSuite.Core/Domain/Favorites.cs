using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class Favorites : IEnumerable<string>
   {
      private readonly HashSet<string> _favorites = new HashSet<string>();

      /// <summary>
      /// Adds the <paramref name="favorites"/> to the managed list of favorites in the current project
      /// </summary>
      public virtual void AddFavorites(IEnumerable<string> favorites)
      {
         favorites.Each(Add);
      }

      public virtual void Add(string favorite)
      {
         _favorites.Add(favorite);
      }

      public virtual void Remove(string favorite)
      {
         _favorites.Remove(favorite);
      }

      public virtual void Clear()
      {
         _favorites.Clear();
      }

      public virtual IEnumerator<string> GetEnumerator()
      {
         return _favorites.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}