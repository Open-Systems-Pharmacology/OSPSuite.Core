using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Chart;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICurveAdapter : IDisposable
   {
      string Id { get;  }
      int? LegendIndex { get; }
      ICurve Curve { get;}
      double? LLOQ { get; }
      bool HasLLOQ { get; }
      IEnumerable<string> SeriesIds { get; }
      void Refresh();
      void RefreshLegendText();
      void AttachAxisAdapters(ICache<AxisTypes, IAxisAdapter> axisAdapters);
      void ShowAllSeries();
      void ShowCurveInLegend(bool visibleInLegend);
      bool ContainsSeries(string id);

      /// <summary>
      /// Tests if the <paramref name="seriesId"/> represents the id of a series representing lower limit of quantification
      /// </summary>
      /// <returns>true if series represents lower limit of quantification, otherwise false</returns>
      bool IsSeriesLLOQ(string seriesId);

      int OriginalCurveIndexForRow(DataRow row);
   }
}