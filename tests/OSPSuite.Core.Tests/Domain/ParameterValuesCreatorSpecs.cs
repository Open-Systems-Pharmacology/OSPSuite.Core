using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterValuesCreator : ContextSpecification<IParameterValuesCreator>
   {
      protected override void Context()
      {
         sut = new ParameterValuesCreator(new IdGenerator(), new EntityPathResolverForSpecs());
      }
   }

   public class When_creating_a_parameter_value_for_a_parameter_and_object_path : concern_for_ParameterValuesCreator
   {
      private ObjectPath _objectPath;
      private IParameter _parameter;
      private ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _objectPath = new ObjectPath("A", "B", "C");
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(5).WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
      }

      protected override void Because()
      {
         _psv = sut.CreateParameterValue(_objectPath, _parameter);
      }

      [Observation]
      public void should_return_a_parameter_value_using_the_provided_path_as_well_as_the_dimension_and_the_value_of_the_parameter()
      {
         _psv.Path.ToString().ShouldBeEqualTo(_objectPath.ToString());
         _psv.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _psv.Value.ShouldBeEqualTo(_parameter.Value);
      }
   }

   public class When_creating_parameter_values_for_spatial_structure_and_molecules : concern_for_ParameterValuesCreator
   {
      private SpatialStructure _spatialStructure;
      private List<MoleculeBuilder> _molecules;
      private IReadOnlyList<ParameterValue> _parameterValues;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure();
         var topContainer = new Container().WithName("Top").WithMode(ContainerMode.Physical);
         _spatialStructure.AddTopContainer(topContainer);
         var physicalContainer = new Container().WithName("physicalContainer").WithMode(ContainerMode.Physical);
         topContainer.Add(physicalContainer);
         topContainer.Add(new Container().WithName("logicalContainer").WithMode(ContainerMode.Logical));

         _molecules = new List<MoleculeBuilder>();

         var constantFormulaParameter = DomainHelperForSpecs.ConstantParameterWithValue(5).WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs()).WithName("constantFormulaParameterName");
         var normalDistributionParameter = DomainHelperForSpecs.NormalDistributedParameter().WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs()).WithName("normalDistributionParameterName");
         var molecule = new MoleculeBuilder().WithName("Molecule1");
         molecule.AddParameter(constantFormulaParameter);
         molecule.AddParameter(normalDistributionParameter);

         _molecules.Add(molecule);
      }

      protected override void Because()
      {
         _parameterValues = sut.CreateFrom(_spatialStructure, _molecules);
      }

      [Observation]
      public void parameter_values_should_only_be_created_for_each_constant_formula_parameter_in_each_physical_container()
      {
         _parameterValues.Select(x => x.Path.ToString()).ShouldOnlyContain("Top|physicalContainer|Molecule1|constantFormulaParameterName", "Top|Molecule1|constantFormulaParameterName");
      }
   }
}