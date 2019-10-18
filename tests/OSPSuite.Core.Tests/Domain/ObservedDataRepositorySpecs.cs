using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObservedDataRepository : ContextSpecification<IObservedDataRepository>
   {
      protected IProjectRetriever _projectRetriever;
      protected IProject _project;
      protected DataRepository _obs1;
      protected DataRepository _obs2;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _project = A.Fake<IProject>();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_project);
         sut = new ObservedDataRepository(_projectRetriever);

         _obs1 = new DataRepository("OBS1");
         _obs2 = new DataRepository("OBS2");

         A.CallTo(() => _project.AllObservedData).Returns(new[] {_obs1, _obs2});
      }
   }

   public class When_retrieving_all_observed_data_available_in_the_repository : concern_for_ObservedDataRepository
   {
      [Observation]
      public void should_return_the_observed_data_defined_in_the_project()
      {
         sut.All().ShouldOnlyContain(_project.AllObservedData);
      }
   }

   public class When_retrieving_all_observed_data_available_in_the_repository_when_no_project_is_available : concern_for_ObservedDataRepository
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(null);
      }

      [Observation]
      public void should_return_an_empty_list_of_observed_data()
      {
         sut.All().ShouldBeEmpty();
      }
   }

   public class When_retrieving_all_observed_data_used_by_a_simulation : concern_for_ObservedDataRepository
   {
      private IUsesObservedData _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IUsesObservedData>();
         A.CallTo(() => _simulation.UsesObservedData(_obs1)).Returns(false);
         A.CallTo(() => _simulation.UsesObservedData(_obs2)).Returns(true);
      }
      [Observation]
      public void should_return_all_observed_data_from_the_underlying_project_used_by_the_simulation()
      {
         sut.AllObservedDataUsedBy(_simulation).ShouldOnlyContain(_obs2);
      }
   }


   public class When_retrieving_all_observed_data_used_by_a_set_of_simulations : concern_for_ObservedDataRepository
   {
      private IUsesObservedData _simulation1;
      private IUsesObservedData _simulation2;

      protected override void Context()
      {
         base.Context();
         _simulation1 = A.Fake<IUsesObservedData>();
         _simulation2 = A.Fake<IUsesObservedData>();
         A.CallTo(() => _simulation1.UsesObservedData(_obs1)).Returns(false);
         A.CallTo(() => _simulation1.UsesObservedData(_obs2)).Returns(true);
         A.CallTo(() => _simulation2.UsesObservedData(_obs1)).Returns(true);
         A.CallTo(() => _simulation2.UsesObservedData(_obs2)).Returns(true);
      }
      [Observation]
      public void should_return_all_distinct_observed_data_from_the_underlying_project_used_by_the_simulations()
      {
         sut.AllObservedDataUsedBy(new []{_simulation1, _simulation2, }).ShouldOnlyContain(_obs1, _obs2);
      }
   }
}