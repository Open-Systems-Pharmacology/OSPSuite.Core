using System.Windows.Forms;
using OSPSuite.Starter.Forms;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IGridTestStarter : ITestStarter
   {
   }

   public class GridTestStarter : IGridTestStarter
   {
      private readonly Form _dataGridView;

      public GridTestStarter(ValueOriginBinder<ParameterDTO> valueOriginBinder )
      {
         _dataGridView = new GridViewForm(valueOriginBinder);
      }

      public void Start(int width = 0, int height = 0)
      {
         _dataGridView.Show();
      }
   }
}