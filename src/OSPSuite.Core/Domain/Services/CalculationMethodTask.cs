using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ICalculationMethodTask
   {
      void MergeCalculationMethodInModel(IModel model, IBuildConfiguration buildConfiguration);
   }

   public class CalculationMethodTask : ICalculationMethodTask
   {
      private readonly IFormulaTask _formulaTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private EntityDescriptorMapList<IContainer> _allContainers;
      private IList<IParameter> _allBlackBoxParameters;
      private IModel _model;
      private IBuildConfiguration _buildConfiguration;

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

      public void MergeCalculationMethodInModel(IModel model, IBuildConfiguration buildConfiguration)
      {
         try
         {
            _buildConfiguration = buildConfiguration;
            _allContainers = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
            _allBlackBoxParameters = model.Root.GetAllChildren<IParameter>().Where(p => p.Formula.IsBlackBox()).ToList();
            _model = model;
            foreach (var calculationMethod in buildConfiguration.AllCalculationMethods())
            {
               var allMoleculesUsingMethod = allMoleculesUsing(calculationMethod, buildConfiguration.Molecules).ToList();

               createFormulaForBlackBoxParameters(calculationMethod, allMoleculesUsingMethod);

               addHelpParametersFor(calculationMethod, allMoleculesUsingMethod);
            }
         }
         finally
         {
            _buildConfiguration = null;
            _allContainers.Clear();
            _allBlackBoxParameters.Clear();
            _model = null;
         }
      }

      private void addHelpParametersFor(ICoreCalculationMethod calculationMethod, IList<IMoleculeBuilder> allMoleculesUsingMethod)
      {
         foreach (var helpParameter in calculationMethod.AllHelpParameters())
         {
            var containerDescriptor = calculationMethod.DescriptorFor(helpParameter);
            foreach (var molecule in allMoleculesUsingMethod)
            {
               foreach (var container in allMoleculeContainersFor(containerDescriptor, molecule))
               {
                  var exisitingParameter = container.GetSingleChildByName<IParameter>(helpParameter.Name);
                  //does not exist yet
                  if (exisitingParameter == null)
                  {
                     var parameter = _parameterMapper.MapFrom(helpParameter,_buildConfiguration);
                     container.Add(parameter);
                     replaceKeyWordsIn(parameter, molecule.Name);
                  }
                  else if (!formulasAreTheSameForParameter(exisitingParameter, helpParameter.Formula,molecule.Name))
                     throw new OSPSuiteException(Error.HelpParameterAlreadyDefinedWithAnotherFormula(calculationMethod.Name, _objectPathFactory.CreateAbsoluteObjectPath(helpParameter).ToString()));
               }
            }
         }
      }

      private void createFormulaForBlackBoxParameters(ICoreCalculationMethod calculationMethod, IList<IMoleculeBuilder> allMoleculesUsingMethod)
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

                  //parmeter formula was not set yet
                  if (parameter.Formula.IsBlackBox())
                  {
                     parameter.Formula = _formulaMapper.MapFrom(formula,_buildConfiguration);
                     replaceKeyWordsIn(parameter, molecule.Name);
                  }
                  else
                  {
                     if (!formulasAreTheSameForParameter(parameter, formula, molecule.Name))
                        throw new OSPSuiteException(Error.TwoDifferentFormulaForSameParameter(parameter.Name, _objectPathFactory.CreateAbsoluteObjectPath(parameter).ToPathString()));
                  }
               }
            }
         }
      }

      private void replaceKeyWordsIn(IParameter parameter, string moleculeName)
      {
         _keywordReplacerTask.ReplaceIn(parameter, _model.Root, moleculeName);
         //check if parameter is in neighborhood. In that case, retrieve the neighborhood and repalce the keywords as well
         var neighborhood = neighborhoodAncestorFor(parameter);
         if (neighborhood == null) return;
         _keywordReplacerTask.ReplaceIn(neighborhood, _model.Root);
      }

      private static INeighborhood neighborhoodAncestorFor(IEntity entity)
      {
         if (entity == null) return null;
         if (entity.IsAnImplementationOf<INeighborhood>())
            return entity.DowncastTo<INeighborhood>();
         return neighborhoodAncestorFor(entity.ParentContainer);
      }

      private bool formulasAreTheSameForParameter(IParameter originalParameter, IFormula calculationMethodFormula,string moleculeName)
      {
         var previousFormula = originalParameter.Formula;
         //needs to use the parameter in order to keep the hierarchy. Hence we set the formula in the parameter
         originalParameter.Formula = _formulaMapper.MapFrom(calculationMethodFormula,_buildConfiguration);
         
         //check  if the formula set are the same. it is necessary to replace keywords before doing that
         replaceKeyWordsIn(originalParameter, moleculeName);

         try
         {
            return _formulaTask.FormulasAreTheSame(originalParameter.Formula, previousFormula);
         }
         finally
         {
            //reset the origianl formula in any case
            originalParameter.Formula = previousFormula;
         }
      }

      private bool parameterIsNotBlackBoxParameter(IParameter parameter)
      {
         return !_allBlackBoxParameters.Contains(parameter);
      }

      private IEnumerable<IMoleculeBuilder> allMoleculesUsing(ICoreCalculationMethod calculationMethod, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         return from molecule in moleculeBuildingBlock
                where molecule.IsFloatingXenobiotic
                from usedCalculationMethod in molecule.UsedCalculationMethods
                where usedCalculationMethod.CalculationMethod == calculationMethod.Name
                select molecule;
      }

      private IEnumerable<IContainer> allMoleculeContainersFor(DescriptorCriteria containerDescriptor, IMoleculeBuilder molecule)
      {
         return from container in _allContainers.AllSatisfiedBy(containerDescriptor)
                let moleculeContainer = container.GetSingleChildByName<IContainer>(molecule.Name)
                where moleculeContainer != null
                select moleculeContainer;
      }

      private IEnumerable<IParameter> allMoleculeParameterForFormula(ParameterDescriptor parameterDescriptor, IMoleculeBuilder molecule)
      {
         return from container in allMoleculeContainersFor(parameterDescriptor.ContainerCriteria, molecule)
                let parameter = container.GetSingleChildByName<IParameter>(parameterDescriptor.ParameterName)
                where parameter != null
                select parameter;
      }
   }
}