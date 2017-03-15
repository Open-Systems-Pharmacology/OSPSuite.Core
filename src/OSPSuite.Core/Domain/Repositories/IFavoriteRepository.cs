using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Repositories
{
   public interface IFavoriteRepository : IRepository<string>
   {
      void AddFavorite(string favorite);
      void AddFavorites(IEnumerable<string> favorites);
      void RemoveFavorite(string favorite);
      bool Contains(IObjectPath parameterPath);
      void Clear();
      Favorites Favorites { get; }
   }
}