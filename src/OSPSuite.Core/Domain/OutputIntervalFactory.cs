using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IOutputIntervalFactory
   {
      OutputInterval CreateDefault();
      OutputInterval Create(double startTimeInMinute, double endTimeInMinute, double resolutionInPtsPerMin);
      OutputInterval CreateFor(OutputSchema outputSchema, double startTimeInMinute, double endTimeInMinute, double resolutionInPtsPerMin);
   }

   public class OutputIntervalFactory : IOutputIntervalFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IContainerTask _containerTask;

      public OutputIntervalFactory(
         IObjectBaseFactory objectBaseFactory,
         IDimensionFactory dimensionFactory,
         IContainerTask containerTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _dimensionFactory = dimensionFactory;
         _containerTask = containerTask;
      }

      public OutputInterval CreateDefault()
      {
         return Create(0, 1440, 1.0 / 15);
      }

      public OutputInterval Create(double startTimeInMinute, double endTimeInMinute, double resolutionInPtsPerMin)
      {
         var interval = _objectBaseFactory.Create<OutputInterval>();
         interval.Add(createParameter(Constants.Parameters.START_TIME, Constants.Dimension.TIME, startTimeInMinute));
         interval.Add(createParameter(Constants.Parameters.END_TIME, Constants.Dimension.TIME, endTimeInMinute));
         interval.Add(createParameter(Constants.Parameters.RESOLUTION, Constants.Dimension.RESOLUTION, resolutionInPtsPerMin));
         interval.Resolution.MinIsAllowed = false;

         return interval;
      }

      public OutputInterval CreateFor(OutputSchema outputSchema, double startTimeInMinute, double endTimeInMinute, double resolutionInPtsPerMin)
      {
         return Create(startTimeInMinute, endTimeInMinute, resolutionInPtsPerMin)
            .WithName(_containerTask.CreateUniqueName(outputSchema, Constants.OUTPUT_INTERVAL, canUseBaseName: true));
      }

      private IParameter createParameter(string name, string dimension, double value)
      {
         var parameter = _objectBaseFactory.Create<IParameter>()
            .WithName(name)
            .WithDimension(_dimensionFactory.Dimension(dimension))
            .WithFormula(_objectBaseFactory.Create<ConstantFormula>().WithValue(value))
            .WithGroup(Constants.Groups.SIMULATION_SETTINGS);

         parameter.Editable = true;
         parameter.CanBeVaried = false;
         parameter.Visible = true;
         parameter.MinValue = 0;
         parameter.MinIsAllowed = true;
         parameter.IsDefault = true;

         return parameter;
      }
   }
}