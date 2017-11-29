using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Extensions
{
   public static class CurveChartExtensions
   {
      public static void AddCurvesFor(this CurveChart curveChart, DataRepository dataRepository, Func<DataColumn, string> columnNameRetriever, IDimensionFactory dimensionFactory, Action<DataColumn, Curve> action = null)
      {
         AddCurvesFor(curveChart, dataRepository.AllButBaseGrid(), columnNameRetriever, dimensionFactory, action);
      }

      public static void AddCurvesFor(this CurveChart curveChart, IEnumerable<DataColumn> columns, Func<DataColumn, string> columnNameRetriever, IDimensionFactory dimensionFactory, Action<DataColumn, Curve> action = null)
      {
         foreach (var column in columns)
         {
            var curve = curveChart.FindCurveWithSameData(column.BaseGrid, column);
            if (curve != null)
               continue;

            curve = curveChart.CreateCurve(column.BaseGrid, column, columnNameRetriever(column), dimensionFactory);
            curve.Color = Color.Black;
            curve.UpdateStyleForObservedData();

            curveChart.AddCurve(curve);

            action?.Invoke(column, curve);
         }
      }
   }
}