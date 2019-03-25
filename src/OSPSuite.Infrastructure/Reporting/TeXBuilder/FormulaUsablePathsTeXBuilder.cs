using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Reporting.TeXBuilder
{
   internal class FormulaUsablePathsTeXBuilder : TeXChunkBuilder<IEnumerable<IFormulaUsablePath>>
   {
      private const string PATH = "Path";
      private const string ALIAS = "Alias";
      private const string REFERENCES = "References";
      private const string DIMENSION = "Dimension";

      private readonly ITeXBuilderRepository _builderRepository;

      public FormulaUsablePathsTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEnumerable<IFormulaUsablePath> formulaUsablePaths, BuildTracker buildTracker)
      {
         _builderRepository.Report(tableFor(formulaUsablePaths), buildTracker);
      }

      public override string TeXChunk(IEnumerable<IFormulaUsablePath> formulaUsablePaths)
      {
         return _builderRepository.ChunkFor(tableFor(formulaUsablePaths));
      }

      private static SimpleTable tableFor(IEnumerable<IFormulaUsablePath> formulaUsablePaths)
      {
         var dataTable = new DataTable(REFERENCES);
         dataTable.AddColumn(ALIAS);
         dataTable.AddColumn(PATH).SetAlignment(TableWriter.ColumnAlignments.l);
         dataTable.AddColumn(DIMENSION);

         dataTable.BeginLoadData();
         foreach (var path in formulaUsablePaths)
         {
            var newRow = dataTable.NewRow();
            newRow[ALIAS] = path.Alias;
            newRow[PATH] = path.ToString();
            newRow[DIMENSION] = path.Dimension.DisplayName;
            dataTable.Rows.Add(newRow);
         }

         dataTable.EndLoadData();
         return new SimpleTable(dataTable.DefaultView);
      }
   }
}