using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataNormalizer
   {
      void UpdateQuantityInfo(DataRepository observedData);
   }

   public class ObservedDataNormalizer : IObservedDataNormalizer
   {
      private const string PATH_DELIMITER_REPLACEMENT = "\\";

      public void UpdateQuantityInfo(DataRepository observedData)
      {
         var organ = metaDataValueFor(observedData, Constants.ObservedData.ORGAN);
         var compartment = metaDataValueFor(observedData, Constants.ObservedData.COMPARTMENT);
         var molecule = metaDataValueFor(observedData, Constants.ObservedData.MOLECULE);

         observedData.AllButBaseGrid().Each(column =>
         {
            //same depth as a simulation output path so that charts group and label observed data like simulation outputs
            var path = new[] {observedData.Name, Constants.ObservedData.CONTAINER_NAME, organ, compartment, molecule, pathSafeNameFor(column)};
            column.QuantityInfo = new QuantityInfo(path, QuantityType.Undefined);
         });

         var baseGrid = observedData.BaseGrid;
         if (baseGrid == null)
            return;

         baseGrid.QuantityInfo = new QuantityInfo(new[] {observedData.Name, pathSafeNameFor(baseGrid)}, QuantityType.Time);
      }

      private static string metaDataValueFor(DataRepository observedData, string propertyName) =>
         observedData.ExtendedPropertyValueFor(propertyName) ?? string.Empty;

      private static string pathSafeNameFor(DataColumn column) => column.Name.Replace(ObjectPath.PATH_DELIMITER, PATH_DELIMITER_REPLACEMENT);
   }
}
