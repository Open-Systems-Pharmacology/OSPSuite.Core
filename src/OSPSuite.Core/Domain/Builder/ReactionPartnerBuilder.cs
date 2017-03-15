using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Contains information necessary for the <see cref="IModelConstructor" />  to create a <see cref="IReactionPartner" />
   /// </summary>
   public interface IReactionPartnerBuilder : IWithDimension
   {
      string MoleculeName { get; set; }
      double StoichiometricCoefficient { get; set; }

      /// <summary>
      ///    Return clone of reaction partner
      /// </summary>
      /// <returns></returns>
      IReactionPartnerBuilder Clone();
   }

   /// <summary>
   ///    Contains information necessary for the <see cref="IModelConstructor" />  to create a <see cref="IReactionPartner" />
   /// </summary>
   public class ReactionPartnerBuilder : IReactionPartnerBuilder
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

      public IReactionPartnerBuilder Clone()
      {
         return new ReactionPartnerBuilder(MoleculeName, StoichiometricCoefficient);
      }
   }
}