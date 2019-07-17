using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Diagram;

namespace OSPSuite.Presentation.Views.Diagram
{
   public interface IBaseDiagramView : IView<IBaseDiagramPresenter>
   {
      IDiagramModel Model { set; }
      IBaseDiagramPresenter Presenter { get; }

      void BeginUpdate();
      void EndUpdate();

      void ClearSelection();
      void Select<T>(T node);
      IEnumerable<T> GetSelectedNodes<T>() where T : class;
      bool SelectionContains<T>(T obj);

      void CenterAt<T>(T node);
      void Refresh();
      bool GridVisible { set; get; }
      void Zoom(PointF currentLocation, float factor);
      void SetBackColor(Color color);

      Bitmap GetBitmap(IContainerBase containerBase);
      void PrintPreview();

      void CopyToClipboard(Image image);
   }
}