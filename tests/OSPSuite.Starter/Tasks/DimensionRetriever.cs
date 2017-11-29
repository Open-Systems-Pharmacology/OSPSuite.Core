using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Starter.Tasks
{
   internal class DimensionRetriever : IReactionDimensionRetriever
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DimensionRetriever(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public IDimension ReactionDimension
      {
         get { return _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME); }
      }

      public IDimension MoleculeDimension
      {
         get { return _dimensionFactory.Dimension(Constants.Dimension.AMOUNT); }
      }

      public ReactionDimensionMode SelectedDimensionMode
      {
         get { return ReactionDimensionMode.AmountBased; }
      }
   }
}