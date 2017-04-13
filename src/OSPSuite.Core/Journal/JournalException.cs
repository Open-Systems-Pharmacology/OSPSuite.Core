using System;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Journal
{
   public class JournalException : Exception
   {
      public JournalException()
      {
      }

      public JournalException(string message) : base(message)
      {
      }

      public JournalException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }

   public class CannotCreateNewItemForPersitedEntity : JournalException
   {
      public CannotCreateNewItemForPersitedEntity(string id) : base($"Cannot create a new item for an existing id: {id}")
      {
      }
   }
}