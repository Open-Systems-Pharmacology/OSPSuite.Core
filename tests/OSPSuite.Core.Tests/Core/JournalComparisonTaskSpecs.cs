using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalComparisonTask : ContextSpecification<IJournalComparisonTask>
   {
      protected IRelatedItemSerializer _relatedItemSerializer;
      protected IEventPublisher _eventPublisher;
      protected IContentLoader _contentLoader;

      protected override void Context()
      {
         _relatedItemSerializer = A.Fake<IRelatedItemSerializer>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _contentLoader = A.Fake<IContentLoader>();
         sut = new JournalComparisonTask(_eventPublisher, _contentLoader, _relatedItemSerializer);
      }
   }

   public class When_starting_the_comparison_for_a_given_journal_related_item_and_an_object : concern_for_JournalComparisonTask
   {
      private RelatedItem _relatedItem;
      private IObjectBase _objectToCompare;
      private StartComparisonEvent _event;
      private IObjectBase _relatedObject;
      private byte[] _data;
      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem();
         _objectToCompare = A.Fake<IObjectBase>().WithName("A");
         _relatedObject = A.Fake<IObjectBase>();
         _relatedItem.Name = "B";
         _data = new byte[] { 12, 56 };

         //simulte content loading from dB
         A.CallTo(() => _contentLoader.Load(_relatedItem))
            .Invokes(x => { _relatedItem.Content = new Content { Data = _data }; });

         A.CallTo(() => _eventPublisher.PublishEvent(A<StartComparisonEvent>._))
            .Invokes(x => { _event = x.GetArgument<StartComparisonEvent>(0); });

         A.CallTo(() => _relatedItemSerializer.Deserialize(_relatedItem)).Returns(_relatedObject);
      }

      protected override void Because()
      {
         sut.StartComparison(_objectToCompare, _relatedItem);
      }

      [Observation]
      public void should_retrieve_the_content_of_the_related_item_from_the_database()
      {
         A.CallTo(() => _contentLoader.Load(_relatedItem)).MustHaveHappened();
      }

      [Observation]
      public void should_publish_the_comparison_start_even_to_trigger_the_comparison_between_the_object_and_the_related_object()
      {
         _event.LeftObject.ShouldBeEqualTo(_relatedObject);
         _event.LeftCaption.ShouldBeEqualTo(Captions.Journal.CompareRelatedItem(_relatedItem.Name));
         _event.RightObject.ShouldBeEqualTo(_objectToCompare);
         _event.RightCaption.ShouldBeEqualTo(Captions.Journal.CompareProjectItem(_objectToCompare.Name));
      }
   }

}	