using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Journal
{
   public interface IJournalComparisonTask
   {
      /// <summary>
      ///    Starts the comparison between the related object defined in the <paramref name="relatedItem" /> and
      ///    the <paramref name="selectedObject" />
      /// </summary>
      void StartComparison(IObjectBase selectedObject, RelatedItem relatedItem);

      void StartComparison(RelatedItem firstItem, RelatedItem secondItem);
   }

   public class JournalComparisonTask : IJournalComparisonTask
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IContentLoader _contentLoader;
      private readonly IRelatedItemSerializer _relatedItemSerializer;

      public JournalComparisonTask(IEventPublisher eventPublisher, IContentLoader contentLoader, IRelatedItemSerializer relatedItemSerializer)
      {
         _eventPublisher = eventPublisher;
         _contentLoader = contentLoader;
         _relatedItemSerializer = relatedItemSerializer;
      }

      public void StartComparison(RelatedItem firstItem, RelatedItem secondItem)
      {
         if (!loadContent(firstItem)) return;
         var firstRelatedObject = _relatedItemSerializer.Deserialize(firstItem);
         StartComparison(firstRelatedObject, secondItem);
      }

      public void StartComparison(IObjectBase selectedObject, RelatedItem relatedItem)
      {
         if (selectedObject == null)
            return;

         if (!loadContent(relatedItem)) return;

         var relatedObject = _relatedItemSerializer.Deserialize(relatedItem);
         _eventPublisher.PublishEvent(new StartComparisonEvent(
            leftObject: relatedObject,
            leftCaption: Captions.Journal.CompareRelatedItem(relatedItem.Name),
            rightObject: selectedObject,
            rightCaption: Captions.Journal.CompareProjectItem(selectedObject.Name))
            );
      }

      private bool loadContent(RelatedItem relatedItem)
      {
         _contentLoader.Load(relatedItem);
         return relatedItem.IsLoaded;
      }
   }
}