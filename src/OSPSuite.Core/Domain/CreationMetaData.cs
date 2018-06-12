using System;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Indicates the workflow used to create a new object
   /// </summary>
   public enum CreationMode
   {
      /// <summary>
      ///    Object is created from scratch
      /// </summary>
      New,

      /// <summary>
      ///    Object is created as a result of a clone operation
      /// </summary>
      Clone,

      /// <summary>
      ///    Object is created as a result of a configure operation
      /// </summary>
      Configure
   }

   public class CreationMetaData
   {
      private DateTime _createdAt;
      public virtual string CreatedBy { get; }

      public virtual Origin Origin { get; set; }
      
      /// <summary>
      /// Version of the application (typically X.Y.Z)
      /// </summary>
      public virtual string Version { get; set; }

      /// <summary>
      /// Internal version that can be referenced to check the real internal version of the project
      /// </summary>
      public virtual int? InternalVersion { get; set; }

      public virtual CreationMode CreationMode { get; set; }

      /// <summary>
      ///    Name of the object used to create the clone. This is only used when <see cref="CreationMode" /> is set to
      ///    <c>CreationMode.Clone</c>
      /// </summary>
      public virtual string ClonedFrom { get; set; }

      public virtual DateTime CreatedAt
      {
         get => _createdAt;
         private set => _createdAt = value.ToUniversalTime();
      }

      public CreationMetaData AsCloneOf(IWithName objectToClone)
      {
         return AsCloneOf(objectToClone.Name);
      }

      public CreationMetaData AsCloneOf(string objectToCloneName)
      {
         CreationMode = CreationMode.Clone;
         ClonedFrom = objectToCloneName;
         return this;
      }

      public CreationMetaData()
      {
         CreatedAt = SystemTime.UtcNow();
         CreatedBy = EnvironmentHelper.UserName();
         Origin = Origins.Other;
      }

      public CreationMetaData Clone()
      {
         return new CreationMetaData
         {
            Origin = Origin,
            Version = Version,
            InternalVersion = InternalVersion,
            CreationMode = CreationMode,
            ClonedFrom = ClonedFrom,
         };
      }

      public override string ToString()
      {
         return ToDisplayString();
      }

      public string ToDisplayString()
      {
         var dateTimeFormatter = new DateTimeFormatter();
         var creationDate = dateTimeFormatter.Format(CreatedAt);
         switch (CreationMode)
         {
            case CreationMode.New:
               return Captions.CreatedOn(creationDate);
            case CreationMode.Clone:
               return Captions.ClonedOn(creationDate, ClonedFrom);
            case CreationMode.Configure:
               return Captions.ConfiguredOn(creationDate);
            default:
               return string.Empty;
         }
      }
   }
}