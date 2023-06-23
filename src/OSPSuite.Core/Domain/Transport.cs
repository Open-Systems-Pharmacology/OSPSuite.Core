using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public class Transport : Process
   {
      public MoleculeAmount SourceAmount { get; set; }
      public MoleculeAmount TargetAmount { get; set; }

      public Transport()
      {
         ContainerType = ContainerType.Transport;
      }

      public override bool Uses(MoleculeAmount amount)
      {
         return Equals(SourceAmount, amount) || Equals(TargetAmount, amount);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcTransport = source as Transport;
         if (srcTransport == null) return;

         //Source/Target molecule amount should NOT be cloned
         //Instead, some Model-Finalizer must be called
      }
   }
}