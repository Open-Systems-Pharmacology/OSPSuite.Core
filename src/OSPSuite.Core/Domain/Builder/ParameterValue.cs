using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class ParameterValue : PathAndValueEntity, IWithDefaultState
   {
      public string ParameterName => Name;

      public ParameterOrigin Origin { get; set; }

      public ParameterInfo Info { get; set; }

      public bool IsDefault { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceParameterValue = source as ParameterValue;
         if (sourceParameterValue == null) return;

         IsDefault = sourceParameterValue.IsDefault;
         Info = sourceParameterValue.Info?.Clone();
         Origin = sourceParameterValue.Origin?.Clone();
      }
   }
}