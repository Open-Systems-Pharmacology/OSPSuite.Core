using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RelatedItemComparablePresenter : ContextSpecification<IRelatedItemComparablePresenter>
   {
      protected IJournalComparisonTask _journalComparisonTask;
      protected IApplicationDiscriminator _applicationDiscriminator;
      protected IRelatedItemComparableView _view;
      protected RelatedItem _relatedItem;
      protected string _simulationDiscriminator = "SIM";
      protected List<IObjectBase> _comparableObjects;
      protected List<ObjectSelectionDTO> _allObjectSelectionDTO;

      protected override void Context()
      {
         _journalComparisonTask = A.Fake<IJournalComparisonTask>();
         _applicationDiscriminator = A.Fake<IApplicationDiscriminator>();
         _view = A.Fake<IRelatedItemComparableView>();
         sut = new RelatedItemComparablePresenter(_view, _applicationDiscriminator, _journalComparisonTask);

         _relatedItem = new RelatedItem { Discriminator = _simulationDiscriminator, Name = "TOTO", ItemType = "SIM" };
         _comparableObjects = new List<IObjectBase>();
         A.CallTo(() => _applicationDiscriminator.AllFor(_simulationDiscriminator)).Returns(_comparableObjects);

         A.CallTo(() => _view.BindTo(A<IEnumerable<ObjectSelectionDTO>>._))
            .Invokes(x => _allObjectSelectionDTO = x.GetArgument<IEnumerable<ObjectSelectionDTO>>(0).ToList());
      }
   }

   public class When_starting_the_comparison_for_a_given_related_item_and_comparable_items_are_available_in_the_application : concern_for_RelatedItemComparablePresenter
   {
      private IObjectBase _objectBase1;
      private IObjectBase _objectBase2;

      protected override void Context()
      {
         base.Context();
         _objectBase1 = A.Fake<IObjectBase>().WithName(_relatedItem.Name);
         _objectBase2 = A.Fake<IObjectBase>().WithName("OTHER");

         _comparableObjects.Add(_objectBase1);
         _comparableObjects.Add(_objectBase2);
      }

      protected override void Because()
      {
         sut.StartComparisonFor(_relatedItem);
      }

      [Observation]
      public void should_bind_the_available_items_to_the_view()
      {
         _allObjectSelectionDTO.ShouldNotBeNull();
         _allObjectSelectionDTO.Count.ShouldBeEqualTo(2);
         _allObjectSelectionDTO[0].Object.ShouldBeEqualTo(_objectBase1);
         _allObjectSelectionDTO[1].Object.ShouldBeEqualTo(_objectBase2);
      }

      [Observation]
      public void should_select_the_one_with_the_same_name_as_default_for_the_selection()
      {
         _allObjectSelectionDTO[0].Selected.ShouldBeTrue();
         _allObjectSelectionDTO[1].Selected.ShouldBeFalse();
      }

      [Observation]
      public void should_update_the_view_caption_using_the_related_item_type()
      {
         _view.Caption.ShouldBeEqualTo(Captions.Journal.AvailbleItemsForComparison(_relatedItem.ItemType));
      }
   }

   public class When_starting_the_comparison_for_a_given_related_item_and_no_comparable_item_is_available_in_the_application : concern_for_RelatedItemComparablePresenter
   {
      protected override void Because()
      {
         sut.StartComparisonFor(_relatedItem);
      }

      [Observation]
      public void should_show_a_warning_to_the_user_that_no_items_are_available_for_comparison()
      {
         A.CallTo(() => _view.ShowWarning(Captions.Journal.NoObjectAvailableForComparison(_relatedItem.ItemType))).MustHaveHappened();
      }

      [Observation]
      public void should_not_bind_to_the_view()
      {
         _allObjectSelectionDTO.ShouldBeNull();
      }
   }

   public class When_the_user_selects_another_item_that_the_one_previously_selected : concern_for_RelatedItemComparablePresenter
   {
      private IObjectBase _objectBase1;
      private IObjectBase _objectBase2;

      protected override void Context()
      {
         base.Context();
         _objectBase1 = A.Fake<IObjectBase>();
         _objectBase2 = A.Fake<IObjectBase>();

         _comparableObjects.Add(_objectBase1);
         _comparableObjects.Add(_objectBase2);

         sut.StartComparisonFor(_relatedItem);
         _allObjectSelectionDTO[0].Selected = true;
      }

      protected override void Because()
      {
         //simulate user click
         _allObjectSelectionDTO[1].Selected = true;
         sut.ItemSelectionChanged(_allObjectSelectionDTO[1]);
      }

      [Observation]
      public void should_deselect_the_previously_selected_item()
      {
         _allObjectSelectionDTO[0].Selected.ShouldBeFalse();
      }

      [Observation]
      public void should_enable_the_run_comparison()
      {
         _view.RunComparisonEnabled.ShouldBeTrue();
      }
   }

   public class When_the_user_deselects_an_item_that_was_previously_selected : concern_for_RelatedItemComparablePresenter
   {
      private IObjectBase _objectBase1;
      private IObjectBase _objectBase2;

      protected override void Context()
      {
         base.Context();
         _objectBase1 = A.Fake<IObjectBase>();
         _objectBase2 = A.Fake<IObjectBase>();

         _comparableObjects.Add(_objectBase1);
         _comparableObjects.Add(_objectBase2);

         sut.StartComparisonFor(_relatedItem);
      }

      protected override void Because()
      {
         //simulate user click
         _allObjectSelectionDTO[0].Selected = false;
         sut.ItemSelectionChanged(_allObjectSelectionDTO[0]);
      }

      [Observation]
      public void should_disable_the_run_comparison()
      {
         _view.RunComparisonEnabled.ShouldBeFalse();
      }
   }

}	