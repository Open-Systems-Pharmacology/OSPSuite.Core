using System;
using OSPSuite.Core;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.Starter.Tasks
{

   public class ReloadRelatedItemTask : OSPSuite.Core.Journal.ReloadRelatedItemTask
   {
      public ReloadRelatedItemTask(IApplicationConfiguration applicationConfiguration, IContentLoader contentLoader, IDialogCreator dialogCreator) : base(applicationConfiguration, contentLoader, dialogCreator)
      {
      }

      protected override void StartSisterApplicationWithContentFile(string contentFile)
      {
         throw new NotSupportedException();
      }

      protected override void LoadOwnContent(RelatedItem relatedItem)
      {
         throw new NotSupportedException();
      }

      protected override bool RelatedItemCanBeLaunchedBySisterApplication(RelatedItem relatedItem)
      {
         return false;
      }
   }

   public class RelatedItemDescriptionCreator : IRelatedItemDescriptionCreator
   {
      public string DescriptionFor<T>(T relatedObject)
      {
         throw new NotSupportedException();
      }
   }
}