using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_object_base_extensions  : StaticContextSpecification
   {
      protected override void Context()
      {
      }
   }

   
   public class When_setting_the_name_of_an_object_base_with_the_extensions : concern_for_object_base_extensions
   {
      private IObjectBase _myObject;

      protected override void Context()
      {
         base.Context();
         _myObject = A.Fake<IObjectBase>();
      }
      protected override void Because()
      {
         _myObject.WithName("toto");
      }
      [Observation]
      public void should_have_set_the_name_of_the_object()
      {
         _myObject.Name.ShouldBeEqualTo("toto");
      }
   }

   public class When_setting_the_idof_an_object_base_with_the_extensions : concern_for_object_base_extensions
   {
      private IObjectBase _myObject;

      protected override void Context()
      {
         base.Context();
         _myObject = A.Fake<IObjectBase>();
      }
      protected override void Because()
      {
         _myObject.WithId("toto");
      }
      [Observation]
      public void should_have_set_the_id_of_the_object()
      {
         _myObject.Id.ShouldBeEqualTo("toto");
      }
   }
}	