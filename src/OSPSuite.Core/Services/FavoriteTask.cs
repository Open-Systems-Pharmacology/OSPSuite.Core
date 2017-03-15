using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Services
{
   public interface IFavoriteTask
   {
      void SetParameterFavorite(IParameter parameter, bool isFavorite);
      void SaveToFile();
      void LoadFromFile();
   }

   public class FavoriteTask : IFavoriteTask
   {
      private readonly IFavoriteRepository _favoriteRepository;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDialogCreator _dialogCreator;
      private readonly ISerializationTask _serializationTask;

      public FavoriteTask(IFavoriteRepository favoriteRepository, IEntityPathResolver entityPathResolver, IEventPublisher eventPublisher,
         IDialogCreator dialogCreator, ISerializationTask serializationTask)
      {
         _favoriteRepository = favoriteRepository;
         _entityPathResolver = entityPathResolver;
         _eventPublisher = eventPublisher;
         _dialogCreator = dialogCreator;
         _serializationTask = serializationTask;
      }

      public void SetParameterFavorite(IParameter parameter, bool isFavorite)
      {
         var parameterPath = _entityPathResolver.PathFor(parameter);
         if (isFavorite)
         {
            _favoriteRepository.AddFavorite(parameterPath);
            _eventPublisher.PublishEvent(new AddParameterToFavoritesEvent(parameterPath));
         }
         else
         {
            _favoriteRepository.RemoveFavorite(parameterPath);
            _eventPublisher.PublishEvent(new RemoveParameterFromFavoritesEvent(parameterPath));
         }
      }

      public void SaveToFile()
      {
         var filename = _dialogCreator.AskForFileToSave(Captions.SaveFavoritesToFile, Constants.Filter.FAVORITES_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(filename)) return;
         _serializationTask.SaveModelPart(_favoriteRepository.Favorites, filename);
      }

      public void LoadFromFile()
      {
         var filename = _dialogCreator.AskForFileToOpen(Captions.LoadFavoritesFromFile, Constants.Filter.FAVORITES_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(filename)) return;
         var favoritesFromFiles = _serializationTask.Load<Favorites>(filename);
         if (favoritesFromFiles == null) return;
         _favoriteRepository.Clear();
         _favoriteRepository.AddFavorites(favoritesFromFiles);
         _eventPublisher.PublishEvent(new FavoritesLoadedEvent());
      }
   }
}