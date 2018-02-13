namespace OSPSuite.Core.Events
{
   public class AddParameterToFavoritesEvent
   {
      public string ParameterPath { get; private set; }

      public AddParameterToFavoritesEvent(string parameterPath)
      {
         ParameterPath = parameterPath;
      }
   }

   public class RemoveParameterFromFavoritesEvent
   {
      public string ParameterPath { get; private set; }

      public RemoveParameterFromFavoritesEvent(string parameterPath)
      {
         ParameterPath = parameterPath;
      }
   }

   public class FavoritesLoadedEvent
   {
      
   }

   public class FavoritesOrderChangedEvent
   {

   }
}