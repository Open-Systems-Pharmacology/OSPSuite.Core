using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeBuilderFactory
   {
      /// <summary>
      /// Create a molecule builder containing the expected default parameter and formula
      /// </summary>
      /// <returns></returns>
      MoleculeBuilder Create(IFormulaCache formulaCache);
   }

   public class MoleculeBuilderFactory : IMoleculeBuilderFactory
   {
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;
      private readonly IParameterFactory _parameterFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public MoleculeBuilderFactory(IReactionDimensionRetriever reactionDimensionRetriever, IParameterFactory parameterFactory, 
         IObjectBaseFactory objectBaseFactory)
      {
         _reactionDimensionRetriever = reactionDimensionRetriever;
         _parameterFactory = parameterFactory;
         _objectBaseFactory = objectBaseFactory;
      }

      public MoleculeBuilder Create(IFormulaCache formulaCache)
      {
         var moleculeBuilder = _objectBaseFactory.Create<MoleculeBuilder>()
            .WithDimension(_reactionDimensionRetriever.MoleculeDimension);
        
         moleculeBuilder.QuantityType = QuantityType.Drug;
         var concentrationParameter = _parameterFactory.CreateConcentrationParameter(formulaCache);
         moleculeBuilder.Add(concentrationParameter);

         if (_reactionDimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.ConcentrationBased)
            concentrationParameter.Persistable = true;

         return moleculeBuilder;
      }
   }
}