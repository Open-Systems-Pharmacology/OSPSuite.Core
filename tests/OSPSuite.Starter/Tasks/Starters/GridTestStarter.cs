using System.Windows.Forms;
using OSPSuite.Starter.Forms;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IGridTestStarter : ITestStarter
   {
   }

   public class GridTestStarter : IGridTestStarter
   {
      private readonly Form _dataGridView = new GridViewForm();

      public void Start(int width = 0, int height = 0)
      {
         _dataGridView.Show();
      }
   }
}