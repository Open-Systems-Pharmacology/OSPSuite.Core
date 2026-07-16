namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ParameterIdentificationContext : SnapshotContext
   {
      public Domain.ParameterIdentifications.ParameterIdentification ParameterIdentification { get; }

      public ParameterIdentificationContext(Domain.ParameterIdentifications.ParameterIdentification parameterIdentification, SnapshotContext snapshotContext) : base(snapshotContext)
      {
         ParameterIdentification = parameterIdentification;
      }
   }
}