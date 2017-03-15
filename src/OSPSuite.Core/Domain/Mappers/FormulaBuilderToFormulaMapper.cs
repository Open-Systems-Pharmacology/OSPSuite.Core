using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps formula used in a any Builder object to the formula used by
   ///    <para></para>
   ///    the corresponding object in the simulation.
   ///    <para></para>
   ///    At the moment, there is no special FormulaBuilder class, so the mapper
   ///    <para></para>
   ///    will just create the clone of the input formula
   /// </summary>
   public interface IFormulaBuilderToFormulaMapper : IBuilderMapper<IFormula, IFormula>
   {
   }

   public class FormulaBuilderToFormulaMapper : IFormulaBuilderToFormulaMapper
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public FormulaBuilderToFormulaMapper(ICloneManagerForModel cloneManagerForModel)
      {
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IFormula MapFrom(IFormula formulaBuilder, IBuildConfiguration buildConfiguration)
      {
         return _cloneManagerForModel.Clone(formulaBuilder);
      }
   }
}