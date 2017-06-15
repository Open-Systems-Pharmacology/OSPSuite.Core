using System.Drawing;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

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
         _sideMarginsEnabled = true;
         _legendPosition = LegendPositions.RightInside;
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
         set
         {
            _sideMarginsEnabled = value;
            OnPropertyChanged(() => SideMarginsEnabled);
         }
      }

      public virtual LegendPositions LegendPosition
      {
         get => _legendPosition;
         set
         {
            _legendPosition = value;
            OnPropertyChanged(() => LegendPosition);
         }
      }

      public virtual Color BackColor
      {
         get => _backColor;
         set
         {
            _backColor = value;
            OnPropertyChanged(() => BackColor);
         }
      }

      public virtual Color DiagramBackColor
      {
         get => _diagramBackColor;
         set
         {
            _diagramBackColor = value;
            OnPropertyChanged(() => DiagramBackColor);
         }
      }
   }
}