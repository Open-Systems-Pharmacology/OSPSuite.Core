using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public class when_creating_initial_condition_with_CreateInitialCondition : ContextSpecification<InitialConditionsCreator>
   {
      private InitialCondition _result;
      private ObjectPath _containerPath;
      private string _moleculeName;
      private IDimension _dimension;
      private Unit _displayUnit;
      private ValueOrigin _valueOrigin;
      private IObjectBaseFactory _objectBaseFactory;
      private IIdGenerator _idGenerator;
      private ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _idGenerator = A.Fake<IIdGenerator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();

         _containerPath = new ObjectPath("Organism|Liver");
         _moleculeName = "Glucose";
         _dimension = A.Fake<IDimension>();
         _displayUnit = A.Fake<Unit>();
         _valueOrigin = new ValueOrigin();

         A.CallTo(() => _idGenerator.NewId()).Returns("test-id");

         sut = new InitialConditionsCreator(_objectBaseFactory, _entityPathResolver, _idGenerator, _cloneManagerForBuildingBlock);
      }

      protected override void Because()
      {
         _result = sut.CreateInitialCondition(_containerPath, _moleculeName, _dimension, _displayUnit, _valueOrigin);
      }

      [Observation]
      public void should_set_value_to_zero()
      {
         _result.Value.ShouldBeEqualTo(0);
      }
   }
}