using OSPSuite.Infrastructure.Import.Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace OSPSuite.Infrastructure.Import.Extensions
{
   public static class ColumnInfoExtensions
   {
      public static bool IsBase(this ColumnInfo info)
      {
         return string.IsNullOrEmpty(info.BaseGridName) || info.BaseGridName == info.Name;
      }

      public static bool IsAuxiliary(this ColumnInfo info)
      {
         return !info.IsMandatory;
      }
   }
}
