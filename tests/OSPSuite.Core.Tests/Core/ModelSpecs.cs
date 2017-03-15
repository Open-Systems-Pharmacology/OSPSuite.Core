using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

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

      protected override void Context()
      {
         base.Context();
         var root = new Container().WithName("Root").WithId("Root");
         sut.Root = root;
         _quantityWithMolWeight = new MoleculeAmount().WithName("Molecule");
         var quantityWithMolWeightContainer = new Container().WithName(_quantityWithMolWeight.Name);
         quantityWithMolWeightContainer.Add(new Parameter().WithName(Constants.Parameters.MOL_WEIGHT).WithFormula(new ConstantFormula(400)));

         _quantityWithoutMolWeight = new MoleculeAmount().WithName("Molecule2");
         var quantityWithoutMolWeightContainer = new Container().WithName(_quantityWithoutMolWeight.Name);

         sut.Root.Add(quantityWithMolWeightContainer);
         sut.Root.Add(quantityWithoutMolWeightContainer);
      }

      [Observation]
      public void should_return_null_if_the_quantity_is_not_an_observer_or_an_amount()
      {
         sut.MolWeightFor(new Parameter()).ShouldBeNull();
      }

      [Observation]
      public void should_return_null_if_no_molweight_parameter_was_found_unter_the_global_container()
      {
         sut.MolWeightFor(_quantityWithoutMolWeight).ShouldBeNull();
      }

      [Observation]
      public void should_return_the_molweight_parameter_value_otherwise()
      {
         sut.MolWeightFor(_quantityWithMolWeight).ShouldBeEqualTo(400);
      }
   }
}