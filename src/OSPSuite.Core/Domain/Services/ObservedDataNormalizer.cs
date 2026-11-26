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
         var organ = pathElementFor(observedData, Constants.ObservedData.ORGAN);
         var compartment = pathElementFor(observedData, Constants.ObservedData.COMPARTMENT);
         var molecule = pathElementFor(observedData, Constants.ObservedData.MOLECULE);

         observedData.AllButBaseGrid().Each(column =>
         {
            //same depth as a simulation output path so that charts group and label observed data like simulation outputs
            var path = new[] {pathSafe(observedData.Name), Constants.ObservedData.CONTAINER_NAME, organ, compartment, molecule, pathSafe(column.Name)};
            column.QuantityInfo = new QuantityInfo(path, QuantityType.Undefined);
         });

         var baseGrid = observedData.BaseGrid;
         if (baseGrid == null)
            return;

         baseGrid.QuantityInfo = new QuantityInfo(new[] {pathSafe(observedData.Name), pathSafe(baseGrid.Name)}, QuantityType.Time);
      }

      private static string pathElementFor(DataRepository observedData, string propertyName) =>
         pathSafe(observedData.ExtendedPropertyValueFor(propertyName)) ?? string.Empty;

      private static string pathSafe(string pathElement) => pathElement?.Replace(ObjectPath.PATH_DELIMITER, PATH_DELIMITER_REPLACEMENT);
   }
}
