using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RenameObjectDTOFactory : ContextSpecification<IRenameObjectDTOFactory>
   {
      private IProjectRetriever _projectRetriever;
      private IObjectTypeResolver _objectTypeResolver;
      protected IWithName _entityToRename;
      protected RenameObjectDTO _dto;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         sut = new RenameObjectDTOFactory(_projectRetriever, _objectTypeResolver);
      }

      protected override void Because()
      {
         _dto = sut.CreateFor(_entityToRename);
      }
   }

   public class When_creating_a_rename_dto_for_a_simple_object_base : concern_for_RenameObjectDTOFactory
   {
      protected override void Context()
      {
         base.Context();
         _entityToRename = A.Fake<IWithName>();
         _entityToRename.Name = "Test";
      }

      [Observation]
      public void should_return_a_dto_without_any_forbidden_names()
      {
         _dto.Name.ShouldBeEqualTo(_entityToRename.Name);
         _dto.UsedNames.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_creating_a_rename_dto_for_a_entity_within_a_container : concern_for_RenameObjectDTOFactory
   {
      private Parameter _parameter;
      private Parameter _otherParameter;

      protected override void Context()
      {
         base.Context();
         _parameter= new Parameter {Name = "P1"};
         _otherParameter = new Parameter {Name = "P2"};
         var container = new Container {_parameter, _otherParameter};
         _entityToRename = _parameter;
      }

      [Observation]
      public void should_return_a_dto_without_the_expected_forbidden_names()
      {
         _dto.Name.ShouldBeEqualTo(_entityToRename.Name);
         _dto.UsedNames.ShouldContain(_otherParameter.Name.ToLower());
      }
   }
}