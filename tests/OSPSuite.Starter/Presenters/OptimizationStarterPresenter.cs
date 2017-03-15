using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Starter.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Presenters
{
   public interface IOptimizationStarterPresenter : IPresenter<IOptimizationStarterView>
   {
      void StartMatrixTest();
   }

   public class OptimizationStarterPresenter : AbstractPresenter<IOptimizationStarterView, IOptimizationStarterPresenter>, IOptimizationStarterPresenter
   {
      private readonly IParameterIdentificationMatrixView _matrixView;
      private readonly IMatrixToDataTableMapper _matrixToDataTableMapper;

      public OptimizationStarterPresenter(IOptimizationStarterView view, IParameterIdentificationMatrixView matrixView, IMatrixToDataTableMapper matrixToDataTableMapper) : base(view)
      {
         _matrixView = matrixView;
         _matrixToDataTableMapper = matrixToDataTableMapper;
      }

      public void StartMatrixTest()
      {
         var form = new Form { Size = new Size(700, 500) };
         form.FillWith(_matrixView as Control);
         _matrixView.BindTo(getDataTable(), 22);
         form.ShowDialog();
      }

      private DataTable getDataTable()
      {
         var captions = new List<string> { "parameter 1", "parameter 2", "parameter 3", "parameter 4", "parameter 5", "parameter 6", "parameter 7", "parameter 8", "parameter 9", "parameter 10" };
         var matrix = new Matrix(captions, captions);
         matrix.SetRow(0, new[] { 11.123456789d, 12d, -0.000003, 11.123456789d, 12d, -0.000003, 11.123456789d, 12d, -0.000003, 0d });
         matrix.SetRow(1, new[] { 21d, 22d, 0d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(2, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(3, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(4, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(5, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(6, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(7, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(8, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });
         matrix.SetRow(9, new[] { 0d, 0d, -1d, 11.123456789d, 12d, double.NaN, 11.123456789d, 12d, -15.000003, 0d });

         return _matrixToDataTableMapper.MapFrom(matrix);
      }
   }
}
