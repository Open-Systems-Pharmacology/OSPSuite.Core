using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Comparison;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Views.Comparisons;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ComparerSettingsPresenter : ContextSpecification<IComparerSettingsPresenter>
   {
      protected IComparerSettingsView _view;
      protected ComparerSettings _comparerSettings;

      protected override void Context()
      {
         _view = A.Fake<IComparerSettingsView>();
         _comparerSettings = new ComparerSettings();
         sut = new ComparerSettingsPresenter(_view);
      }
   }

   public class When_the_comparer_settings_presenter_is_saving_the_user_changes : concern_for_ComparerSettingsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_comparerSettings);
      }

      protected override void Because()
      {
         sut.SaveChanges();
      }

      [Observation]
      public void should_notify_the_view_to_save_the_changes()
      {
         A.CallTo(() => _view.SaveChanges()).MustHaveHappened();
      }
   }
}