using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IParametersReportCreator
   {
      /// <summary>
      /// Export the parameters to a csv file having the following structure: ParameterPath, Value, FormulaString, RHSFormulaString
      /// </summary>
      /// <param name="parametersToExport">Parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IEnumerable<IParameter> parametersToExport, string csvFile);

      /// <summary>
      /// Export all parameters defined in the container using container.GetAllChildren
      /// </summary>
      /// <param name="container">Container containing the parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IContainer container, string csvFile);


      /// <summary>
      /// Export all parameters defined in the model using model.root
      /// </summary>
      /// <param name="model">Model containing the parameters to export</param>
      /// <param name="csvFile">Full path of csv file where the parameter will be exported</param>
      void ExportParametersTo(IModel model, string csvFile);

      /// <summary>
      /// Export the parameters to a datatable
      /// </summary>
      /// <param name="parametersToExport">Parameters to export</param>
      /// <returns>A datatable that should have 4 columns  ParameterPath, Value, FormulaString, RHSFormulaString</returns>
      DataTable ExportParametersToTable(IEnumerable<IParameter> parametersToExport);
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
         {
            //Remove ; that would interfere with csv export
            return formula.DowncastTo<ExplicitFormula>()
                          .FormulaString
                          .Replace(";", ",");
         }
       
         return string.Empty;
      }

      public void ExportParametersTo(IContainer container, string csvFile)
      {
         ExportParametersTo(container.GetAllChildren<IParameter>(),csvFile);
      }

      public void ExportParametersTo(IModel model, string csvFile)
      {
         ExportParametersTo(model.Root, csvFile);
      }

      public DataTable ExportParametersToTable(IEnumerable<IParameter> parametersToExport)
      {
         var dataTable = new DataTable();
         var colPath = dataTable.Columns.Add(Constants.ParameterExport.PARAMETER_PATH, typeof(string));
         var colValue = dataTable.Columns.Add(Constants.ParameterExport.VALUE, typeof(double));
         var colFormula = dataTable.Columns.Add(Constants.ParameterExport.FORMULA, typeof(string));
         var colRHSFormula = dataTable.Columns.Add(Constants.ParameterExport.RHS_FORMULA, typeof(string));

         foreach (var parameter in parametersToExport)
         {
            DataRow row = dataTable.NewRow();
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

            dataTable.Rows.Add(row);
         }

         return dataTable;
      }
   }
}