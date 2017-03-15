using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IRelatedItemComparablePresenter : IPresenter<IRelatedItemComparableView>
   {
      /// <summary>
      /// Starts the comparison for the given <paramref name="relatedItem"/>
      /// </summary>
      void StartComparisonFor(RelatedItem relatedItem);

      /// <summary>
      /// Notifies that the user selected another object for the comparison
      /// </summary>
      void ItemSelectionChanged(ObjectSelectionDTO objectSelectionDTO);

      /// <summary>
      /// Runs the comparison according to the user selection
      /// </summary>
      void RunComparison();
   }

   public class RelatedItemComparablePresenter : AbstractPresenter<IRelatedItemComparableView, IRelatedItemComparablePresenter>, IRelatedItemComparablePresenter
   {
      private readonly IApplicationDiscriminator _applicationDiscriminator;
      private readonly IJournalComparisonTask _journalComparisonTask;
      private readonly List<ObjectSelectionDTO> _allComparables;
      private RelatedItem _relatedItem;

      public RelatedItemComparablePresenter(IRelatedItemComparableView view, IApplicationDiscriminator applicationDiscriminator,
         IJournalComparisonTask journalComparisonTask)
         : base(view)
      {
         _applicationDiscriminator = applicationDiscriminator;
         _journalComparisonTask = journalComparisonTask;
         _allComparables = new List<ObjectSelectionDTO>();
      }

      public void StartComparisonFor(RelatedItem relatedItem)
      {
         _relatedItem = relatedItem;
         var allComparableItems = _applicationDiscriminator.AllFor(relatedItem.Discriminator);
         if (!allComparableItems.Any())
         {
            _view.ShowWarning(Captions.Journal.NoObjectAvailableForComparison(relatedItem.ItemType));
            return;
         }

         bindTo(allComparableItems);
      }

      public void ItemSelectionChanged(ObjectSelectionDTO objectSelectionDTO)
      {
         _allComparables.Where(x => x != objectSelectionDTO).Each(x => x.Selected = false);
         updateRunComparisonEnabled();
      }

      public void RunComparison()
      {
         _journalComparisonTask.StartComparison(selectedObject, _relatedItem);
      }

      private void bindTo(IReadOnlyCollection<IObjectBase> allComparableItems)
      {
         _allComparables.Clear();
         _allComparables.AddRange(allComparableItems.Select(x => new ObjectSelectionDTO(x)));

         var possibleDefault = _allComparables.Find(x => string.Equals(x.Name, _relatedItem.Name));
         if (possibleDefault != null)
            possibleDefault.Selected = true;

         _view.BindTo(_allComparables);
         _view.Caption = Captions.Journal.AvailbleItemsForComparison(_relatedItem.ItemType);
         updateRunComparisonEnabled();
      }

      private void updateRunComparisonEnabled()
      {
         var objectToCompare = selectedObject;
         _view.RunComparisonEnabled = objectToCompare != null;
      }

      private IObjectBase selectedObject
      {
         get
         {
            var selectedDTO = _allComparables.FirstOrDefault(x => x.Selected);
            return selectedDTO == null ? null : selectedDTO.Object;
         }
      }
   }
}