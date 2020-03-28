using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   public class PKParameter : IWithName, IWithDescription
   {
      private string _displayName;
      private string _displayUnitName;

      /// <summary>
      ///    Internal name used to identify a pk parameter
      /// </summary>
      public virtual string Name { get; set; }

      /// <summary>
      ///    Display name of a pk Parameter. Is the Display name is not set, returns name
      /// </summary>
      public virtual string DisplayName
      {
         set => _displayName = value;
         get => string.IsNullOrEmpty(_displayName) ? Name : _displayName;
      }

      /// <summary>
      ///    Descriptions associated with the pk parameter. This can be shown for instance as tool tip
      /// </summary>
      public virtual string Description { get; set; }

      /// <summary>
      ///    The dimension of the underlying pk parameter
      /// </summary>
      public virtual IDimension Dimension { get; set; }

      public virtual PKParameterMode Mode { get; set; }

      public virtual string DisplayUnitName
      {
         get => _displayUnitName ?? Dimension?.DefaultUnitName;
         set => _displayUnitName = value;
      }
   }
}