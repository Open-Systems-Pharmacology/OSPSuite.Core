using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public class ApplicationMoleculeBuilder : Entity, IUsingFormula
   {
      /// <summary>
      ///    Path to the container where the molecule amount will be created
      ///    <para></para>
      ///    Must be defined relative to the root container of the application
      /// </summary>
      public ObjectPath RelativeContainerPath { get; set; }

      public IFormula Formula { get; set; }
      public IDimension Dimension { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcAppMoleculeBuilder = source as ApplicationMoleculeBuilder;
         if (srcAppMoleculeBuilder == null) return;

         RelativeContainerPath = srcAppMoleculeBuilder.RelativeContainerPath.Clone<ObjectPath>();
         Formula = cloneManager.Clone(srcAppMoleculeBuilder.Formula);
         Dimension = srcAppMoleculeBuilder.Dimension;
      }
   }
}