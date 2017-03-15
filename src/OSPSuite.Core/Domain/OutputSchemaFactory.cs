namespace OSPSuite.Core.Domain
{
   public interface IOutputSchemaFactory
   {
      OutputSchema CreateDefault();
      OutputSchema Create(double startTimeInMin, double endTimeInMin, double resolutionInPtsPerHour);
   }

   public class OutputSchemaFactory : IOutputSchemaFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IOutputIntervalFactory _outputIntervalFactory;

      public OutputSchemaFactory(IObjectBaseFactory objectBaseFactory, IOutputIntervalFactory outputIntervalFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _outputIntervalFactory = outputIntervalFactory;
      }

      public OutputSchema CreateDefault()
      {
         return createWith(_outputIntervalFactory.CreateDefault());
      }

      public OutputSchema Create(double startTimeInMin, double endTimeInMin, double resolutionInPtsPerHour)
      {
         return createWith(_outputIntervalFactory.Create(startTimeInMin, endTimeInMin, resolutionInPtsPerHour));
      }

      private OutputSchema createWith(OutputInterval interval)
      {
         var outputSchema = _objectBaseFactory.Create<OutputSchema>();
         outputSchema.AddInterval(interval);
         return outputSchema;
      }
   }
}