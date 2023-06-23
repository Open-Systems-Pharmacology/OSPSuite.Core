using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Base class for all concrete observer builders
   /// </summary>
   public class ObserverBuilder : Entity, IUsingFormula, IMoleculeDependentBuilder
   {
      public IFormula Formula { get; set; }
      public IDimension Dimension { get; set; }

      /// <summary>
      ///    Criteria for containers where given observer should be created
      /// </summary>
      public DescriptorCriteria ContainerCriteria { get; set; }

      public MoleculeList MoleculeList { get; }

      public IBuildingBlock BuildingBlock { get; set; }

      public ObserverBuilder()
      {
         MoleculeList = new MoleculeList {ForAll = false};
         ContainerCriteria = new DescriptorCriteria();
         Icon = IconNames.OBSERVER;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceObserverBuilder = source as ObserverBuilder;
         if (sourceObserverBuilder == null) return;
         Dimension = sourceObserverBuilder.Dimension;
         ContainerCriteria = sourceObserverBuilder.ContainerCriteria.Clone();
         MoleculeList.UpdatePropertiesFrom(sourceObserverBuilder.MoleculeList, cloneManager);
      }

      public bool ForAll
      {
         get => MoleculeList.ForAll;
         set => MoleculeList.ForAll = value;
      }
   }
}