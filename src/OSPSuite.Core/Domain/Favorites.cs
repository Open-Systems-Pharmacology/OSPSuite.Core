using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class Favorites : IEnumerable<string>
   {
      private readonly List<string> _favorites = new List<string>();

      /// <summary>
      ///    Adds the <paramref name="favorites" /> to the managed list of favorites in the current project
      /// </summary>
      public virtual void AddFavorites(IEnumerable<string> favorites)
      {
         favorites.Each(Add);
      }

      public virtual void Add(string favorite)
      {
         if (Contains(favorite))
            return;

         _favorites.Add(favorite);
      }

      public bool Contains(string favorite)
      {
         return _favorites.Contains(favorite);
      }

      public virtual void Remove(string favorite)
      {
         _favorites.Remove(favorite);
      }

      public virtual void Clear()
      {
         _favorites.Clear();
      }

      public virtual void Move(IEnumerable<string> entriesToMove, int delta)
      {
         var initialLength = _favorites.Count;
         if (initialLength == 0)
            return;

         var detlaToUse = delta % initialLength;

         var favoritesToMoves = entriesToMove.Select(x => _favorites.IndexOf(x))
            .Where(index => index >= 0)
            .Select(index => new {index, favorite = _favorites[index], targetIndex = targetIndexFor(index, detlaToUse, initialLength)})
            .OrderBy(x => x.targetIndex)
            .ToList();
         
         favoritesToMoves.Each(x => Remove(x.favorite));

         favoritesToMoves.Each(x => _favorites.Insert(x.targetIndex, x.favorite));
      }

      private int targetIndexFor(int currentIndex, int delta, int length)
      {
         var newIndex = currentIndex + delta;
         if (newIndex < 0)
            newIndex += length;

         if (newIndex >= length)
            newIndex -= length;

         return newIndex;
      }

      public virtual void MoveUp(params string[] entriesToMove) => MoveUp(entriesToMove.ToList());

      public virtual void MoveUp(IEnumerable<string> entriesToMove) => Move(entriesToMove, -1);

      public virtual void MoveDown(IEnumerable<string> entriesToMove) => Move(entriesToMove, 1);

      public virtual void MoveDown(params string[] entriesToMove) => MoveDown(entriesToMove.ToList());

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