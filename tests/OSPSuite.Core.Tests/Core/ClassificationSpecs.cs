using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_Classification : ContextSpecification<Classification>
   {
      protected override void Context()
      {
         base.Context();
         sut = new Classification { Name = "Child", Parent = new Classification { Name = "Parent" } };
      }
   }
   
   public class when_reading_classification_path : concern_for_Classification
   {
      protected override void Context()
      {
         sut = new Classification { Name = "Path", Parent = new Classification { Name = "The", Parent = new Classification { Name = "Is", Parent = new Classification { Name = "This" } } } };
      }

      [Observation]
      public void should_return_path_including_name_and_ancestor_names()
      {
         sut.Path.ShouldBeEqualTo("ThisIsThePath");
      }
   }
   public class when_comparing_with_objects_with_different_ancestry : concern_for_Classification
   {
      private bool _result;

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      protected override void Because()
      {
         base.Because();
         var fake = A.Fake<Classification>();
         fake.Name = "Child";
         fake.Parent = new Classification { Name = "Parent2" };

         _result = sut.HasEquivalentClassification(fake);
      }
   }

   public class when_comparing_with_equivalent_object : concern_for_Classification
   {
      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
      private bool _result;
      protected override void Because()
      {
         base.Because();
         var fake = A.Fake<Classification>();
         fake.Name = "Child";
         fake.Parent = sut.Parent;
         
         _result = sut.HasEquivalentClassification(fake);
      }
   }

   public class when_comparing_with_null_object : concern_for_Classification
   {
      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      private bool _result;
      protected override void Because()
      {
         base.Because();
         _result = sut.HasEquivalentClassification(null);
      }
   }
}
