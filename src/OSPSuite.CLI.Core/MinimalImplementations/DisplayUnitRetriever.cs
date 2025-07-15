using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
    public class DisplayUnitRetriever : IDisplayUnitRetriever
    {
        public Unit PreferredUnitFor(IWithDimension withDimension, Unit defaultUnit = null)
        {
            return withDimension == null ? null : preferredUnitFor(withDimension.Dimension, defaultUnit);
        }

        public Unit PreferredUnitFor(IWithDisplayUnit withDisplayUnit)
        {
            return withDisplayUnit == null ? null : preferredUnitFor(withDisplayUnit.Dimension, withDisplayUnit.DisplayUnit);
        }

        public Unit PreferredUnitFor(IDimension dimension)
        {
            return preferredUnitFor(dimension, null);
        }

        private Unit preferredUnitFor(IDimension dimension, Unit defaultUnit)
        {
            if (dimension == null)
                return null;

            return defaultUnitForDimension(dimension, defaultUnit);
        }

        private Unit defaultUnitForDimension(IDimension dimension, Unit defaultUnit)
        {
            return dimension.HasUnit(defaultUnit)
               ? defaultUnit
               : dimension.DefaultUnit;
        }
    }
}