using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IProcess : IUsingFormula, IContainer
   {
      /// <summary>
      /// Returns if a molecule amount is being actively used by the process 
      /// <para/>
      /// For reaction: Is the amount in educt or product?
      /// <para/>
      /// For transport: Is the amount a source or target?
      /// </summary>
      /// <param name="amount"></param>
      /// <returns></returns>
      bool Uses(IMoleculeAmount amount);
   }

   public abstract class Process : Container, IProcess
   {
      public IDimension Dimension { get; set; }
      public IFormula Formula { get; set; }
      public abstract bool Uses(IMoleculeAmount amount);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcProcess = source as IProcess;
         if (srcProcess == null) return;

         Dimension = srcProcess.Dimension;
      }
   }
}