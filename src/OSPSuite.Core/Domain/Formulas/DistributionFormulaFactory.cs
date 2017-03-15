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

   }

   public class DistributionFormulaFactory : IDistributionFormulaFactory
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public DistributionFormulaFactory(IObjectPathFactory objectPathFactory,IObjectBaseFactory objectBaseFactory)
      {
         _objectPathFactory = objectPathFactory;
         _objectBaseFactory = objectBaseFactory;
      }

      public NormalDistributionFormula CreateNormalDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter, IParameter deviationParameter)
      {
         var distribution = createDistribution<NormalDistributionFormula>();
         distribution.Dimension = meanParameter.Dimension;
         addObjectPathToDistributionFormula(distribution, distributedParameter,meanParameter,Constants.Distribution.MEAN);
         addObjectPathToDistributionFormula(distribution, distributedParameter,deviationParameter,Constants.Distribution.DEVIATION);
         return distribution;
      }

      public DiscreteDistributionFormula CreateDiscreteDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter)
      {
         var distribution = createDistribution<DiscreteDistributionFormula>();
         distribution.Dimension = meanParameter.Dimension;
         addObjectPathToDistributionFormula(distribution, distributedParameter,meanParameter,Constants.Distribution.MEAN);
         return distribution;
      }

      public LogNormalDistributionFormula CreateLogNormalDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter meanParameter, IParameter deviationParameter)
      {
         var distribution = createDistribution<LogNormalDistributionFormula>();
         distribution.Dimension = meanParameter.Dimension;
         addObjectPathToDistributionFormula(distribution, distributedParameter,meanParameter,Constants.Distribution.MEAN);
         addObjectPathToDistributionFormula(distribution, distributedParameter, deviationParameter, Constants.Distribution.GEOMETRIC_DEVIATION);
         return distribution;
      }

      public UniformDistributionFormula CreateUniformDistributionFormulaFor(IDistributedParameter distributedParameter, IParameter minParameter, IParameter maxParameter)
      {
         var distribution = createDistribution<UniformDistributionFormula>();
         distribution.Dimension = minParameter.Dimension;
         addObjectPathToDistributionFormula(distribution, distributedParameter,minParameter,Constants.Distribution.MINIMUM);
         addObjectPathToDistributionFormula(distribution, distributedParameter,maxParameter,Constants.Distribution.MAXIMUM);
         return distribution;
      }

      private TDistribution createDistribution<TDistribution>() where TDistribution:IDistributionFormula
      {
         return _objectBaseFactory.CreateObjectBaseFrom<TDistribution>(typeof (TDistribution));
      }

      private void addObjectPathToDistributionFormula(IDistributionFormula distribution,
                                                      IDistributedParameter distributedParameter,
                                                      IParameter usedParameter,
                                                      string alias)
      {
         var usedParameterPath = _objectPathFactory.CreateRelativeFormulaUsablePath(distributedParameter, usedParameter);

         usedParameterPath.Alias = alias;
         usedParameterPath.Dimension = usedParameter.Dimension;

         distribution.AddObjectPath(usedParameterPath);
      }
   }
}