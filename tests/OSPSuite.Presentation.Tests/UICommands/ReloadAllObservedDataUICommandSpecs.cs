using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_ReloadAllObservedDataUICommand : ContextSpecification<ReloadAllObservedDataUICommand>
   {
      protected IReloadObservedDataTask _reloadObservedDataTask;
      protected DataRepository _observedData;

      protected override void Context()
      {
         _reloadObservedDataTask = A.Fake<IReloadObservedDataTask>();
         _observedData = new DataRepository();

         sut = new ReloadAllObservedDataUICommand(_reloadObservedDataTask);
         sut.For(_observedData);
      }

      protected override void Because()
      {
         sut.Execute();
      }
   }

   public class When_reloading_all_observed_data_imported_with_the_configuration_of_an_observed_data : concern_for_ReloadAllObservedDataUICommand
   {
      protected override void Context()
      {
         base.Context();
         _observedData.ConfigurationId = "ConfigurationId";
      }

      [Observation]
      public void should_reload_all_observed_data_imported_with_that_configuration()
      {
         A.CallTo(() => _reloadObservedDataTask.ReloadAllObservedDataImportedWith("ConfigurationId")).MustHaveHappened();
      }
   }

   public class When_reloading_all_observed_data_for_an_observed_data_that_was_not_imported_with_a_configuration : concern_for_ReloadAllObservedDataUICommand
   {
      [Observation]
      public void should_not_reload_anything()
      {
         A.CallTo(() => _reloadObservedDataTask.ReloadAllObservedDataImportedWith(A<string>._)).MustNotHaveHappened();
      }
   }
}
