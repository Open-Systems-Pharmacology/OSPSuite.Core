using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_PathToPathElementsMapper : ContextSpecification<IPathToPathElementsMapper>
   {
      protected IContainer _rootContainer;
      protected List<string> _path;
      protected PathElements _result;
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _entityPathResolver = new EntityPathResolverForSpecs();
         sut = new PathToPathElementsMapper(_entityPathResolver);
         _rootContainer = new Container();
         _path = new List<string>();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_rootContainer, _path);
      }

      protected IContainer CreateContainer(IContainer parentContainer, string name, ContainerType containerType = ContainerType.Other)
      {
         var container = new Container().WithName(name).WithContainerType(containerType);
         if (parentContainer != null)
            container.WithParentContainer(parentContainer);

         return container;
      }

      protected void ShouldReturnPathElementValues(string simulation, string topContainer, string container, string compartment, string molecule, string name)
      {
         _result[PathElement.Simulation].DisplayName.ShouldBeEqualTo(simulation);
         _result[PathElement.TopContainer].DisplayName.ShouldBeEqualTo(topContainer);
         _result[PathElement.Container].DisplayName.ShouldBeEqualTo(container);
         _result[PathElement.BottomCompartment].DisplayName.ShouldBeEqualTo(compartment);
         _result[PathElement.Molecule].DisplayName.ShouldBeEqualTo(molecule);
         _result[PathElement.Name].DisplayName.ShouldBeEqualTo(name);
      }
   }

   public class When_mapping_the_path_of_a_molecule_observer_defined_in_the_plama_liver_pericentral_which_starts_with_simulation_name : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var organism = CreateContainer(_rootContainer, "Organism");
         var liver = CreateContainer(organism, "Liver");
         var pericentral = CreateContainer(liver, "Pericentral");
         var plasma = CreateContainer(pericentral, "Plasma", ContainerType.Compartment);
         var molecule = CreateContainer(plasma, "ABC", ContainerType.Molecule);
         var observer = new Observer().WithName("Concentration").WithParentContainer(molecule);

         _path = new List<string> {"Sim", "Organism", "Liver", "Pericentral", "Plasma", "ABC", "Concentration"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("Sim", "Organism", new[] {"Liver", "Pericentral"}.ToString(Constants.DISPLAY_PATH_SEPARATOR), "Plasma", "ABC", "Concentration");
      }
   }

   public class When_mapping_the_path_of_a_molecule_observer_defined_in_the_plama_liver_pericentral_which_starts_with_organism_name : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var organism = CreateContainer(_rootContainer, "Organism");
         var liver = CreateContainer(organism, "Liver");
         var pericentral = CreateContainer(liver, "Pericentral");
         var plasma = CreateContainer(pericentral, "Plasma", ContainerType.Compartment);
         var molecule = CreateContainer(plasma, "ABC", ContainerType.Molecule);
         var observer = new Observer().WithName("Concentration").WithParentContainer(molecule);

         _path = new List<string> {"Organism", "Liver", "Pericentral", "Plasma", "ABC", "Concentration"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("", "Organism", new[] {"Liver", "Pericentral"}.ToString(Constants.DISPLAY_PATH_SEPARATOR), "Plasma", "ABC", "Concentration");
      }
   }

   public class When_mapping_the_path_of_a_molecule_observer_defined_in_the_neighborhoods : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var neighborhoods = CreateContainer(_rootContainer, "Neighborhoods");
         var bc_bc = CreateContainer(neighborhoods, "ArterialBlood_bc_Bone_bc");
         var molecule = CreateContainer(bc_bc, "ABC", ContainerType.Molecule);
         var observer = new Observer().WithName("Observe").WithParentContainer(molecule);

         _path = new List<string> {"Sim", "Neighborhoods", "ArterialBlood_bc_Bone_bc", "ABC", "Observe"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("Sim", "Neighborhoods", "ArterialBlood_bc_Bone_bc", "", "ABC", "Observe");
      }
   }

   public class When_mapping_the_path_of_a_molecule_observer_defined_in_the_applications : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var applications = CreateContainer(_rootContainer, "Applications");
         var application = CreateContainer(applications, "Application_1");
         var molecule = CreateContainer(application, "ABC", ContainerType.Molecule);
         var observer = new Observer().WithName("Observe").WithParentContainer(molecule);

         _path = new List<string> {"Sim", "Applications", "Application_1", "ABC", "Observe"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("Sim", "Applications", "Application_1", "", "ABC", "Observe");
      }
   }

   public class When_mapping_the_path_of_a_plotparameter_defined_in_the_neighborhoods : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var neighborhoods = CreateContainer(_rootContainer, "Neighborhoods");
         var bc_bc = CreateContainer(neighborhoods, "ArterialBlood_bc_Bone_bc");
         var observer = new Observer().WithName("Observe").WithParentContainer(bc_bc);

         _path = new List<string> {"Sim", "Neighborhoods", "ArterialBlood_bc_Bone_bc", "Observe"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("Sim", "Neighborhoods", "ArterialBlood_bc_Bone_bc", "", "", "Observe");
      }
   }

   public class When_mapping_the_path_of_an_organ_volume_parameter_in_individual : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = new ARootContainer().WithName("ROOT");
         var organism = CreateContainer(_rootContainer, "Organism", ContainerType.Organism);
         var liver = CreateContainer(organism, "Liver");
         var parameter = new DistributedParameter().WithName("Volume").WithParentContainer(liver);
         _path = new List<string> {"Organism", "Liver", "Volume"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues(string.Empty, "Organism", "Liver", string.Empty, string.Empty, "Volume");
      }
   }

   public class When_mapping_the_path_of_a_molecule_amount_in_a_container : concern_for_PathToPathElementsMapper
   {
      private MoleculeAmount _moleculeAmount;

      protected override void Context()
      {
         base.Context();
         _rootContainer = CreateContainer(null, "Sim", ContainerType.Simulation);
         var organism = CreateContainer(_rootContainer, "Organism");
         var liver = CreateContainer(organism, "Liver");
         var plasma = CreateContainer(liver, "Plasma", ContainerType.Compartment);
         _moleculeAmount = new MoleculeAmount().WithName("ABC").WithParentContainer(plasma);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moleculeAmount);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues("Sim", "Organism", "Liver", "Plasma", "ABC", Captions.AmountInContainer);
      }
   }

   public class When_mapping_the_path_of_a_container : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Organism", ContainerType.Organism);
         var liver = CreateContainer(_rootContainer, "Liver");
         _path = new List<string> {"Organism", "Liver"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues(string.Empty, "Organism", "Liver", string.Empty, string.Empty, "Liver");
      }
   }

   public class When_mapping_the_path_of_an_entity : concern_for_PathToPathElementsMapper
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();

         var rootContainer = new ARootContainer().WithName("ROOT");
         var organism = CreateContainer(rootContainer, "Organism", ContainerType.Organism);
         var liver = CreateContainer(organism, "Liver");
         _parameter = new DistributedParameter().WithName("Volume").WithParentContainer(liver);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_parameter);
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues(string.Empty, "Organism", "Liver", string.Empty, string.Empty, "Volume");
      }
   }

   public class When_mapping_the_path_of_an_null_entity : concern_for_PathToPathElementsMapper
   {
      protected override void Because()
      {
         _result = sut.MapFrom(null);
      }

      [Observation]
      public void should_return_an_empty_path()
      {
         ShouldReturnPathElementValues(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
      }
   }

   public class When_mapping_the_path_of_a_compartment : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Organism", ContainerType.Organism);
         var liver = CreateContainer(_rootContainer, "Liver");
         var plasma = CreateContainer(liver, "Plasma", ContainerType.Compartment);
         _path = new List<string> {"Organism", "Liver", "Plasma"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues(string.Empty, "Organism", "Liver", "Plasma", string.Empty, "Plasma");
      }
   }

   public class When_mapping_a_path_that_has_more_entries_that_the_actual_container_structure : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();

         _rootContainer = CreateContainer(null, "Organism", ContainerType.Organism);
         var liver = CreateContainer(_rootContainer, "Liver");
         var plasma = CreateContainer(liver, "Plasma", ContainerType.Compartment);
         _path = new List<string> {"Organism", "Liver", "DOES_NOT_EXIST", "HELLO", "Plasma"};
      }

      [Observation]
      public void should_return_the_expected_path()
      {
         ShouldReturnPathElementValues(string.Empty, "Organism", "Liver", string.Empty, string.Empty, "Plasma");
      }
   }

   public class When_mapping_the_path_of_an_element_without_root_container : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();
         _rootContainer = null;
         _path = new List<string> {"P"};
      }

      [Observation]
      public void should_simply_return_the_name_of_the_element()
      {
         ShouldReturnPathElementValues(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "P");
      }
   }

   public class When_mapping_the_path_of_a_simulation : concern_for_PathToPathElementsMapper
   {
      protected override void Context()
      {
         base.Context();
         _rootContainer = new ARootContainer().WithName("ROOT");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_rootContainer);
      }

      [Observation]
      public void should_simply_return_the_name_of_the_element()
      {
         ShouldReturnPathElementValues("ROOT", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
      }
   }
}