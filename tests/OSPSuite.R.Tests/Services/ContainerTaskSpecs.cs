using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;

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
         _volumeLiver = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(Constants.Parameters.VOLUME);
         _volumeOrganism = DomainHelperForSpecs.ConstantParameterWithValue(20).WithName(Constants.Parameters.VOLUME);
         _volumeLiverCell = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName(Constants.Parameters.VOLUME);
         _liverIntracellularSubContainer = new Container().WithName("Intracellular Sub Container");
         _volumeKidneyCell = DomainHelperForSpecs.ConstantParameterWithValue(3).WithName(Constants.Parameters.VOLUME);
         _gfr = DomainHelperForSpecs.NormalDistributedParameter(3).WithName("GFR");
         _volumeKidney = DomainHelperForSpecs.ConstantParameterWithValue(14).WithName(Constants.Parameters.VOLUME);
         _height = DomainHelperForSpecs.ConstantParameterWithValue(175).WithName("Height");
         _weight = DomainHelperForSpecs.ConstantParameterWithValue(75).WithName("Weight");

         _clearance = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName("CL");

         _organism.AddChildren(_liver, _kidney, _height, _weight, _volumeOrganism);
         _liver.AddChildren(_liverIntracellular, _volumeLiver);
         _liverIntracellular.AddChildren(_volumeLiverCell, _liverIntracellularSubContainer);
         _liverIntracellularSubContainer.Add(_clearance);

         _kidney.AddChildren(_kidneyIntracellular, _volumeKidney);
         _kidneyIntracellular.AddChildren(_volumeKidneyCell, _gfr);

         _liverIntracellularMoleculeAmount = new MoleculeAmount().WithName("Drug");
         _liverIntracellular.Add(_liverIntracellularMoleculeAmount);
      }

      protected string pathFrom(params string[] paths) => paths.ToPathString();
   }

   public class When_resolving_all_parameters_of_a_container_with_an_explicit_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_one_parameter_if_it_exists()
      {
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, INTRACELLULAR, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiver);
         sut.AllParametersMatching(_organism, pathFrom(_kidney.Name, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeKidney);

         sut.AllParametersMatching(_kidney, Constants.Parameters.VOLUME).ShouldOnlyContain(_volumeKidney);
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
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD, INTRACELLULAR, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiver, _volumeKidney);
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
         sut.AllParametersMatching(_organism, pathFrom($"{Constants.WILD_CARD}Liv", INTRACELLULAR, Constants.Parameters.VOLUME)).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiver, _volumeKidney, _volumeKidneyCell, _volumeLiverCell, _volumeOrganism);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, _clearance.Name)).ShouldOnlyContain(_clearance);
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive_and_wildcard : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD, Constants.Parameters.VOLUME)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _volumeLiver, _volumeKidney);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, INTRACELLULAR, Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _gfr);
         sut.AllParametersMatching(_organism, pathFrom(_liver.Name, Constants.WILD_CARD_RECURSIVE, "Volume")).ShouldOnlyContain(_volumeLiverCell, _volumeLiver);

         sut.AllParametersMatching(_liver, pathFrom(Constants.WILD_CARD_RECURSIVE, Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiver, _volumeLiverCell, _clearance);
         sut.AllParametersMatching(_organism, pathFrom($"Liv{Constants.WILD_CARD}", $"{Constants.WILD_CARD}INTR{Constants.WILD_CARD}", $"{Constants.WILD_CARD}ol{Constants.WILD_CARD}")).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, pathFrom(Constants.WILD_CARD_RECURSIVE, $"INTR{Constants.WILD_CARD}", Constants.WILD_CARD)).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _clearance, _gfr);
         sut.AllParametersMatching(_organism, pathFrom($"Liv{Constants.WILD_CARD}", $"INTRA{Constants.WILD_CARD}", $"{Constants.WILD_CARD}o*")).ShouldOnlyContain(_volumeLiverCell);
      }
   }

   public class When_resolving_all_parameters_recursively_from_a_container : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, Constants.WILD_CARD_RECURSIVE).ShouldOnlyContain(_organism.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_liver, Constants.WILD_CARD_RECURSIVE).ShouldOnlyContain(_liver.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_organism, Constants.WILD_CARD).ShouldOnlyContain(_organism.GetChildren<IParameter>());
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
         sut.AllMoleculesMatching(_organism, Constants.WILD_CARD_RECURSIVE).ShouldOnlyContain(_liverIntracellularMoleculeAmount);
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
            _volumeLiver, _volumeOrganism, _volumeLiverCell, _volumeKidneyCell, _gfr, _volumeKidney, _height, _weight, _clearance, _gfr.MeanParameter, _gfr.DeviationParameter, _gfr.PercentileParameter
         };
         var expected = parameters.Select(x => x.EntityPath()).ToArray();
         _result.ShouldOnlyContain(expected);
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
         var quantities = new IQuantity[] {_volumeLiver, _volumeLiverCell, _clearance, _liverIntracellularMoleculeAmount};
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
}