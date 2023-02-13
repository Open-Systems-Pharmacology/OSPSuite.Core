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
   public class concern_for_Module<T> : ContextSpecification<T> where T: Module, new()
   {
      protected override void Context()
      {
         sut = new T
         {
            PassiveTransport = new PassiveTransportBuildingBlock(),
            SpatialStructure = new SpatialStructure(),
            Observer = new ObserverBuildingBlock(),
            EventGroup = null,
            Reaction = new ReactionBuildingBlock(),
            Molecule = new MoleculeBuildingBlock()
         };
         
         sut.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         sut.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
      }
   }

   public class When_getting_the_list_of_building_blocks : concern_for_Module<Module>
   {
      private IReadOnlyList<IBuildingBlock> _result;

      protected override void Context()
      {
         base.Context();
         sut.EventGroup = new EventGroupBuildingBlock();
      }

      protected override void Because()
      {
         _result = sut.AllBuildingBlocks();
      }

      [Observation] 
      public void the_list_should_include_all_the_building_blocks()
      {
         _result.ShouldOnlyContain(sut.PassiveTransport, sut.EventGroup, sut.Molecule, sut.Reaction, sut.Observer, sut.SpatialStructure, sut.ParameterStartValuesCollection.First(), sut.MoleculeStartValuesCollection.First());
      }
   }

   public class When_the_module_is_cloned<T> : concern_for_Module<T> where T : Module, new()
   {
      protected T _clone;
      private DimensionFactoryForIntegrationTests _dimensionFactory;
      private IModelFinalizer _modelFinalizer;
      private CloneManagerForModel _cloneManager;

      protected override void Context()
      {
         base.Context();
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
         _clone.PassiveTransport.ShouldNotBeNull();
         _clone.SpatialStructure.ShouldNotBeNull();
         _clone.Observer.ShouldNotBeNull();
         _clone.EventGroup.ShouldBeNull();
         _clone.Reaction.ShouldNotBeNull();
         _clone.Molecule.ShouldNotBeNull();

         _clone.MoleculeStartValuesCollection.ShouldNotBeEmpty();
         _clone.ParameterStartValuesCollection.ShouldNotBeNull();
         
      }
   }

   public class When_the_extension_module_is_cloned : When_the_module_is_cloned<Module>
   {

   }

   public class When_the_pksim_module_is_cloned : When_the_module_is_cloned<PKSimModule>
   {
      protected override void Context()
      {
         base.Context();
         sut.PKSimVersion = "PKSimVersion";
      }

      [Observation]
      public void should_have_created_a_clone_with_the_same_PKSimVersion()
      {
         _clone.PKSimVersion.ShouldBeEqualTo("PKSimVersion");
      }
   }
}
