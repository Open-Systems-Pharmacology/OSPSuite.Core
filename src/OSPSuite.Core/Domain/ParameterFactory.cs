using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain
{
   public interface IParameterFactory
   {
      /// <summary>
      ///    Creates a concentration parameter whose formula will be added into the cache of the
      ///    <paramref name="formulaCache" /> if not already available
      /// </summary>
      IParameter CreateConcentrationParameter(IFormulaCache formulaCache);

      /// <summary>
      ///    Create a volume parameter with value 1L
      /// </summary>
      IParameter CreateVolumeParameter();

      /// <summary>
      ///    Creates a start value parameter for the given <paramref name="moleculeAmount" />. The parameter formula will be set
      ///    to <paramref name="modelFormulaToUse" />.
      /// </summary>
      /// <remarks>
      ///    The parameter won't be added to the <paramref name="moleculeAmount" />.
      ///    The object paths used in <paramref name="modelFormulaToUse" /> will be edited to ensure proper reference
      /// </remarks>
      IParameter CreateStartValueParameter(IMoleculeAmount moleculeAmount, IFormula modelFormulaToUse, Unit displayUnit = null);

      /// <summary>
      ///    Creates a parameter named <paramref name="name" /> with a constant formula of value <paramref name="value" />. It
      ///    the <paramref name="dimension" />
      ///    is defined, sets the given dimension in the parameter
      /// </summary>
      IParameter CreateParameter(string name, double? value = null, IDimension dimension = null, string groupName = null, IFormula formula = null, Unit displayUnit = null);
   }

   public class ParameterFactory : IParameterFactory
   {
      private readonly IFormulaFactory _formulaFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ParameterFactory(IFormulaFactory formulaFactory, IObjectBaseFactory objectBaseFactory, IDimensionFactory dimensionFactory,
         IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater, IDisplayUnitRetriever displayUnitRetriever)
      {
         _formulaFactory = formulaFactory;
         _objectBaseFactory = objectBaseFactory;
         _dimensionFactory = dimensionFactory;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public IParameter CreateConcentrationParameter(IFormulaCache formulaCache)
      {
         var concentrationFormula = _formulaFactory.ConcentrationFormulaFor(formulaCache);
         var molarConcentrationDimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         var concentrationParameter = CreateParameter(Constants.Parameters.CONCENTRATION, dimension: molarConcentrationDimension, formula: concentrationFormula)
            .WithMode(ParameterBuildMode.Local);

         concentrationParameter.Info.ReadOnly = true;

         return concentrationParameter;
      }

      public IParameter CreateVolumeParameter()
      {
         var volumeDimension = _dimensionFactory.Dimension(Constants.Dimension.VOLUME);
         return CreateParameter(Constants.Parameters.VOLUME, value: 1, dimension: volumeDimension, groupName: Constants.Groups.ORGAN_VOLUMES);
      }

      public IParameter CreateStartValueParameter(IMoleculeAmount moleculeAmount, IFormula modelFormulaToUse, Unit displayUnit = null)
      {
         _concentrationBasedFormulaUpdater.UpdateRelativePathForStartValueMolecule(moleculeAmount, modelFormulaToUse);
         return CreateParameter(Constants.Parameters.START_VALUE, dimension: modelFormulaToUse.Dimension, displayUnit: displayUnit, formula: modelFormulaToUse);
      }

      public IParameter CreateParameter(string name, double? value = null, IDimension dimension = null, string groupName = null, IFormula formula = null, Unit displayUnit = null)
      {
         var dimensionToUse = dimension ?? _dimensionFactory.NoDimension;
         var displayUnitToUse = displayUnit ?? _displayUnitRetriever.PreferredUnitFor(dimensionToUse);
         var formulaToUse = formula ?? _formulaFactory.ConstantFormula(value.GetValueOrDefault(double.NaN), dimensionToUse);

         return _objectBaseFactory.Create<IParameter>()
            .WithName(name)
            .WithDimension(dimensionToUse)
            .WithFormula(formulaToUse)
            .WithDisplayUnit(displayUnitToUse)
            .WithGroup(groupName ?? Constants.Groups.MOBI);
      }
   }
}