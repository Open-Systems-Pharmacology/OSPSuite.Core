using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IParameterConverter
   {
      void Convert(IParameter parameter, bool convertFormulasAtUsingFormula);
   }

   internal class ParameterConverter : IParameterConverter
   {
      private readonly IDimensionMapper _dimensionMapper;
      private readonly IUsingFormulaConverter _usingFormulaConverter;

      public ParameterConverter(IDimensionMapper dimensionMapper, IUsingFormulaConverter usingFormulaConverter)
      {
         _dimensionMapper = dimensionMapper;
         _usingFormulaConverter = usingFormulaConverter;
      }

      /// <summary>
      /// Converts the specified parameter.
      /// </summary>
      /// <param name="parameter">The parameter.</param>
      /// <param name="convertFormulasAtUsingFormula">
      /// if set to <c>true</c> [convert formulas at using formula]./// if set
      /// to <c>true</c> [convert formulas at using formula]. Use
      /// <see langword="true" /> for UsingFormulas in Simulation. Use
      /// <see langword="false" /> in BuildingBlocks to prevent multiple
      /// Conversion of identical formula. Formulas should be only converted in
      /// <see cref="FormulaCache" />
      /// </param>
      public void Convert(IParameter parameter, bool convertFormulasAtUsingFormula)
      {
         //some dimensions were not set properly in pksim in older version
         if (parameter.NameIsOneOf(Constants.Distribution.PERCENTILE, Constants.Distribution.GEOMETRIC_DEVIATION))
         {
            parameter.Dimension = Constants.Dimension.NO_DIMENSION;
            parameter.Formula.Dimension = parameter.Dimension;
            return;
         }

         //This has to be retrieved before updating the formula
         var conversionFactor = _dimensionMapper.ConversionFactor(parameter);

         _usingFormulaConverter.Convert(parameter, convertFormulasAtUsingFormula);
         if(parameter.RHSFormula.IsConstant()||convertFormulasAtUsingFormula)
         {
            _usingFormulaConverter.Convert(parameter.RHSFormula);
         }

         if (parameter.IsDistributed())
            return;

         //no conversion, exit
         if (conversionFactor == 1)
            return;

         if (!parameter.IsFixedValue)
            return;

         //parameter is fixed. We need to scale the value as well
         parameter.Value *= conversionFactor;
      }
   }
}