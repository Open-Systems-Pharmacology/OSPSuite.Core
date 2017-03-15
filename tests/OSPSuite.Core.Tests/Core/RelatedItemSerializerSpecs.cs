using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_RelatedItemSerializer : ContextSpecification<IRelatedItemSerializer>
   {
      protected IApplicationConfiguration _applicationConfiguration;
      protected IOSPSuiteExecutionContext _context;

      protected RelatedItem _relatedItem;
      protected override void Context()
      {
         _context= A.Fake<IOSPSuiteExecutionContext>();
         _applicationConfiguration= A.Fake<IApplicationConfiguration>();
         sut = new RelatedItemSerializer(_context,_applicationConfiguration);

         _relatedItem = new RelatedItem
         {
            Name = "toto",
            Origin = Origins.MoBi,
            Content = new Content {Data = new byte[] {}}
         };
      }
   }

   public class When_loading_a_related_item_that_was_serialized_with_the_current_application : concern_for_RelatedItemSerializer
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _applicationConfiguration.Product).Returns(Origins.MoBi);
      }

      protected override void Because()
      {
         sut.Deserialize(_relatedItem);
      }

      [Observation]
      public void should_leverage_the_context_to_load_the_related_item()
      {
         A.CallTo(() => _context.Deserialize<IObjectBase>(_relatedItem.Content.Data)).MustHaveHappened();
      }
   }

   public class When_loading_a_related_item_that_was_not_serialized_with_the_current_application : concern_for_RelatedItemSerializer
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _applicationConfiguration.Product).Returns(Origins.PKSim);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(()=>sut.Deserialize(_relatedItem)).ShouldThrowAn<OSPSuiteException>();
      }
   }
}	