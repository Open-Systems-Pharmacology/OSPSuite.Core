using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Starter.Tasks;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Views
{
   public partial class StaticCommandList : XtraForm
   {
      private readonly IHistoryBrowserPresenter _historyBrowserPresenter;
      private readonly IHistoryManager _historyManager;
      private readonly MyContext _myContext;

      public StaticCommandList()
      {
         InitializeComponent();
         _myContext = new MyContext();
         IoC.Container.Register<IHistoryManager, HistoryManager<MyContext>>();
         IoC.Container.Register<MyContext, MyContext>();

         var historyBrowserConfiguation = IoC.Resolve<IHistoryBrowserConfiguration>();
         var historyManagerRetriever = IoC.Resolve<IHistoryManagerRetriever>();
         historyBrowserConfiguation.AddDynamicColumn("p1", "A wonderful p1");
         historyBrowserConfiguation.AddDynamicColumn("p2", "A great p2");

         HistoryColumns.ColumnByName("p1").Position = 0;
         HistoryColumns.User.Position = 1;
         HistoryColumns.Description.Position = 2;
         HistoryColumns.ColumnByName("p2").Position = 3;
         HistoryColumns.Description.Position = 4;
         HistoryColumns.State.Position = 5;

         _historyManager = historyManagerRetriever.Current;

         _historyBrowserPresenter = IoC.Resolve<IHistoryBrowserPresenter>();
         _historyBrowserPresenter.Initialize();

         var control = _historyBrowserPresenter.View as Control;
         control.Dock = DockStyle.Fill;
         panelControl1.Controls.Add(control);

         _historyBrowserPresenter.HistoryManager = _historyManager;
         _historyBrowserPresenter.UpdateHistory();
      }

      private void simpleButton1_Click(object sender, EventArgs e)
      {
         var command1 = new ParameterValueSetCommand(new Parameter(), 1).Run(_myContext);
         command1.AddExtendedProperty("p1", "command1 p1");
         command1.AddExtendedProperty("p2", "command1 p2");
         var command2 = new MacroCommand<MyContext>().Add(command1, command1);
         command2.AddExtendedProperty("p1", "command2 p1");
         command2.AddExtendedProperty("p2", "command2 p2");
         var command3 = new ParameterValueSetCommand(new Parameter(), 3).Run(_myContext);
         command3.AddExtendedProperty("p1", "command3 p1");
         command3.AddExtendedProperty("p2", "command3 p2");
         var command4 = new ParameterValueSetCommand(new Parameter(), 4).Run(_myContext);
         command4.AddExtendedProperty("p1", "command4 p1");
         command4.AddExtendedProperty("p2", "command4 p2");
         var macroCommand = new MacroCommand<MyContext>().Add(command1, command2, command3);
         _historyManager.AddToHistory(macroCommand);
         _historyManager.AddToHistory(command4);
      }

      private void simpleButton2_Click(object sender, EventArgs e)
      {
         _historyManager.Undo();
      }
   }
}