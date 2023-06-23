using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core
{
   public abstract class concern_for_ConcentrationBaseModelIntegrationTests : ContextForIntegration<IModelConstructor>
   {
      protected CreationResult _result;
      protected IModel _model;
      private IModelCoreSimulation _simulation;
      protected IContainer _organism;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = IoC.Resolve<ConcentrationBaseModelHelperForSpecs>().CreateSimulation();
         _organism = _simulation.Model.Root.GetSingleChildByName<IContainer>(Constants.ORGANISM);
      }

      protected MoleculeAmount Organsim_Molecule_A => _organism.EntityAt<MoleculeAmount>("A");

      protected MoleculeAmount Organism_Molecule_B => _organism.EntityAt<MoleculeAmount>("B");

      protected MoleculeAmount Lung_Molecule_A => _organism.EntityAt<MoleculeAmount>(ConstantsForSpecs.Lung, "A");

      protected MoleculeAmount Lung_Molecule_B => _organism.EntityAt<MoleculeAmount>(ConstantsForSpecs.Lung, "B");

      protected IParameter Organism_StartValue_A => startValueFor(Organsim_Molecule_A);

      protected IParameter Organism_StartValue_B => startValueFor(Organism_Molecule_B);

      protected IParameter Lung_StartValue_A => startValueFor(Lung_Molecule_A);

      protected IParameter Lung_StartValue_B => startValueFor(Lung_Molecule_B);

      private IParameter startValueFor(IContainer molecule)
      {
         return molecule.EntityAt<IParameter>(Constants.Parameters.START_VALUE);
      }
      protected IParameter Volume => _organism.EntityAt<IParameter>(Constants.Parameters.VOLUME);
   }

   public class When_creating_a_concentration_based_model : concern_for_ConcentrationBaseModelIntegrationTests
   {
      [Observation]
      public void should_have_created_the_amount_in_amount()
      {
         Organsim_Molecule_A.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Organism_Molecule_B.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Lung_Molecule_A.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Lung_Molecule_B.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
      }

      [Observation]
      public void should_have_created_the_molecule_amount_formula_in_amount()
      {
         Organsim_Molecule_A.Formula.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Organism_Molecule_B.Formula.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Lung_Molecule_A.Formula.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
         Lung_Molecule_B.Formula.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AMOUNT);
      }

      [Observation]
      public void should_have_created_the_initial_condition_formula_parameter_in_concentration()
      {
         Organism_StartValue_A.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         Organism_StartValue_B.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         Lung_StartValue_A.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         Lung_StartValue_B.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
      }

      [Observation]
      public void should_be_able_to_evaluate_the_start_formula_as_expected()
      {
         Organism_StartValue_A.Value.ShouldBeEqualTo(5);
         Organism_StartValue_B.Value.ShouldBeEqualTo(Organism_StartValue_A.Value / 2);
      }

      [Observation]
      public void should_have_used_the_constant_value_defined_in_the_msv_when_expected()
      {
         Lung_StartValue_B.Value.ShouldBeEqualTo(100);
      }
   }
}