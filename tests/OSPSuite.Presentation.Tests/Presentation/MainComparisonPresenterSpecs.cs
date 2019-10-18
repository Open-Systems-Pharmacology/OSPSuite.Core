using System.Data;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_MainComparisonPresenter : ContextSpecification<IMainComparisonPresenter>
   {
      protected IPresentationUserSettings _presentationUserSettings;
      protected IMainComparisonView _view;
      protected IComparisonPresenter _comparisonPresenter;
      protected IComparerSettingsPresenter _comparerSettingsPresenter;
      protected ComparerSettings _userComparerSettings;
      protected IDialogCreator _dialogCreator;
      protected IExportDataTableToExcelTask _exportToExcelTask;
      private IRegionResolver _regionResolver;
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _presentationUserSettings = A.Fake<IPresentationUserSettings>();
         _view = A.Fake<IMainComparisonView>();
         _regionResolver = A.Fake<IRegionResolver>();
         _comparisonPresenter = A.Fake<IComparisonPresenter>();
         _comparerSettingsPresenter = A.Fake<IComparerSettingsPresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _exportToExcelTask = A.Fake<IExportDataTableToExcelTask>();
         _userComparerSettings = A.Fake<ComparerSettings>();
         _executionContext= A.Fake<IOSPSuiteExecutionContext>();
         A.CallTo(() => _presentationUserSettings.ComparerSettings).Returns(_userComparerSettings);
         sut = new MainComparisonPresenterForSpecs(_view, _regionResolver, _comparisonPresenter, _comparerSettingsPresenter, _presentationUserSettings, _dialogCreator, _exportToExcelTask,_executionContext);
      }
   }

   public class When_comparing_two_objects_in_the_main_comparison_presenter : concern_for_MainComparisonPresenter
   {
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;

      protected override void Context()
      {
         base.Context();
         _leftObject = A.Fake<IObjectBase>();
         _rightObject = A.Fake<IObjectBase>();
      }

      protected override void Because()
      {
         sut.CompareObjects(_leftObject, _rightObject, runComparison: true, leftCaption: "LEFT", rightCaption: "RIGHT", viewCaption: "ABC");
      }

      [Observation]
      public void should_retrieve_the_current_comparison_settings_from_the_user_settings_and_use_them_for_the_comparison()
      {
         A.CallTo(() => _comparerSettingsPresenter.Edit(_userComparerSettings)).MustHaveHappened();
      }

      [Observation]
      public void should_start_the_comparison()
      {
         A.CallTo(() => _comparisonPresenter.StartComparison(_leftObject, _rightObject, "LEFT", "RIGHT", _userComparerSettings)).MustHaveHappened();
      }
   }

   public class When_exporting_the_comparison_to_excel_and_the_user_cancels_in_the_main_comparison_presenter : concern_for_MainComparisonPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.ExportToExcel();
      }

      [Observation]
      public void should_not_export_anything()
      {
         A.CallTo(() => _comparisonPresenter.ComparisonAsTable()).MustNotHaveHappened();
      }
   }

   public class When_exporting_the_comparison_to_excel_and_the_user_accepts_the_export_in_the_main_comparison_presenter : concern_for_MainComparisonPresenter
   {
      private string _fileName;
      private DataTable _dataTable;

      protected override void Context()
      {
         base.Context();
         _fileName = FileHelper.GenerateTemporaryFileName();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileName);
         _dataTable = new DataTable();
         A.CallTo(() => _comparisonPresenter.ComparisonAsTable()).Returns(_dataTable);
      }

      protected override void Because()
      {
         sut.ExportToExcel();
      }

      [Observation]
      public void should_export_the_file_to_excel()
      {
         A.CallTo(() => _exportToExcelTask.ExportDataTableToExcel(_dataTable, _fileName, true)).MustHaveHappened();
      }
   }

   public class When_checking_if_a_comparison_can_be_performed : concern_for_MainComparisonPresenter
   {
      [Observation]
      public void should_return_true_if_both_left_and_right_objects_are_defined()
      {
         sut.CompareObjects(A.Fake<IObjectBase>(), A.Fake<IObjectBase>());
         sut.CanCompare.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_either_left_or_right_or_both_objects_are_undefined()
      {
         sut.CanCompare.ShouldBeFalse();
      }
   }

   public class When_clearing_the_comparison_conditionally_using_an_object_not_being_compared : concern_for_MainComparisonPresenter
   {
      private IObjectBase _objectNotBeingCompared;
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;

      protected override void Context()
      {
         base.Context();
         _leftObject = A.Fake<IObjectBase>();
         _rightObject = A.Fake<IObjectBase>();
         _objectNotBeingCompared = A.Fake<IObjectBase>();
         sut.CompareObjects(_leftObject, _rightObject);
      }

      protected override void Because()
      {
         sut.ClearComparisonIfComparing(_objectNotBeingCompared);
      }

      [Observation]
      public void should_not_clear_the_comparison()
      {
         A.CallTo(() => _comparisonPresenter.Clear()).MustNotHaveHappened();
      }
   }


   public class When_clearing_the_comparison_conditionally_using_an_object_being_compared : concern_for_MainComparisonPresenter
   {
      private IObjectBase _leftObject;
      private IObjectBase _rightObject;

      protected override void Context()
      {
         base.Context();
         _leftObject = A.Fake<IObjectBase>();
         _rightObject = A.Fake<IObjectBase>();
         sut.CompareObjects(_leftObject, _rightObject);
      }

      protected override void Because()
      {
         sut.ClearComparisonIfComparing(_leftObject);
      }

      [Observation]
      public void should_not_clear_the_comparison()
      {
         A.CallTo(() => _comparisonPresenter.Clear()).MustHaveHappened();
      }
   }

   public class MainComparisonPresenterForSpecs : MainComparisonPresenter
   {
      public MainComparisonPresenterForSpecs(IMainComparisonView view, IRegionResolver regionResolver, IComparisonPresenter comparisonPresenter,
         IComparerSettingsPresenter comparerSettingsPresenter, IPresentationUserSettings presentationUserSettings,
         IDialogCreator dialogCreator, IExportDataTableToExcelTask exportToExcelTask, IOSPSuiteExecutionContext executionContext) : 
         base(view, regionResolver, comparisonPresenter, comparerSettingsPresenter, presentationUserSettings, dialogCreator, exportToExcelTask,executionContext, new RegionName("A", "A", ApplicationIcons.Comparison))
      {
      }
   }
}