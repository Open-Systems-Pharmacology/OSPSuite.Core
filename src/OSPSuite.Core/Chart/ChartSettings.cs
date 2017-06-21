using System.Drawing;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public enum LegendPositions
   {
      None,
      Right,
      RightInside,
      Bottom,
      BottomInside
   }

   public class ChartSettings : Notifier, IUpdatable
   {
      private bool _sideMarginsEnabled;
      private LegendPositions _legendPosition;
      private Color _backColor;
      private Color _diagramBackColor;

      public ChartSettings()
      {
         SideMarginsEnabled = true;
         LegendPosition = LegendPositions.RightInside;
         BackColor = Color.Transparent;
         DiagramBackColor = Color.White;
      }

      public void UpdatePropertiesFrom(ChartSettings source)
      {
         UpdatePropertiesFrom(source, null);
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var chartSettings = source as ChartSettings;
         if (chartSettings == null) return;
         BackColor = chartSettings.BackColor;
         DiagramBackColor = chartSettings.DiagramBackColor;
         LegendPosition = chartSettings.LegendPosition;
         SideMarginsEnabled = chartSettings.SideMarginsEnabled;
      }

      public virtual bool SideMarginsEnabled
      {
         get => _sideMarginsEnabled;
         set => SetProperty(ref _sideMarginsEnabled, value);
      }

      public virtual LegendPositions LegendPosition
      {
         get => _legendPosition;
         set => SetProperty(ref _legendPosition, value);
      }

      public virtual Color BackColor
      {
         get => _backColor;
         set => SetProperty(ref _backColor, value);
      }

      public virtual Color DiagramBackColor
      {
         get => _diagramBackColor;
         set => SetProperty(ref _diagramBackColor, value);
      }
   }
}