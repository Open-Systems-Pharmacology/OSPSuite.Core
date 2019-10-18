using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObjectBaseExtensions : StaticContextSpecification
   {
      protected IObjectBase _object3;
      protected IObjectBase _object1;
      protected IObjectBase _object2;
      protected IList<IObjectBase> _objectBaseList;

      protected override void Context()
      {
         _object1 = A.Fake<IObjectBase>();
         _object2 = A.Fake<IObjectBase>();
         _object3 = A.Fake<IObjectBase>();

         _object1.Id = "id1";
         _object2.Id = "id2";
         _object3.Id = "id3";
         _object1.Name = "tutu";
         _object2.Name = "tata";
         _object3.Name = "titi";

         _objectBaseList = new List<IObjectBase> { _object1, _object2, _object3 };
      }
   }

      public class When_retrieving_an_object_by_id_that_does_not_exist : concern_for_ObjectBaseExtensions
   {
      [Observation]
      public void should_return_null()
      {
         _objectBaseList.FindById("tasasd").ShouldBeNull();
      }
   }

      public class When_retrieving_an_object_by_id_that_does_exist : concern_for_ObjectBaseExtensions
   {
      [Observation]
      public void should_return_the_object()
      {
         _objectBaseList.FindById("id1").ShouldBeEqualTo(_object1);
      }
   }

   
   public class When_retrieving_an_object_by_name_that_does_not_exist : concern_for_ObjectBaseExtensions
   {
      [Observation]
      public void should_return_null()
      {
         _objectBaseList.FindByName("tasasd").ShouldBeNull();
      }
   }

   
   public class When_retrieving_an_object_by_name_that_does_exist : concern_for_ObjectBaseExtensions
   {
      [Observation]
      public void should_return_the_object()
      {
         _objectBaseList.FindByName("tutu").ShouldBeEqualTo(_object1);
      }
   }

  
}