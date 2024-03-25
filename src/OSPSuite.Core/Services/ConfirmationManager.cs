using System;

namespace OSPSuite.Core.Services
{
   [Flags]
   public enum ConfirmationFlags
   {
      None = 0,
      ObservedDataEntryRemoved = 1 << 0,
      AnotherConfirmation = 1 << 1,
      YetAnotherConfirmation = 1 << 2,
   }

   public interface IConfirmationManager
   {
      void SuppressConfirmation(ConfirmationFlags confirmation);
      bool IsConfirmationSuppressed(ConfirmationFlags confirmation);
   }

   public class ConfirmationManager : IConfirmationManager
   {

      public void SuppressConfirmation(ConfirmationFlags confirmation)
      {
         suppressConfirmationFlags |= confirmation;
      }

      public bool IsConfirmationSuppressed(ConfirmationFlags confirmation)
      {
         return (suppressConfirmationFlags & confirmation) == confirmation;
      }
      private ConfirmationFlags suppressConfirmationFlags { get; set; }
   }
}