using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public class ValidationMessage
   {
      private readonly List<string> _details;

      /// <summary>
      ///    Gets the type of the message.
      /// </summary>
      public virtual NotificationType NotificationType { get; private set; }

      /// <summary>
      ///    Gets the  text describing the validation problem
      /// </summary>
      public virtual string Text { get; set; }

      /// <summary>
      ///    returns the underlying object for which the validation message was created. (e.g Builder object)
      /// </summary>
      public virtual IObjectBase Object { get; private set; }

      /// <summary>
      ///    returns the Building Block object where the Object is defined. Building Block is only set if the object
      ///    is a builder. It is typically null when dealing with objects defined in a simulation
      /// </summary>
      public virtual IBuildingBlock BuildingBlock { get; private set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="ValidationMessage" /> class.
      /// </summary>
      /// <param name="notificationType">Type of the message.</param>
      /// <param name="text">The text describing the validation problem</param>
      /// <param name="objectBase">The invalid object .</param>
      /// <param name="buildingBlock">Building block where the invalid object is defined. </param>
      public ValidationMessage(NotificationType notificationType, string text, IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         NotificationType = notificationType;
         Text = text;
         Object = objectBase;
         BuildingBlock = buildingBlock;
         _details = new List<string>();
      }

      public virtual void AddDetails(IEnumerable<string> details)
      {
         _details.AddRange(details);
      }

      public virtual void AddDetail(string detail)
      {
         _details.Add(detail);
      }

      /// <summary>
      ///    Returns details if available on the error (usefull to aggregate more than one error for a given  object)
      /// </summary>
      public virtual IEnumerable<string> Details => _details;
   }

   /// <summary>
   ///    Type of a <see cref="ValidationMessage" />
   /// </summary>
   [Flags]
   public enum NotificationType
   {
      /// <summary>
      ///    Default notification type
      /// </summary>
      None = 0,

      /// <summary>
      ///    Indicating a problem that may produce an incorrect model
      /// </summary>
      Warning = 1 << 0,

      /// <summary>
      ///    Indicating a problem that leads to a corrupted model
      /// </summary>
      Error = 1 << 1,

      /// <summary>
      ///    Indicating a message. Model is valid
      /// </summary>
      Info = 1 << 2,

      /// <summary>
      ///    Indicates a debug information. Is only use for development
      /// </summary>
      Debug = 1 << 3,


      All = Debug | Warning | Info | Error
   }

   public static class MessageTypeExtensions
   {
      public static bool Is(this NotificationType notificationType, NotificationType typeToCompare)
      {
         return (notificationType & typeToCompare) != 0;
      }
   }
}