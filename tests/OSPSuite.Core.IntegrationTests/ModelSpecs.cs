using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_Model : ContextForIntegration<IModel>
   {
      protected override void Context()
      {
         sut = new Model();
      }
   }

   public class When_cloning_a_model : concern_for_Model
   {
      protected IModel _clonedModel;

      protected override void Context()
      {
         base.Context();

         var root = new Container().WithName("Root").WithId("Root");
         sut.Root = root;

         var childContainer = new Container().WithName("Tralala").WithId("Trululu");
         root.Add(childContainer);

         var neighborhoods = new Container().WithName("NeighborhoodsForTest").WithId("XXX");
         sut.Neighborhoods = neighborhoods;
      }

      protected override void Because()
      {
         var cloneManager = IoC.Resolve<ICloneManagerForModel>();
         _clonedModel = cloneManager.Clone(sut);
      }

      [Observation]
      public void neighborhoods_should_be_set()
      {
         var clonedNeighborhoods = _clonedModel.Neighborhoods;

         clonedNeighborhoods.ShouldNotBeNull();
         clonedNeighborhoods.Name.ShouldBeEqualTo(sut.Neighborhoods.Name);
      }
   }

   public class When_retrieving_the_mol_weight_for_a_given_quantity : concern_for_Model
   {
      private MoleculeAmount _quantityWithMolWeight;
      private MoleculeAmount _quantityWithoutMolWeight;
      private Parameter _parameterUnderMoleculeWithMolWeight;
      private Parameter _parameterUnderMoleculeWithoutMolWeight;

      protected override void Context()
      {
         base.Context();
         var root = new Container().WithName("Root").WithId("Root");
         sut.Root = root;
         _quantityWithMolWeight = new MoleculeAmount().WithName("Molecule");
         var quantityWithMolWeightContainer = new Container().WithName(_quantityWithMolWeight.Name).WithContainerType(ContainerType.Molecule);
         quantityWithMolWeightContainer.Add(new Parameter().WithName(Constants.Parameters.MOL_WEIGHT).WithFormula(new ConstantFormula(400)));
         _parameterUnderMoleculeWithMolWeight = new Parameter().WithName("Concentration");
         quantityWithMolWeightContainer.Add(_parameterUnderMoleculeWithMolWeight);

         _quantityWithoutMolWeight = new MoleculeAmount().WithName("Molecule2");
         var quantityWithoutMolWeightContainer = new Container().WithName(_quantityWithoutMolWeight.Name);
         _parameterUnderMoleculeWithoutMolWeight = new Parameter().WithName("Concentration");

         quantityWithoutMolWeightContainer.Add(_parameterUnderMoleculeWithoutMolWeight);

         sut.Root.Add(quantityWithMolWeightContainer);
         sut.Root.Add(quantityWithoutMolWeightContainer);
      }

      [Observation]
      public void should_return_null_if_the_quantity_is_a_parameter_without_parent()
      {
         sut.MolWeightFor(new Parameter()).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_if_the_quantity_is_null()
      {
         sut.MolWeightFor((IQuantity) null).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_if_no_molweight_parameter_was_found_under_the_global_container()
      {
         sut.MolWeightFor(_quantityWithoutMolWeight).ShouldBeNull();
         sut.MolWeightFor(_parameterUnderMoleculeWithoutMolWeight).ShouldBeNull();
      }

      [Observation]
      public void should_return_the_molweight_parameter_for_a_parameter_defined_in_a_molecule_for_which_a_molweight_parameter_exists()
      {
         sut.MolWeightFor(_parameterUnderMoleculeWithMolWeight).ShouldBeEqualTo(400);
      }

      [Observation]
      public void should_return_the_molweight_parameter_value_otherwise()
      {
         sut.MolWeightFor(_quantityWithMolWeight).ShouldBeEqualTo(400);
      }

      [Observation]
      public void should_return_the_molweight_parameter_value_when_passing_the_molecule_name()
      {
         sut.MolWeightFor(_quantityWithMolWeight.Name).ShouldBeEqualTo(400);
      }
   }

   public class When_retrieving_the_molecule_name_for_a_given_quantity : concern_for_Model
   {
      private MoleculeAmount _molecule;
      private Parameter _parameterUnderMolecule;
      private Parameter _parameterUnderAnotherContainer;
      private Parameter _aParameterWithoutParent;

      protected override void Context()
      {
         base.Context();
         var root = new Container().WithName("Root").WithId("Root");
         sut.Root = root;
         _molecule = new MoleculeAmount().WithName("Molecule");
         var globalMoleculeContainer = new Container().WithName("toto").WithContainerType(ContainerType.Molecule);
         _parameterUnderMolecule = new Parameter().WithName("Param").WithFormula(new ConstantFormula(400));
         _parameterUnderAnotherContainer = new Parameter().WithName("Param").WithFormula(new ConstantFormula(400));
         globalMoleculeContainer.Add(_parameterUnderMolecule);

         sut.Root.Add(_molecule);
         sut.Root.Add(globalMoleculeContainer);
         var anotherContainer = new Container().WithName("anotherContainer");
         sut.Root.Add(anotherContainer);
         anotherContainer.Add(_parameterUnderAnotherContainer);

         _aParameterWithoutParent = new Parameter().WithName("_aParameterWithoutParent").WithFormula(new ConstantFormula(400));
      }

      [Observation]
      public void should_return_empty_string_if_the_quantity_is_null()
      {
         sut.MoleculeNameFor((string) null).ShouldBeNullOrEmpty();
         sut.MoleculeNameFor((IQuantity) null).ShouldBeNullOrEmpty();
      }

      [Observation]
      public void should_return_the_name_of_the_molecule_for_a_molecule_amount()
      {
         sut.MoleculeNameFor(_molecule).ShouldBeEqualTo(_molecule.Name);
      }

      [Observation]
      public void should_return_the_name_of_the_molecule_container_for_a_parameter_otherwise()
      {
         sut.MoleculeNameFor(_parameterUnderMolecule).ShouldBeEqualTo("toto");
      }

      [Observation]
      public void should_return_empty_string_if_the_quantity_is_in_a_container_that_is_not_a_molecule()
      {
         sut.MoleculeNameFor(_parameterUnderAnotherContainer).ShouldBeNullOrEmpty();
      }

      [Observation]
      public void should_return_empty_string_if_the_quantity_is_not_a_molecule_and_not_in_a_container()
      {
         sut.MoleculeNameFor(_aParameterWithoutParent).ShouldBeNullOrEmpty();
      }
   }
}