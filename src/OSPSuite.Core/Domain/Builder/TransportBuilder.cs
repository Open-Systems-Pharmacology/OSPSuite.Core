using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class TransportBuilder : ProcessBuilder, IMoleculeDependentBuilder
   {
      /// <summary>
      ///    Gets or sets the source criteria to match for the source container of this transport.
      /// </summary>
      /// <value>The source criteria.</value>
      public DescriptorCriteria SourceCriteria { get; set; }

      /// <summary>
      ///    Gets or sets the target criteria to match for the target container of this transport.
      /// </summary>
      /// <value>The target criteria.</value>
      public DescriptorCriteria TargetCriteria { get; set; }

      /// <summary>
      ///    Gets or sets the type of the transport.
      /// </summary>
      /// <remarks>
      ///    Just informational use
      /// </remarks>
      /// <value>The type of the transport.</value>
      public TransportType TransportType { get; set; }

      public TransportBuilder()
      {
         SourceCriteria = new DescriptorCriteria();
         TargetCriteria = new DescriptorCriteria();
         MoleculeList = new MoleculeList {ForAll = true};
      }

      public MoleculeList MoleculeList { get; }

      public bool ForAll
      {
         get => MoleculeList.ForAll;
         set => MoleculeList.ForAll = value;
      }

      /// <summary>
      ///    Checks if the molecule should be transported - depending on the value of ForAll,
      /// </summary>
      public bool TransportsMolecule(string moleculeName) => MoleculeList.Uses(moleculeName);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var srcTransportBuilder = source as TransportBuilder;

         if (srcTransportBuilder == null) return;
         SourceCriteria = srcTransportBuilder.SourceCriteria.Clone();
         TargetCriteria = srcTransportBuilder.TargetCriteria.Clone();
         TransportType = srcTransportBuilder.TransportType;
         MoleculeList.UpdatePropertiesFrom(srcTransportBuilder.MoleculeList, cloneManager);
      }
   }
}