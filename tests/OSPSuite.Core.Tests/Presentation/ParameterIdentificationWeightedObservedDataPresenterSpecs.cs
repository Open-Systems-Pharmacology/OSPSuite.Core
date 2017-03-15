using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationWeightedObservedDataPresenter : ContextSpecification<ParameterIdentificationWeightedObservedDataPresenter>
   {
      protected ISimpleChartPresenter _simpleChartPresenter;
      protected IWeightedDataRepositoryDataPresenter _weightedDataRepositoryDataPresenter;
      protected IParameterIdentificationWeightedObservedDataView _view;
      protected WeightedObservedData _weightedObservedData;

      protected override void Context()
      {
         _simpleChartPresenter = A.Fake<ISimpleChartPresenter>();
         _weightedDataRepositoryDataPresenter = A.Fake<IWeightedDataRepositoryDataPresenter>();
         _view = A.Fake<IParameterIdentificationWeightedObservedDataView>();
         var dataRepository = new DataRepository {Name = "weightedObservedData" };
         dataRepository.Add(new BaseGrid("name", DomainHelperForSpecs.TimeDimensionForSpecs()));
         _weightedObservedData = new WeightedObservedData(dataRepository);

         sut = new ParameterIdentificationWeightedObservedDataPresenter(_view, _weightedDataRepositoryDataPresenter, _simpleChartPresenter);
      }
   }

   public class When_editing_a_weighted_data_repository : concern_for_ParameterIdentificationWeightedObservedDataPresenter
   {
      protected override void Because()
      {
         sut.Edit(_weightedObservedData);
      }

      [Observation]
      public void the_view_name_should_be_the_name_of_the_observed_data()
      {
         _view.Caption.ShouldBeEqualTo(_weightedObservedData.Name);
      }

      [Observation]
      public void the_chart_presenter_must_plot_the_data_repository()
      {
         A.CallTo(() => _simpleChartPresenter.PlotObservedData(_weightedObservedData)).MustHaveHappened();
      }

      [Observation]
      public void the_data_presenter_must_edit_the_data_repository()
      {
         A.CallTo(() => _weightedDataRepositoryDataPresenter.EditObservedData(_weightedObservedData)).MustHaveHappened();
      }
      [Observation]
      public void the_chart_should_show_the_log_lin_toggle()
      {
         _simpleChartPresenter.LogLinSelectionEnabled.ShouldBeTrue();
      }
   }
}
