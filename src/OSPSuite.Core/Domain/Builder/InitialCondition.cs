using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class InitialCondition : PathAndValueEntity, IWithScaleDivisor
   {
      private bool _isPresent;
      private double _scaleDivisor;
      public bool NegativeValuesAllowed { get; set; }

      public InitialCondition()
      {
         Rules.Add(ScaleDivisorRules.ScaleDivisorStrictlyPositive);
         ScaleDivisor = Constants.DEFAULT_SCALE_DIVISOR;
         NegativeValuesAllowed = false;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceInitialCondition = source as InitialCondition;
         if (sourceInitialCondition == null) return;
         IsPresent = sourceInitialCondition.IsPresent;
         ScaleDivisor = sourceInitialCondition.ScaleDivisor;
         NegativeValuesAllowed = sourceInitialCondition.NegativeValuesAllowed;
      }

      public bool IsPresent
      {
         get => _isPresent;
         set => SetProperty(ref _isPresent, value);
      }

      public string MoleculeName => Name;

      public double ScaleDivisor
      {
         get => _scaleDivisor;
         set => SetProperty(ref _scaleDivisor, value);
      }
   }
}