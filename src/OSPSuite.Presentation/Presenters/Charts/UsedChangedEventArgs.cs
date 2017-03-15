using System;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public class UsedChangedEventArgs : EventArgs
   {
      public UsedChangedEventArgs(IEnumerable<string> columnIds, bool used)
      {
         ColumnIds = columnIds;
         Used = used;
      }

      public bool Used { get; private set; }
      public IEnumerable<string> ColumnIds { get; private set; }
   }
}