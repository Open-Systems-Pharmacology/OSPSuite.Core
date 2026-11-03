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
      protected ICopyChartTemplatesPresenter _copyChartTemplatesPresenter;
      protected ICloneManager _cloneManager;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         _modalChartTemplateManagerPresenter = A.Fake<IModalChartTemplateManagerPresenter>();
         A.CallTo(() => _applicationController.Start<IModalChartTemplateManagerPresenter>()).Returns(_modalChartTemplateManagerPresenter);
         _copyChartTemplatesPresenter = A.Fake<ICopyChartTemplatesPresenter>();
         A.CallTo(() => _applicationController.Start<ICopyChartTemplatesPresenter>()).Returns(_copyChartTemplatesPresenter);
         _cloneManager = A.Fake<ICloneManager>();
         _dialogCreator = A.Fake<IDialogCreator>();

         _timeProfileTemplate = new CurveChartTemplate {Name = "TimeProfileTemplate", ChartType = CurveChartTypes.TimeProfile};
         _predictedVsObservedTemplate = new CurveChartTemplate {Name = "PredictedVsObservedTemplate", ChartType = CurveChartTypes.PredictedVsObserved};
         _withChartTemplates = A.Fake<IWithChartTemplates>();
         A.CallTo(() => _withChartTemplates.ChartTemplates).Returns(new[] {_timeProfileTemplate, _predictedVsObservedTemplate});

         sut = new TestChartTemplatingTask(_applicationController, A.Fake<IChartTemplatePersistor>(), _cloneManager,
            A.Fake<ICurveChartToCurveChartTemplateMapper>(), A.Fake<IChartFromTemplateService>(), A.Fake<IChartUpdater>(), _dialogCreator);
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
         A.CallTo(() => _modalChartTemplateManagerPresenter.EditTemplates(_withChartTemplates, CurveChartTypes.PredictedVsObserved)).MustHaveHappened();
      }

      [Observation]
      public void should_replace_the_templates_with_the_edited_templates_including_the_templates_of_other_chart_types()
      {
         sut.ReplacedIn.ShouldBeEqualTo(_withChartTemplates);
         sut.ReplacedTemplates.ShouldOnlyContain(_predictedVsObservedTemplate, _timeProfileTemplate);
      }
   }

   public class When_copying_chart_templates_from_the_project : concern_for_ChartTemplatingTask
   {
      private CurveChartTemplate _templateWithUniqueName;
      private CurveChartTemplate _templateWithForbiddenName;
      private CurveChartTemplate _templateWithCanceledRename;
      private CurveChartTemplate _copyWithUniqueName;
      private CurveChartTemplate _copyWithForbiddenName;
      private CurveChartTemplate _copyWithCanceledRename;
      private IReadOnlyList<CurveChartTemplate> _result;

      protected override void Context()
      {
         base.Context();
         _templateWithUniqueName = new CurveChartTemplate {Name = "Unique", ChartType = CurveChartTypes.PredictedVsObserved};
         _templateWithForbiddenName = new CurveChartTemplate {Name = "Forbidden", ChartType = CurveChartTypes.PredictedVsObserved};
         _templateWithCanceledRename = new CurveChartTemplate {Name = "Unique", ChartType = CurveChartTypes.PredictedVsObserved};
         _copyWithUniqueName = new CurveChartTemplate {Name = "Unique", ChartType = CurveChartTypes.PredictedVsObserved};
         _copyWithForbiddenName = new CurveChartTemplate {Name = "Forbidden", ChartType = CurveChartTypes.PredictedVsObserved};
         _copyWithCanceledRename = new CurveChartTemplate {Name = "Unique", ChartType = CurveChartTypes.PredictedVsObserved};
         A.CallTo(() => _cloneManager.Clone(_templateWithUniqueName)).Returns(_copyWithUniqueName);
         A.CallTo(() => _cloneManager.Clone(_templateWithForbiddenName)).Returns(_copyWithForbiddenName);
         A.CallTo(() => _cloneManager.Clone(_templateWithCanceledRename)).Returns(_copyWithCanceledRename);

         A.CallTo(() => _copyChartTemplatesPresenter.SelectTemplatesToCopy(_withChartTemplates, CurveChartTypes.PredictedVsObserved))
            .Returns(new[] {_templateWithUniqueName, _templateWithForbiddenName, _templateWithCanceledRename});

         A.CallTo(() => _dialogCreator.AskForInput(A<string>._, A<string>._, "Forbidden", A<IEnumerable<string>>._, A<IEnumerable<string>>._, A<string>._)).Returns("Renamed");
         A.CallTo(() => _dialogCreator.AskForInput(A<string>._, A<string>._, "Unique", A<IEnumerable<string>>._, A<IEnumerable<string>>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         _result = sut.CopyTemplatesFromProject(_withChartTemplates, CurveChartTypes.PredictedVsObserved, new[] {"Forbidden"});
      }

      [Observation]
      public void should_return_a_copy_of_the_templates_selected_by_the_user()
      {
         _result.ShouldOnlyContainInOrder(_copyWithUniqueName, _copyWithForbiddenName);
      }

      [Observation]
      public void should_ask_the_user_to_rename_a_copy_whose_name_is_forbidden()
      {
         _copyWithForbiddenName.Name.ShouldBeEqualTo("Renamed");
      }

      [Observation]
      public void should_not_return_a_copy_whose_name_is_used_by_another_copy_if_the_user_cancels_the_rename()
      {
         _result.ShouldNotContain(_copyWithCanceledRename);
      }

      [Observation]
      public void should_not_modify_the_templates_of_the_source()
      {
         _templateWithForbiddenName.Name.ShouldBeEqualTo("Forbidden");
      }
   }
}
