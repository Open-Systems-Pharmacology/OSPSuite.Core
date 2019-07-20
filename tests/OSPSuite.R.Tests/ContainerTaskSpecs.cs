using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Core.Domain.Constants.Parameters;
using ContainerTask = OSPSuite.R.Services.ContainerTask;
using IContainerTask = OSPSuite.R.Services.IContainerTask;

namespace OSPSuite.R
{
   public abstract class concern_for_ContainerTask : ContextSpecification<IContainerTask>
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
      protected IParameter _gfr;
      private IEntityPathResolver _entityPathResolver;
      protected IParameter _clearance;

      protected override void Context()
      {
         _entityPathResolver = new EntityPathResolverForSpecs();

         sut = new ContainerTask(_entityPathResolver);

         _organism = new Container().WithName(ORGANISM);
         _liver = new Container().WithName("Liver");
         _kidney = new Container().WithName("Kidney");
         _liverIntracellular = new Container().WithName(INTRACELLULAR);
         _kidneyIntracellular = new Container().WithName(INTRACELLULAR);
         _volumeLiver = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(VOLUME);
         _volumeOrganism = DomainHelperForSpecs.ConstantParameterWithValue(20).WithName(VOLUME);
         _volumeLiverCell = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName(VOLUME);
         _liverIntracellularSubContainer = new Container().WithName("Intracellular Sub Container");
         _volumeKidneyCell = DomainHelperForSpecs.ConstantParameterWithValue(3).WithName(VOLUME);
         _gfr = DomainHelperForSpecs.ConstantParameterWithValue(3).WithName("GFR");
         _volumeKidney = DomainHelperForSpecs.ConstantParameterWithValue(14).WithName(VOLUME);
         _height = DomainHelperForSpecs.ConstantParameterWithValue(175).WithName("Height");
         _weight = DomainHelperForSpecs.ConstantParameterWithValue(75).WithName("Weight");

         _clearance = DomainHelperForSpecs.ConstantParameterWithValue(5).WithName("CL");

         _organism.AddChildren(_liver, _kidney, _height, _weight, _volumeOrganism);
         _liver.AddChildren(_liverIntracellular, _volumeLiver);
         _liverIntracellular.AddChildren(_volumeLiverCell, _liverIntracellularSubContainer);
         _liverIntracellularSubContainer.Add(_clearance);

         _kidney.AddChildren(_kidneyIntracellular, _volumeKidney);
         _kidneyIntracellular.AddChildren(_volumeKidneyCell, _gfr);
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_an_explicit_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_one_parameter_if_it_exists()
      {
         sut.AllParametersMatching(_organism, _liver.Name, INTRACELLULAR, VOLUME).ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, _liver.Name, VOLUME).ShouldOnlyContain(_volumeLiver);
         sut.AllParametersMatching(_organism, _kidney.Name, VOLUME).ShouldOnlyContain(_volumeKidney);

         sut.AllParametersMatching(_kidney, VOLUME).ShouldOnlyContain(_volumeKidney);
         sut.AllParametersMatching(_kidney, INTRACELLULAR, _gfr.Name).ShouldOnlyContain(_gfr);
      }

      [Observation]
      public void should_return_an_empty_enumerable_if_it_does_not_exist()
      {
         sut.AllParametersMatching(_organism, _liver.Name, INTRACELLULAR, "Does not exist").ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, WILD_CARD, INTRACELLULAR, VOLUME).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, WILD_CARD, VOLUME).ShouldOnlyContain(_volumeLiver, _volumeKidney);
         sut.AllParametersMatching(_kidney, WILD_CARD, _gfr.Name).ShouldOnlyContain(_gfr);
         sut.AllParametersMatching(_kidney, WILD_CARD, _clearance.Name).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_parameter_name_using_wildcards : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, _liver.Name, INTRACELLULAR, $"Vol{WILD_CARD}").ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, _liver.Name, INTRACELLULAR, $"{WILD_CARD}Vol").ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_container_name_using_wildcards : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, $"Liv{WILD_CARD}", INTRACELLULAR, $"Vol{WILD_CARD}").ShouldOnlyContain(_volumeLiverCell);
         sut.AllParametersMatching(_organism, $"{WILD_CARD}Liv", INTRACELLULAR, VOLUME).ShouldBeEmpty();
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, WILD_CARD_REC, INTRACELLULAR, VOLUME).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, WILD_CARD_REC, VOLUME).ShouldOnlyContain(_volumeLiver, _volumeKidney, _volumeKidneyCell, _volumeLiverCell);
         sut.AllParametersMatching(_organism, WILD_CARD_REC, _clearance.Name).ShouldOnlyContain(_clearance);
      }
   }

   public class When_resolving_all_parameters_of_a_container_with_a_path_containing_the_container_wildcard_recursive_and_wildcard : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, WILD_CARD_REC, WILD_CARD, VOLUME).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell);
         sut.AllParametersMatching(_organism, WILD_CARD_REC, INTRACELLULAR, WILD_CARD).ShouldOnlyContain(_volumeLiverCell, _volumeKidneyCell, _gfr);

         sut.AllParametersMatching(_organism, WILD_CARD_REC, WILD_CARD).ShouldOnlyContain(_volumeLiver, _volumeKidney, _gfr, _volumeKidneyCell, _volumeLiverCell, _clearance);
         sut.AllParametersMatching(_organism, $"Liv{WILD_CARD}", $"{WILD_CARD}INTR{WILD_CARD}", $"{WILD_CARD}ol{WILD_CARD}").ShouldOnlyContain(_volumeLiverCell);
      }
   }

   public class When_resolving_all_parameters_recursively_from_a_container : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_matching_parameters()
      {
         sut.AllParametersMatching(_organism, WILD_CARD_REC).ShouldOnlyContain(_organism.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_liver, WILD_CARD_REC).ShouldOnlyContain(_liver.GetAllChildren<IParameter>());
         sut.AllParametersMatching(_organism, WILD_CARD).ShouldOnlyContain(_organism.GetChildren<IParameter>());
      }
   }


   public class When_retrieving_all_containers_matching_a_given_path : concern_for_ContainerTask
   {
      [Observation]
      public void should_return_the_expected_containers ()
      {
         sut.AllContainersMatching(_organism, "Liver").ShouldOnlyContain(_liver);
         sut.AllContainersMatching(_organism, WILD_CARD).ShouldOnlyContain(_liver,_kidney);
         sut.AllContainersMatching(_organism, WILD_CARD, WILD_CARD).ShouldOnlyContain(_liverIntracellular,_kidneyIntracellular);
         sut.AllContainersMatching(_organism, WILD_CARD_REC, WILD_CARD).ShouldOnlyContain(_liverIntracellular,_kidneyIntracellular, _liverIntracellularSubContainer);
      }
   }
}