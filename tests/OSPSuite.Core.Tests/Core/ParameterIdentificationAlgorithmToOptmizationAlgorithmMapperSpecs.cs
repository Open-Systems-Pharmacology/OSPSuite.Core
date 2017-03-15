using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterIdentificationAlgorithmToOptmizationAlgorithmMapper : ContextSpecification<IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper>
   {
      private IContainer _container;
      protected OptimizationAlgorithmProperties _optimizationAlgorithmProperties;
      protected IOptimizationAlgorithm _optimizationAlgorithm;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         sut = new ParameterIdentificationAlgorithmToOptmizationAlgorithmMapper(_container);

         _optimizationAlgorithmProperties = new OptimizationAlgorithmProperties("Algo")
         {
            new ExtendedProperty<string> {Name = "Toto", Value = "Test"},
            new ExtendedProperty<double> {Name = "Tata", Value = 10d},
            new ExtendedProperty<bool> {Name = "Does not exist", Value = false}
         };

         _optimizationAlgorithm = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _optimizationAlgorithm.Properties).Returns(new OptimizationAlgorithmProperties("Algo"));
         _optimizationAlgorithm.Properties.Add(new ExtendedProperty<string> {Name = "Toto", Value = "OLD"});
         _optimizationAlgorithm.Properties.Add(new ExtendedProperty<double> {Name = "Tata", Value = 5d});
         _optimizationAlgorithm.Properties.Add(new ExtendedProperty<bool> {Name = "TUTU", Value = true});
         A.CallTo(() => _container.Resolve<IOptimizationAlgorithm>(_optimizationAlgorithmProperties.Name)).Returns(_optimizationAlgorithm);
      }
   }

   public class When_mappingn_a_parameter_identification_algorithm_to_an_optimization_algorithm : concern_for_ParameterIdentificationAlgorithmToOptmizationAlgorithmMapper
   {
      private IOptimizationAlgorithm _result;

      protected override void Because()
      {
         _result = sut.MapFrom(_optimizationAlgorithmProperties);
      }

      [Observation]
      public void should_retrieve_a_new_instance_from_the_container_by_key()
      {
         _result.ShouldBeEqualTo(_optimizationAlgorithm);
      }

      [Observation]
      public void should_update_all_exsiting_properties()
      {
         _result.Properties["Toto"].ValueAsObject.DowncastTo<string>().ShouldBeEqualTo("Test");
         _result.Properties["Tata"].ValueAsObject.DowncastTo<double>().ShouldBeEqualTo(10d);
         _result.Properties["TUTU"].ValueAsObject.DowncastTo<bool>().ShouldBeEqualTo(true);
         _result.Properties.Contains("Does not exist").ShouldBeFalse();
      }
   }
}