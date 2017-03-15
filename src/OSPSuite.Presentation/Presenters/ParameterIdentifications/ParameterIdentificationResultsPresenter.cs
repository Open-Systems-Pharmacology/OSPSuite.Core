using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationResultsPresenter : IParameterIdentificationItemPresenter,
      IListener<ParameterIdentificationResultsUpdatedEvent>
   {
   }

   public class ParameterIdentificationResultsPresenter : AbstractSubPresenter<IParameterIdentificationResultsView, IParameterIdentificationResultsPresenter>, IParameterIdentificationResultsPresenter
   {
      private readonly ISingleParameterIdentificationResultsPresenter _singleResultsPresenter;
      private readonly IMultipleParameterIdentificationResultsPresenter _multipleResultsPresenter;
      private ParameterIdentification _parameterIdentification;

      public ParameterIdentificationResultsPresenter(IParameterIdentificationResultsView view, ISingleParameterIdentificationResultsPresenter singleResultsPresenter, 
         IMultipleParameterIdentificationResultsPresenter multipleResultsPresenter) : base(view)
      {
         _singleResultsPresenter = singleResultsPresenter;
         _multipleResultsPresenter = multipleResultsPresenter;
         AddSubPresenters(_singleResultsPresenter, _multipleResultsPresenter);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         showParameterIdentificationResults();
      }

      public void Handle(ParameterIdentificationResultsUpdatedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         showParameterIdentificationResults();
      }

      private void showParameterIdentificationResults()
      {
         if (!_parameterIdentification.HasResults)
            showNoResultsAvailable();

         else if (_parameterIdentification.IsSingleRun)
            showSingleIdentification();
         else
            showMultipleIdentification();
      }

      private void showNoResultsAvailable()
      {
         _view.HideResultsView();
      }

      private void showMultipleIdentification()
      {
         showResultsOfIdentification(_multipleResultsPresenter);
      }

      private void showSingleIdentification()
      {
         showResultsOfIdentification(_singleResultsPresenter);
      }

      private void showResultsOfIdentification(IParameterIdentificationPresenter resultsPresenter)
      {
         _view.ShowResultsView(resultsPresenter.BaseView);
         resultsPresenter.EditParameterIdentification(_parameterIdentification);
      }

      private bool canHandle(ParameterIdentificationEvent eventToHandle)
      {
         return Equals(eventToHandle.ParameterIdentification, _parameterIdentification);
      }
   }
}