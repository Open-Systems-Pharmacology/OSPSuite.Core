using System;
using OSPSuite.Infrastructure.ORM.MetaData;

namespace OSPSuite.Infrastructure.ORM.History
{
   public class HistoryItemMetaData : MetaData<string>
   {
      public virtual string User { get; set; }
      public virtual DateTime DateTime { get; set; }
      public virtual int State { get; set; }
      public virtual int Sequence { get; set; }
      public virtual CommandMetaData Command { get; set; }
   }
}