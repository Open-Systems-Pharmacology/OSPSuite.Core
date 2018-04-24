namespace OSPSuite.Core.Comparison
{
   /// <summary>
   ///    Represents the difference between two objects (x, y)
   /// </summary>
   public abstract class DiffItem
   {
      /// <summary>
      ///    The first object in the difference
      /// </summary>
      public object Object1 { get; set; }

      /// <summary>
      ///    The second object in the difference
      /// </summary>
      public object Object2 { get; set; }

      /// <summary>
      ///    The actual description of the difference between the two objects
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      ///    The common ancestor in the hiearchy where the difference was found. This is in general the containing entity of
      ///    Object 1
      /// </summary>
      public object CommonAncestor { get; set; }
   }

   /// <summary>
   ///    Represents a simple difference in property values between two objects
   /// </summary>
   public class PropertyValueDiffItem : DiffItem
   {
      /// <summary>
      ///    Name of the property where difference were found
      /// </summary>
      public string PropertyName { get; set; }

      /// <summary>
      ///    Display value of object 1
      /// </summary>
      public string FormattedValue1 { get; set; }

      /// <summary>
      ///    Display value of object 1
      /// </summary>
      public string FormattedValue2 { get; set; }
   }

   /// <summary>
   ///    One object is missing from the hiearchy
   /// </summary>
   public class MissingDiffItem : DiffItem
   {
      /// <summary>
      ///    Missing object defined in object 1 that is missing in object 2 (in that case MissingObject2 should be null)
      /// </summary>
      public object MissingObject1 { get; set; }

      /// <summary>
      ///    Missing object defined in object 2 that is missing in object 1 (in that case MissingObject1 should be null)
      /// </summary>
      public object MissingObject2 { get; set; }

      /// <summary>
      ///    The name of the missing object.
      /// </summary>
      public string MissingObjectName { get; set; }

      /// <summary>
      ///    The type name of the missing object.
      /// </summary>
      public string MissingObjectType { get; set; }

      /// <summary>
      /// Some extra information describing the object that was found and can help understand the output of the differentiation better (either for object1 or 2)
      /// </summary>
      public string PresentObjectDetails { get; set; }
   }

   public class MismatchDiffItem : DiffItem
   {
      
   }

   public class EmptyDiffItem : DiffItem
   {
      private EmptyDiffItem()
      {
      }

      public static EmptyDiffItem Instance = new EmptyDiffItem();
   }
}