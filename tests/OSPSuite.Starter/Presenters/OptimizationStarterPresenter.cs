using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Presenters
{
   public interface IOptimizationStarterPresenter : IPresenter<IOptimizationStarterView>
   {
      void StartMatrixTest();
      void StartParameterIdentificationTest();
   }

   public class OptimizationStarterPresenter : AbstractPresenter<IOptimizationStarterView, IOptimizationStarterPresenter>, IOptimizationStarterPresenter
   {
      private readonly IParameterIdentificationMatrixView _matrixView;
      private readonly IMatrixToDataTableMapper _matrixToDataTableMapper;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly IShellPresenter _shellPresenter;
      private readonly ISimulationRepository _simulationRepository;

      public OptimizationStarterPresenter(IOptimizationStarterView view, IParameterIdentificationMatrixView matrixView, IMatrixToDataTableMapper matrixToDataTableMapper, IParameterIdentificationTask parameterIdentificationTask,
         IShellPresenter shellPresenter, ISimulationRepository simulationRepository) : base(view)
      {
         _matrixView = matrixView;
         _matrixToDataTableMapper = matrixToDataTableMapper;
         _parameterIdentificationTask = parameterIdentificationTask;
         _shellPresenter = shellPresenter;
         _simulationRepository = simulationRepository;
      }

      public void StartMatrixTest()
      {
         var form = new Form {Size = new Size(700, 500)};
         form.FillWith(_matrixView as Control);
         _matrixView.BindTo(getDataTable(), 22);
         form.ShowDialog();
      }

      public void StartParameterIdentificationTest()
      {
         _shellPresenter.Start();
         var paramterIdentification = _parameterIdentificationTask.CreateParameterIdentificationBasedOn(_simulationRepository.All());
         var presenter = IoC.Resolve<IEditParameterIdentificationPresenter>();
         presenter.InitializeWith(new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         presenter.Edit(paramterIdentification);
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
   }
}