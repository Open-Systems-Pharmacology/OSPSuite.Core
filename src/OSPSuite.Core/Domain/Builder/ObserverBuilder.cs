using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Base interface for all observer builder types
   /// </summary>
   public interface IObserverBuilder : IUsingFormula, IMoleculeDependentBuilder
   {
      /// <summary>
      ///    Criteria for containers where given observer should be created
      /// </summary>
      DescriptorCriteria ContainerCriteria { get; set; }
   }

   /// <summary>
   ///    Base class for all concrete observer builders
   /// </summary>
   public class ObserverBuilder : Entity, IObserverBuilder
   {
      private readonly MoleculeList _moleculeList;
      public IFormula Formula { get; set; }
      public IDimension Dimension { get; set; }
      public DescriptorCriteria ContainerCriteria { get; set; }

      public ObserverBuilder()
      {
         _moleculeList = new MoleculeList {ForAll = false};
         ContainerCriteria = new DescriptorCriteria();
         Icon = IconNames.OBSERVER;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceObserverBuilder = source as IObserverBuilder;
         if (sourceObserverBuilder == null) return;
         Dimension = sourceObserverBuilder.Dimension;
         ContainerCriteria = sourceObserverBuilder.ContainerCriteria.Clone();
         _moleculeList.UpdatePropertiesFrom(sourceObserverBuilder.MoleculeList, cloneManager);
      }

      public MoleculeList MoleculeList
      {
         get { return _moleculeList; }
      }

      public bool ForAll
      {
         get { return MoleculeList.ForAll; }
         set { MoleculeList.ForAll = value; }
      }
   }
}