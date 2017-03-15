using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_PresenterSettingsTask : ContextSpecification<IPresentationSettingsTask>
   {
      protected IWithId _subject;
      protected IWithWorkspaceLayout _workspace;

      protected override void Context()
      {
         _workspace = A.Fake<IWithWorkspaceLayout>();
         sut = new PresentationSettingsTask(_workspace);
         _subject = A.Fake<IWithId>();
      }
   }

   public class when_removing_presenter_settings_for_a_subject : concern_for_PresenterSettingsTask
   {
      private IList<WorkspaceLayoutItem> _layoutItems;

      protected override void Context()
      {
         base.Context();
         _layoutItems = new List<WorkspaceLayoutItem>();
         A.CallTo(() => _subject.Id).Returns("subjectId");
         A.CallTo(() => _workspace.WorkspaceLayout.AddLayoutItem(A<WorkspaceLayoutItem>._)).Invokes(x => _layoutItems.Add(x.GetArgument<WorkspaceLayoutItem>(0)));
         A.CallTo(() => _workspace.WorkspaceLayout.RemoveLayoutItem(A<WorkspaceLayoutItem>._)).Invokes(x => _layoutItems.Remove(x.GetArgument<WorkspaceLayoutItem>(0)));
         A.CallTo(() => _workspace.WorkspaceLayout.LayoutItems).Returns(_layoutItems);
         sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), _subject);
         sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), _subject);
         sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), A.Fake<IWithId>());
      }

      protected override void Because()
      {
         sut.RemovePresentationSettingsFor(_subject);
      }

      [Observation]
      public void should_not_remove_settings_on_layout_items_that_are_not_related_to_subject()
      {
         _layoutItems.Where(x => string.Equals(x.SubjectId, _subject.Id)).ShouldBeEmpty();
      }

      [Observation]
      public void should_remove_presenter_settings_on_layout_items_related_to_subject()
      {
         _layoutItems.Count(x => !string.Equals(x.SubjectId, _subject.Id)).ShouldBeEqualTo(1);
      }
   }

   public class when_retrieving_presenter_settings_with_a_new_presenter_whose_type_was_already_used : concern_for_PresenterSettingsTask
   {
      private IPresentationSettings _result;
      private IPresentationSettings _initial;
      private List<WorkspaceLayoutItem> _items;
      private IWorkspaceLayout _layout;

      protected override void Context()
      {
         base.Context();
         _items = new List<WorkspaceLayoutItem>();
         _layout = A.Fake<IWorkspaceLayout>();

         A.CallTo(() => _layout.LayoutItems).Returns(_items);
         A.CallTo(() => _workspace.WorkspaceLayout).Returns(_layout);
         A.CallTo(() => _layout.AddLayoutItem(A<WorkspaceLayoutItem>._)).Invokes(x => _items.Add(x.Arguments.Get<WorkspaceLayoutItem>(0)));
         _initial = sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), _subject);
      }

      protected override void Because()
      {
         _result = sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), _subject);
      }

      [Observation]
      public void must_return_the_same_settings_object_when_using_a_new_presenter_of_same_type()
      {
         ReferenceEquals(_initial, _result).ShouldBeTrue();
      }
   }

   public class when_retrieving_presenter_settings_for_presenter_and_subject_that_were_not_registered : concern_for_PresenterSettingsTask
   {
      private IPresentationSettings _result;

      protected override void Because()
      {
         _result = sut.PresentationSettingsFor<DefaultPresentationSettings>(A.Fake<IPresenterWithSettings>(), _subject);
      }

      [Observation]
      public void should_return_new_settings_object()
      {
         _result.ShouldNotBeNull();
      }
   }
}
