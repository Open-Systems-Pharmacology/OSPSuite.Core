using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Services
{
   public class ChartFactory : IChartFactory
   {
      private readonly IContainer _container;
      private readonly IIdGenerator _idGenerator;
      private readonly IPresentationUserSettings _presentationUserSettings;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ITableFormulaToDataRepositoryMapper _dataRepositoryMapper;

      public ChartFactory(IContainer container, IIdGenerator idGenerator, IPresentationUserSettings presentationUserSettings, IDimensionFactory dimensionFactory, ITableFormulaToDataRepositoryMapper dataRepositoryMapper)
      {
         _container = container;
         _idGenerator = idGenerator;
         _presentationUserSettings = presentationUserSettings;
         _dimensionFactory = dimensionFactory;
         _dataRepositoryMapper = dataRepositoryMapper;
      }

      public TChartType Create<TChartType>() where TChartType : CurveChart
      {
         var chart = _container.Resolve<TChartType>();
         chart.Id = _idGenerator.NewId();
         chart.ChartSettings.BackColor = _presentationUserSettings.ChartBackColor;
         chart.ChartSettings.DiagramBackColor = _presentationUserSettings.ChartDiagramBackColor;
         chart.DefaultYAxisScaling = _presentationUserSettings.DefaultChartYScaling;
         return chart;
      }

      public CurveChart CreateChartFor(DataRepository dataRepository, Scalings defaultYScale)
      {
         var chart = Create<CurveChart>().WithName(dataRepository.Name);
         chart.DefaultYAxisScaling = defaultYScale;

         foreach (var valueColumn in dataRepository.AllButBaseGrid().OrderByDescending(x => x.RelatedColumns.Count()))
         {
            var curve = chart.CreateCurve(valueColumn.BaseGrid, valueColumn, dataRepository.Name, _dimensionFactory);
            chart.AddCurve(curve);
         }
         return chart;
      }

      public CurveChart CreateChartFor(TableFormula tableFormula)
      {
         return CreateChartFor(_dataRepositoryMapper.MapFrom(tableFormula));
      }

      public CurveChart CreateChartFor(DataRepository dataRepository)
      {
         return CreateChartFor(dataRepository, _presentationUserSettings.DefaultChartYScaling);
      }
   }
}