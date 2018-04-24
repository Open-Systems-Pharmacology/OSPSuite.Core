using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Base interface for  most of the objects in <c>PKModelCore.Domain.Model</c>
   ///    provides access to base properties and change notification
   /// </summary>
   public interface IObjectBase : IWithId, IWithName, INotifier, IVisitable<IVisitor>, IUpdatable, IWithDescription
   {
      string Icon { get; set; }
   }

   /// <summary>
   ///    Base interface for  most of the objects in <c>PKModelCore.Domain.Model</c>
   ///    provides access to base properties and change notification
   /// </summary>
   public abstract class ObjectBase : Notifier, IObjectBase
   {
      public virtual string Id { get; set; }
      public virtual string Icon { get; set; }
      public virtual string Description { get; set; }

      private int? _hashCode;

      private string _name;

      /// <summary>
      ///    Initializes a new instance of the <see cref="ObjectBase" /> class.
      /// </summary>
      protected ObjectBase() : this(string.Empty)
      {
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="ObjectBase" /> class.
      ///    and sets it's id
      /// </summary>
      /// <param name="id">The id.</param>
      protected ObjectBase(string id)
      {
         Id = id;
         _name = string.Empty;
         Icon = string.Empty;
         Description = string.Empty;
      }

      public virtual string Name
      {
         get => _name;
         set => SetProperty(ref _name, value);
      }

      public virtual void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceObjectBase = source as IObjectBase;
         if (sourceObjectBase == null) return;

         //ID should not be updated since it should have been defined in constructor or during the object construction process
         _name = sourceObjectBase.Name;
         Icon = sourceObjectBase.Icon;
         Description = sourceObjectBase.Description;
      }

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         visitor.Visit(this);
      }

      public override string ToString()
      {
         return Name;
      }

      public virtual bool Equals(IObjectBase other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         if (string.Equals(other.Id, string.Empty)) return false;
         return Equals(other.Id, Id);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return Equals(obj as IObjectBase);
      }

      public override int GetHashCode()
      {
         // Once we have a hash code we'll never change it  
         if (_hashCode.HasValue)
            return _hashCode.Value;

         var thisIsTransient = string.IsNullOrEmpty(Id);

         // When this instance is transient, we use the base GetHashCode()  
         // and remember it, so an instance can NEVER change its hash code.  
         _hashCode = thisIsTransient ? base.GetHashCode() : Id.GetHashCode();
         return _hashCode.Value;
      }
   }
}