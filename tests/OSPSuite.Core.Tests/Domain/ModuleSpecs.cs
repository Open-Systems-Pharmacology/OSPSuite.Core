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
            ObserverBlock = new ObserverBuildingBlock(),
            EventBlock = null,
            ReactionBlock = new ReactionBuildingBlock(),
            MoleculeBlock = new MoleculeBuildingBlock()
         };
         
         sut.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         sut.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
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
         _clone.ObserverBlock.ShouldNotBeNull();
         _clone.EventBlock.ShouldBeNull();
         _clone.ReactionBlock.ShouldNotBeNull();
         _clone.MoleculeBlock.ShouldNotBeNull();

         _clone.MoleculeStartValuesBlockCollection.ShouldNotBeEmpty();
         _clone.ParameterStartValuesBlockCollection.ShouldNotBeNull();
         
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
