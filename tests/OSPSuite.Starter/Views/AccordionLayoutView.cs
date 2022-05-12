using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class AccordionLayoutView : BaseContainerUserControl, IAccordionLayoutView
   {
      public AccordionLayoutView()
      {
         InitializeComponent();
      }

      public void StartAddingViews()
      {
         layoutControl.SuspendLayout();
      }

      public void AddView(IView view)
      {
         var group = layoutControl.Root.AddGroup();
         AddViewToGroup(group, view);

         group.ExpandButtonVisible = true;
      }

      public void FinishedAddingViews()
      {
         AddEmptyPlaceHolder(layoutControl);
         layoutControl.ResumeLayout();
      }
   }
}