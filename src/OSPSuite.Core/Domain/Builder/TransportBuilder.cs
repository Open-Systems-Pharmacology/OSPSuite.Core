using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ITransportBuilder : IProcessBuilder, IMoleculeDependentBuilder
   {
      /// <summary>
      ///    Gets or sets the source criteria to match for the source container of this transport.
      /// </summary>
      /// <value>The source criteria.</value>
      DescriptorCriteria SourceCriteria { get; set; }

      /// <summary>
      ///    Gets or sets the target criteria to match for the target container of this transport.
      /// </summary>
      /// <value>The target criteria.</value>
      DescriptorCriteria TargetCriteria { get; set; }

      /// <summary>
      ///    Gets or sets the type of the transport.
      /// </summary>
      /// <remarks>
      ///    Just informational use
      /// </remarks>
      /// <value>The type of the transport.</value>
      TransportType TransportType { get; set; }

      /// <summary>
      ///    Checks if the molecule should be transported - depending on the value of ForAll,
      /// </summary>
      bool TransportsMolecule(string moleculeName);
   }

   public class TransportBuilder : ProcessBuilder, ITransportBuilder
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

      private readonly MoleculeList _moleculeList;

      public TransportBuilder()
      {
         SourceCriteria = new DescriptorCriteria();
         TargetCriteria = new DescriptorCriteria();
         _moleculeList = new MoleculeList {ForAll = true};
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

      public bool TransportsMolecule(string moleculeName)
      {
         return _moleculeList.Uses(moleculeName);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var srcTransportBuilder = source as ITransportBuilder;

         if (srcTransportBuilder == null) return;
         SourceCriteria = srcTransportBuilder.SourceCriteria.Clone();
         TargetCriteria = srcTransportBuilder.TargetCriteria.Clone();
         TransportType = srcTransportBuilder.TransportType;
         _moleculeList.UpdatePropertiesFrom(srcTransportBuilder.MoleculeList, cloneManager);
      }
   }
}