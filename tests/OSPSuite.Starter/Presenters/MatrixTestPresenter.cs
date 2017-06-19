using System;
using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace OSPSuite.Starter.Presenters
{
   public interface IMatrixTestPresenter : IPresenter
   {
   }

   public class MatrixTestPresenter : IMatrixTestPresenter
   {
      private readonly IMatrixToDataTableMapper _matrixToDataTableMapper;
      private readonly IParameterIdentificationMatrixPresenter _matrixPresenter;

      public MatrixTestPresenter(IMatrixToDataTableMapper matrixToDataTableMapper, IParameterIdentificationMatrixPresenter matrixPresenter)
      {
         _matrixToDataTableMapper = matrixToDataTableMapper;
         _matrixPresenter = matrixPresenter;
         matrixPresenter.View.BindTo(getDataTable(), 20);
      }

      private DataTable getDataTable()
      {
         var captions = new List<string> {"parameter 1", "parameter 2", "parameter 3", "parameter 4", "parameter 5", "parameter 6", "parameter 7", "parameter 8", "parameter 9", "parameter 10"};
         var matrix = new Matrix(captions, captions);
         matrix.SetRow(0, new[] {11.123456789d, 12d, -0.000003, 11.123456789d, 12d, -0.000003, 11.123456789d, 12d, -0.000003, 0d});
         matrix.SetRow(1, new[] {21d, 22d, 0d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(2, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(3, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(4, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(5, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(6, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(7, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(8, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});
         matrix.SetRow(9, new[] {0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d});

         return _matrixToDataTableMapper.MapFrom(matrix);
      }

      public void ReleaseFrom(IEventPublisher eventPublisher)
      {
      }

      public bool CanClose { get; } = true;
      public event EventHandler StatusChanged = delegate { };

      public void ViewChanged()
      {
      }

      public IView BaseView => _matrixPresenter.BaseView;

      public void Initialize()
      {
      }
   }
}