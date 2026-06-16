using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartTemplatingTask : ContextSpecification<TestChartTemplatingTask>
   {
      protected IApplicationController _applicationController;
      protected IModalChartTemplateManagerPresenter _modalChartTemplateManagerPresenter;
      protected IWithChartTemplates _withChartTemplates;
      protected CurveChartTemplate _timeProfileTemplate;
      protected CurveChartTemplate _predictedVsObservedTemplate;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         _modalChartTemplateManagerPresenter = A.Fake<IModalChartTemplateManagerPresenter>();
         A.CallTo(() => _applicationController.Start<IModalChartTemplateManagerPresenter>()).Returns(_modalChartTemplateManagerPresenter);

         _timeProfileTemplate = new CurveChartTemplate {Name = "TimeProfileTemplate", ChartType = CurveChartTypes.TimeProfile};
         _predictedVsObservedTemplate = new CurveChartTemplate {Name = "PredictedVsObservedTemplate", ChartType = CurveChartTypes.PredictedVsObserved};
         _withChartTemplates = A.Fake<IWithChartTemplates>();
         A.CallTo(() => _withChartTemplates.ChartTemplates).Returns(new[] {_timeProfileTemplate, _predictedVsObservedTemplate});

         sut = new TestChartTemplatingTask(_applicationController, A.Fake<IChartTemplatePersistor>(), A.Fake<ICloneManager>(),
            A.Fake<ICurveChartToCurveChartTemplateMapper>(), A.Fake<IChartFromTemplateService>(), A.Fake<IChartUpdater>(), A.Fake<IDialogCreator>());
      }
   }

   public class TestChartTemplatingTask : ChartTemplatingTask
   {
      public IWithChartTemplates ReplacedIn { get; private set; }
      public IReadOnlyList<CurveChartTemplate> ReplacedTemplates { get; private set; }

      public TestChartTemplatingTask(IApplicationController applicationController, IChartTemplatePersistor chartTemplatePersistor, ICloneManager cloneManager,
         ICurveChartToCurveChartTemplateMapper chartTemplateMapper, IChartFromTemplateService chartFromTemplateService, IChartUpdater chartUpdater, IDialogCreator dialogCreator)
         : base(applicationController, chartTemplatePersistor, cloneManager, chartTemplateMapper, chartFromTemplateService, chartUpdater, dialogCreator)
      {
      }

      protected override ICommand ReplaceTemplatesCommand(IWithChartTemplates withChartTemplates, IEnumerable<CurveChartTemplate> curveChartTemplates)
      {
         ReplacedIn = withChartTemplates;
         ReplacedTemplates = curveChartTemplates.ToList();
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }

      public override ICommand AddChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates)
      {
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }

      public override ICommand UpdateChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates, string templateName)
      {
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }
   }

   public class When_managing_the_chart_templates_for_a_given_chart_type : concern_for_ChartTemplatingTask
   {
      private List<CurveChartTemplate> _editedTemplates;

      protected override void Context()
      {
         base.Context();
         _editedTemplates = new List<CurveChartTemplate> {_predictedVsObservedTemplate, _timeProfileTemplate};
         A.CallTo(() => _modalChartTemplateManagerPresenter.HasChanged).Returns(true);
         A.CallTo(() => _modalChartTemplateManagerPresenter.Canceled()).Returns(false);
         A.CallTo(() => _modalChartTemplateManagerPresenter.EditedTemplates).Returns(_editedTemplates);
      }

      protected override void Because()
      {
         sut.ManageTemplates(_withChartTemplates, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_edit_the_templates_for_the_given_chart_type()
      {
         A.CallTo(() => _modalChartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>._, CurveChartTypes.PredictedVsObserved)).MustHaveHappened();
      }

      [Observation]
      public void should_replace_the_templates_with_the_edited_templates_including_the_templates_of_other_chart_types()
      {
         sut.ReplacedIn.ShouldBeEqualTo(_withChartTemplates);
         sut.ReplacedTemplates.ShouldOnlyContain(_predictedVsObservedTemplate, _timeProfileTemplate);
      }
   }
}
