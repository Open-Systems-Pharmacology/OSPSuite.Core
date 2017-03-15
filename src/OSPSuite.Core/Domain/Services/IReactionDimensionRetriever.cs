using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   public enum ReactionDimensionMode
   {
      /// <summary>
      /// Amount based reaction network (Molecule in amount and reaction in amount per time)
      /// </summary>
      AmountBased,

      /// <summary>
      /// Concentration based reaction network (Molecule in molar concentration and reaction in molar concentration per time)
      /// </summary>
      ConcentrationBased
   }

   /// <summary>
   /// Specifies in which dimension the rate and molecule amount should be created in the model.
   /// PKSim typically used concentration based model and MoBi can use Amount or Concentration
   /// </summary>
   public interface IReactionDimensionRetriever
   {
      /// <summary>
      /// Returns the dimension for the kinetic that should be used when creating a new reaction
      /// </summary>
      IDimension ReactionDimension { get; }

      /// <summary>
      /// Returns the dimension that should be used when creating a new molecule 
      /// </summary>
      IDimension MoleculeDimension { get; }

      /// <summary>
      /// Returns the <see cref="ReactionDimensionMode"/> currently active in the simulation environment
      /// </summary>
      ReactionDimensionMode SelectedDimensionMode { get; }
   }
}