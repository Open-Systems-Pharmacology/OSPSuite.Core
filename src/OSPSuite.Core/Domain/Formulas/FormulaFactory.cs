using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Formulas
{
   public interface IFormulaFactory
   {
      /// <summary>
      ///    Creates a concentration formula. This formula should be used in a <see cref="IParameter" /> that is located in a
      ///    <see cref="MoleculeAmount" />.
      ///    The <see cref="MoleculeAmount" /> should be defined in a <see cref="IContainer" /> containing a
      ///    <see cref="IParameter" /> named Volume.
      ///    Only then can the created formula be succesfully computed.
      /// </summary>
      /// <remarks>
      ///    If a formula named <see cref="Constants.CONCENTRATION_FORMULA" /> already exists in the
      ///    <paramref name="formulaCache" />, the
      ///    existing formula will be returned instead
      /// </remarks>
      IFormula ConcentrationFormulaFor(IFormulaCache formulaCache);

      /// <summary>
      ///    Creates a <see cref="ConstantFormula" /> whose value and dimension will be set to <paramref name="value" /> and
      ///    <paramref name="dimension" /> respectively
      /// </summary>
      /// <param name="value">
      ///    Value of the constant formula. The value should be entered in the base unit of the
      ///    <paramref name="dimension" />
      /// </param>
      /// <param name="dimension">Dimension of the created formula</param>
      IFormula ConstantFormula(double value, IDimension dimension);

      /// <summary>
      ///    Creates a formula that should be used in a Molecule amount referencing the Start value parameter
      /// </summary>
      /// <returns></returns>
      IFormula CreateMoleculeAmountReferenceToStartValue(IParameter startValueParameter);
   }

   public class FormulaFactory : IFormulaFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectPathFactory _objectPathFactory;

      public FormulaFactory(IObjectBaseFactory objectBaseFactory, IDimensionFactory dimensionFactory, IObjectPathFactory objectPathFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _dimensionFactory = dimensionFactory;
         _objectPathFactory = objectPathFactory;
      }

      public IFormula ConcentrationFormulaFor(IFormulaCache formulaCache)
      {
         if (formulaCache.ExistsByName(Constants.CONCENTRATION_FORMULA))
            return formulaCache.FindByName(Constants.CONCENTRATION_FORMULA);

         var formula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithName(Constants.CONCENTRATION_FORMULA)
            .WithFormulaString("V>0 ? M/V : 0");

         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER)
            .WithAlias("M")
            .WithDimension(_dimensionFactory.Dimension(Constants.Dimension.AMOUNT)));

         formula.AddObjectPath(createVolumeReferencePath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME));

         formulaCache.Add(formula);
         formula.Dimension = _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         return formula;
      }

      public IFormula ConstantFormula(double value, IDimension dimension)
      {
         return _objectBaseFactory.Create<ConstantFormula>()
            .WithValue(value)
            .WithDimension(dimension);
      }

      public IFormula CreateMoleculeAmountReferenceToStartValue(IParameter startValueParameter)
      {
         var formula = startValueParameter.IsAmountBased()
            ? createReferenceStartValueFormula(Constants.START_VALUE_ALIAS, Constants.MOLECULE_CONCENTRATION_IN_AMOUNT_FORMULA)
            : createReferenceStartValueFormula($"{Constants.START_VALUE_ALIAS} * {Constants.VOLUME_ALIAS}", Constants.MOLECULE_AMOUNT_FORMULA);

         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(Constants.Parameters.START_VALUE)
            .WithAlias(Constants.START_VALUE_ALIAS)
            .WithDimension(startValueParameter.Dimension));

         if (startValueParameter.IsConcentrationBased())
            formula.AddObjectPath(createVolumeReferencePath(ObjectPath.PARENT_CONTAINER, Constants.Parameters.VOLUME));

         return formula;
      }

      private ExplicitFormula createReferenceStartValueFormula(string formulaString, string formulaName)
      {
         return _objectBaseFactory.Create<ExplicitFormula>()
            .WithName(formulaName)
            .WithOriginId(formulaName)
            .WithDimension(_dimensionFactory.Dimension(Constants.Dimension.AMOUNT))
            .WithFormulaString(formulaString);
      }

      private IFormulaUsablePath createVolumeReferencePath(params string[] pathToVolumeParameter)
      {
         return _objectPathFactory.CreateFormulaUsablePathFrom(pathToVolumeParameter)
            .WithAlias(Constants.VOLUME_ALIAS)
            .WithDimension(_dimensionFactory.Dimension(Constants.Dimension.VOLUME));
      }
   }
}