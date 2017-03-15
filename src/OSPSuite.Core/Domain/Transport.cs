using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public interface ITransport : IProcess
   {
      IMoleculeAmount SourceAmount { get; set; }
      IMoleculeAmount TargetAmount { get; set; }
   }

   public class Transport : Process, ITransport
   {
      public IMoleculeAmount SourceAmount { get; set; }
      public IMoleculeAmount TargetAmount { get; set; }

      public Transport()
      {
         ContainerType = ContainerType.Transport;
      }

      public override bool Uses(IMoleculeAmount amount)
      {
         return Equals(SourceAmount, amount) || Equals(TargetAmount, amount);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcTransport = source as ITransport;
         if (srcTransport == null) return;

         //Source/Target molecule amount should NOT be cloned
         //Instead, some Model-Finalizer must be called
      }
   }
}