using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectBaseFactory : ContextForIntegration<IObjectBaseFactory>
   {
      private IDimensionFactory _dimensionFactory;
      protected IDimension _noDimension;
      protected ICreationMetaDataFactory _creationMetaDataFactory;

      protected override void Context()
      {
         _noDimension = A.Fake<IDimension>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _creationMetaDataFactory = A.Fake<ICreationMetaDataFactory>();
         A.CallTo(() => _dimensionFactory.NoDimension).Returns(_noDimension);
         sut = new ObjectBaseFactory(IoC.Container, _dimensionFactory, new IdGenerator(), _creationMetaDataFactory);
      }
   }

   public class When_creating_an_object_for_an_inteface_without_any_specified_id : concern_for_ObjectBaseFactory
   {
      private IParameter _result;

      protected override void Because()
      {
         _result = sut.Create<IParameter>();
      }

      [Observation]
      public void should_return_a_new_object_with_a_new_id()
      {
         _result.Id.ShouldNotBeNull();
      }
   }

   public class When_creating_an_object_for_an_interface_having_a_dimension : concern_for_ObjectBaseFactory
   {
      private IParameter _result;

      protected override void Because()
      {
         _result = sut.Create<IParameter>();
      }

      [Observation]
      public void should_return_an_object_initialized_with_the_default_dimension()
      {
         _result.Dimension.ShouldBeEqualTo(_noDimension);
      }
   }

   public class When_creating_an_object_for_an_interface_with_a_specified_id : concern_for_ObjectBaseFactory
   {
      private IParameter _result;
      private string _id;

      protected override void Context()
      {
         base.Context();
         _id = "titi";
      }

      protected override void Because()
      {
         _result = sut.Create<IParameter>(_id);
      }

      [Observation]
      public void should_return_a_new_object_with_the_accurate_id()
      {
         _result.Id.ShouldBeEqualTo(_id);
      }
   }

   public class When_creating_an_object_from_a_type : concern_for_ObjectBaseFactory
   {
      private IParameter _result;

      protected override void Because()
      {
         _result = sut.CreateObjectBaseFrom<Parameter>(typeof (Parameter));
      }

      [Observation]
      public void should_create_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_return_a_new_object_with_a_new_id()
      {
         _result.Id.ShouldNotBeNull();
      }
   }

   public class When_creating_an_object_from_another_object : concern_for_ObjectBaseFactory
   {
      private IParameter _result;
      private IParameter _source;

      protected override void Context()
      {
         base.Context();
         _source = new Parameter();
      }

      protected override void Because()
      {
         _result = sut.CreateObjectBaseFrom(_source);
      }

      [Observation]
      public void should_create_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_return_a_new_object_with_a_new_id()
      {
         _result.Id.ShouldNotBeNull();
      }
   }

   public class When_creating_an_object_that_implemented_the_with_creation_meta_data_interface : concern_for_ObjectBaseFactory
   {
      private IObserverBuildingBlock _result;
      private CreationMetaData _creation;

      protected override void Context()
      {
         base.Context();
         _creation = new CreationMetaData();
         A.CallTo(() => _creationMetaDataFactory.Create()).Returns(_creation);
      }

      protected override void Because()
      {
         _result = sut.Create<IObserverBuildingBlock>();
      }

      [Observation]
      public void should_create_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_create_a_default_meta_data()
      {
         _result.Creation.ShouldBeEqualTo(_creation);
      }
   }

   public class When_creating_an_object_from_a_type_with_a_given_id : concern_for_ObjectBaseFactory
   {
      private IParameter _result;
      private string _id;

      protected override void Context()
      {
         base.Context();
         _id = "toto";
      }

      protected override void Because()
      {
         _result = sut.CreateObjectBaseFrom<Parameter>(typeof (Parameter), _id);
      }

      [Observation]
      public void should_create_a_valid_object()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_return_a_new_object_with_the_given_id()
      {
         _result.Id.ShouldBeEqualTo(_id);
      }
   }
}