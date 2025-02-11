using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Services
{
   public enum ConfirmationFlags
   {
      ObservedDataEntryRemoved = 0
   }

   public interface IConfirmationManager
   {
      void SuppressConfirmation(ConfirmationFlags confirmation);
      bool IsConfirmationSuppressed(ConfirmationFlags confirmation);
   }

   public class ConfirmationManager : IConfirmationManager
   {
      private readonly HashSet<ConfirmationFlags> _suppressedConfirmations = new HashSet<ConfirmationFlags>();

      public void SuppressConfirmation(ConfirmationFlags confirmation)
      {
         _suppressedConfirmations.Add(confirmation);
      }

      public bool IsConfirmationSuppressed(ConfirmationFlags confirmation)
      {
         return _suppressedConfirmations.Contains(confirmation);
      }
   }
}