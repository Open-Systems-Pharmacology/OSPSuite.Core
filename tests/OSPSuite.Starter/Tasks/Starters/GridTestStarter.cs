using System.Windows.Forms;
using OSPSuite.Starter.Forms;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IGridTestStarter : ITestStarter
   {
   }

   public class GridTestStarter : IGridTestStarter
   {
      private readonly Form _dataGridView;

      public GridTestStarter(IImageListRetriever imageListRetriever)
      {
         _dataGridView = new GridViewForm(imageListRetriever);
      }

      public void Start(int width = 0, int height = 0)
      {
         _dataGridView.Show();
      }
   }
}