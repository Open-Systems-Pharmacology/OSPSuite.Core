using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryItemPresenter : ISubPresenter
   {
      /// <summary>
      /// Edit a single dataSheet repository
      /// </summary>
      /// <param name="observedData">The repository being edited</param>
      void EditObservedData(DataRepository observedData);
   }
}