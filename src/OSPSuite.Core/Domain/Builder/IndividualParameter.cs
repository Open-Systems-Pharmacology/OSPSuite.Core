using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualParameter : PathAndValueEntity, IWithDefaultState
   {
      public ParameterOrigin Origin { get; set; }

      public ParameterInfo Info { get; set; }

      public bool IsDefault { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIndividualParameter = source as IndividualParameter;
         if (sourceIndividualParameter == null) return;

         DistributionType = sourceIndividualParameter.DistributionType;
         Info = sourceIndividualParameter.Info?.Clone();
         Origin = sourceIndividualParameter.Origin?.Clone();
         IsDefault = sourceIndividualParameter.IsDefault;
      }
   }
}