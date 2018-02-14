using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_FavoriteTask : ContextSpecification<IFavoriteTask>
   {
      protected IParameter _parameter;
      protected IEventPublisher _eventPublisher;
      protected IFavoriteRepository _favoriteRepository;
      protected IEntityPathResolver _entityPathResolver;
      protected ISerializationTask _serializationTask;
      protected IDialogCreator _dialogCreator;
      protected Favorites _favorites;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _serializationTask = A.Fake<ISerializationTask>();
         _dialogCreator = A.Fake<IDialogCreator>();

         sut = new FavoriteTask(_favoriteRepository, _entityPathResolver, _eventPublisher, _dialogCreator, _serializationTask);

         _favorites = new Favorites();
         A.CallTo(() => _favoriteRepository.Favorites).Returns(_favorites);
      }
   }

   public class When_a_paraemter_is_set_as_favorite : concern_for_FavoriteTask
   {
      private string _parameterPath;

      private AddParameterToFavoritesEvent _event;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter();
         _parameterPath = "TOTO";

         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns(_parameterPath);
         A.CallTo(() => _eventPublisher.PublishEvent(A<AddParameterToFavoritesEvent>._))
            .Invokes(x => _event = x.GetArgument<AddParameterToFavoritesEvent>(0));
      }

      protected override void Because()
      {
         sut.SetParameterFavorite(_parameter, true);
      }

      [Observation]
      public void should_throw_the_event_specifing_that_the_parameter_was_set_as_favorite()
      {
         _event.ParameterPath.ShouldBeEqualTo(_parameterPath);
      }
   }

   public class When_a_paraemter_is_removed_from_the_favorites : concern_for_FavoriteTask
   {
      private string _parameterPath;
      private RemoveParameterFromFavoritesEvent _event;

      protected override void Context()
      {
         base.Context();

         _parameter = new Parameter();
         _parameterPath = "TRALALA";

         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns(_parameterPath);

         A.CallTo(() => _eventPublisher.PublishEvent(A<RemoveParameterFromFavoritesEvent>._))
            .Invokes(x => _event = x.GetArgument<RemoveParameterFromFavoritesEvent>(0));
      }

      protected override void Because()
      {
         sut.SetParameterFavorite(_parameter, false);
      }

      [Observation]
      public void should_throw_the_event_specifing_that_the_parameter_was_remove_from_the_favorites()
      {
         _event.ParameterPath.ShouldBeEqualTo(_parameterPath);
      }
   }

   public class When_saving_the_current_favorites_to_a_file : concern_for_FavoriteTask
   {
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         _fileName = "file";
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.SaveFavoritesToFile, Constants.Filter.FAVORITES_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns(_fileName);
      }

      protected override void Because()
      {
         sut.SaveToFile();
      }

      [Observation]
      public void should_save_the_favorites_into_a_file_selected_by_the_user()
      {
         A.CallTo(() => _serializationTask.SaveModelPart(_favorites, _fileName)).MustHaveHappened();
      }
   }

   public class When_loading_the_current_favorites_from_a_file : concern_for_FavoriteTask
   {
      private string _fileName;
      private Favorites _newFavorites;

      protected override void Context()
      {
         base.Context();
         _fileName = "file";
         A.CallTo(() => _dialogCreator.AskForFileToOpen(Captions.LoadFavoritesFromFile, Constants.Filter.FAVORITES_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns(_fileName);
         _newFavorites = new Favorites();
         A.CallTo(() => _serializationTask.Load<Favorites>(_fileName, false)).Returns(_newFavorites);
      }

      protected override void Because()
      {
         sut.LoadFromFile();
      }

      [Observation]
      public void should_ask_the_user_to_select_a_file_containing_the_favorites_to_load_and_update_the_favorites_defined_in_the_repository()
      {
         A.CallTo(() => _favoriteRepository.AddFavorites(_newFavorites)).MustHaveHappened();
      }

      [Observation]
      public void should_raise_the_favorites_loaded_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<FavoritesLoadedEvent>._)).MustHaveHappened();
      }
   }

   public class When_reordering_favorites_by_moving_some_entries_up : concern_for_FavoriteTask
   {
      private readonly IReadOnlyList<string> _entriesToMove = new[] { "FAV1" };

      protected override void Context()
      {
         base.Context();
         _favorites.AddFavorites(new []{"FAV1", "FAV2" });
      }

      protected override void Because()
      {
         sut.MoveUp(_entriesToMove);
      }

      [Observation]
      public void should_have_reordered_the_favorites()
      {
         _favorites.ShouldOnlyContainInOrder("FAV2", "FAV1");
      }

      [Observation]
      public void should_raise_the_favorites_reordered_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<FavoritesOrderChangedEvent>._)).MustHaveHappened();
      }
   }

   public class When_reordering_favorites_by_moving_some_entries_down : concern_for_FavoriteTask
   {
      private readonly IReadOnlyList<string> _entriesToMove = new[] { "FAV1" };

      protected override void Context()
      {
         base.Context();
         _favorites.AddFavorites(new[] { "FAV1", "FAV2" });
      }

      protected override void Because()
      {
         sut.MoveDown(_entriesToMove);
      }

      [Observation]
      public void should_have_reordered_the_favorites()
      {  
         _favorites.ShouldOnlyContainInOrder("FAV2", "FAV1");
      }

      [Observation]
      public void should_raise_the_favorites_reordered_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<FavoritesOrderChangedEvent>._)).MustHaveHappened();
      }
   }
}