using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_IdentificationParameterFactory : ContextSpecification<IIdentificationParameterFactory>
   {
      protected IContainerTask _containerTask;

      protected List<ParameterSelection> _simulationParameterSelection;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation;
      protected IParameter _lengthLiver;
      private IParameter _lengthKidney;
      private IParameter _fractionLiver;
      protected IIdentificationParameterTask _identificationParameterTask;
      private IParameter _lipoKidney;
      private IParameter _fracKidney;
      private IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _containerTask = A.Fake<IContainerTask>();
         _identificationParameterTask = A.Fake<IIdentificationParameterTask>();
         _objectBaseFactory= A.Fake<IObjectBaseFactory>();
         sut = new IdentificationParameterFactory(_objectBaseFactory, _containerTask, _identificationParameterTask);

         _simulationParameterSelection = new List<ParameterSelection>();
         _parameterIdentification = new ParameterIdentification();

         _simulation = A.Fake<ISimulation>();
         _simulation.Model.Root = new Container();

         A.CallTo(() => _objectBaseFactory.Create<IdentificationParameter>()).Returns(new IdentificationParameter());
         _parameterIdentification.AddSimulation(_simulation);

         _lengthLiver = A.Fake<IParameter>().WithName("Length").WithDimension(DomainHelperForSpecs.LengthDimensionForSpecs());
         _fractionLiver = A.Fake<IParameter>().WithName("Fraction").WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
         _lengthKidney = A.Fake<IParameter>().WithName("Length").WithDimension(DomainHelperForSpecs.LengthDimensionForSpecs());
         var logDimension = A.Fake<IDimension>();
         A.CallTo(() => logDimension.Name).Returns(Constants.Dimension.LOG_UNITS);
         _lipoKidney = A.Fake<IParameter>().WithName("Lipo").WithDimension(logDimension);
         _fracKidney = A.Fake<IParameter>().WithName("Frac").WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());

         var liver = new Container().WithName("Liver");
         var kidney = new Container().WithName("Kidney");
         liver.Add(_lengthLiver);
         liver.Add(_fractionLiver);
         kidney.Add(_lengthKidney);
         kidney.Add(_lipoKidney);
         kidney.Add(_fracKidney);
         _simulation.Model.Root.Add(liver);
         _simulation.Model.Root.Add(kidney);


         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Liver|Length", QuantityType.Parameter)));
      }
   }

   public class When_creating_an_identification_parameter_for_parameters_with_different_dimension : concern_for_IdentificationParameterFactory
   {
      protected override void Context()
      {
         base.Context();
         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Liver|Fraction", QuantityType.Parameter)));
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateFor(_simulationParameterSelection, _parameterIdentification)).ShouldThrowAn<DimensionMismatchException>();
      }
   }

   public class When_creating_an_identification_parameter_for_parameters_already_used_in_a_parameter_identification : concern_for_IdentificationParameterFactory
   {
      protected override void Context()
      {
         base.Context();
         var identificationParameter = new IdentificationParameter();
         identificationParameter.AddLinkedParameter(_simulationParameterSelection[0]);
         _parameterIdentification.AddIdentificationParameter(identificationParameter);
      }

      [Observation]
      public void should_return_null()
      {
         sut.CreateFor(_simulationParameterSelection, _parameterIdentification).ShouldBeNull();
      }
   }

   public class When_creating_an_identification_parameter_for_parameters_having_the_same_name : concern_for_IdentificationParameterFactory
   {
      private IdentificationParameter _result;

      protected override void Context()
      {
         base.Context();
         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Kidney|Length", QuantityType.Parameter)));
         A.CallTo(() => _containerTask.CreateUniqueName(A<IEnumerable<IWithName>>._, "Length", true)).Returns("LENGTH");
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_simulationParameterSelection, _parameterIdentification);
      }

      [Observation]
      public void should_use_the_name_of_the_parameter_adjusted_to_be_unique()
      {
         _result.Name.ShouldBeEqualTo("LENGTH");
      }

      [Observation]
      public void should_return_a_log_scaling()
      {
         _result.Scaling.ShouldBeEqualTo(Scalings.Log);
      }

      [Observation]
      public void should_have_linked_the_parameters_in_the_identification_parameter()
      {
         _result.AllLinkedParameters.ShouldOnlyContain(_simulationParameterSelection.ToArray());
      }

      [Observation]
      public void should_have_created_the_range_parameters_based_on_the_first_linked_parameter()
      {
         A.CallTo(() => _identificationParameterTask.AddParameterRangeBasedOn(_result, _lengthLiver)).MustHaveHappened();
      }
   }

   public class When_creating_an_identification_parameter_for_a_parameter_using_the_log_dimension : concern_for_IdentificationParameterFactory
   {
      private IdentificationParameter _result;

      protected override void Context()
      {
         base.Context();
         _simulationParameterSelection.Clear();
         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Kidney|Lipo", QuantityType.Parameter)));
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_simulationParameterSelection, _parameterIdentification);
      }

      [Observation]
      public void should_return_a_lin_scaling()
      {
         _result.Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_creating_an_identification_parameter_for_a_parameter_using_the_lin_dimension_but_with_a_minimum_value_set_to_zero : concern_for_IdentificationParameterFactory
   {
      private IdentificationParameter _result;

      protected override void Context()
      {
         base.Context();
         _simulationParameterSelection.Clear();
         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Liver|Length", QuantityType.Parameter)));
         _lengthLiver.MinValue = 0;
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_simulationParameterSelection, _parameterIdentification);
      }

      [Observation]
      public void should_return_a_lin_scaling()
      {
         _result.Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }

   public class When_creating_an_identification_parameter_for_a_parameter_using_the_fraction_dimension : concern_for_IdentificationParameterFactory
   {
      private IdentificationParameter _result;

      protected override void Context()
      {
         base.Context();
         _simulationParameterSelection.Clear();
         _simulationParameterSelection.Add(new ParameterSelection(_simulation, new QuantitySelection("Kidney|Frac", QuantityType.Parameter)));
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_simulationParameterSelection, _parameterIdentification);
      }

      [Observation]
      public void should_return_a_lin_scaling()
      {
         _result.Scaling.ShouldBeEqualTo(Scalings.Linear);
      }
   }
}