using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationMatrixPresenter : IPresenter<IParameterIdentificationMatrixView>
   {
      void Edit(Matrix matrix);
      void ShowCalculationError(string calculationError);
      string NotificationMessage { get; }
      string DefaultNotificationMessage { get; set; }
      IFormatter<double> NumberFormatter { get; set; }
   }

   public class ParameterIdentificationMatrixPresenter : AbstractPresenter<IParameterIdentificationMatrixView, IParameterIdentificationMatrixPresenter>, IParameterIdentificationMatrixPresenter
   {
      private readonly IMatrixToDataTableMapper _matrixToDataTableMapper;
      public string NotificationMessage { get; private set; }
      public string DefaultNotificationMessage { get; set; }
      

      public ParameterIdentificationMatrixPresenter(IParameterIdentificationMatrixView view, IMatrixToDataTableMapper matrixToDataTableMapper) : base(view)
      {
         _matrixToDataTableMapper = matrixToDataTableMapper;
      }

      public void Edit(Matrix matrix)
      {
         resetView();
         if (matrix == null) return;

         bindTo(matrix);
      }

      private void resetView()
      {
         resetView(DefaultNotificationMessage);
      }

      public void ShowCalculationError(string calculationError)
      {
         resetView(calculationError);
      }

      private void resetView(string notificationMessage)
      {
         NotificationMessage = notificationMessage;
         View.DeleteBinding();
      }

      private void bindTo(Matrix matrix)
      {
         View.BindTo(_matrixToDataTableMapper.MapFrom(matrix), matrix.Max);
      }

      public IFormatter<double> NumberFormatter
      {
         get { return View.NumberFormatter; }
         set { View.NumberFormatter = value; }
      }
   }
}