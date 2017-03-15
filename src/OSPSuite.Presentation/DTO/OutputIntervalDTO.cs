using OSPSuite.Assets;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class OutputIntervalDTO : ValidatableDTO
   {
      public OutputInterval OutputInterval { get; set; }
      public IParameterDTO StartTimeParameter { get; set; }
      public IParameterDTO EndTimeParameter { get; set; }
      public IParameterDTO ResolutionParameter { get; set; }

      public OutputIntervalDTO()
      {
         Rules.Add(startTimeLessThanEndTime);
         Rules.Add(endTimeGreaterThanStartTime);
      }

      /// <summary>
      ///    Start time value in display unit
      /// </summary>
      public double StartTime
      {
         get { return StartTimeParameter.Value; }
         set { StartTimeParameter.Value = value; }
      }

      /// <summary>
      ///    End time value in display unit
      /// </summary>
      public double EndTime
      {
         get { return EndTimeParameter.Value; }
         set { EndTimeParameter.Value = value; }
      }

      public double Resolution
      {
         get { return ResolutionParameter.Value; }
         set { ResolutionParameter.Value = value; }
      }

      private IBusinessRule startTimeLessThanEndTime
      {
         get
         {
            return CreateRule.For<OutputIntervalDTO>()
               .Property(x => x.StartTime)
               .WithRule((dto, value) => StartTimeParameter.Parameter.ConvertToBaseUnit(value) < EndTimeParameter.KernelValue)
               .WithError(Validation.StartTimeLessThanOrEqualToEndTime);
         }
      }

      private IBusinessRule endTimeGreaterThanStartTime
      {
         get
         {
            return CreateRule.For<OutputIntervalDTO>()
               .Property(x => x.EndTime)
               .WithRule((dto, value) => EndTimeParameter.Parameter.ConvertToBaseUnit(value) > StartTimeParameter.KernelValue)
               .WithError(Validation.EndTimeGreaterThanOrEqualToStartTime);
         }
      }
   }
}