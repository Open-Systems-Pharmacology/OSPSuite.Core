using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IPivotGridTestPresenter : IPresenter<IPivotGridTestView>
   {
      void Edit();
   }

   public class PivotGridTestPresenter : AbstractPresenter<IPivotGridTestView, IPivotGridTestPresenter>, IPivotGridTestPresenter
   {
      public PivotGridTestPresenter(IPivotGridTestView view) : base(view)
      {
         Edit();
      }

      public void Edit()
      {
         var calculationMethods = createCalculationMethods();
         var compounds = createCompounds();

         var dataTable = createDataTable(compounds, calculationMethods);

         _view.BindTo(dataTable);
      }

      private DataTable createDataTable(IEnumerable<string> compounds, IEnumerable<CalculationMethod> calculationMethods)
      {
         var dataTable = new DataTable();;

         dataTable.AddColumns<string>("Compound", "Category", "Calculation Method");
         dataTable.AddColumn<bool>("Value");
         compounds.Each(compound => addRows(compound, calculationMethods, dataTable));


         return dataTable;
      }

      private void addRows(string compound, IEnumerable<CalculationMethod> calculationMethods, DataTable dataTable)
      {
         calculationMethods.Each(method => addRow(compound, method, dataTable));
      }

      private void addRow(string compound, CalculationMethod method, DataTable dataTable)
      {
         var row = dataTable.NewRow();
         row["Compound"] = compound;
         row["Category"] = method.Category;
         row["Calculation Method"] = method.Name;
         row["Value"] = Convert.ToBoolean(0);

         dataTable.Rows.Add(row);
      }

      private IEnumerable<string> createCompounds()
      {
         yield return "Compound 1";
         yield return "Compound 2";
         yield return "Compound 3";
         yield return "Compound 4";
         yield return "Compound 5";
         yield return "Compound 6";
         yield return "Compound 7";
      }

      private IEnumerable<CalculationMethod> createCalculationMethods()
      {
         yield return new CalculationMethod {Category = "category 1", Name = "Calculation Method 1"};
         yield return new CalculationMethod {Category = "category 1", Name = "Calculation Method 2" };
         yield return new CalculationMethod {Category = "category 2", Name = "Calculation Method 3" };
         yield return new CalculationMethod {Category = "category 2", Name = "Calculation Method 4" };
      }
   }
}