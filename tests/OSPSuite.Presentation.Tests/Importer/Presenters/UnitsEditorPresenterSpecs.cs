using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_UnitsEditorPresenter : ContextSpecification<UnitsEditorPresenter>
   {
      protected IUnitsEditorView _view;

      protected override void Context()
      {
         _view = A.Fake<IUnitsEditorView>();
         sut = new UnitsEditorPresenter(_view);
      }
   }

   public class When_options_set_no_column : concern_for_UnitsEditorPresenter
   {
      protected override void Because()
      {
         sut.SetOptions(new Column() {Unit = new UnitDescription("min")}, A.Fake<IReadOnlyList<IDimension>>(), A.Fake<IEnumerable<string>>());
      }

      [Observation]
      public void column_mapping_flag_is_calculated()
      {
         A.CallTo(() => _view.SetParams(false, A<bool>.Ignored)).MustHaveHappened();
      }
   }

   public class When_options_set_from_column : concern_for_UnitsEditorPresenter
   {
      protected override void Because()
      {
         sut.SetOptions(new Column() {Unit = new UnitDescription("min", "columnName")}, A.Fake<IReadOnlyList<IDimension>>(), A.Fake<IEnumerable<string>>());
      }

      [Observation]
      public void column_mapping_flag_is_calculated()
      {
         A.CallTo(() => _view.SetParams(true, A<bool>.Ignored)).MustHaveHappened();
      }
   }

   public class When_options_set_no_dimensions : concern_for_UnitsEditorPresenter
   {
      protected override void Because()
      {
         sut.SetOptions(new Column() {Unit = new UnitDescription("min"), Dimension = A.Fake<IDimension>()},  new List<IDimension>() {}, A.Fake<IEnumerable<string>>());
      }

      [Observation]
      public void column_show_dimensions_flag_is_calculated()
      {
         A.CallTo(() => _view.SetParams(A<bool>.Ignored, false)).MustHaveHappened();
      }
   }

   public class When_options_set_with_dimensions : concern_for_UnitsEditorPresenter
   {
      protected override void Because()
      {
         sut.SetOptions(new Column() {Unit = new UnitDescription("min"), Dimension =DomainHelperForSpecs.TimeDimensionForSpecs() }, new List<IDimension>() {new Dimension(), new Dimension()}, A.Fake<IEnumerable<string>>());
      }

      [Observation]
      public void column_show_dimensions_flag_is_calculated()
      {
         A.CallTo(() => _view.SetParams(A<bool>.Ignored, true)).MustHaveHappened();
      }
   }
}
