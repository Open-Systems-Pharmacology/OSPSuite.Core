using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationConfidenceIntervalPresenter : ContextSpecification<IParameterIdentificationConfidenceIntervalPresenter>
   {
      protected IConfidenceIntervalCalculator _confidenceIntervalCalculator;
      protected IParameterIdentificationConfidenceIntervalView _view;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _runResult;
      protected List<ParameterConfidenceIntervalDTO> _allParameterConfidenceIntervalDTOs;

      protected override void Context()
      {
         _confidenceIntervalCalculator= A.Fake<IConfidenceIntervalCalculator>(); 
         _view= A.Fake<IParameterIdentificationConfidenceIntervalView>();
         sut = new ParameterIdentificationConfidenceIntervalPresenter(_view,_confidenceIntervalCalculator);

         _parameterIdentification = A.Fake<ParameterIdentification>();
         _runResult = A.Fake<ParameterIdentificationRunResult>();

         A.CallTo(() => _view.BindTo(A<IEnumerable<ParameterConfidenceIntervalDTO>>._))
            .Invokes(x => _allParameterConfidenceIntervalDTOs = x.GetArgument<IEnumerable<ParameterConfidenceIntervalDTO>>(0).ToList());

      }
   }

   public class When_calculating_the_confidence_interval_for_a_parameter_identification_whose_jacobian_matrix_is_not_available : concern_for_ParameterIdentificationConfidenceIntervalPresenter
   {
      protected override void Context()
      {
         base.Context();
         _runResult.JacobianMatrix = null;
      }

      protected override void Because()
      {
         sut.CalculateConfidenceIntervalFor(_parameterIdentification,_runResult);
      }

      [Observation]
      public void should_delete_the_binding_in_the_view()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();   
      }
   }


   public class When_calculating_the_confidence_interval_for_a_parameter_identificaiton_with_a_valid_jacobian : concern_for_ParameterIdentificationConfidenceIntervalPresenter
   {
      private ICache<string, double> _confidenceIntervals;
      private ParameterSelection _parameterSelection;
      private IDimension _dimension;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _dimension= A.Fake<IDimension>();
         _parameterSelection = A.Fake<ParameterSelection>();
         A.CallTo(() => _parameterSelection.FullQuantityPath).Returns("P1");
         A.CallTo(() => _parameterSelection.Dimension).Returns(_dimension);
         _runResult.JacobianMatrix  = new JacobianMatrix(new []{ _parameterSelection.FullQuantityPath});

         _identificationParameter = DomainHelperForSpecs.IdentificationParameter("IP1");
         _confidenceIntervals = new Cache<string, double>();
         _confidenceIntervals.Add(_identificationParameter.Name, 5);

         A.CallTo(_confidenceIntervalCalculator).WithReturnType<ICache<string, double>>().Returns(_confidenceIntervals);
         A.CallTo(() => _runResult.BestResult.Values).Returns(new []{new OptimizedParameterValue(_identificationParameter.Name, 10, 20) });
         A.CallTo(() => _parameterIdentification.IdentificationParameterByName(_identificationParameter.Name)).Returns(_identificationParameter);

         _identificationParameter.AddLinkedParameter(_parameterSelection);

         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_identificationParameter.StartValueParameter.DisplayUnit,5)).Returns(50);
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_identificationParameter.StartValueParameter.DisplayUnit, 10)).Returns(100);
      }

      protected override void Because()
      {
         sut.CalculateConfidenceIntervalFor(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_bind_the_epxected_confidence_interval_to_the_view()
      {
         _allParameterConfidenceIntervalDTOs.Count.ShouldBeEqualTo(1);
         var confidenceIntervalDTO = _allParameterConfidenceIntervalDTOs[0];
         confidenceIntervalDTO.Name.ShouldBeEqualTo(_identificationParameter.Name);
         confidenceIntervalDTO.ConfidenceInterval.ShouldBeEqualTo(50);
         confidenceIntervalDTO.Value.ShouldBeEqualTo(100);
         confidenceIntervalDTO.Unit.ShouldBeEqualTo(_identificationParameter.DisplayUnit);
      }
   }
}	