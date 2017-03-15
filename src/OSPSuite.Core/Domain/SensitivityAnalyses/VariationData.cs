using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class VariationData : IWithName
   {
      public string Name { get; set; }
      public IReadOnlyList<double> DefaultValues { get; set; }

      public int DefaultVariationId => _variationValues.Count;

      /// <summary>
      /// Number of total variations (parameter variations + default variation)
      /// </summary>
      //+1 because of default variation
      public int NumberOfVariations => _variationValues.Count + 1;

      public IReadOnlyList<string> ParameterPaths { get; set; }

      private readonly List<ParameterVariation> _variationValues = new List<ParameterVariation>();

      public virtual DataTable ToDataTable()
      {
         var dataTable = new DataTable(Name);
         addColumnsToTable(dataTable);
         addVariationValuesToTable(dataTable);
         addDefaultValuesToTable(dataTable);

         return dataTable;
      }

      private void addColumnsToTable(DataTable dataTable)
      {
         ParameterPaths.Each(p => dataTable.AddColumn<double>(p));
         dataTable.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
      }

      private void addDefaultValuesToTable(DataTable dataTable)
      {
         addRowToTable(DefaultValues, dataTable, DefaultVariationId);
      }

      private void addVariationValuesToTable(DataTable dataTable)
      {
         foreach (var parameterVariation in _variationValues)
         {
            addRowToTable(parameterVariation.Variation, dataTable, parameterVariation.VariationId);
         }
      }

      private void addRowToTable(IReadOnlyList<double> values, DataTable dataTable, int variationId)
      {
         var row = dataTable.NewRow();
         values.Each((value, i) => { row[i] = value; });

         row[Constants.Population.INDIVIDUAL_ID_COLUMN] = variationId;
         dataTable.Rows.Add(row);
      }

      public void AddVariationValues(string parameterName, IReadOnlyList<IReadOnlyList<double>> variations)
      {
         variations.Each(v =>
         {
            var parameterVariation = new ParameterVariation
            {
               ParameterName = parameterName,
               Variation = v,
               VariationId = _variationValues.Count
            };

            AddVariation(parameterVariation);
         });
      }

      public void AddVariation(ParameterVariation parameterVariation)
      {
         _variationValues.Add(parameterVariation);
      }

      public virtual IReadOnlyList<ParameterVariation> VariationsFor(string parameterName) => _variationValues.Where(x => string.Equals(x.ParameterName, parameterName)).ToList();
   }
}