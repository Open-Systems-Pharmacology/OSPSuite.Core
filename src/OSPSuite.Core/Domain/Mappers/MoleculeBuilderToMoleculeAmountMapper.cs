using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps molecule builder instance on molecule amount
   ///    <para></para>
   ///    Only parameters with the build mode "Local" will be added;
   ///    <para></para>
   ///    parameters with other build modes will be added elsewhere
   ///    <para></para>
   ///    (by the global molecule properties mapper)
   /// </summary>
   public interface IMoleculeBuilderToMoleculeAmountMapper : IBuilderMapper<IMoleculeBuilder, IMoleculeAmount>
   {
   }

   public class MoleculeBuilderToMoleculeAmountMapper : IMoleculeBuilderToMoleculeAmountMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly IDimension _amountDimension;

      public MoleculeBuilderToMoleculeAmountMapper(IObjectBaseFactory objectBaseFactory,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderCollectionToParameterCollectionMapper parameterMapper,
         IDimensionFactory dimensionFactory, IKeywordReplacerTask keywordReplacerTask, IFormulaFactory formulaFactory,
         IParameterFactory parameterFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _formulaFactory = formulaFactory;
         _parameterFactory = parameterFactory;
         _amountDimension = dimensionFactory.Dimension(Constants.Dimension.AMOUNT);
      }

      public IMoleculeAmount MapFrom(IMoleculeBuilder moleculeBuilder, IBuildConfiguration buildConfiguration)
      {
         //molecule amount always in amount
         var moleculeAmount = _objectBaseFactory.Create<IMoleculeAmount>()
            .WithName(moleculeBuilder.Name)
            .WithDescription(moleculeBuilder.Description)
            .WithContainerType(ContainerType.Molecule)
            .WithIcon(moleculeBuilder.Icon)
            .WithQuantityType(moleculeBuilder.QuantityType)
            .WithDimension(_amountDimension)
            .WithDisplayUnit(_amountDimension.UnitOrDefault(_amountDimension.DefaultUnit.Name));

         buildConfiguration.AddBuilderReference(moleculeAmount, moleculeBuilder);

         createMoleculeAmountDefaultFormula(moleculeBuilder, buildConfiguration, moleculeAmount);

         //map parameters. Only parameters having BuildMode="Local" will
         //be added to the molecule amount. Global/Property-Parameters
         //will be filled in elsewhere (by the GlobalProperties-Mapper)
         moleculeAmount.AddChildren(_parameterMapper.MapLocalFrom(moleculeBuilder, buildConfiguration));

         _keywordReplacerTask.ReplaceIn(moleculeAmount);
         return moleculeAmount;
      }

      private void createMoleculeAmountDefaultFormula(IMoleculeBuilder moleculeBuilder, IBuildConfiguration buildConfiguration, IMoleculeAmount moleculeAmount)
      {
         //set start value formula to the default. If user has specified
         //a new start value in MoleculesStartValueCollection-BB, default formula
         //will be overwritten during setting of molecule start values

         var modelFormula = _formulaMapper.MapFrom(moleculeBuilder.DefaultStartFormula, buildConfiguration);

         //amount based, we can just the formula as is
         if (moleculeBuilder.IsAmountBased())
         {
            moleculeAmount.Formula = modelFormula;
            return;
         }

         //create a start value parameter that will be referenced in the molecule formula 
         var startValueParameter = _parameterFactory.CreateStartValueParameter(moleculeAmount, modelFormula, moleculeBuilder.DisplayUnit);
         buildConfiguration.AddBuilderReference(startValueParameter,moleculeBuilder);
         moleculeAmount.Add(startValueParameter);
         moleculeAmount.Formula = _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValueParameter);
      }
   }
}