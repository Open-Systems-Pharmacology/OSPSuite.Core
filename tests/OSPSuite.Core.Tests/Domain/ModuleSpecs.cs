using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Module : ContextSpecification<Module>
   {
      protected override void Context()
      {
         sut = new Module()
         {
            PassiveTransports = new PassiveTransportBuildingBlock(),
            SpatialStructure = new SpatialStructure(),
            Observers = new ObserverBuildingBlock(),
            EventGroups = null,
            Reactions = new ReactionBuildingBlock(),
            Molecules = new MoleculeBuildingBlock()
         };

         sut.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         sut.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
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

   public class When_getting_the_list_of_building_blocks : concern_for_Module
   {
      private IReadOnlyList<IBuildingBlock> _result;

      protected override void Context()
      {
         base.Context();
         sut.EventGroups = new EventGroupBuildingBlock();
      }

      protected override void Because()
      {
         _result = sut.AllBuildingBlocks();
      }

      [Observation]
      public void the_list_should_include_all_the_building_blocks()
      {
         _result.ShouldOnlyContain(sut.PassiveTransports, sut.EventGroups, sut.Molecules, sut.Reactions, sut.Observers, sut.SpatialStructure, sut.ParameterStartValuesCollection.First(), sut.MoleculeStartValuesCollection.First());
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

         _clone.MoleculeStartValuesCollection.ShouldNotBeEmpty();
         _clone.ParameterStartValuesCollection.ShouldNotBeNull();

         _clone.ExtendedPropertyValueFor("PKSimVersion").ShouldBeEqualTo("1.2.3");
      }
   }
}