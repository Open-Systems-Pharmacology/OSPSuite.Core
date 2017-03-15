using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Maths.Optimization.NelderMead;
using PropertyNames = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Names;
using PropertyDescriptions = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Descriptions;

namespace OSPSuite.Core.Domain.ParameterIdentifications.Algorithms
{
   public class NelderMeadOptimizer : OptimizationAlgorithm<NelderMeadOptimizer>
   {
      public NelderMeadOptimizer() : base(Constants.OptimizationAlgorithm.NELDER_MEAD_PKSIM, Captions.ParameterIdentification.Algorithms.NelderMeadPKSim)
      {
         ConvergenceTolerance = 1e-3;
         MaxEvaluation = 10000;

         _extendedPropertyStore.ConfigureProperty(x => x.ConvergenceTolerance, PropertyNames.ConvergenceTolerance, PropertyDescriptions.ConvergenceTolerance);
         _extendedPropertyStore.ConfigureProperty(x => x.MaxEvaluation, PropertyNames.MaxNumberOfEvaluations, PropertyDescriptions.MaxEvaluations);
      }

      protected override OptimizationRunProperties RunOptimization()
      {
         var result = NelderMeadSimplex.Regress(createSimplexConstants(), ConvergenceTolerance, MaxEvaluation, values => _objectiveFunc(ParameterValuesFrom(values)).TotalError);
         return new OptimizationRunProperties(result.EvaluationCount);
      }

      private SimplexConstant[] createSimplexConstants()
      {
         var startValues = CreateVectorFor(x => x.StartValue);
         return startValues.Select(value => new SimplexConstant(value, 1)).ToArray();
      }

      public double ConvergenceTolerance
      {
         get { return _extendedPropertyStore.Get(x => x.ConvergenceTolerance); }
         set { _extendedPropertyStore.Set(x => x.ConvergenceTolerance, value); }
      }

      public int MaxEvaluation
      {
         get { return _extendedPropertyStore.Get(x => x.MaxEvaluation); }
         set { _extendedPropertyStore.Set(x => x.MaxEvaluation, value); }
      }
   }
}