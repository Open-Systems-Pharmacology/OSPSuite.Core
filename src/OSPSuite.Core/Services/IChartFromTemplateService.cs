using System;
using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
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
      ///    selected exceed the <paramref name="warningThreshold" />. This can happen
      ///    when a container contains too many quantities with the same type. <paramref name="curveNameDefinition" /> specifies
      ///    how the default column name should be created. Default returns name of column if not specified.  If the flag <paramref name="propogateChartChangeEvent"/>
      ///   is set to <c>true</c> (default), the chartChange event will be propagated 
      /// </summary>
      /// <remarks>
      ///    We first try to find the exact match using <c>Quantity Path</c> and <c>QuantityType</c>.
      ///    If the match is not successful, with use the KeyMapper implemetnation to match similar curves
      /// </remarks>
      void InitializeChartFromTemplate(CurveChart chart, IEnumerable<DataColumn> dataColumns, CurveChartTemplate template, 
         Func<DataColumn, string> curveNameDefinition = null, 
         bool warnIfNumberOfCurvesAboveThreshold = false,
         bool propogateChartChangeEvent = true,
         int warningThreshold = Constants.DEFAULT_TEMPLATE_WARNING_THRESHOLD);
   }
}