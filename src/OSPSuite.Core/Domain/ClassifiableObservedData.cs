using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    A wrapper for Datarepository which can resolve a classification tree
   /// </summary>
   public class ClassifiableObservedData : Classifiable<DataRepository>
   {
      public ClassifiableObservedData() : base(ClassificationType.ObservedData)
      {
      }

      /// <summary>
      ///    Gets the value for the named property from the underlying DataRepository
      /// </summary>
      public string ExtendedPropertyValueFor(string propertyName)
      {
         return Subject.ExtendedPropertyValueFor(propertyName);
      }

      public DataRepository Repository => Subject;
   }
}