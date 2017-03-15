using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Services
{
   public class DisplayUnitRetriever : IDisplayUnitRetriever
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly IPresentationUserSettings _userSettings;

      public DisplayUnitRetriever(IProjectRetriever projectRetriever, IPresentationUserSettings userSettings)
      {
         _projectRetriever = projectRetriever;
         _userSettings = userSettings;
      }

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

         return displayInProjectFor(dimension) ??
                displayInUserSettingsFor(dimension) ??
                defaultUnitForDimension(dimension, defaultUnit);
      }

      private Unit defaultUnitForDimension(IDimension dimension, Unit defaultUnit)
      {
         return dimension.HasUnit(defaultUnit)
            ? defaultUnit
            : dimension.DefaultUnit;
      }

      private Unit displayInUserSettingsFor(IDimension dimension)
      {
         return displayUnitIn(_userSettings.DisplayUnits, dimension);
      }

      private Unit displayInProjectFor(IDimension dimension)
      {
         if (_projectRetriever.CurrentProject == null)
            return null;

         return displayUnitIn(_projectRetriever.CurrentProject.DisplayUnits, dimension);
      }

      private Unit displayUnitIn(DisplayUnitsManager displayUnitsManager, IDimension dimension)
      {
         return displayUnitsManager.DisplayUnitFor(dimension);
      }
   }
}