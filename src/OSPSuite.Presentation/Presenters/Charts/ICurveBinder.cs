using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   /// Binding a Curve to a DevExpress series. Is responsible for series creation, and update
   /// </summary>
   public interface ICurveBinder : IDisposable
   {
      string Id { get; }
      Curve Curve { get; }
      double? LLOQ { get; }
      bool HasLLOQ { get; }
      IEnumerable<string> SeriesIds { get; }
      void Refresh();
      void ShowAllSeries();
      void ShowCurveInLegend(bool showInLegend);
      bool ContainsSeries(string seriesId);

      /// <summary>
      ///    Tests if the <paramref name="seriesId" /> represents the id of a series representing lower limit of quantification
      /// </summary>
      /// <returns>true if series represents lower limit of quantification, otherwise false</returns>
      bool IsSeriesLLOQ(string seriesId);

      int OriginalCurveIndexForRow(DataRow row);

      bool IsValidFor(DataMode dataMode, AxisTypes curveAxisType);
   }
}