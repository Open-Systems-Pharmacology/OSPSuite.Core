using System;
using System.ComponentModel;
using System.Drawing;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Chart
{
   public interface ICurve : INotifier, INotifying, IDataErrorInfo
   {
      string Id { get; }
      string Name { get; set; }
      string Description { get; set; }

      DataColumn xData { get; }
      DataColumn yData { get; }

      IDimension XDimension { get; set; }
      IDimension YDimension { get; set; }

      void SetxData(DataColumn column, IDimensionFactory dimensionFactory);
      void SetyData(DataColumn column, IDimensionFactory dimensionFactory);

      CurveOptions CurveOptions { get; }
      ErrorInfos<ICurve> ErrorInfos { get; }

      InterpolationModes InterpolationMode { get; set; }
      AxisTypes yAxisType { get; set; }
      bool Visible { get; set; }
      Color Color { get; set; }
      LineStyles LineStyle { get; set; }
      Symbols Symbol { get; set; }
      int LineThickness { get; set; }
      bool VisibleInLegend { get; set; }

      /// <summary>
      /// This value indicates relative place in the legend for this curve
      /// </summary>
      int? LegendIndex { get; set; }

      bool ShowLLOQ { get; set; }
   }

   public class Curve : Notifier, ICurve
   {
      private readonly CurveOptions _curveOptions;
      private string _id;
      private string _name;
      private DataColumn _xData;
      private DataColumn _yData;

      public Curve()
         : this(Guid.NewGuid().ToString())
      {
      }

      public Curve(string id)
      {
         _id = id;
         _name = string.Empty;
         _description = string.Empty;
         _xData = null;
         _yData = null;
         _curveOptions = new CurveOptions();
         _curveOptions.PropertyChanged += onCurveOptionsPropertyChanged;
         ErrorInfos = new ErrorInfos<ICurve>(this);
      }

      private void onCurveOptionsPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         RaisePropertyChanged(e.PropertyName);
      }

      //Returns an error description set for the item's property
      string IDataErrorInfo.this[string propertyName] => ErrorInfos[propertyName];

      //Returns an error description set for the current item
      string IDataErrorInfo.Error => ErrorInfos.Global;

      private IDimension _yDimension;
      private string _description;

      public string Id
      {
         get { return _id; }
         internal set
         {
            _id = value;
            OnPropertyChanged();
         }
      }

      public string Description
      {
         get { return _description; }
         set
         {
            _description = value;
            OnPropertyChanged();
         }
      }

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      public DataColumn xData
      {
         get { return _xData; }
         private set
         {
            _xData = value;
            OnPropertyChanged();
         }
      }

      public DataColumn yData
      {
         get { return _yData; }
         private set
         {
            _yData = value;
            OnPropertyChanged();
         }
      }

      private IDimension _xDimension;

      public IDimension XDimension
      {
         get { return _xDimension; }
         set
         {
            _xDimension = value;
            OnPropertyChanged();
         }
      }

      public IDimension YDimension
      {
         get { return _yDimension; }
         set
         {
            _yDimension = value;
            OnPropertyChanged();
         }
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

      public CurveOptions CurveOptions => _curveOptions;

      public InterpolationModes InterpolationMode
      {
         get { return _curveOptions.InterpolationMode; }
         set
         {
            _curveOptions.InterpolationMode = value;
            OnPropertyChanged();
         }
      }

      public AxisTypes yAxisType
      {
         get { return _curveOptions.yAxisType; }
         set
         {
            _curveOptions.yAxisType = value;
            OnPropertyChanged();
         }
      }

      public bool Visible
      {
         get { return _curveOptions.Visible; }
         set
         {
            _curveOptions.Visible = value;
            OnPropertyChanged();
         }
      }

      public Color Color
      {
         get { return _curveOptions.Color; }
         set
         {
            _curveOptions.Color = value;
            OnPropertyChanged();
         }
      }

      public LineStyles LineStyle
      {
         get { return _curveOptions.LineStyle; }
         set
         {
            _curveOptions.LineStyle = value;
            OnPropertyChanged();
         }
      }

      public Symbols Symbol
      {
         get { return _curveOptions.Symbol; }
         set
         {
            _curveOptions.Symbol = value;
            OnPropertyChanged();
         }
      }

      public int LineThickness
      {
         get { return _curveOptions.LineThickness; }
         set
         {
            _curveOptions.LineThickness = value;
            OnPropertyChanged();
         }
      }

      public bool VisibleInLegend
      {
         get { return _curveOptions.VisibleInLegend; }
         set
         {
            _curveOptions.VisibleInLegend = value;
            OnPropertyChanged();
         }
      }

      public int? LegendIndex
      {
         get { return _curveOptions.LegendIndex; }
         set
         {
            _curveOptions.LegendIndex = value;
            OnPropertyChanged();
         }
      }

      public bool ShowLLOQ
      {
         get { return _curveOptions.ShouldShowLLOQ; }
         set
         {
            _curveOptions.ShouldShowLLOQ = value;
            OnPropertyChanged();
         }
      }

      public ErrorInfos<ICurve> ErrorInfos { get; }

      public void DoRaisePropertyChanged(string propertyName)
      {
         RaisePropertyChanged(propertyName);
      }
   }
}