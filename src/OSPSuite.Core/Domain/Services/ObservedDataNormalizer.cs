using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataNormalizer
   {
      /// <summary>
      ///    Sets the <see cref="QuantityInfo" /> of all columns of <paramref name="observedData" /> from its metadata so that
      ///    an observed data set is described the same way regardless of the application that created it.
      ///    The base grid gets the path [repository name, base grid name] and <see cref="QuantityType.Time" />.
      ///    Every other column gets the path [repository name, <see cref="Constants.ObservedData.CONTAINER_NAME" />, organ,
      ///    compartment, molecule, column name] and <see cref="QuantityType.Undefined" />, so that charts can group and label
      ///    observed data like simulation outputs (ROOT|ORGANISM|ORGAN|COMPARTMENT|MOLECULE|Name).
      ///    Missing metadata becomes an empty path element. Column names containing <see cref="ObjectPath.PATH_DELIMITER" />
      ///    are escaped so that they do not add path levels.
      /// </summary>
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
