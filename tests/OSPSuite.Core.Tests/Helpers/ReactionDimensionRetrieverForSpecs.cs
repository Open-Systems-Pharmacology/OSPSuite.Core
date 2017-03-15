using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers
{
   public class ReactionDimensionRetrieverForSpecs : IReactionDimensionRetriever
   {
      private readonly IDimensionFactory _dimensionFactory;

      public ReactionDimensionRetrieverForSpecs(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
         SelectedDimensionMode = ReactionDimensionMode.AmountBased;
      }

      public IDimension ReactionDimension
      {
         get
         {
            if (SelectedDimensionMode==ReactionDimensionMode.AmountBased)
               return _dimensionFactory.GetDimension(Constants.Dimension.AMOUNT_PER_TIME);

            return _dimensionFactory.GetDimension(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME);
         }
      }

      public IDimension MoleculeDimension
      {
         get
         {
            if (SelectedDimensionMode == ReactionDimensionMode.AmountBased)
               return _dimensionFactory.GetDimension(Constants.Dimension.AMOUNT);

            return _dimensionFactory.GetDimension(Constants.Dimension.MOLAR_CONCENTRATION);
         }
      }

      public ReactionDimensionMode SelectedDimensionMode { get; set; }
   }
}