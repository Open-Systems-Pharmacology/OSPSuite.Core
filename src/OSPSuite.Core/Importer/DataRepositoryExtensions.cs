using System;
using System.Linq;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Importer
{
   public static class DataRepositoryExtensions
   {
      /// <summary>
      ///    This methods renames all repository columns of all repositories to the name used in the source file.
      /// </summary>
      public static void RenameColumnsToSource(this DataRepository repository)
      {
         foreach (var column in repository.Columns.Where(column => column.DataInfo != null)
            .Where(column => !string.IsNullOrEmpty(column.DataInfo.Source)))
         {
            column.Name = column.DataInfo.Source.Trim();
            column.DataInfo.Source = column.Repository.Name + "." + column.DataInfo.Source; 
         }
      }

      /// <summary>
      ///    This methods renames all repository columns by removing unit information enclosed in brackets.
      /// </summary>
      public static void CutUnitFromColumnNames(this DataRepository repository)
      {
         foreach (var column in repository.Columns)
            column.Name = cutUnit(column.Name);
      }

      private static string cutUnit(string name)
      {
         if ((name.IndexOf('[') > 0 && name.IndexOf(']') > 0))
            return name.Substring(0, name.IndexOf('[')) + name.Substring(name.IndexOf(']') + 1).Trim();
         return name;
      }
   }
}