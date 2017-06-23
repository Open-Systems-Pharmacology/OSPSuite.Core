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

      public UsedColumnsEventArgs(IReadOnlyList<DataColumn> columns, bool used) : base(columns)
      {
         Used = used;
      }
   }
}