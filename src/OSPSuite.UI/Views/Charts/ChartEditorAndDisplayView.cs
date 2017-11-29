using System.IO;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartEditorAndDisplayView : BaseUserControl, IChartEditorAndDisplayView
   {
      public ChartEditorAndDisplayView()
      {
         InitializeComponent();

         _dockManager.DockingOptions.ShowCaptionImage = true;
         _dockManager.DockingOptions.HideImmediatelyOnAutoHide = true;
         _dockManager.AutoHideSpeed = 100;
      }

      public string SaveLayoutToString()
      {
         var streamMainView = new MemoryStream();
         _dockManager.SaveLayoutToStream(streamMainView);
         return streamToString(streamMainView);
      }

      public void LoadLayoutFromString(string layoutString)
      {
         if (!string.IsNullOrEmpty(layoutString))
            _dockManager.RestoreLayoutFromStream(streamFromString(layoutString));
      }

      public void AddEditor(IView view)
      {
         _contChartEditor.FillWith(view);
      }

      public void AddDisplay(IView view)
      {
         _pnlChartDisplay.FillWith(view);
      }

      private MemoryStream streamFromString(string stringToConvert)
      {
         return new MemoryStream(stringToConvert.ToByteArray());
      }

      private string streamToString(MemoryStream streamToConvert)
      {
         return streamToConvert.ToArray().ToByteString();
      }

      public void AttachPresenter(IChartEditorAndDisplayPresenter presenter)
      {
         
      }
   }
}