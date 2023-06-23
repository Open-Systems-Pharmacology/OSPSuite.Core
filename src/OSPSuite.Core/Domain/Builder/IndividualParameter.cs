using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualParameter : PathAndValueEntity
   {
      public ParameterOrigin Origin { get; set; }

      public ParameterInfo Info { get; set; }

      /// <summary>
      ///    Only set for a parameter that is a distributed parameter.
      ///    This is required in order to be able to create distributed parameter dynamically in the simulation
      /// </summary>
      public DistributionType? DistributionType { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIndividualParameter = source as IndividualParameter;
         if (sourceIndividualParameter == null) return;

         DistributionType = sourceIndividualParameter.DistributionType;
         Info = sourceIndividualParameter.Info?.Clone();
         Origin = sourceIndividualParameter.Origin?.Clone();
      }
   }
}