using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Importer.Presenters
{
   public abstract class concern_for_NamingPatternPresenter : ContextSpecification<NamingPatternPresenter>
   {
      protected INamingPatternView _view;

      protected override void Context()
      {
         base.Context();
         _view = A.Fake<INamingPatternView>();
         sut = new NamingPatternPresenter(_view);
      }
   }

   public class when_adding_preset_patterns : concern_for_NamingPatternPresenter
   {
      protected override void Because()
      {
         sut.AddPresetNamingPatterns("");
      }

      [Observation]
      public void a_call_to_view_setter_must_result()
      {
         A.CallTo(() => _view.UpdateNamingPatterns(A<List<string>>.That.Contains(""))).MustHaveHappened(Repeated.Exactly.Once);
      }
   }
}