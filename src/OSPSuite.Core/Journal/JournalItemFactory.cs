using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Journal
{
   public interface IJournalPageFactory
   {
      JournalPage Create();
   }

   public class JournalPageFactory : IJournalPageFactory
   {
      private readonly IApplicationConfiguration _applicationConfiguration;
      private readonly DateTimeFormatter _dateTimeFormatter;

      public JournalPageFactory(IApplicationConfiguration applicationConfiguration)
      {
         _applicationConfiguration = applicationConfiguration;
         _dateTimeFormatter = new DateTimeFormatter();
      }

      public JournalPage Create()
      {
         var journalPage = new JournalPage
         {
            CreatedAt = SystemTime.UtcNow(),
            CreatedBy = EnvironmentHelper.UserName(),
            Content = new Content(),
            Origin = _applicationConfiguration.Product,
         };

         journalPage.Title = Captions.Journal.CreatedAtBy(_dateTimeFormatter.Format(journalPage.CreatedAt), journalPage.CreatedBy);
         return journalPage;
      }
   }
}