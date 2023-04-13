using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Contains information necessary for the <see cref="IModelConstructor" />  to create a <see cref="ReactionPartner" />
   /// </summary>
   public class ReactionPartnerBuilder : IWithDimension
   {
      public IDimension Dimension { get; set; }
      public string MoleculeName { get; set; }
      public double StoichiometricCoefficient { get; set; }

      public ReactionPartnerBuilder() : this(string.Empty, 0.0)
      {
         Dimension = Constants.Dimension.NO_DIMENSION;
      }

      public ReactionPartnerBuilder(string moleculeName, double stoichiometricCoefficient)
      {
         MoleculeName = moleculeName;
         StoichiometricCoefficient = stoichiometricCoefficient;
      }

      public ReactionPartnerBuilder Clone()
      {
         return new ReactionPartnerBuilder(MoleculeName, StoichiometricCoefficient);
      }
   }
}