using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public class ColumnsEventArgs : EventArgs
   {
      public IReadOnlyList<DataColumn> Columns { get; }

      public ColumnsEventArgs(IReadOnlyList<DataColumn> columns) => Columns = columns;
   }

   public class UsedColumnsEventArgs : ColumnsEventArgs
   {
      public bool Used { get; }

      /// <summary>
      ///    Full path of the simulation output the columns are linked to so that the same color can be applied,
      ///    or null when the columns are not linked to an output
      /// </summary>
      public string LinkedOutputPath { get; }

      public UsedColumnsEventArgs(IReadOnlyList<DataColumn> columns, bool used, string linkedOutputPath = null) : base(columns)
      {
         Used = used;
         LinkedOutputPath = linkedOutputPath;
      }
   }
}