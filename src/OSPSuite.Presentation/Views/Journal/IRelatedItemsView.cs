using System.Collections.Generic;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IRelatedItemsView : IView<IRelatedItemsPresenter>
   {
      void AddComparableView(IView view);
      void BindTo(IEnumerable<RelatedItem> relatedItems);
      void DeleteBinding();
   }
}