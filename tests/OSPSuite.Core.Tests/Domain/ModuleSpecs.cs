﻿using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Module : ContextSpecification<Module>
   {
      protected override void Context()
      {
         sut = new Module
         {
            new PassiveTransportBuildingBlock(),
            new SpatialStructure(),
            new ObserverBuildingBlock(),
            new ReactionBuildingBlock(),
            new MoleculeBuildingBlock(),
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };
      }
   }

   public class When_settings_properties_via_extended_properties_in_a_module : concern_for_Module
   {
      [Observation]
      public void should_be_able_to_update_the_value_for_different_types()
      {
         sut.AddExtendedProperty("a double", 1.2);
         sut.ExtendedPropertyValueFor<double>("a double").ShouldBeEqualTo(1.2);
         sut.ExtendedPropertyValueFor("a double").ShouldBeEqualTo("1.2");

         sut.AddExtendedProperty("a string", "value");
         sut.ExtendedPropertyValueFor<string>("a string").ShouldBeEqualTo("value");
         sut.ExtendedPropertyValueFor("a string").ShouldBeEqualTo("value");
      }
   }

   public class When_removing_start_values_building_blocks : concern_for_Module
   {
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private ParameterValuesBuildingBlock _parameterValuesBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock().WithId("newInitialConditions");
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock().WithId("newParameterValues");
      }

      protected override void Because()
      {
         sut.Add(_initialConditionsBuildingBlock);
         sut.Add(_parameterValuesBuildingBlock);
         sut.Remove(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_correct_initial_conditions_should_have_been_removed()
      {
         sut.ParameterValuesCollection.Count.ShouldBeEqualTo(2);
         sut.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
         sut.InitialConditionsCollection.FindById(_initialConditionsBuildingBlock.Id).ShouldBeNull();
      }
   }

   public class When_getting_the_list_of_building_blocks : concern_for_Module
   {
      private IReadOnlyList<IBuildingBlock> _result;

      protected override void Context()
      {
         base.Context();
         sut.Add(new EventGroupBuildingBlock());
      }

      protected override void Because()
      {
         _result = sut.BuildingBlocks;
      }

      [Observation]
      public void the_list_should_include_all_the_building_blocks()
      {
         _result.ShouldOnlyContain(sut.PassiveTransports, sut.EventGroups, sut.Molecules, sut.Reactions, sut.Observers, sut.SpatialStructure,
            sut.ParameterValuesCollection.First(), sut.InitialConditionsCollection.First());
      }
   }

   public class When_the_module_is_cloned : concern_for_Module
   {
      protected Module _clone;
      private DimensionFactoryForIntegrationTests _dimensionFactory;
      private IModelFinalizer _modelFinalizer;
      private CloneManagerForModel _cloneManager;

      protected override void Context()
      {
         base.Context();
         sut.AddExtendedProperty("PKSimVersion", "1.2.3");
         _modelFinalizer = A.Fake<IModelFinalizer>();
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         _cloneManager = new CloneManagerForModel(new ObjectBaseFactoryForSpecs(_dimensionFactory), new DataRepositoryTask(), _modelFinalizer);
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      [Observation]
      public void should_have_created_a_clone_with_the_same_properties()
      {
         _clone.PassiveTransports.ShouldNotBeNull();
         _clone.SpatialStructure.ShouldNotBeNull();
         _clone.Observers.ShouldNotBeNull();
         _clone.EventGroups.ShouldBeNull();
         _clone.Reactions.ShouldNotBeNull();
         _clone.Molecules.ShouldNotBeNull();

         _clone.InitialConditionsCollection.ShouldNotBeEmpty();
         _clone.ParameterValuesCollection.ShouldNotBeNull();

         _clone.ExtendedPropertyValueFor("PKSimVersion").ShouldBeEqualTo("1.2.3");

         _clone.PassiveTransports.Module.ShouldBeEqualTo(_clone);
      }
   }

   public class When_adding_a_building_block_to_a_module : concern_for_Module
   {
      protected IBuildingBlock _buildingBlock;
      private SpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new ReactionBuildingBlock().WithId("newReactionBuildingBlock");
         sut = new Module();
         _spatialStructure = new SpatialStructure();
         sut.Add(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Add(_buildingBlock);
      }

      [Observation]
      public void should_set_the_reference_to_the_module_in_the_building_block()
      {
         sut.Reactions.Module.ShouldBeEqualTo(sut);
         sut.SpatialStructure.Module.ShouldBeEqualTo(sut);
      }

      [Observation]
      public void should_add_a_reaction()
      {
         sut.Reactions.ShouldBeEqualTo(_buildingBlock);
         sut.BuildingBlocks.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_adding_a_building_block_that_already_exists_by_type_to_a_module : concern_for_Module
   {
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new ReactionBuildingBlock();
         sut = new Module { _buildingBlock };
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Add(new ReactionBuildingBlock())).ShouldThrowAn<OSPSuiteException>();
      }
   }
   
   public class When_comparing_module_versions_with_different_ordering_of_building_block : concern_for_Module
   {
      private string _initialVersion;

      protected override void Context()
      {
         sut = new Module
         {
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock(),
            new EventGroupBuildingBlock()
         };

         _initialVersion = sut.Version;
      }

      protected override void Because()
      {
         sut = new Module
         {
            new ParameterValuesBuildingBlock(),
            new InitialConditionsBuildingBlock(),
            new EventGroupBuildingBlock()
         };
      }

      [Observation]
      public void the_version_should_not_match()
      {
         sut.Version.ShouldBeEqualTo(_initialVersion);
      }
   }
   
   public class When_removing_a_building_block : concern_for_Module
   {
      private string _preAddVersion;
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;

      protected override void Context()
      {
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         sut = new Module
         {
            _initialConditionsBuildingBlock,
            new ParameterValuesBuildingBlock(),
            new EventGroupBuildingBlock()
         };

         _preAddVersion = sut.Version;
      }

      protected override void Because()
      {
         sut.Remove(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_version_should_not_match()
      {
         sut.Version.ShouldNotBeEqualTo(_preAddVersion);
      }
   }

   public class When_building_blocks_with_same_version_but_different_types_should_give_different_version : concern_for_Module
   {
      private string _initialVersion;

      protected override void Context()
      {
         var initialConditionsBuildingBlock = new InitialConditionsBuildingBlock { Version = 1 };
         sut = new Module
         {
            initialConditionsBuildingBlock
         };

         _initialVersion = sut.Version;

         sut.Remove(initialConditionsBuildingBlock);
      }

      protected override void Because()
      {
         sut.Add(new MoleculeBuildingBlock { Version = 1 });
      }

      [Observation]
      public void the_version_should_not_match()
      {
         sut.Version.ShouldNotBeEqualTo(_initialVersion);
      }
   }

   public class When_adding_a_building_block : concern_for_Module
   {
      private string _preAddVersion;

      protected override void Context()
      {
         sut = new Module
         {
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock(),
            new EventGroupBuildingBlock()
         };

         _preAddVersion = sut.Version;
      }

      protected override void Because()
      {
         sut.Add(new MoleculeBuildingBlock());
      }

      [Observation]
      public void the_version_should_not_match()
      {
         sut.Version.ShouldNotBeEqualTo(_preAddVersion);
      }
   }

   public class When_calculating_a_context_specific_version_for_a_module : concern_for_Module
   {
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      private string _originalVersion;

      protected override void Context()
      {
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         sut = new Module
         {
            _initialConditionsBuildingBlock,
            _parameterValuesBuildingBlock,
            new EventGroupBuildingBlock()
         };
         _originalVersion = sut.VersionWith(_parameterValuesBuildingBlock, _initialConditionsBuildingBlock);
      }

      protected override void Because()
      {
         sut.Add(new InitialConditionsBuildingBlock());
         sut.Add(new ParameterValuesBuildingBlock());
      }

      [Observation]
      public void the_context_specific_version_should_match_the_original_context_specific_version()
      {
         sut.VersionWith(_parameterValuesBuildingBlock, _initialConditionsBuildingBlock).ShouldBeEqualTo(_originalVersion);
      }
   }
}