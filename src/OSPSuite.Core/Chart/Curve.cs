using System.Drawing;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Chart
{
   public class Curve : Notifier, IValidatable
   {
      private string _name;
      private IDimension _xDimension;
      private IDimension _yDimension;
      private string _description;
      public CurveOptions CurveOptions { get; }
      private DataColumn _xData;
      private DataColumn _yData;
      public IBusinessRuleSet Rules { get; }
      public string Id { get; }

      public Curve()
      {
         Id = ShortGuid.NewGuid();
         _name = string.Empty;
         _description = string.Empty;
         _xData = null;
         _yData = null;
         CurveOptions = new CurveOptions();
         Rules = new BusinessRuleSet();
      }

      public void SetxData(DataColumn column, IDimensionFactory dimensionFactory)
      {
         xData = column;
         XDimension = dimensionFactory.GetMergedDimensionFor(xData);
      }

      public void SetyData(DataColumn column, IDimensionFactory dimensionFactory)
      {
         yData = column;
         if (yData.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
            YDimension = dimensionFactory.GetMergedDimensionFor(yData.GetRelatedColumn(AuxiliaryType.GeometricMeanPop));
         else if (yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
            YDimension = dimensionFactory.GetMergedDimensionFor(yData.GetRelatedColumn(AuxiliaryType.ArithmeticMeanPop));
         else
            YDimension = dimensionFactory.GetMergedDimensionFor(yData);
      }

      public Curve Clone()
      {
         var clone = new Curve();
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(Curve curve)
      {
         Name = curve.Name;
         xData = curve.xData;
         yData = curve.yData;
         XDimension = curve.XDimension;
         YDimension = curve.YDimension;
         Description = curve.Description;
         CurveOptions.UpdateFrom(curve.CurveOptions);
      }

      public string Description
      {
         get => _description;
         set => SetProperty(ref _description, value);
      }

      public string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
      }

      public virtual DataColumn xData
      {
         get => _xData;
         set => SetProperty(ref _xData, value);
      }

      public virtual DataColumn yData
      {
         get => _yData;
         set => SetProperty(ref _yData, value);
      }

      public virtual IDimension XDimension
      {
         get => _xDimension;
         set => SetProperty(ref _xDimension, value);
      }

      public virtual IDimension YDimension
      {
         get => _yDimension;
         set => SetProperty(ref _yDimension, value);
      }

      public InterpolationModes InterpolationMode
      {
         get => CurveOptions.InterpolationMode;
         set
         {
            CurveOptions.InterpolationMode = value;
            OnPropertyChanged();
         }
      }

      public AxisTypes yAxisType
      {
         get => CurveOptions.yAxisType;
         set
         {
            CurveOptions.yAxisType = value;
            OnPropertyChanged();
         }
      }

      public bool Visible
      {
         get => CurveOptions.Visible;
         set
         {
            CurveOptions.Visible = value;
            OnPropertyChanged();
         }
      }

      public Color Color
      {
         get => CurveOptions.Color;
         set
         {
            CurveOptions.Color = value;
            OnPropertyChanged();
         }
      }

      public LineStyles LineStyle
      {
         get => CurveOptions.LineStyle;
         set
         {
            CurveOptions.LineStyle = value;
            OnPropertyChanged();
         }
      }

      public Symbols Symbol
      {
         get => CurveOptions.Symbol;
         set
         {
            CurveOptions.Symbol = value;
            OnPropertyChanged();
         }
      }

      public int LineThickness
      {
         get => CurveOptions.LineThickness;
         set
         {
            CurveOptions.LineThickness = value;
            OnPropertyChanged();
         }
      }

      public bool VisibleInLegend
      {
         get => CurveOptions.VisibleInLegend;
         set
         {
            CurveOptions.VisibleInLegend = value;
            OnPropertyChanged();
         }
      }

      public int? LegendIndex
      {
         get => CurveOptions.LegendIndex;
         set
         {
            CurveOptions.LegendIndex = value;
            OnPropertyChanged();
         }
      }

      public bool ShowLLOQ
      {
         get => CurveOptions.ShouldShowLLOQ;
         set
         {
            CurveOptions.ShouldShowLLOQ = value;
            OnPropertyChanged();
         }
      }

      public bool IsReallyVisible => CurveOptions.IsReallyVisible;
   }
}