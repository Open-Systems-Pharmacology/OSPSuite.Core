using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Services
{
   public interface IDisplayUnitRetriever
   {
      /// <summary>
      /// Returns the preferred <see cref="Unit"/> defined for the dimension used by the given <paramref name="withDimension"/>.
      /// </summary>
      /// <remarks>
      /// Preferred means: Look first in project settings. If not found, in the user settings. Last but not least, the <paramref name="defaultUnit"/> 
      /// if defined or the default unit of the underyling dimension is returned 
      /// </remarks>
      /// <param name="withDimension">Object using a dimension for which the preferred <see cref="Unit"/> should be retrieved</param>
      /// <param name="defaultUnit">Default unit to return if no preferred unit is found</param>
      Unit PreferredUnitFor(IWithDimension withDimension, Unit defaultUnit = null);

      /// <summary>
      /// Returns the preferred <see cref="Unit"/> defined for the dimension used by the given <paramref name="withDisplayUnit"/>.
      /// </summary>
      /// <remarks>
      /// Preferred means: Look first in project settings. If not found, in the user settings. Last but not least, the default unit 
      /// of the underyling dimension is used if the displayUnit is not set in the <paramref name="withDisplayUnit"/>.
      /// </remarks>
      /// <param name="withDisplayUnit">Object using a display unit for which the preferred <see cref="Unit"/> should be retrieved</param>
      Unit PreferredUnitFor(IWithDisplayUnit withDisplayUnit);

      /// <summary>
      /// Returns the preferred <see cref="Unit"/> defined for the <paramref name="dimension"/>.
      /// </summary>
      /// <remarks>
      /// Preferred means: Look first in project settings. If not found, in the user settings. Last but not least, the default unit of the underyling dimension is returned 
      /// </remarks>
      /// <param name="dimension">Dimension for which the preferred <see cref="Unit"/> should be retrieved</param>
      Unit PreferredUnitFor(IDimension dimension);
   }
}