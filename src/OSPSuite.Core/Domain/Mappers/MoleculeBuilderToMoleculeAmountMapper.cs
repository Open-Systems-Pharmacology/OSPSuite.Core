using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

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
   public interface IMoleculeBuilderToMoleculeAmountMapper
   {
      /// <summary>
      ///    Maps the <paramref name="moleculeBuilder" /> to a MoleculeAmount. <paramref name="targetContainer" /> is where the
      ///    molecule amount will be added to.
      /// </summary>
      /// <param name="moleculeBuilder">Molecule builder to map to a MoleculeAmount</param>
      /// <param name="targetContainer">
      ///    Container where the molecule amount will be added. This is required in order to evaluate
      ///    local parameters container criteria
      /// </param>
      /// <param name="simulationConfiguration">Build configuration</param>
      /// <returns></returns>
      IMoleculeAmount MapFrom(IMoleculeBuilder moleculeBuilder, IContainer targetContainer, SimulationConfiguration simulationConfiguration);
   }

   public class MoleculeBuilderToMoleculeAmountMapper : IMoleculeBuilderToMoleculeAmountMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly IDimension _amountDimension;

      public MoleculeBuilderToMoleculeAmountMapper(IObjectBaseFactory objectBaseFactory,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderToParameterMapper parameterMapper,
         IDimensionFactory dimensionFactory, IKeywordReplacerTask keywordReplacerTask, IFormulaFactory formulaFactory,
         IParameterFactory parameterFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _formulaFactory = formulaFactory;
         _parameterFactory = parameterFactory;
         _amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
      }

      public IMoleculeAmount MapFrom(IMoleculeBuilder moleculeBuilder, IContainer targetContainer, SimulationConfiguration simulationConfiguration)
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

         simulationConfiguration.AddBuilderReference(moleculeAmount, moleculeBuilder);

         createMoleculeAmountDefaultFormula(moleculeBuilder, simulationConfiguration, moleculeAmount);

         //map parameters. Only parameters having BuildMode="Local" will
         //be added to the molecule amount. Global/Property-Parameters
         //will be filled in elsewhere (by the GlobalProperties-Mapper)
         var allLocalParameters = moleculeBuilder.Parameters
            .Where(x => x.BuildMode == ParameterBuildMode.Local)
            .Where(x => x.ContainerCriteria?.IsSatisfiedBy(targetContainer) ?? true);

         allLocalParameters.Each(x => moleculeAmount.Add(_parameterMapper.MapFrom(x, simulationConfiguration)));

         _keywordReplacerTask.ReplaceIn(moleculeAmount);
         return moleculeAmount;
      }

      private void createMoleculeAmountDefaultFormula(IMoleculeBuilder moleculeBuilder, SimulationConfiguration simulationConfiguration, IMoleculeAmount moleculeAmount)
      {
         //set start value formula to the default. If user has specified
         //a new start value in MoleculesStartValueCollection-BB, default formula
         //will be overwritten during setting of molecule start values

         var modelFormula = _formulaMapper.MapFrom(moleculeBuilder.DefaultStartFormula, simulationConfiguration);

         //amount based, we can just the formula as is
         if (moleculeBuilder.IsAmountBased())
         {
            moleculeAmount.Formula = modelFormula;
            return;
         }

         //create a start value parameter that will be referenced in the molecule formula 
         var startValueParameter = _parameterFactory.CreateStartValueParameter(moleculeAmount, modelFormula, moleculeBuilder.DisplayUnit);
         simulationConfiguration.AddBuilderReference(startValueParameter, moleculeBuilder);
         moleculeAmount.Add(startValueParameter);
         moleculeAmount.Formula = _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValueParameter);
      }
   }
}