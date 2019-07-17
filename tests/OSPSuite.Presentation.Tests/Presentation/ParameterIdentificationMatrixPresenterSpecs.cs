using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationMatrixPresenter : ContextSpecification<IParameterIdentificationMatrixPresenter>
   {
      protected IMatrixToDataTableMapper _matrixToDataTableMapper;
      protected IParameterIdentificationMatrixView _view;

      protected Matrix _matrix;
      protected DataTable _dataTable;
      protected string _defaultNotificationMessage = "TOTO";

      protected override void Context()
      {
         _matrixToDataTableMapper = A.Fake<IMatrixToDataTableMapper>();
         _view = A.Fake<IParameterIdentificationMatrixView>();
         sut = new ParameterIdentificationMatrixPresenter(_view, _matrixToDataTableMapper);
         sut.DefaultNotificationMessage = _defaultNotificationMessage;
         _matrix = new Matrix(new[] {"row1", "row2"}, new[] {"col1", "col2"});
         _matrix.SetRow(0, new[] {1d, 2d});
         _matrix.SetRow(1, new[] {3d, 4d});

         _dataTable = new DataTable();

         A.CallTo(() => _matrixToDataTableMapper.MapFrom(_matrix)).Returns(_dataTable);
      }
   }

   public class When_editing_a_matrix : concern_for_ParameterIdentificationMatrixPresenter
   {
      protected override void Because()
      {
         sut.Edit(_matrix);
      }

      [Observation]
      public void should_leverage_the_matrix_mapper_to_create_a_new_datatable_and_bind_it_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dataTable, _matrix.Max)).MustHaveHappened();
      }

      [Observation]
      public void should_reset_the_notification_messsage_in_the_view()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
         sut.NotificationMessage.ShouldBeEqualTo(_defaultNotificationMessage);
      }
   }

   public class When_showing_the_error_that_occured_during_some_matrix_claculation : concern_for_ParameterIdentificationMatrixPresenter
   {
      protected override void Because()
      {
         sut.ShowCalculationError("ERROR");
      }

      [Observation]
      public void should_reset_the_binding()
      {
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_notification()
      {
         sut.NotificationMessage.ShouldBeEqualTo("ERROR");
      }
   }
}