using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DistributionFormulaFactory : ContextSpecification<IDistributionFormulaFactory>
   {
      protected IDistributedParameter _distributedParam;
      protected IDimension _dim;
      protected IObjectBaseFactory _objectBaseFacotry;
      private Utility.Container.IContainer _container;

      protected override void Context()
      {
         var percentileParam = new Parameter().WithName(Constants.Distribution.PERCENTILE).WithDimension(_dim).WithValue(0.5);
         _dim = new Dimension(new BaseDimensionRepresentation(), "dimenion", "unit");
         _distributedParam = new DistributedParameter().WithName("P1").WithDimension(_dim);
         _distributedParam.Add(percentileParam);
         _container = A.Fake<Utility.Container.IContainer>();
         var noDimension = A.Fake<IDimension>();
         var dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => dimensionFactory.NoDimension).Returns(noDimension);
         sut = new DistributionFormulaFactory(new ObjectPathFactory(new AliasCreator()), new ObjectBaseFactory(_container, dimensionFactory, new IdGenerator(), A.Fake<ICreationMetaDataFactory>()));
      }
   }

   public class When_creating_normal_distribution : concern_for_DistributionFormulaFactory
   {
      protected NormalDistributionFormula _normalDistribution;
      protected IParameter _meanParam, _deviationParam;

      protected override void Context()
      {
         base.Context();
         _meanParam = new Parameter().WithName(Constants.Distribution.MEAN).WithDimension(_dim).WithValue(2.2);
         _deviationParam = new Parameter().WithName(Constants.Distribution.DEVIATION).WithDimension(_dim).WithValue(3.5);

         _distributedParam.Add(_meanParam);
         _distributedParam.Add(_deviationParam);
      }

      protected override void Because()
      {
         _normalDistribution = sut.CreateNormalDistributionFormulaFor(_distributedParam, _meanParam,
            _deviationParam);

         _distributedParam.Formula = _normalDistribution;
         _distributedParam.Percentile = 0.2;
      }

      [Observation]
      public void distributed_parameter_should_return_correct_value()
      {
         var distr = new NormalDistribution(_meanParam.Value, _deviationParam.Value);

         _distributedParam.Value.ShouldBeEqualTo(distr.CalculateValueFromPercentile(_distributedParam.Percentile), 1e-5);
      }
   }

   public class When_creating_discrete_distribution : concern_for_DistributionFormulaFactory
   {
      protected DiscreteDistributionFormula _discreteDistribution;
      protected IParameter _meanParam;

      protected override void Context()
      {
         base.Context();
         _meanParam = new Parameter().WithName(Constants.Distribution.MEAN).WithDimension(_dim).WithValue(2.2);

         _distributedParam.Add(_meanParam);
      }

      protected override void Because()
      {
         _discreteDistribution = sut.CreateDiscreteDistributionFormulaFor(_distributedParam, _meanParam);

         _distributedParam.Formula = _discreteDistribution;
         _distributedParam.Percentile = 0.2;
      }

      [Observation]
      public void distributed_parameter_should_return_correct_value()
      {
         var distr = new DiscreteDistributionFormula();

         _distributedParam.Value.ShouldBeEqualTo(distr.CalculateValueFromPercentile(_distributedParam.Percentile, _distributedParam), 1e-5);
      }
   }

   public class When_creating_lognormal_distribution : concern_for_DistributionFormulaFactory
   {
      protected LogNormalDistributionFormula _logNormalDistribution;
      protected IParameter _meanParam, _gsd;

      protected override void Context()
      {
         base.Context();
         _meanParam = new Parameter().WithName(Constants.Distribution.MEAN).WithDimension(_dim).WithValue(2.2);
         _gsd = new Parameter().WithName(Constants.Distribution.GEOMETRIC_DEVIATION).WithDimension(_dim).WithValue(Math.Exp(3.5));

         _distributedParam.Add(_meanParam);
         _distributedParam.Add(_gsd);
      }

      protected override void Because()
      {
         _logNormalDistribution = sut.CreateLogNormalDistributionFormulaFor(_distributedParam, _meanParam, _gsd);

         _distributedParam.Formula = _logNormalDistribution;
         _distributedParam.Percentile = 0.2;
      }

      [Observation]
      public void distributed_parameter_should_return_correct_value()
      {
         var distr = new LogNormalDistribution(Math.Log(_meanParam.Value), Math.Log(_gsd.Value));

         _distributedParam.Value.ShouldBeEqualTo(distr.CalculateValueFromPercentile(_distributedParam.Percentile), 1e-5);
      }
   }

   public class When_creating_uniform_distribution : concern_for_DistributionFormulaFactory
   {
      protected UniformDistributionFormula _uniformDistribution;
      protected IParameter _minParam, _maxParam;

      private class UniformDistrForTest : UniformDistributionFormula
      {
         private readonly IParameter _minParam;
         private readonly IParameter _maxParam;

         public UniformDistrForTest(IParameter minParam, IParameter maxParam)
         {
            _minParam = minParam;
            _maxParam = maxParam;
         }

         protected override double Minimum(IUsingFormula dependentObject)
         {
            return _minParam.Value;
         }

         protected override double Maximum(IUsingFormula dependentObject)
         {
            return _maxParam.Value;
         }
      }

      protected override void Context()
      {
         base.Context();
         _minParam = new Parameter().WithName(Constants.Distribution.MINIMUM).WithDimension(_dim).WithValue(2.2);
         _maxParam = new Parameter().WithName(Constants.Distribution.MAXIMUM).WithDimension(_dim).WithValue(8.8);

         _distributedParam.Add(_minParam);
         _distributedParam.Add(_maxParam);
      }

      protected override void Because()
      {
         _uniformDistribution = sut.CreateUniformDistributionFormulaFor(_distributedParam, _minParam,
            _maxParam);

         _distributedParam.Formula = _uniformDistribution;
         _distributedParam.Percentile = 0.2;
      }

      [Observation]
      public void distributed_parameter_should_have_correct_value()
      {
         var distr = new UniformDistrForTest(_minParam, _maxParam);
         _distributedParam.Value.ShouldBeEqualTo(distr.CalculateValueFromPercentile(_distributedParam.Percentile, _distributedParam), 1e-5);
      }
   }
}