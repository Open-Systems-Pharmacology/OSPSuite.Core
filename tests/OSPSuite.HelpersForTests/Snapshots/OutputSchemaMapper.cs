using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Helpers.Snapshots
{
   public class OutputSchemaMapper : Core.Snapshots.Mappers.OutputSchemaMapper
   {
      private readonly IOutputSchemaFactory _outputSchemaFactory;

      public OutputSchemaMapper(Core.Snapshots.Mappers.OutputIntervalMapper outputIntervalMapper, IContainerTask containerTask, IOutputSchemaFactory outputSchemaFactory) : base(outputIntervalMapper, containerTask)
      {
         _outputSchemaFactory = outputSchemaFactory;
      }

      protected override OutputSchema CreateEmpty()
      {
         return _outputSchemaFactory.CreateDefault();
      }
   }
}