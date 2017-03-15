using DevExpress.XtraRichEdit.Forms;
using OSPSuite.Core.Journal;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalSearchTextForm : SearchTextForm
   {
      public JournalSearchTextForm(SearchFormControllerParameters controllerParameters, JournalSearch journalSearch)
         : base(controllerParameters)
      {
         cbFndSearchString.Text = journalSearch.Search;
         chbFndMatchCase.Checked = journalSearch.MatchCase;
      }
   }
}