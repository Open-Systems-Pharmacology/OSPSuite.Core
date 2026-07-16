using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface ICalculationMethodTask
   {
      void MergeCalculationMethodInModel(ModelConfiguration modelConfiguration);
   }

   internal class CalculationMethodTask : ICalculationMethodTask
   {
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public CalculationMethodTask(
         IKeywordReplacerTask keywordReplacerTask,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderToParameterMapper parameterMapper
      )
      {
         _keywordReplacerTask = keywordReplacerTask;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
      }

      public void MergeCalculationMethodInModel(ModelConfiguration modelConfiguration)
      {
         var context = new MergeContext(modelConfiguration);
         var simulationConfiguration = modelConfiguration.SimulationConfiguration;
         foreach (var calculationMethod in simulationConfiguration.AllCalculationMethods)
         {
            var allMoleculesUsingMethod = allMoleculesUsing(calculationMethod, context.SimulationBuilder.Molecules, simulationConfiguration).ToList();

            createFormulaForBlackBoxParameters(calculationMethod, allMoleculesUsingMethod, context);

            addHelpParametersFor(calculationMethod, allMoleculesUsingMethod, context);
         }
      }

      private void addHelpParametersFor(CoreCalculationMethod calculationMethod, IList<MoleculeBuilder> allMoleculesUsingMethod, MergeContext context)
      {
         foreach (var helpParameter in calculationMethod.AllHelpParameters())
         {
            var containerDescriptor = calculationMethod.DescriptorFor(helpParameter);
            context.SimulationBuilder.AddToBuilderSource(helpParameter, calculationMethod);
            foreach (var molecule in allMoleculesUsingMethod)
            {
               foreach (var container in allMoleculeContainersFor(containerDescriptor, molecule, context))
               {
                  //make sure we remove the parameter if it exists already
                  var existingParameter = container.Parameter(helpParameter.Name);
                  if (existingParameter != null)
                     container.RemoveChild(existingParameter);

                  var parameter = _parameterMapper.MapFrom(helpParameter, context.SimulationBuilder);
                  container.Add(parameter);
                  replaceKeyWordsIn(parameter, molecule.Name, context.ReplacementContext);
               }
            }
         }
      }

      private void createFormulaForBlackBoxParameters(CoreCalculationMethod calculationMethod, IList<MoleculeBuilder> allMoleculesUsingMethod, MergeContext context)
      {
         foreach (var formula in calculationMethod.AllOutputFormulas())
         {
            var parameterDescriptor = calculationMethod.DescriptorFor(formula);
            foreach (var molecule in allMoleculesUsingMethod)
            {
               foreach (var parameter in allMoleculeParameterForFormula(parameterDescriptor, molecule, context))
               {
                  //not a black box parameter. Should not be overridden by cm
                  if (parameterIsNotBlackBoxParameter(parameter, context))
                     continue;

                  parameter.Formula = _formulaMapper.MapFrom(formula, context.SimulationBuilder);
                  replaceKeyWordsIn(parameter, molecule.Name, context.ReplacementContext);
               }
            }
         }
      }

      private void replaceKeyWordsIn(IParameter parameter, string moleculeName, ReplacementContext replacementContext)
      {
         _keywordReplacerTask.ReplaceIn(parameter, moleculeName, replacementContext);
         //check if parameter is in neighborhood. In that case, retrieve the neighborhood and replace the keywords as well
         var neighborhood = neighborhoodAncestorFor(parameter);
         if (neighborhood == null) return;
         _keywordReplacerTask.ReplaceIn(neighborhood, replacementContext);
      }

      private static Neighborhood neighborhoodAncestorFor(IEntity entity)
      {
         if (entity == null)
            return null;

         if (entity.IsAnImplementationOf<Neighborhood>())
            return entity.DowncastTo<Neighborhood>();

         return neighborhoodAncestorFor(entity.ParentContainer);
      }

      private bool parameterIsNotBlackBoxParameter(IParameter parameter, MergeContext context) => !context.AllBlackBoxParameters.Contains(parameter);

      private IEnumerable<MoleculeBuilder> allMoleculesUsing(CoreCalculationMethod calculationMethod, IReadOnlyCollection<MoleculeBuilder> molecules, SimulationConfiguration simulationConfiguration)
      {
         return molecules
            .Where(molecule => molecule.IsFloatingXenobiotic)
            .SelectMany(molecule => usedCalculationMethodsFor(molecule, simulationConfiguration), (molecule, usedCalculationMethod) => new {molecule, usedCalculationMethod})
            .Where(x => x.usedCalculationMethod.CalculationMethod == calculationMethod.Name)
            .Select(x => x.molecule);
      }

      private static IReadOnlyCollection<UsedCalculationMethod> usedCalculationMethodsFor(MoleculeBuilder molecule, SimulationConfiguration simulationConfiguration)
      {
         var cache = new Cache<string, UsedCalculationMethod> (getKey: x => x.Category);

         // use molecule defined calculation methods first
         molecule.UsedCalculationMethods.Each(x => cache[x.Category] = x);

         // override when a calculation method override is defined for the molecule in the simulation configuration
         simulationConfiguration.CalculationMethodOverridesFor(molecule.Name).UsedCalculationMethods.Each(x => cache[x.Category] = x);
         
         return cache;
      }

      private IEnumerable<IContainer> allMoleculeContainersFor(DescriptorCriteria containerDescriptor, MoleculeBuilder molecule, MergeContext context)
      {
         return from container in context.AllContainers.AllSatisfiedBy(containerDescriptor)
            let moleculeContainer = container.GetSingleChildByName<IContainer>(molecule.Name)
            where moleculeContainer != null
            select moleculeContainer;
      }

      private IEnumerable<IParameter> allMoleculeParameterForFormula(ParameterDescriptor parameterDescriptor, MoleculeBuilder molecule, MergeContext context)
      {
         return from container in allMoleculeContainersFor(parameterDescriptor.ContainerCriteria, molecule, context)
            let parameter = container.GetSingleChildByName<IParameter>(parameterDescriptor.ParameterName)
            where parameter != null
            select parameter;
      }

      /// <summary>
      ///    Per-call state required to merge the calculation methods in one model. Created per call so that the task remains
      ///    stateless
      /// </summary>
      private class MergeContext
      {
         public SimulationBuilder SimulationBuilder { get; }
         public ReplacementContext ReplacementContext { get; }

         //caches only used to speed up the merge
         public EntityDescriptorMapList<IContainer> AllContainers { get; }
         public IList<IParameter> AllBlackBoxParameters { get; }

         public MergeContext(ModelConfiguration modelConfiguration)
         {
            var (model, simulationBuilder, replacementContext) = modelConfiguration;
            SimulationBuilder = simulationBuilder;
            ReplacementContext = replacementContext;
            AllContainers = model.Root.GetAllContainersAndSelf<IContainer>().ToEntityDescriptorMapList();
            AllBlackBoxParameters = model.Root.GetAllChildren<IParameter>().Where(p => p.Formula.IsBlackBox()).ToList();
         }
      }
   }
}