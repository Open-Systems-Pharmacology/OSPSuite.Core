using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class OutputIntervalMapper : Core.Snapshots.Mappers.OutputIntervalMapper
   {
      public OutputIntervalMapper(ParameterMapper parameterMapper) : base(parameterMapper)
      {
      }

      protected override OutputInterval CreateDefault()
      {
         return Create(0, 1440, 1.0 / 15);
      }

      public OutputInterval Create(double startTimeInMinute, double endTimeInMinute, double resolutionInPtsPerMin)
      {
         var interval = new OutputInterval
         {
            createParameter(Constants.Parameters.START_TIME, Constants.Dimension.TIME, startTimeInMinute),
            createParameter(Constants.Parameters.END_TIME, Constants.Dimension.TIME, endTimeInMinute),
            createParameter(Constants.Parameters.RESOLUTION, Constants.Dimension.RESOLUTION, resolutionInPtsPerMin)
         };
         interval.Resolution.MinIsAllowed = false;

         return interval;
      }

      protected override bool ShouldExportToSnapshot(IParameter parameter)
      {
         return base.ShouldExportToSnapshot(parameter) || !parameter.IsDefault;
      }

      private IParameter createParameter(string name, string dimension, double value)
      {
         var parameter = new Parameter()
            .WithName(name)
            .WithDimension(DimensionFactoryForSpecs.Factory.Dimension(dimension))
            .WithFormula(new ConstantFormula().WithValue(value))
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