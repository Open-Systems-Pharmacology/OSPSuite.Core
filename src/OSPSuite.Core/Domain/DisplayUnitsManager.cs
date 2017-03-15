using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Manages the default <see cref="Unit" /> for some <see cref="IDimension" /> defined in the dimension factory
   /// </summary>
   public class DisplayUnitsManager : IUpdatable
   {
      private readonly List<DisplayUnitMap> _allDisplayUnits = new List<DisplayUnitMap>();

      public virtual void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceUnitsManager = source as DisplayUnitsManager;
         if (sourceUnitsManager == null) return;
         _allDisplayUnits.Clear();
         sourceUnitsManager.AllDisplayUnits.Each(d => AddDisplayUnit(d.Clone()));
      }

      public virtual void AddDisplayUnit(DisplayUnitMap displayUnitMap)
      {
         _allDisplayUnits.Add(displayUnitMap);
      }

      public virtual IEnumerable<DisplayUnitMap> AllDisplayUnits => _allDisplayUnits;

      /// <summary>
      ///    Returns the display unit defined for the given <paramref name="dimension" /> or <c>null</c>
      ///    if to mapping was defined for this specific <paramref name="dimension" />.
      ///    It also ensures that the returned unit actually belongs in the <paramref name="dimension" />.
      /// </summary>
      public virtual Unit DisplayUnitFor(IDimension dimension)
      {
         var defaultMap = retrieveDisplayUnitMapFor(dimension);
         if (defaultMap?.DisplayUnit == null)
            return null;

         var unitName = defaultMap.DisplayUnit.Name;
         return dimension.HasUnit(unitName) ? dimension.Unit(unitName) : null;
      }

      private DisplayUnitMap retrieveDisplayUnitMapFor(IDimension dimension)
      {
         var mergedDimension = dimension as IMergedDimension;
         if (mergedDimension == null)
            return _allDisplayUnits.Find(x => Equals(x.Dimension, dimension));

         var allDimensions = new List<IDimension>(mergedDimension.TargetDimensions);
         allDimensions.Insert(0, mergedDimension.SourceDimension);

         return allDimensions.Select(retrieveDisplayUnitMapFor).FirstOrDefault(x => x != null);
      }

      public virtual void RemoveDefaultUnit(DisplayUnitMap displayUnitMap)
      {
         _allDisplayUnits.Remove(displayUnitMap);
      }
   }
}