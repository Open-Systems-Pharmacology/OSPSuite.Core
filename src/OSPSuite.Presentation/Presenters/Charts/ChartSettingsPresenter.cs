using System;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartSettingsPresenter : IPresenter<IChartSettingsView>
   {
      void Edit(IChart chart);

      /// <summary>
      ///    specifies whether the <c>Name</c> property of the settings can be edited or not. If not, the name field will be
      ///    hidden.
      ///    Default is <c>true</c>
      /// </summary>
      bool NameVisible { get; set; }

      void Edit(CurveChartTemplate chartTemplate);

      void Clear();

      event EventHandler ChartSettingsChanged;

      void NotifyChartSettingsChanged();

      /// <summary>
      ///    Returns the display string shown in the legend position selection, indicating the vertical and horizontal
      ///    alignment as well as whether the legend is placed inside or outside the diagram.
      /// </summary>
      string LegendPositionDisplayFor(LegendPositions legendPosition);
   }

   internal class ChartSettingsPresenter : AbstractPresenter<IChartSettingsView, IChartSettingsPresenter>, IChartSettingsPresenter
   {
      public event EventHandler ChartSettingsChanged = delegate { };

      public void NotifyChartSettingsChanged() => ChartSettingsChanged(this, EventArgs.Empty);

      public ChartSettingsPresenter(IChartSettingsView view) : base(view)
      {
      }

      public bool NameVisible
      {
         get => _view.NameVisible;
         set => _view.NameVisible = value;
      }

      public void Edit(CurveChartTemplate chartTemplate)
      {
         _view.BindTo(chartTemplate);
      }

      public void Clear()
      {
         _view.DeleteBinding();
      }

      public void Edit(IChart chart)
      {
         _view.BindTo(chart);
      }

      public string LegendPositionDisplayFor(LegendPositions legendPosition)
      {
         switch (legendPosition)
         {
            case LegendPositions.Right:
               return legendPositionDisplay(Captions.Top, Captions.Right, isInside: false);
            case LegendPositions.RightInside:
               return legendPositionDisplay(Captions.Top, Captions.Right, isInside: true);
            case LegendPositions.Bottom:
               return legendPositionDisplay(Captions.Bottom, Captions.Right, isInside: false);
            case LegendPositions.BottomInside:
               return legendPositionDisplay(Captions.Bottom, Captions.Right, isInside: true);
            case LegendPositions.TopLeftOutside:
               return legendPositionDisplay(Captions.Top, Captions.Left, isInside: false);
            case LegendPositions.TopLeftInside:
               return legendPositionDisplay(Captions.Top, Captions.Left, isInside: true);
            case LegendPositions.BottomLeftOutside:
               return legendPositionDisplay(Captions.Bottom, Captions.Left, isInside: false);
            case LegendPositions.BottomLeftInside:
               return legendPositionDisplay(Captions.Bottom, Captions.Left, isInside: true);
            default:
               return legendPosition.ToString();
         }
      }

      private static string legendPositionDisplay(string vertical, string horizontal, bool isInside)
      {
         return $"{vertical} {horizontal} ({(isInside ? Captions.Inside : Captions.Outside)})";
      }
   }
}