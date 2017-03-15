using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectPathFactory : ContextSpecification<IObjectPathFactory>
   {
      protected IContainer _comp;
      protected IContainer _organ;
      protected IDistributedParameter _parameter;
      protected IUsingFormula _reak;
      protected IContainer _topContainer;

      protected override void Context()
      {
         _topContainer = new Container().WithName("top");
         _organ = new Container().WithName("organ").WithParentContainer(_topContainer);
         _comp = new Container().WithName("comp").WithParentContainer(_organ);
         _parameter = new DistributedParameter().WithName("P1").WithParentContainer(_comp);
         new Parameter().WithName("LALA").WithParentContainer(_parameter);
         _reak = A.Fake<IUsingFormula>();
          A.CallTo(()=>_reak.ParentContainer).Returns(_topContainer);
          A.CallTo(() => _reak.RootContainer).Returns(_topContainer);
         sut = new ObjectPathFactory(new AliasCreator());
      }
   }

   
   public class When_we_create_an_absolute_path_to_a_top_container : concern_for_ObjectPathFactory
   {
      private IObjectPath _objectPath;

      protected override void Because()
      {
         _objectPath = sut.CreateAbsoluteObjectPath(_topContainer);
      }

      [Observation]
      public void should_return_path_to_itself()
      {
         _objectPath.PathAsString.ShouldBeEqualTo(_topContainer.Name);
      }

      [Observation]
      public void resolve_should_return_root()
      {
         _objectPath.Resolve<IContainer>(_reak).ShouldBeEqualTo(_topContainer);
      }
   }
   
   public class When_we_create_an_absolute_path_to_an_entity_without_a_parent_container : concern_for_ObjectPathFactory
   {
      private IObjectPath _objectPath;
      private IEntity _aParameter;

      protected override void Context()
      {
         base.Context();
         _aParameter = new Parameter().WithName("toto");
      }
      protected override void Because()
      {
         _objectPath = sut.CreateAbsoluteObjectPath(_aParameter);
      }

      [Observation]
      public void should_return_path_to_itself()
      {
         _objectPath.PathAsString.ShouldBeEqualTo(_aParameter.Name);
      }

    
   }

   
   public class When_we_create_an_absolute_path_to_a_root_container : concern_for_ObjectPathFactory
   {
      private IObjectPath _objectPath;
      private IEntity _rootContainer;

      protected override void Context()
      {
         base.Context();
         _rootContainer = new ARootContainer().WithName("ROOT");
         _topContainer.Add(_rootContainer);
      }

      protected override void Because()
      {
         _objectPath = sut.CreateAbsoluteObjectPath(_rootContainer);
      }

      [Observation]
      public void should_return_path_to_itself()
      {
         _objectPath.PathAsString.ShouldBeEqualTo(_rootContainer.Name);
      }
   }

   
   public class When_we_create_an_absolute_path_to_parameter : concern_for_ObjectPathFactory
   {
      private IFormulaUsablePath _formulaUsablePath;

      protected override void Because()
      {
         _formulaUsablePath = sut.CreateAbsoluteFormulaUsablePath(_parameter);
      }

      [Observation]
      public void should_return_path_to_parameter()
      {
         _formulaUsablePath.PathAsString.ShouldBeEqualTo(_topContainer.Name + ObjectPath.PATH_DELIMITER + _organ.Name + ObjectPath.PATH_DELIMITER + _comp.Name + FormulaUsablePath.PATH_DELIMITER +
                                              _parameter.Name);
      }

      [Observation]
      public void should_return_parameter()
      {
         _formulaUsablePath.Resolve<IUsingFormula>(_reak).ShouldBeEqualTo(_parameter);
      }
   }

   
   public class When_creating_a_time_path_object_path : concern_for_ObjectPathFactory
   {
      private IFormulaUsablePath _timePath;
      private IDimension _timeDimension;

      protected override void Context()
      {
         base.Context();
         _timeDimension =A.Fake<IDimension>();
      }
      protected override void Because()
      {
         _timePath = sut.CreateTimePath(_timeDimension);
      }
      [Observation]
      public void should_return_a_time_path_object_path()
      {
         _timePath.ShouldBeAnInstanceOf<TimePath>();
      }

      [Observation]
      public void should_have_set_the_dimension_of_the_object_path_to_the_time_dimension()
      {
         _timePath.DowncastTo<TimePath>().TimeDimension.ShouldBeEqualTo(_timeDimension);
      }
   }

   
   public class When_creating_a_relative_path_to_ourself:concern_for_ObjectPathFactory
   {
      private IObjectPath _path;
      protected override void Because()
      {
         _path = sut.CreateRelativeObjectPath(_parameter, _parameter);
      }
      [Observation]
      public void should_return_path_to_ourself()
      {
         _path.PathAsString.ShouldBeEqualTo(ObjectPath.PARENT_CONTAINER+ObjectPath.PATH_DELIMITER+_parameter.Name);
      }
      [Observation]
      public void resolved_path_should_return_ourself()
      {
         _path.Resolve<IUsingFormula>(_parameter).ShouldBeEqualTo(_parameter);
      }
   }
}