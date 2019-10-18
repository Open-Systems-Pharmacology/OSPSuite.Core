using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DataBrowserPresenter : ContextSpecification<IDataBrowserPresenter>
   {
      protected IDataBrowserView _view;
      protected List<DataColumnDTO> _allDataColumnDTOs;
      protected DataRepository _obsData1;
      protected DataRepository _obsData2;
      protected DataColumn _column1;
      protected DataColumn _column2;

      protected override void Context()
      {
         _view = A.Fake<IDataBrowserView>();
         sut = new DataBrowserPresenter(_view);
         sut.SetDisplayQuantityPathDefinition(x => new PathElements {{PathElement.Name, new PathElementDTO {DisplayName = x.Name}}});

         A.CallTo(() => _view.BindTo(A<IEnumerable<DataColumnDTO>>._))
            .Invokes(x => _allDataColumnDTOs = x.GetArgument<IEnumerable<DataColumnDTO>>(0).ToList());

         _obsData1 = DomainHelperForSpecs.ObservedData("OBS1");
         _column1 = _obsData1.FirstDataColumn();
         _obsData2 = DomainHelperForSpecs.ObservedData("OBS2");
         _column2 = _obsData2.FirstDataColumn();
      }
   }

   public class When_adding_columns_to_the_data_browser_presenter : concern_for_DataBrowserPresenter
   {
      private List<DataColumn> _columnsToAdd;

      protected override void Context()
      {
         base.Context();
         _columnsToAdd = new List<DataColumn> {_column1};
      }

      protected override void Because()
      {
         sut.AddDataColumns(_columnsToAdd);
      }

      [Observation]
      public void should_bind_those_new_added_columns_to_the_view()
      {
         _allDataColumnDTOs.Count.ShouldBeEqualTo(1);
         _allDataColumnDTOs[0].DataColumn.ShouldBeEqualTo(_column1);
      }
   }

   public class When_the_data_browser_presenter_is_asked_if_a_data_columns_is_being_contained : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1});
      }

      [Observation]
      public void should_return_true_for_a_columns_that_was_added()
      {
         sut.ContainsDataColumn(_column1).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_a_column_that_was_not_added()
      {
         sut.ContainsDataColumn(_column2).ShouldBeFalse();
      }
   }

   public class When_returning_all_data_columns_available_in_the_data_browser : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2});
      }

      [Observation]
      public void should_return_all_columns_added_to_the_data_browser()
      {
         sut.AllDataColumns.ShouldOnlyContain(_column1, _column2);
      }
   }

   public class When_removing_a_data_column_from_the_data_browser : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2});
      }

      protected override void Because()
      {
         sut.RemoveDataColumns(new[] {_column1,});
      }

      [Observation]
      public void should_remove_the_column_from_the_view()
      {
         _allDataColumnDTOs.Any(x => x.DataColumn == _column1).ShouldBeFalse();
         _allDataColumnDTOs.Any(x => x.DataColumn == _column2).ShouldBeTrue();
      }

      [Observation]
      public void should_not_contain_the_remove_column_anymore()
      {
         sut.ContainsDataColumn(_column1).ShouldBeFalse();
      }

      [Observation]
      public void should_still_contain_columns_that_were_not_removed()
      {
         sut.ContainsDataColumn(_column2).ShouldBeTrue();
      }
   }

   public class When_the_data_browser_presenter_is_clearing_its_content : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2});
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_not_contain_any_column()
      {
         sut.ContainsDataColumn(_column1).ShouldBeFalse();
         sut.ContainsDataColumn(_column2).ShouldBeFalse();
      }

      [Observation]
      public void should_not_display_any_column_in_the_view()
      {
         _allDataColumnDTOs.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_the_data_browser_presenter_is_notified_that_the_use_state_for_a_data_column_has_changed : concern_for_DataBrowserPresenter
   {
      private List<DataColumn> _usedChangedColumns;
      private bool _usedState;
      private bool _selectedDataChangedRaised;

      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1});
         sut.UsedChanged += (o, e) =>
         {
            _usedChangedColumns = e.Columns.ToList();
            _usedState = e.Used;
         };

         sut.SelectionChanged += (o,e) => { _selectedDataChangedRaised = true; };
      }

      protected override void Because()
      {
         sut.UsedChangedFor(_allDataColumnDTOs[0], used: true);
      }

      [Observation]
      public void should_notify_that_the_used_state_has_changed()
      {
         _usedChangedColumns.ShouldOnlyContain(_column1);
         _usedState.ShouldBeEqualTo(true);
      }

      [Observation]
      public void should_also_notify_a_selection_changed_event()
      {
         _selectedDataChangedRaised.ShouldBeTrue();
      }
   }

   public class When_the_data_browser_is_checking_if_a_data_column_is_used : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2,});
         _allDataColumnDTOs[0].Used = true;
      }

      [Observation]
      public void should_return_true_if_the_data_column_was_added_and_is_used()
      {
         sut.IsUsed(_column1).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_data_column_was_added_and_not_used()
      {
         sut.IsUsed(_column2).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_data_column_was_not_added()
      {
         sut.IsUsed(_column2.BaseGrid).ShouldBeFalse();
      }
   }

   public class When_retrieving_the_selected_columns_from_the_data_column_presenter : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2,});
         A.CallTo(() => _view.SelectedColumns).Returns(new[] {_allDataColumnDTOs[1]});
      }

      [Observation]
      public void should_return_the_column_selected_by_the_user()
      {
         sut.SelectedDataColumns.ShouldOnlyContain(_column2);
      }
   }

   public class When_the_data_browser_presenter_is_updating_the_used_state_for_the_selected_data_column : concern_for_DataBrowserPresenter
   {
      private IReadOnlyList<DataColumn> _usedStateChangedColumns;

      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2,});
         sut.UsedChanged += (o, e) => _usedStateChangedColumns = e.Columns;
         A.CallTo(() => _view.SelectedDescendantColumns).Returns(new []{_allDataColumnDTOs[1]});
      }

      protected override void Because()
      {
         sut.UpdateUsedStateForSelection(used: true);
      }

      [Observation]
      public void should_update_the_used_state_for_all_selected_columns_and_their_descendants()
      {
         _allDataColumnDTOs[0].Used.ShouldBeFalse();
         _allDataColumnDTOs[1].Used.ShouldBeTrue();
      }

      [Observation]
      public void should_notify_a_use_changed_event_with_the_columns_for_which_the_used_state_has_changed()
      {
         _usedStateChangedColumns.ShouldOnlyContain(_column2);
      }
   }

   public class When_the_data_browser_presenter_is_notified_that_the_column_selection_has_changed : concern_for_DataBrowserPresenter
   {
      private IReadOnlyList<DataColumn> _selectedColumns;

      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] {_column1, _column2,});
         sut.SelectionChanged += (o,e) => _selectedColumns = e.Columns;
         A.CallTo(() => _view.SelectedDescendantColumns).Returns(_allDataColumnDTOs);
      }

      protected override void Because()
      {
         sut.SelectedDataColumnsChanged();
      }

      [Observation]
      public void should_notify_a_data_selection_changed_with_all_selected_columns_and_their_descendants()
      {
         _selectedColumns.ShouldOnlyContain(_column1, _column2);
      }
   }

   public class When_the_data_browser_presenter_is_initializing_the_used_state_of_a_set_of_used_columns : concern_for_DataBrowserPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddDataColumns(new[] { _column1, _column2 });
         _allDataColumnDTOs[0].Used = true;
      }

      protected override void Because()
      {
         sut.InitializeIsUsedForDataColumns(new []{_column2, _column2.BaseGrid});
      }

      [Observation]
      public void should_only_select_the_used_columns_that_exist()
      {
         _allDataColumnDTOs[0].Used.ShouldBeFalse();
         _allDataColumnDTOs[1].Used.ShouldBeTrue();
      }
   }
}