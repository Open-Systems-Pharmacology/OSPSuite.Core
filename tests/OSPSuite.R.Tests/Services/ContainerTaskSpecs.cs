using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;
using static OSPSuite.Core.Domain.Constants.Parameters;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_ContainerTask : ContextForIntegration<IContainerTask>
   {
      protected const string INTRACELLULAR = "Intracellular";
      protected IContainer _organism;
      protected IContainer _liver;
      protected IContainer _kidney;
      protected IContainer _liverIntracellular;
      protected IContainer _kidneyIntracellular;
      protected IParameter _volumeLiver;
      protected IParameter _volumeOrganism;
      protected IParameter _volumeLiverCell;
      protected IParameter _volumeKidneyCell;
      protected IParameter _volumeKidney;
      protected IParameter _height;
      protected IParameter _weight;
      protected IContainer _liverIntracellularSubContainer;
      protected IDistributedParameter _gfr;
      protected IParameter _clearance;
      protected MoleculeAmount _liverIntracellularMoleculeAmount;
      protected ISimulation _simulation;
      protected IParameter _paramWithRHS;
      protected IParameter _liverRelExp;
      protected IParameter _volumeLiverCellOtherEntity;

      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetContainerTask();
      }

      protected override void Context()
      {
         _organism = new Container().WithName(Constants.ORGANISM);
         _liver = new Container().WithName("Liver");
         _kidney = new Container().WithName("Kidney");
         _liverIntracellular = new Container().WithName(INTRACELLULAR);
         _kidneyIntracellular = new Container().WithName(INTRACELLULAR);
         _liverRelExp = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("Relative expression (normalized)");
         _volumeLiver = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(VOLUME);

         _volumeLiver.Formula = new ExplicitFormula("1+2");
         _volumeOrganism = DomainHelperForSpecs.ConstantParameterWithValue(20).WithName(VOLUME);
         _volumeLiverCell = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName(VOLUME);
         _volumeLiverCellOtherEntity = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName($"ANOTHER_ENTITY-{VOLUME}");
         _liverIntracellularSubContainer = new Container().WithName("Intracellular Sub Container");
         _volumeKidneyCell = DomainHelperForSpecs.ConstantParameterWithValue(3).WithName(VOLUME);
         _gfr = DomainHelperForSpecs.NormalDistributedParameter(3).WithName("GFR");
         _volumeKidney = DomainHelperForSpecs.ConstantParameterWithValue(14).WithName(VOLUME);
         _height = DomainHelperForSpecs.ConstantParameterWithValue(175).WithName("Height");
         _weight = DomainHelperForSpecs.ConstantParameterWithValue(75).WithName("Weight");
         _paramWithRHS = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("RHSParam");
         _paramWithRHS.RHSFormula = new ExplicitFormula();

         _clearance = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName("CL");

         _organism.AddChildren(_liver, _kidney, _height, _weight, _volumeOrganism, _paramWithRHS);
         _liver.AddChildren(_liverIntracellular, _volumeLiver);
         _liverIntracellular.AddChildren(_volumeLiverCell, _liverIntracellularSubContainer, _liverRelExp, _volumeLiverCellOtherEntity);
         _liverIntracellularSubContainer.Add(_clearance);

         _kidney.AddChildren(_kidneyIntracellular, _volumeKidney);
         _kidneyIntracellular.AddChildren(_volumeKidneyCell, _gfr);

         _liverIntracellularMoleculeAmount = new MoleculeAmount().WithName("Drug");
         _liverIntracellular.Add(_liverIntracellularMoleculeAmount);

         _simulation = A.Fake<ISimulation>();
         _simulation.Model.Root = _organism;
      }

      protected string pathFrom(params string[] paths) => paths.ToPathString();
   }

   public class When_resolving_all_parameters_of_a_container_with_an_explicit_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_one_parameter_if_it_exists()
      {
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, VOLUME)).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, VOLUME)).ShouldOnlyContain(_volumeLiver);
         sut.AllParametersMatching(_organism, pathFrom(_kidney.Name, VOLUME)).ShouldOnlyContain(_volumeKidney);

         sut.AllParametersMatching(_kidney, VOLUME).ShouldOnlyContain(_volumeKidney);
         sut.AllParametersMatching(_kidney, pathFrom(INTRACELLULAR, _gfr.Name)).ShouldOnlyContain(_gfr);
      }

      [Observation]
      public void should_return_an_empty_enumerable_if_it_does_not_exist()
      {
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, "Does not exist")).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD, INTRACELLULAR, VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD, VOLUME)).ShouldOnlyContain(_volumeLiver, _volumeKidney);
         sut.AllParametersMatching(_kidney, pathFrom(Constants.WILD_CARD, _gfr.Name)).ShouldOnlyContain(_gfr);
         sut.AllParametersMatching(_kidney, pathFrom(Constants.WILD_CARD, _clearance.Name)).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_parameter_name_using_wildcards : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, $"Vol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, $"{Constants.WILD_CARD}Vol")).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_container_name_using_wildcards : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom($"Liv{Constants.WILD_CARD}", INTRACELLULAR, $"Vol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, pathFrom($"{Constants.WILD_CARD}Liv", INTRACELLULAR, VOLUME)).ShouldBeEmpty();
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, Constants.WILD_CARD_RECURSIVE, $"{VOLUME}{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiver, _volumeLiverCell);
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, VOLUME)).ShouldOnlyContain(_volumeLiver, _volumeKidney, _volumeKidneyCell, _volumeLiverCell, _volumeOrganism);
          sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, _clearance.Name)).ShouldOnlyContain(_clearance);
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive_and_wildcard : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD, VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _volumeLiver, _volumeKidney);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _gfr, _liverRelExp, _volumeLiverCellOtherEntity);
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, Constants.WILD_CARD_RECURSIVE, VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeLiver);

         sut.AllParametersMatching(_liver, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiver, _volumeLiverCell, _clearance, _liverRelExp, _volumeLiverCellOtherEntity);
         sut.AllParametersMatching(_organism, pathFrom($"Liv{Constants.WILD_CARD}", $"{Constants.WILD_CARD}INTR{Constants.WILD_CARD}", $"{Constants.WILD_CARD}ol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell, _volumeLiverCellOtherEntity);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, $"INTR{Constants.WILD_CARD}", Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _clearance, _gfr, _liverRelExp, _volumeLiverCellOtherEntity);
         sut.AllParametersMatching(_organism, pathFrom($"Liv{Constants.WILD_CARD}", $"INTRA{Constants.WILD_CARD}", $"Vol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell);
      }
   }

   public class When_resolving_all_parameters_recursively_from_a_container : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_organism.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_liver, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_liver.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_organism, Constants.WILD_CARD).ShouldOnlyContain(_organism.GetChildren<IParameter>());
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE)).ShouldOnlyContain(_organism.GetAllChildren<IParameter>());
      }
   }

   public class When_retrieving_all_containers_matching_a_given_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_expected_containers()
      {
         sut.AllContainersMatching(_organism, "Liver").ShouldOnlyContain(_liver);
         sut.AllContainersMatching(_organism, Constants.WILD_CARD).ShouldOnlyContain(_liver, _kidney);
         sut.AllContainersMatching(_organism, pathFrom(Constants.WILD_CARD, Constants.WILD_CARD)).ShouldOnlyContain(_liverIntracellular, _kidneyIntracellular);
         sut.AllContainersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_liverIntracellular, _kidneyIntracellular, _liverIntracellularSubContainer, _liver, _kidney);
      }
   }

   public class When_retrieving_all_parameters_using_a_path_not_formatted_properly : concern_for_ContainerTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AllParametersMatching(_organism, pathFrom(_liver.Name, $"INTR{Constants.WILD_CARD_RECURSIVE}"))).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_resolving_all_quantities_of_a_container_with_a_quantity_name_using_wildcards : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_quantities()
      {
         sut.AllQuantitiesMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, $"Vol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell);
         sut.AllQuantitiesMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, $"{Constants.WILD_CARD}Vol")).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_molecule_amounts_of_a_simulation : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_molecule_amounts()
      {
         sut.AllMoleculesMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_liverIntracellularMoleculeAmount);
         sut.AllMoleculesMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, $"{Constants.WILD_CARD}")).ShouldOnlyContain(_liverIntracellularMoleculeAmount);
         sut.AllMoleculesMatching(_organism, pathFrom(_liver.Name, _liverIntracellularMoleculeAmount.Name)).ShouldBeEmpty();
      }
   }

   public class When_returning_all_parameter_path_from_a_given_container : concern_for_ContainerTask
   {
      private string[] _result;

      protected override void Because()
      {
         _result = sut.AllParameterPathsIn(_organism);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         var parameters = new[]
         {
            _volumeLiver, _volumeOrganism, _volumeLiverCell, _volumeKidneyCell, _gfr, _volumeKidney, _height, _weight, _clearance, _gfr.MeanParameter, _gfr.DeviationParameter, _gfr.PercentileParameter, _paramWithRHS, _liverRelExp, _volumeLiverCellOtherEntity,
         };
         var expected = parameters.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
      }
   }

   public class When_returning_all_parameter_path_from_a_given_simulation : concern_for_ContainerTask
   {
      private string[] _result;

      protected override void Because()
      {
         _result = sut.AllParameterPathsIn(_simulation);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         var parameters = new[]
         {
            _volumeLiver, _volumeOrganism, _volumeLiverCell, _volumeKidneyCell, _gfr, _volumeKidney, _height, _weight, _clearance, _gfr.MeanParameter, _gfr.DeviationParameter, _gfr.PercentileParameter, _paramWithRHS, _liverRelExp, _volumeLiverCellOtherEntity
         };
         var expected = parameters.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
      }
   }

   public class When_retrieving_all_parameters_where_the_name_contains_parenthesis : concern_for_ContainerTask
   {
      [Observation]
      public void should_be_able_to_return_the_parameters()
      {
         sut.AllParametersMatching(_simulation, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, "Relative expression (normalized)")).ShouldOnlyContain(_liverRelExp);
         sut.AllParametersMatching(_simulation, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, "Relative expression (norma*")).ShouldOnlyContain(_liverRelExp);
         sut.AllParametersMatching(_simulation, pathFrom(Constants.WILD_CARD_RECURSIVE, "Relative expression (norma*")).ShouldOnlyContain(_liverRelExp);
      }
   }

   public class When_returning_all_quantity_path_from_a_given_sub_container : concern_for_ContainerTask
   {
      private string[] _result;

      protected override void Because()
      {
         _result = sut.AllQuantityPathsIn(_liver);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         var quantities = new IQuantity[] {_volumeLiver, _volumeLiverCell, _clearance, _liverIntracellularMoleculeAmount, _liverRelExp, _volumeLiverCellOtherEntity};
         var expected = quantities.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
      }
   }

   public class When_returning_all_container_path_from_a_given_container : concern_for_ContainerTask
   {
      private string[] _result;

      protected override void Because()
      {
         _result = sut.AllContainerPathsIn(_organism);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         var containers = new[] {_kidney, _liver, _liverIntracellular, _kidneyIntracellular, _liverIntracellularSubContainer,};
         var expected = containers.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
      }
   }

   public class When_returning_all_state_variable_parameters_from_a_given_container : concern_for_ContainerTask
   {
      private string[] _result;

      protected override void Because()
      {
         _result = sut.AllStateVariableParameterPathsIn(_organism);
      }

      [Observation]
      public void should_only_return_parameters_with_a_RHS_formula_defined()
      {
         var parameters = new[] {_paramWithRHS};
         var expected = parameters.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
      }
   }

   public class When_retrieving_the_is_formula_flag_for_a_given_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_expected_entries()
      {
         The.Action(() => sut.IsExplicitFormulaByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, $"Vol{Constants.WILD_CARD}"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();

         The.Action(() => sut.IsExplicitFormulaByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();

         sut.IsExplicitFormulaByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, VOLUME), throwIfNotFound: true).ShouldBeFalse();
         sut.IsExplicitFormulaByPath(_simulation, pathFrom(_liver.Name, VOLUME), throwIfNotFound: true).ShouldBeTrue();

         //do not exist
         sut.IsExplicitFormulaByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: false).ShouldBeFalse();
      }
   }

   public class When_retrieving_the_base_unit_names_for_a_given_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_expected_entries()
      {
         sut.BaseUnitNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, VOLUME), throwIfNotFound: true).ShouldBeEqualTo(_volumeLiverCell.Dimension.BaseUnit.Name);
         sut.BaseUnitNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: false).ShouldBeNullOrEmpty();
         The.Action(() => sut.BaseUnitNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_retrieving_the_base_dimensions_names_for_a_given_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_expected_entries()
      {
         sut.DimensionNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, VOLUME), throwIfNotFound: true).ShouldBeEqualTo(_volumeLiverCell.Dimension.Name);
         sut.DimensionNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: false).ShouldBeNullOrEmpty();
         The.Action(() => sut.DimensionNameByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "NOPE"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_setting_a_value_by_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_throw_an_exception_if_the_path_contains_wild_cards()
      {
         The.Action(() => sut.SetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, $"Vol{Constants.WILD_CARD}"), 5, throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_an_exception_if_the_path_does_not_exist_in_the_simulation()
      {
         The.Action(() => sut.SetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "TOTO"), 5, throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_not_throw_an_exception_if_the_path_does_not_exist_in_the_simulation_and_the_throw_flag_is_set_to_false()
      {
         sut.SetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "TOTO"), 5, throwIfNotFound: false);
      }

      [Observation]
      public void should_set_the_value_of_the_parameter_as_expected_otherwise()
      {
         sut.SetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, _volumeLiverCell.Name), 666, throwIfNotFound: true);
         _volumeLiverCell.Value.ShouldBeEqualTo(666);
      }
   }

   public class When_getting_a_value_by_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_throw_an_exception_if_the_path_contains_wild_cards()
      {
         The.Action(() => sut.GetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, $"Vol{Constants.WILD_CARD}"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_an_exception_if_the_path_does_not_exist_in_the_simulation()
      {
         The.Action(() => sut.GetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "TOTO"), throwIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_not_throw_an_exception_if_the_path_does_not_exist_in_the_simulation_and_the_throw_flag_is_set_to_false()
      {
         sut.GetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, "TOTO"), throwIfNotFound: false);
      }

      [Observation]
      public void should_get_the_value_of_the_parameter_as_expected_otherwise()
      {
         sut.SetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, _volumeLiverCell.Name), 666, throwIfNotFound: true);
         sut.GetValueByPath(_simulation, pathFrom(_liver.Name, INTRACELLULAR, _volumeLiverCell.Name), throwIfNotFound: true).ShouldBeEqualTo(666);
      }
   }
}