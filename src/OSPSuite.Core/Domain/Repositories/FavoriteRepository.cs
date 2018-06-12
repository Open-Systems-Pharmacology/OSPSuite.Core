using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Repositories
{
   public class FavoriteRepository : IFavoriteRepository
   {
      private readonly IProjectRetriever _projectRetriever;

      public FavoriteRepository(IProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IEnumerable<string> All()
      {
         return Favorites;
      }

      public void AddFavorite(string parameterPath)
      {
         Favorites.Add(parameterPath);
      }

      public void AddFavorites(IEnumerable<string> favorites)
      {
         Favorites.AddFavorites(favorites);
      }

      public void RemoveFavorite(string parameterPath)
      {
         Favorites.Remove(parameterPath);
      }

      public bool Contains(IObjectPath parameterPath)
      {
         return Favorites.Contains(parameterPath.PathAsString);
      }

      public void Clear()
      {
         Favorites.Clear();
      }

      public Favorites Favorites => _projectRetriever.CurrentProject?.Favorites ?? new Favorites();
   }
}