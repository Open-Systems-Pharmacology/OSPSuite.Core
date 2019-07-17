using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CreationMetaData : ContextSpecification<CreationMetaData>
   {
      protected override void Context()
      {
         sut = new CreationMetaData();
      }
   }

   public class When_creating_a_new_creation_meta_data_as_clone_of_an_object_to_clone : concern_for_CreationMetaData
   {
      private IWithName _objectToClone;

      protected override void Context()
      {
         base.Context();
         _objectToClone = A.Fake<IWithName>().WithName("TOTO");
      }

      protected override void Because()
      {
         sut.AsCloneOf(_objectToClone);
      }

      [Observation]
      public void should_set_the_creation_mode_to_clone()
      {
         sut.CreationMode.ShouldBeEqualTo(CreationMode.Clone);
      }

      [Observation]
      public void should_set_the_clone_from_to_the_name_of_the_clone()
      {
         sut.ClonedFrom.ShouldBeEqualTo("TOTO");
      }
   }
}	