using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Converter.v5_4
{
   public interface IDataRepositoryConverter
   {
      void Convert(DataRepository dataRepository);
   }

   public class DataRepositoryConverter : IDataRepositoryConverter
   {
      public void Convert(DataRepository dataRepository)
      {
         var repoExtendedProperties = dataRepository.ExtendedProperties;

         foreach (var column in dataRepository.AllButBaseGrid())
         {
            merge(repoExtendedProperties, column.DataInfo.ExtendedProperties);
         }
      }

      private void merge(ExtendedProperties repoExtendedProperties, ExtendedProperties columnExtendedProperties)
      {
         foreach (var extendedProperty in columnExtendedProperties.ToList())
         {
            if (repoExtendedProperties.Contains(extendedProperty.Name))
               continue;

            repoExtendedProperties.Add(extendedProperty);
            columnExtendedProperties.Remove(extendedProperty.Name);
         }
      }
   }
}