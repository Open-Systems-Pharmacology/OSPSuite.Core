using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterStartValuesCreatorSpecs : ContextSpecification<IParameterStartValuesCreator>
   {
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectBaseFactory.Create<IParameterStartValuesBuildingBlock>()).Returns(new ParameterStartValuesBuildingBlock());
         sut = new ParameterStartValuesCreator(_objectBaseFactory,new ObjectPathFactory(new AliasCreator()), new IdGenerator());
      }
   }

   public class When_creating_parameter_start_values : concern_for_ParameterStartValuesCreatorSpecs
   {
      private IParameterStartValuesBuildingBlock _parameterStartValues;
      private ISpatialStructure _spatialStructure;
      private IMoleculeBuildingBlock _molecules;
      private IParameter _globalParameter;

      protected override void Context()
      {
         base.Context();
         _molecules = new MoleculeBuildingBlock
         {
            createMoleculeBuilder("Drug"), 
            createMoleculeBuilder("Other")
         };
         _spatialStructure = new SpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS), 
            GlobalMoleculeDependentProperties = new Container()
         };
         _globalParameter = new Parameter().WithName("GlobalMoleculeParameter").WithFormula(new ConstantFormula(2));
         _spatialStructure.GlobalMoleculeDependentProperties.Add(_globalParameter);
         var topContainer = new Container().WithName("Organism");
         var organ =new Container().WithName("Organ").WithParentContainer(topContainer);
         var mp = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(organ);
         new Parameter().WithName("Local").WithFormula(new ConstantFormula(3)).WithParentContainer(mp);
         _spatialStructure.AddTopContainer(topContainer);
         var neighborhood = new NeighborhoodBuilder().WithName("A2B");
         var nmp = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(neighborhood);
         new Parameter().WithName("Hallo").WithFormula(new ConstantFormula(4)).WithParentContainer(nmp);
         _spatialStructure.AddNeighborhood(neighborhood);


      }

      private static MoleculeBuilder createMoleculeBuilder(string moleculeName)
      {
         var drug = new MoleculeBuilder().WithName(moleculeName);
         drug.AddParameter(new Parameter().WithName("MoleculeProperty").WithFormula(new ConstantFormula(1)).WithMode(ParameterBuildMode.Global));
         drug.AddParameter(new Parameter().WithName("NaNParameter").WithFormula(new ConstantFormula(double.NaN)).WithMode(ParameterBuildMode.Global));
         return drug;
      }

      protected override void Because()
      {
         _parameterStartValues = sut.CreateFrom(_spatialStructure, _molecules);
      }

      [Observation]
      public void should_create_parameter_start_values_building_block()
      {
         A.CallTo(() => _objectBaseFactory.Create<IParameterStartValuesBuildingBlock>()).MustHaveHappened();
         _parameterStartValues.ShouldNotBeNull();
      }

      [Observation]
      public void should_create_start_values_for_global_molecule_properties_for_all_molecules()
      {
         parameterStartValueForPathShouldExists(_parameterStartValues, "Drug|GlobalMoleculeParameter");
         parameterStartValueForPathShouldExists(_parameterStartValues, "Other|GlobalMoleculeParameter");
      }

      [Observation]
      public void should_create_start_values_for_local_molecule_properties_for_all_molecules()
      {
         parameterStartValueForPathShouldExists(_parameterStartValues, "Organism|Organ|Drug|Local");
         parameterStartValueForPathShouldExists(_parameterStartValues, "Organism|Organ|Other|Local");
      }

      [Observation]
      public void should_create_start_values_for_molecule_properties()
      {
         parameterStartValueForPathShouldExists(_parameterStartValues, "Drug|MoleculeProperty");
         parameterStartValueForPathShouldExists(_parameterStartValues, "Other|MoleculeProperty");
      }
      [Observation]
      public void should_create_StartValues_for_Neighborhood_parameters()
      {
         parameterStartValueForPathShouldExists(_parameterStartValues, $"{Constants.NEIGHBORHOODS}|A2B|Drug|Hallo");
         parameterStartValueForPathShouldExists(_parameterStartValues, $"{Constants.NEIGHBORHOODS}|A2B|Other|Hallo");
      }

      [Observation] 
      public void should_not_create_any_paths_to_molecule_properties()
      {
         _parameterStartValues.Any(psv=>psv.Path.Contains(Constants.MOLECULE_PROPERTIES)).ShouldBeFalse();
      }

      [Observation]
      public void should_not_create_any_path_entries_for_parameter_with_a_nan_value()
      {
         parameterStartValueForPathShouldNotExist(_parameterStartValues, "Drug|NaNParameter");
         parameterStartValueForPathShouldNotExist(_parameterStartValues, "Other|NaNParameter");
      }

      private void parameterStartValueForPathShouldExists(IParameterStartValuesBuildingBlock buildingBlock,string pathAsString )
      {
         buildingBlock[new ObjectPath(pathAsString.ToPathArray())].ShouldNotBeNull();
      }

      private void parameterStartValueForPathShouldNotExist(IParameterStartValuesBuildingBlock buildingBlock, string pathAsString)
      {
         buildingBlock[new ObjectPath(pathAsString.ToPathArray())].ShouldBeNull();
      }

   }
}	