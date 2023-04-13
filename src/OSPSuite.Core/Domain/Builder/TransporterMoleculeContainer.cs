using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class TransporterMoleculeContainer : Container, IContainsParameters
   {
      /// <summary>
      ///    Name of transport process triggering the transport
      /// </summary>
      public virtual string TransportName { get; set; }

      /// <summary>
      ///    Specific localized realizations of the transport process
      /// </summary>
      public virtual IEnumerable<TransportBuilder> ActiveTransportRealizations => GetChildren<TransportBuilder>();

      public virtual IEnumerable<IParameter> Parameters => GetChildren<IParameter>();

      public virtual void AddParameter(IParameter newParameter) => Add(newParameter);

      public virtual void RemoveParameter(IParameter toRemove) => RemoveChild(toRemove);

      /// <summary>
      ///    Add a new localized realization of the transport process
      /// </summary>
      public virtual void AddActiveTransportRealization(TransportBuilder activeTransportBuilder) => Add(activeTransportBuilder);

      /// <summary>
      ///    Remove a localized realization of the transport process
      /// </summary>
      public virtual void RemoveActiveTransportRealization(TransportBuilder activeTransportBuilderToRemove)
      {
         RemoveChild(activeTransportBuilderToRemove);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcTransporterMoleculeContainer = source as TransporterMoleculeContainer;
         if (srcTransporterMoleculeContainer == null) return;
         TransportName = srcTransporterMoleculeContainer.TransportName;
      }
   }
}