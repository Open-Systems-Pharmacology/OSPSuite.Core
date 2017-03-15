using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   public class PKParameter : IWithName, IWithDimension, IWithDescription
   {
      private string _displayName;

      /// <summary>
      ///    Internal name used to identify a pk parameter
      /// </summary>
      public virtual string Name { get; set; }

      /// <summary>
      ///    Display name of a pk Parameter. Is the Display name is not set, returns name
      /// </summary>
      public virtual string DisplayName
      {
         set { _displayName = value; }
         get { return string.IsNullOrEmpty(_displayName) ? Name : _displayName; }
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
   }
}