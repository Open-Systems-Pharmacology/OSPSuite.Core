using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public class UsedChangedEventArgs : EventArgs
   {
      public bool Used { get; }
      public IEnumerable<DataColumn> DataColumns { get; }

      public UsedChangedEventArgs(IEnumerable<DataColumn> dataColumns, bool used)
      {
         DataColumns = dataColumns;
         Used = used;
      }
   }
}