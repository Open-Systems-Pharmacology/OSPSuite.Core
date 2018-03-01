using System.ComponentModel;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Domain
{
   public interface IClassifiableWrapper : IWithName, IClassifiable
   {
      void UpdateSubject(IWithId subject);

      /// <summary>
      ///    Returns the underlying subject wrapped by the classifiable wrapper
      /// </summary>
      IWithId WrappedObject { get; }
   }

   public abstract class Classifiable<T> : Notifier, IClassifiableWrapper where T : IWithId, IWithName
   {
      private T _subject;

      /// <summary>
      ///    Reference to the parent classification
      /// </summary>
      public IClassification Parent { get; set; }

      public ClassificationType ClassificationType { get; private set; }

      protected Classifiable(ClassificationType classificationType)
      {
         ClassificationType = classificationType;
      }

      /// <summary>
      ///    The id of the underlying subject.
      ///    <remarks>We do not use a redirect to Subject.Id to enable deserialization</remarks>
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      ///    The name of the underlying repository
      /// </summary>
      public string Name
      {
         get => Subject.Name;
         set => Subject.Name = value;
      }

      /// <summary>
      ///    This is the underlying object
      /// </summary>
      public T Subject
      {
         get => _subject;
         set
         {
            _subject = value;
            Id = _subject.Id;
            var notifier = _subject as INotifyPropertyChanged;
            if (notifier != null)
               notifier.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
         }
      }

      public void UpdateSubject(IWithId subject)
      {
         Subject = subject.DowncastTo<T>();
      }

      public IWithId WrappedObject => Subject;
   }
}