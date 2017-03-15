using System;
using System.Linq;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Extensions
{
   public static class DataRepositoryExtensions
   {
      public static DataColumn FirstDataColumn(this DataRepository dataRepository)
      {
         return dataRepository.AllButBaseGrid().First(x => x.DataInfo.Origin != ColumnOrigins.ObservationAuxiliary);
      }     
   }
}
