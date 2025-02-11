using System;
using OSPSuite.Core.Domain.UnitSystem;
using static OSPSuite.Core.Domain.Constants.Dimension;
using static OSPSuite.Core.Domain.Constants.Distribution;

namespace OSPSuite.Core.Domain.Formulas
{
   public interface IDistributionFormulaFactory
   {
      NormalDistributionFormula CreateNormalDistributionFormulaFor(
         IDistributedParameter distributedParameter,
         IParameter meanParameter,
         IParameter deviationParameter);

      DiscreteDistributionFormula CreateDiscreteDistributionFormulaFor(
         IDistributedParameter distributedParameter,
         IParameter meanParameter);

      LogNormalDistributionFormula CreateLogNormalDistributionFormulaFor(
         IDistributedParameter distributedParameter,
         IParameter meanParameter,
         IParameter deviationParameter);

      UniformDistributionFormula CreateUniformDistributionFormulaFor(
         IDistributedParameter distributedParameter,
         IParameter minParameter,
         IParameter maxParameter);

      DistributionFormula CreateFor(DistributionType distributionType, IDimension dimension);
   }

   public class DistributionFormulaFactory : IDistributionFormulaFactory
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public DistributionFormulaFactory(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory)
      {
         _objectPathFactory = objectPathFactory;
         _objectBaseFactory = objectBaseFactory;
      }

      public NormalDistributionFormula CreateNormalDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter, IParameter deviationParameter) =>
         createDistribution<NormalDistributionFormula>(meanParameter.Dimension, configureNormalDistribution(meanParameter.Name, deviationParameter.Name));

      private Action<DistributionFormula, IDimension> configureNormalDistribution(string meanParameterName = MEAN, string deviationParameterName = DEVIATION) => (distributionFormula, dimension) =>
      {
         addObjectPathToDistributionFormula(distributionFormula, meanParameterName, MEAN, dimension);
         addObjectPathToDistributionFormula(distributionFormula, deviationParameterName, DEVIATION, dimension);
      };

      public DiscreteDistributionFormula CreateDiscreteDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter) =>
         createDistribution<DiscreteDistributionFormula>(meanParameter.Dimension, configureDiscreteDistribution(meanParameter.Name));

      private Action<DistributionFormula, IDimension> configureDiscreteDistribution(string meanParameterName = MEAN) => (distributionFormula, dimension) =>
         addObjectPathToDistributionFormula(distributionFormula, meanParameterName, MEAN, dimension);

      public LogNormalDistributionFormula CreateLogNormalDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter, IParameter deviationParameter) =>
         createDistribution<LogNormalDistributionFormula>(meanParameter.Dimension, configureLogNormalDistribution(meanParameter.Name, deviationParameter.Name));

      private Action<DistributionFormula, IDimension> configureLogNormalDistribution(string meanParameterName = MEAN, string deviationParameter = GEOMETRIC_DEVIATION) => (distributionFormula, dimension) =>
      {
         addObjectPathToDistributionFormula(distributionFormula, meanParameterName, MEAN, dimension);
         addObjectPathToDistributionFormula(distributionFormula, deviationParameter, GEOMETRIC_DEVIATION, NO_DIMENSION);
      };

      public UniformDistributionFormula CreateUniformDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter minParameter, IParameter maxParameter) =>
         createDistribution<UniformDistributionFormula>(minParameter.Dimension, configureUniformDistribution(minParameter.Name, maxParameter.Name));

      private Action<DistributionFormula, IDimension> configureUniformDistribution(string minParameterName = MINIMUM, string maxParameterName = MAXIMUM) => (distributionFormula, dimension) =>
      {
         addObjectPathToDistributionFormula(distributionFormula, minParameterName, MINIMUM, dimension);
         addObjectPathToDistributionFormula(distributionFormula, maxParameterName, MAXIMUM, dimension);
      };

      public DistributionFormula CreateFor(DistributionType distributionType, IDimension dimension)
      {
         switch (distributionType)
         {
            case DistributionType.Normal:
               return createDistribution<NormalDistributionFormula>(dimension, configureNormalDistribution());
            case DistributionType.LogNormal:
               return createDistribution<LogNormalDistributionFormula>(dimension, configureLogNormalDistribution());
            case DistributionType.Uniform:
               return createDistribution<UniformDistributionFormula>(dimension, configureUniformDistribution());
            case DistributionType.Discrete:
               return createDistribution<DiscreteDistributionFormula>(dimension, configureDiscreteDistribution());
            default:
               throw new ArgumentOutOfRangeException(nameof(distributionType), distributionType, null);
         }
      }

      private TDistribution createDistribution<TDistribution>(IDimension dimension, Action<DistributionFormula, IDimension> configure) where TDistribution : DistributionFormula
      {
         var distribution = _objectBaseFactory.CreateObjectBaseFrom<TDistribution>(typeof(TDistribution))
            .WithDimension(dimension);

         configure(distribution, dimension);
         return distribution;
      }

      private void addObjectPathToDistributionFormula(DistributionFormula distribution, string parameterName, string alias, IDimension dimension)
      {
         var usedParameterPath = _objectPathFactory
            .CreateFormulaUsablePathFrom(parameterName)
            .WithAlias(alias)
            .WithDimension(dimension ?? NO_DIMENSION);

         distribution.AddObjectPath(usedParameterPath);
      }
   }
}