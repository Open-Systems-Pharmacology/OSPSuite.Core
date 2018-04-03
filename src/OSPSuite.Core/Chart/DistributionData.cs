using System.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class DistributionData<T>
   {
      public bool IsRangeSpecified { get; set; }

      public string GroupingName => DataTable.Columns[0].ColumnName;

      public string XAxisName => DataTable.Columns[1].ColumnName;

      public string YAxisName => DataTable.Columns[2].ColumnName;

      public DataTable DataTable { get; }

      protected DistributionData(AxisCountMode axisCountMode)
      {
         DataTable = new DataTable("Histogram");
         DataTable.AddColumn("Grouping");
         DataTable.AddColumn<T>("Value");
         DataTable.AddColumn("Count", axisCountMode == AxisCountMode.Count ? typeof (int) : typeof (double));
      }

      public void AddData(T value, double count, string grouping = "NONE")
      {
         var row = DataTable.NewRow();
         row[0] = grouping;
         row[1] = value;
         row[2] = count;
         DataTable.Rows.Add(row);
      }
   }

   public class ContinuousDistributionData : DistributionData<double>
   {
      private readonly int _numberOfIntervals;
      private const int DEFAULT_CONSTANT_INTERVAL_WIDTH_RATIO = 10;
      private const double DEFAULT_CONSTANT_ZERO_INTERVAL_WIDTH = 0.1;

      /// <summary>
      ///    Minimum value in the distributed data
      /// </summary>
      public double XMinData { get; set; }

      /// <summary>
      ///    Maximum value in the distributed data
      /// </summary>
      public double XMaxData { get; set; }

      public ContinuousDistributionData(AxisCountMode axisCountMode, int numberOfIntervals) : base(axisCountMode)
      {
         _numberOfIntervals = numberOfIntervals;
         IsRangeSpecified = true;
         XMinData = double.NaN;
         XMaxData = double.NaN;
      }

      public double BarWidth
      {
         get
         {
            var width = (XMaxData - XMinData) / _numberOfIntervals;

            if (width == 0)
               width = XMaxData / DEFAULT_CONSTANT_INTERVAL_WIDTH_RATIO;

            //Min and max are equal to 0
            if (width == 0)
               width = DEFAULT_CONSTANT_ZERO_INTERVAL_WIDTH;

            return width;
         }
      }

      /// <summary>
      ///    <see cref="XMaxData" /> augmented of <see cref="BarWidth" />. Useful for maximum value for plot
      /// </summary>
      public virtual double XMaxValue => XMaxData + BarWidth;

      /// <summary>
      ///    <see cref="XMinData" /> reduced by <see cref="BarWidth" />. Useful for minimum value for plot
      /// </summary>
      public virtual double XMinValue => XMinData - BarWidth;
   }

   public class DiscreteDistributionData : DistributionData<string>
   {
      public DiscreteDistributionData(AxisCountMode axisCountMode) : base(axisCountMode)
      {
         IsRangeSpecified = false;
      }
   }
}