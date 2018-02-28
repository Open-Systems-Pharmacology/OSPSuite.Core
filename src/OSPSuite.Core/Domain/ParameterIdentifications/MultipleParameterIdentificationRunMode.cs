using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class MultipleParameterIdentificationRunMode : ParameterIdentificationRunMode
   {
      public int NumberOfRuns { get; set; } = Constants.DEFAULT_NUMBER_OF_RUNS_FOR_MULTIPLE_MODE;

      public MultipleParameterIdentificationRunMode() : base(Captions.ParameterIdentification.RunModes.MultipleRuns, isSingleRun:false)
      {
      }

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new MultipleParameterIdentificationRunMode();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceMultipleOptimizationOption = source as MultipleParameterIdentificationRunMode;
         if (sourceMultipleOptimizationOption == null) return;
         NumberOfRuns = sourceMultipleOptimizationOption.NumberOfRuns;
      }
   }
}