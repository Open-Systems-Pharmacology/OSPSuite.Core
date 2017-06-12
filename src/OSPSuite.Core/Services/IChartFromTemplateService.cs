using System;
using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Services
{
   public interface IChartFromTemplateService
   {
      /// <summary>
      ///    Initializes the given <paramref name="chart" /> based on the <paramref name="template" />. if the flag
      ///    <paramref name="warnIfNumberOfCurvesAboveThreshold" />
      ///    is set to <c>true</c> (default is false), the user will be asked if he really wants to update the chart from
      ///    template when the number of curves that would be
      ///    selected exceed the <see cref="WarningThreshold" />
      /// </summary>
      /// <remarks>
      ///    We first try to find the exact match using <c>Quantity Path</c> and <c>QuantityType</c>.
      ///    If the match is not successful, with use the KeyMapper implemetnation to match similar curves
      /// </remarks>
      void InitializeChartFromTemplate(CurveChart chart, IEnumerable<DataColumn> dataColumns, CurveChartTemplate template, bool warnIfNumberOfCurvesAboveThreshold=false);

      /// <summary>
      ///    This methods allows the caller to specify how the default column name should be created. By default, the name of the
      ///    column is returned
      /// </summary>
      Func<DataColumn, string> CurveNameDefinition { get; set; }

      /// <summary>
      ///    The threshold is used to notify the user when too many curves are being mapped for one give
      ///    <see cref="CurveTemplate" />. This can happen
      ///    when a container contains too many quantities with the same type.
      /// </summary>
      int WarningThreshold { get; set; }
   }
}