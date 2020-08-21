using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      public SourceFilePresenter(ISourceFileControl view) : base(view)
      {
      }
   }
}