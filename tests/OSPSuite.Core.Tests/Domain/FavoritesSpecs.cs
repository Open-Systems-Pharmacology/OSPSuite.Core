using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Favorites : ContextSpecification<Favorites>
   {
      protected string _fav1 = "FAV_1";
      protected string _fav2 = "FAV_2";
      protected string _fav3 = "FAV_3";
      protected string _fav4 = "FAV_4";

      protected override void Context()
      {
         sut = new Favorites();
         sut.AddFavorites(new[] {_fav1, _fav2, _fav3, _fav4,});
      }
   }

   public class When_moving_one_favorite_entry_up : concern_for_Favorites
   {
      [Observation]
      public void should_reoder_the_index_as_expected()
      {
         sut.MoveUp(_fav2);
         sut.ShouldOnlyContainInOrder(_fav2, _fav1, _fav3, _fav4);
      }
   }

   public class When_moving_consecutive_favorite_entries_up : concern_for_Favorites
   {
      [Observation]
      public void should_reoder_the_index_as_expected()
      {
         sut.MoveUp(_fav2, _fav3);
         sut.ShouldOnlyContainInOrder(_fav2, _fav3, _fav1, _fav4);
      }
   }

   public class When_moving_consecutive_favorite_entries_up_including_first_entry : concern_for_Favorites
   {
      [Observation]
      public void should_reoder_the_index_as_expected()
      {
         sut.MoveUp(_fav1, _fav2);
         sut.ShouldOnlyContainInOrder(_fav2, _fav3, _fav4, _fav1);
      }
   }

   public class When_moving_the_first_entry_up : concern_for_Favorites
   {
      [Observation]
      public void should_move_first_entry_last()
      {
         sut.MoveUp(_fav1);
         sut.ShouldOnlyContainInOrder(_fav2, _fav3, _fav4, _fav1);
      }
   }

   public class When_moving_one_entry_that_is_not_a_favorite_up : concern_for_Favorites
   {
      [Observation]
      public void should_not_change_the_order()
      {
         sut.MoveUp("DOES NOT EXIST");
         sut.ShouldOnlyContainInOrder(_fav1, _fav2, _fav3, _fav4);
      }
   }

   public class When_moving_the_last_entry_down : concern_for_Favorites
   {
      [Observation]
      public void should_move_last_entry_first()
      {
         sut.MoveDown(_fav4);
         sut.ShouldOnlyContainInOrder(_fav4, _fav1, _fav2, _fav3);   
      }
   }

   public class When_moving_one_entry_by_a_number_that_is_way_bigger_than_the_overall_length_of_favorites : concern_for_Favorites
   {
      [Observation]
      public void should_reorder_using_a_modulo_of_the_actual_length()
      {
         sut.Move(new []{_fav4 }, 9);
         sut.ShouldOnlyContainInOrder(_fav4, _fav1, _fav2, _fav3);
      }
   }


   public class When_moving_entries_from_an_empty_favorite_list : concern_for_Favorites
   {
      [Observation]
      public void should_not_crash()
      {
         sut.Clear();
         sut.Move(new[] { _fav4 }, 2);
         sut.ShouldBeEmpty();
      }
   }

   public class When_moving_all_entries_up_twice : concern_for_Favorites
   {
      [Observation]
      public void should_reoder_the_index_as_expected()
      {
         sut.MoveUp(sut);
         sut.MoveUp(sut);
         sut.ShouldOnlyContainInOrder(_fav3, _fav4, _fav1, _fav2);
      }
   }
}