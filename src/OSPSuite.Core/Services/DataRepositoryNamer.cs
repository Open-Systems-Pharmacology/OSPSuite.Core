using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface IDataRepositoryNamer
   {
      void Rename(DataRepository dataRepository, string newName);
   }

   public class DataRepositoryNamer : IDataRepositoryNamer
   {
      public void Rename(DataRepository dataRepository, string newName)
      {
         dataRepository.Each(column => { replaceDataRepositoryNameInDataColumn(column, newName); });
         dataRepository.Name = newName;
      }

      private void replaceDataRepositoryNameInDataColumn(DataColumn column, string newName)
      {
         var quantityPath = column.QuantityInfo.Path.ToList();
         replaceDataRepositoryNameIn(quantityPath, column.Repository.Name, newName);
         column.QuantityInfo.Path = quantityPath;
      }

      private void replaceDataRepositoryNameIn(List<string> quantityPath, string oldName, string newName)
      {
         for (var i = 0; i < quantityPath.Count; i++)
         {
            if (string.Equals(quantityPath[i], oldName))
               quantityPath[i] = newName;
         }
      }
   }
}