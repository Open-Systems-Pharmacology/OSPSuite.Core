using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface ICalculationMethodTask
   {
      void MergeCalculationMethodInModel(ModelConfiguration modelConfiguration);
   }

   internal class CalculationMethodTask : ICalculationMethodTask
   {
      private readonly IFormulaTask _formulaTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private EntityDescriptorMapList<IContainer> _allContainers;
      private IList<IParameter> _allBlackBoxParameters;
      private IModel _model;
      private SimulationConfiguration _simulationConfiguration;
      private SimulationBuilder _simulationBuilder;
      private ReplacementContext _replacementContext;

      public CalculationMethodTask(
         IFormulaTask formulaTask,
         IKeywordReplacerTask keywordReplacerTask,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderToParameterMapper parameterMapper,
         IObjectPathFactory objectPathFactory)
      {
         _formulaTask = formulaTask;
         _keywordReplacerTask = keywordReplacerTask;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _objectPathFactory = objectPathFactory;
      }

      public void MergeCalculationMethodInModel(ModelConfiguration modelConfiguration)
      {
         try
         {
            (_model, _simulationBuilder, _replacementContext) = modelConfiguration;
            _simulationConfiguration = modelConfiguration.SimulationConfiguration;
            _allContainers = _model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
            _allBlackBoxParameters = _model.Root.GetAllChildren<IParameter>().Where(p => p.Formula.IsBlackBox()).ToList();
            foreach (var calculationMethod in _simulationConfiguration.AllCalculationMethods)
            {
               var allMoleculesUsingMethod = allMoleculesUsing(calculationMethod, _simulationBuilder.Molecules).ToList();

               createFormulaForBlackBoxParameters(calculationMethod, allMoleculesUsingMethod);

               addHelpParametersFor(calculationMethod, allMoleculesUsingMethod);
            }
         }
         finally
         {
            _simulationConfiguration = null;
            _simulationBuilder = null;
            _allContainers.Clear();
            _allBlackBoxParameters.Clear();
            _model = null;
         }
      }

      private void addHelpParametersFor(CoreCalculationMethod calculationMethod, IList<MoleculeBuilder> allMoleculesUsingMethod)
      {
         foreach (var helpParameter in calculationMethod.AllHelpParameters())
         {
            var containerDescriptor = calculationMethod.DescriptorFor(helpParameter);
            _simulationBuilder.AddToBuilderSource(helpParameter, calculationMethod);
            foreach (var molecule in allMoleculesUsingMethod)
            {
               foreach (var container in allMoleculeContainersFor(containerDescriptor, molecule))
               {
                  //make sure we remove the parameter if it exists already
                  var existingParameter = container.Parameter(helpParameter.Name);
                  if (existingParameter != null)
                     container.RemoveChild(existingParameter);

                  var parameter = _parameterMapper.MapFrom(helpParameter, _simulationBuilder);
                  container.Add(parameter);
                  replaceKeyWordsIn(parameter, molecule.Name);
               }
            }
         }
      }

      private void createFormulaForBlackBoxParameters(CoreCalculationMethod calculationMethod, IList<MoleculeBuilder> allMoleculesUsingMethod)
      {
         foreach (var formula in calculationMethod.AllOutputFormulas())
         {
            var parameterDescriptor = calculationMethod.DescriptorFor(formula);
            foreach (var molecule in allMoleculesUsingMethod)
            {
               foreach (var parameter in allMoleculeParameterForFormula(parameterDescriptor, molecule))
               {
                  //not a black box parameter. Should not be overridden by cm
                  if (parameterIsNotBlackBoxParameter(parameter))
                     continue;

                  parameter.Formula = _formulaMapper.MapFrom(formula, _simulationBuilder);
                  replaceKeyWordsIn(parameter, molecule.Name);
               }
            }
         }
      }

      private void replaceKeyWordsIn(IParameter parameter, string moleculeName)
      {
         _keywordReplacerTask.ReplaceIn(parameter, moleculeName, _replacementContext);
         //check if parameter is in neighborhood. In that case, retrieve the neighborhood and replace the keywords as well
         var neighborhood = neighborhoodAncestorFor(parameter);
         if (neighborhood == null) return;
         _keywordReplacerTask.ReplaceIn(neighborhood, _replacementContext);
      }

      private static Neighborhood neighborhoodAncestorFor(IEntity entity)
      {
         if (entity == null)
            return null;

         if (entity.IsAnImplementationOf<Neighborhood>())
            return entity.DowncastTo<Neighborhood>();

         return neighborhoodAncestorFor(entity.ParentContainer);
      }

      private bool parameterIsNotBlackBoxParameter(IParameter parameter) => !_allBlackBoxParameters.Contains(parameter);

      private IEnumerable<MoleculeBuilder> allMoleculesUsing(CoreCalculationMethod calculationMethod, IReadOnlyCollection<MoleculeBuilder> molecules)
      {
         return molecules
            .Where(molecule => molecule.IsFloatingXenobiotic)
            .SelectMany(molecule => molecule.UsedCalculationMethods, (molecule, usedCalculationMethod) => new {molecule, usedCalculationMethod})
            .Where(x => x.usedCalculationMethod.CalculationMethod == calculationMethod.Name)
            .Select(x => x.molecule);
      }

      private IEnumerable<IContainer> allMoleculeContainersFor(DescriptorCriteria containerDescriptor, MoleculeBuilder molecule)
      {
         return from container in _allContainers.AllSatisfiedBy(containerDescriptor)
            let moleculeContainer = container.GetSingleChildByName<IContainer>(molecule.Name)
            where moleculeContainer != null
            select moleculeContainer;
      }

      private IEnumerable<IParameter> allMoleculeParameterForFormula(ParameterDescriptor parameterDescriptor, MoleculeBuilder molecule)
      {
         return from container in allMoleculeContainersFor(parameterDescriptor.ContainerCriteria, molecule)
            let parameter = container.GetSingleChildByName<IParameter>(parameterDescriptor.ParameterName)
            where parameter != null
            select parameter;
      }
   }
}