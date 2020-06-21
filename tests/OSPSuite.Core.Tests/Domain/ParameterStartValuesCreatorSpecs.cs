using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

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
      private ISpatialStructure _spatialStrcuture;
      private IMoleculeBuildingBlock _molecules;
      private IParameter _globalParameter;

      protected override void Context()
      {
         base.Context();
         _molecules = new MoleculeBuildingBlock();
         _molecules.Add(createMoleculeBuilder("Drug"));
         _molecules.Add(createMoleculeBuilder("Other"));
         _spatialStrcuture = new SpatialStructure();
         _spatialStrcuture.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);
         _spatialStrcuture.GlobalMoleculeDependentProperties = new Container();
         _globalParameter = new Parameter().WithName("GlobalMoleculeParameter").WithFormula(new ConstantFormula(2));
         _spatialStrcuture.GlobalMoleculeDependentProperties.Add(_globalParameter);
         var topcontainer = new Container().WithName("Organism");
         var organ =new Container().WithName("Organ").WithParentContainer(topcontainer);
         var mp = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(organ);
         new Parameter().WithName("Local").WithFormula(new ConstantFormula(3)).WithParentContainer(mp);
         _spatialStrcuture.AddTopContainer(topcontainer);
         var neighborhood = new NeighborhoodBuilder().WithName("A2B");
         var nmp = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(neighborhood);
         new Parameter().WithName("Hallo").WithFormula(new ConstantFormula(4)).WithParentContainer(nmp);
         _spatialStrcuture.AddNeighborhood(neighborhood);


      }

      private static MoleculeBuilder createMoleculeBuilder(string moleculeName)
      {
         var drug = new MoleculeBuilder().WithName(moleculeName);
         drug.AddParameter(new Parameter().WithName("MoleculeProperty").WithFormula(new ConstantFormula(1)).WithMode(ParameterBuildMode.Global));
         return drug;
      }

      protected override void Because()
      {
         _parameterStartValues = sut.CreateFrom(_spatialStrcuture, _molecules);
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
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Drug|GlobalMoleculeParameter");
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Other|GlobalMoleculeParameter");
      }

      [Observation]
      public void should_create_start_values_for_local_molecule_properties_for_all_molecules()
      {
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Organism|Organ|Drug|Local");
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Organism|Organ|Other|Local");
      }

      [Observation]
      public void should_create_start_values_for_Molceule_Properties()
      {
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Drug|MoleculeProperty");
         parameterStartValueForPathSholuldExists(_parameterStartValues, "Other|MoleculeProperty");
      }
      [Observation]
      public void should_create_StartValues_for_Neighborhood_parameters()
      {
         parameterStartValueForPathSholuldExists(_parameterStartValues,string.Format("{0}|A2B|Drug|Hallo",Constants.NEIGHBORHOODS));
         parameterStartValueForPathSholuldExists(_parameterStartValues,string.Format("{0}|A2B|Other|Hallo",Constants.NEIGHBORHOODS));
      }

      [Observation] public void should_not_create_any_paths_to_molecuel_properties()
      {
         _parameterStartValues.Any(psv=>psv.Path.Contains(Constants.MOLECULE_PROPERTIES)).ShouldBeFalse();
      }

      private void parameterStartValueForPathSholuldExists(IParameterStartValuesBuildingBlock buildingBlock,string pathAsString )
      {
         buildingBlock.FirstOrDefault(psv => psv.Path.PathAsString.Equals(pathAsString)).ShouldNotBeNull();
      }

   }
}	