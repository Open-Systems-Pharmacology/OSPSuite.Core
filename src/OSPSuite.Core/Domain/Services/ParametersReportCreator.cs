using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IParametersReportCreator
   {
      /// <summary>
      ///    Export the parameters to a csv file having the following structure: ParameterPath, Value, FormulaString,
      ///    RHSFormulaString
      /// </summary>
      /// <param name="parametersToExport">Parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IEnumerable<IParameter> parametersToExport, string csvFile);

      /// <summary>
      ///    Export all parameters defined in the container using container.GetAllChildren
      /// </summary>
      /// <param name="container">Container containing the parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IContainer container, string csvFile);

      /// <summary>
      ///    Export all parameters defined in the model using model.root
      /// </summary>
      /// <param name="model">Model containing the parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IModel model, string csvFile);

      /// <summary>
      ///    Export the parameters to a datatable
      /// </summary>
      /// <param name="parametersToExport">Parameters to export</param>
      /// <param name="tableConfigurationAction">Allows the calller to add more columns to the table. Called once</param>
      /// <param name="rowConfigurationAction">
      ///    Allows the caller to set the value for added table. This will be called for each
      ///    parameter
      /// </param>
      /// <returns>A datatable that should have 4 columns  ParameterPath, Value, FormulaString, RHSFormulaString</returns>
      DataTable ExportParametersToTable(IEnumerable<IParameter> parametersToExport, Action<DataTable> tableConfigurationAction = null, Action<IParameter, DataRow> rowConfigurationAction = null);
   }

   public class ParametersReportCreator : IParametersReportCreator
   {
      private readonly IObjectPathFactory _objectPathFactory;

      public ParametersReportCreator(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public void ExportParametersTo(IEnumerable<IParameter> parametersToExport, string csvFile)
      {
         var dataTable = ExportParametersToTable(parametersToExport);
         dataTable.ExportToCSV(csvFile);
      }

      private static string formulaStringFrom(IFormula formula)
      {
         if (formula != null && formula.IsExplicit())
            return formula.DowncastTo<ExplicitFormula>().FormulaString;

         return string.Empty;
      }

      public void ExportParametersTo(IContainer container, string csvFile)
      {
         ExportParametersTo(container.GetAllChildren<IParameter>(), csvFile);
      }

      public void ExportParametersTo(IModel model, string csvFile)
      {
         ExportParametersTo(model.Root, csvFile);
      }

      public DataTable ExportParametersToTable(IEnumerable<IParameter> parametersToExport, Action<DataTable> tableConfigurationAction = null, Action<IParameter, DataRow> rowConfigurationAction = null)
      {
         var dataTable = new DataTable();
         var colPath = dataTable.AddColumn(Constants.ParameterExport.PARAMETER_PATH);
         var colValue = dataTable.AddColumn<double>(Constants.ParameterExport.VALUE);
         var colFormula = dataTable.AddColumn(Constants.ParameterExport.FORMULA);
         var colRHSFormula = dataTable.AddColumn(Constants.ParameterExport.RHS_FORMULA);

         tableConfigurationAction?.Invoke(dataTable);

         foreach (var parameter in parametersToExport)
         {
            var row = dataTable.NewRow();
            row[colPath] = _objectPathFactory.CreateAbsoluteObjectPath(parameter);

            try
            {
               row[colValue] = parameter.Value;
            }
            catch (Exception)
            {
               row[colValue] = double.NaN;
            }

            row[colFormula] = formulaStringFrom(parameter.Formula);
            row[colRHSFormula] = formulaStringFrom(parameter.RHSFormula);

            rowConfigurationAction?.Invoke(parameter, row);

            dataTable.Rows.Add(row);
         }

         return dataTable;
      }
   }
}