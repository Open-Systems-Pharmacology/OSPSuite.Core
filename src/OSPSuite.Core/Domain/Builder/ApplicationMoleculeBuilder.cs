using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Location and start formula for the molecule amounts
   ///    <para></para>
   ///    which are defined within any application subcontainer.
   ///    Formula is the start formula of the application molecule amount for the given compartment
   /// </summary>
   public interface IApplicationMoleculeBuilder : IUsingFormula
   {
      /// <summary>
      ///    Path to the container where the molecule amount will be created
      ///    <para></para>
      ///    Must be defined relative to the root container of the application
      /// </summary>
      IObjectPath RelativeContainerPath { get; set; }
   }

   public class ApplicationMoleculeBuilder : Entity, IApplicationMoleculeBuilder
   {
      public IObjectPath RelativeContainerPath { get; set; }
      public IFormula Formula { get; set; }
      public IDimension Dimension { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcAppMoleculeBuilder = source as IApplicationMoleculeBuilder;
         if (srcAppMoleculeBuilder == null) return;

         RelativeContainerPath = srcAppMoleculeBuilder.RelativeContainerPath.Clone<IObjectPath>();
         Formula = cloneManager.Clone(srcAppMoleculeBuilder.Formula);
         Dimension = srcAppMoleculeBuilder.Dimension;
      }
   }
}