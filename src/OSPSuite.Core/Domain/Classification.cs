using System;
using OSPSuite.Utility;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Domain
{
   public interface IClassifiable : IWithId
   {
      /// <summary>
      ///    Classification under which the <see cref="IClassifiable" /> is attached
      /// </summary>
      IClassification Parent { get; set; }

      /// <summary>
      ///    Specifies the type of the classification where this <see cref="IClassifiable" /> is attached
      /// </summary>
      ClassificationType ClassificationType { get; }
   }

   public interface IClassification : IClassifiable, IWithName
   {
      /// <summary>
      ///    Returns the path through the nodes ancestry
      /// </summary>
      string Path { get; }

      /// <summary>
      ///    Determines if the argument has equivalent classification as this object.
      ///    Equivalent classification is determined by identical Id and identical ancestry classification
      /// </summary>
      /// <param name="compared">The object being compared</param>
      /// <returns>True if path is equal</returns>
      bool HasEquivalentClassification(IClassification compared);

   }

   /// <summary>
   ///    A Node which only has the job of providing classification tree for other nodes
   /// </summary>
   public class Classification : Notifier, IClassification
   {
      private string _name;
      public IClassification Parent { set; get; }

      public string Id { get; set; }
      public ClassificationType ClassificationType { get; set; }

      public Classification()
      {
         ClassificationType = ClassificationType.Unknown;
         Id = ShortGuid.NewGuid();
      }

      public string Path
      {
         get { return Parent != null ? Parent.Path + Name : Name; }
      }

      private bool isPathEqual(IClassification g)
      {
         return (string.CompareOrdinal(Path, g.Path) == 0);
      }

      public bool HasEquivalentClassification(IClassification compared)
      {
         return compared != null && isPathEqual(compared) && compared.ClassificationType == ClassificationType;
      }

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged(() => Name);
         }
      }

      public override string ToString()
      {
         return Name;
      }
   }
}