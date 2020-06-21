using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationRunPropertiesPresenter : ContextSpecification<IParameterIdentificationRunPropertiesPresenter>
   {
      private IParameterIdentificationRunPropertiesView _view;
      protected List<IRunPropertyDTO> _allRunPropertyDTO;

      protected override void Context()
      {
         _view= A.Fake<IParameterIdentificationRunPropertiesView>();
         sut = new ParameterIdentificationRunPropertiesPresenter(_view);

         A.CallTo(() => _view.BindTo(A<IEnumerable<IRunPropertyDTO>>._))
            .Invokes(x => _allRunPropertyDTO = x.GetArgument<IEnumerable<IRunPropertyDTO>>(0).ToList());

      }
   }

   public class When_editing_the_run_properties_of_a_parameter_identification_run_result : concern_for_ParameterIdentificationRunPropertiesPresenter
   {
      private ParameterIdentificationRunResult _runResult;

      protected override void Context()
      {
         base.Context();
         _runResult = A.Fake<ParameterIdentificationRunResult>();
         A.CallTo(() => _runResult.TotalError).Returns(0.5);
         A.CallTo(() => _runResult.NumberOfEvaluations).Returns(10);
         A.CallTo(() => _runResult.Duration).Returns(TimeSpan.FromMinutes(2));
         A.CallTo(() => _runResult.Status).Returns(RunStatus.RanToCompletion);
      }

      protected override void Because()
      {
         sut.Edit(_runResult);
      }

      [Observation]
      public void should_create_one_run_property_dto_per_property_to_be_displayed()
      {
         _allRunPropertyDTO.Count.ShouldBeEqualTo(4);
         _allRunPropertyDTO[0].Name.ShouldBeEqualTo(Captions.ParameterIdentification.TotalError);
         _allRunPropertyDTO[0].FormattedValue.ShouldBeEqualTo(new DoubleFormatter().Format(_runResult.TotalError));
         _allRunPropertyDTO[0].Icon.ShouldBeNull();

         _allRunPropertyDTO[1].Name.ShouldBeEqualTo(Captions.ParameterIdentification.NumberOfEvaluations);
         _allRunPropertyDTO[1].FormattedValue.ShouldBeEqualTo(new IntFormatter().Format(_runResult.NumberOfEvaluations));
         _allRunPropertyDTO[1].Icon.ShouldBeNull();

         _allRunPropertyDTO[2].Name.ShouldBeEqualTo(Captions.ParameterIdentification.Duration);
         _allRunPropertyDTO[2].FormattedValue.ShouldBeEqualTo(new TimeSpanFormatter().Format(_runResult.Duration));
         _allRunPropertyDTO[2].Icon.ShouldBeNull();

         _allRunPropertyDTO[3].Name.ShouldBeEqualTo(Captions.ParameterIdentification.Status);
         _allRunPropertyDTO[3].FormattedValue.ShouldBeEqualTo(_runResult.Status.DisplayName);
         _allRunPropertyDTO[3].Icon.ShouldBeEqualTo(ApplicationIcons.OK);
      }
   }
}	