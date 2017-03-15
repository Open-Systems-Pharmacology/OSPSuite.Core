using System.Windows.Forms;
using OSPSuite.Starter.Forms;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IGridTestStarter : ITestStarter
   {
   }

   public class GridTestStarter : IGridTestStarter
   {
      public void Start()
      {
         FrmDataGrid.Show();
      }

      public static Form FrmDataGrid => createDataGrid();

      private static Form createDataGrid()
      {
         return new GridViewForm();
      }

   }
}