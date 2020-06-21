using System;
using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_HistoryManagerExtensions : StaticContextSpecification
   {
   }

   public class When_an_history_to_datatable : concern_for_HistoryManagerExtensions
   {
      private IHistoryManager _historyManager;
      private DataTable _dataTable;
      private List<IHistoryItem> _allHistory;

      protected override void Context()
      {
         base.Context();
         _historyManager = A.Fake<IHistoryManager>();
         _allHistory = new List<IHistoryItem>();
         _allHistory.Add(new HistoryItem("TOTO", DateTime.Now, new MySimpleCommand {CommandType = "EDIT", Comment = "toto", Description = "tralala", Visible = true}));
         var commandWithExtendedProperties = new HistoryItem("TATA", DateTime.Now, new MySimpleCommand {CommandType = "EDIT", Comment = "toto", Description = "tralala", Visible = true});
         commandWithExtendedProperties.Command.AddExtendedProperty("BB", "Simulation");
         commandWithExtendedProperties.Command.AddExtendedProperty("BBType", "Toto");
         _allHistory.Add(commandWithExtendedProperties);
         _allHistory.Add(new HistoryItem("ZTMSE", DateTime.Now, new MySimpleCommand {CommandType = "EDIT", Comment = "toto", Description = "tralala", Visible = true}));
         A.CallTo(() => _historyManager.History).Returns(_allHistory);
      }

      protected override void Because()
      {
         _dataTable = _historyManager.ToDataTable();
      }

      [Observation]
      public void should_create_a_valid_datatable_containing_a_column_for_all_extended_columns()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(HistoryManagerExtensions.ReportColumn.LastColumnIndex + 1 + 2);
      }
   }
}