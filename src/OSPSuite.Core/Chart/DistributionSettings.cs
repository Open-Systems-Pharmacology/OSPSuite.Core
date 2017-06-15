using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public class DistributionSettings : Notifier
   {
      public string XAxisTitle { get; set; }
      public string YAxisTitle { get; set; }
      public string PlotCaption { get; set; }

      private BarType _barType;

      public virtual BarType BarType
      {
         get => _barType;
         set
         {
            _barType = value;
            OnPropertyChanged(() => BarType);
         }
      }

      private AxisCountMode _axisCountMode;

      public virtual AxisCountMode AxisCountMode
      {
         get => _axisCountMode;
         set
         {
            _axisCountMode = value;
            OnPropertyChanged(() => AxisCountMode);
         }
      }

      public void Reset()
      {
         XAxisTitle = string.Empty;
         YAxisTitle = string.Empty;
         PlotCaption = string.Empty;
      }
   }
}