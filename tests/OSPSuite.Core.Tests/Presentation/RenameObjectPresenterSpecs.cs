using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RenameObjectBasePresenter : ContextSpecification<IRenameObjectPresenter>
   {
      protected IObjectBaseView _view;
      protected IWithName _objectToRename;
      protected IObjectTypeResolver _objectTypeResolver;
      protected string _entityType;
      protected string _entityName;
      protected RenameObjectDTO _renameObjectDTO;
      protected IRenameObjectDTOFactory _renameObjectDTOFactory;

      protected override void Context()
      {
         _view = A.Fake<IObjectBaseView>();
         _objectToRename = A.Fake<IWithName>();
         _renameObjectDTOFactory = A.Fake<IRenameObjectDTOFactory>();
         _entityType = "type";
         _entityName = "tutu";
         _objectToRename.Name = _entityName;
         _renameObjectDTO = new RenameObjectDTO(_entityName);
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         A.CallTo(() => _objectTypeResolver.TypeFor(_objectToRename)).Returns(_entityType);
         A.CallTo(() => _renameObjectDTOFactory.CreateFor(_objectToRename)).Returns(_renameObjectDTO);
         sut = new RenameObjectPresenter(_view, _objectTypeResolver, _renameObjectDTOFactory);
      }
   }

   public class When_the_rename_presenter_is_told_to_rename_a_given_object_base : concern_for_RenameObjectBasePresenter
   {
      protected override void Because()
      {
         sut.Edit(_objectToRename);
      }

      [Observation]
      public void should_set_the_description_for_the_view_according_to_the_entity_type_and_name()
      {
         _view.NameDescription.ShouldBeEqualTo(Captions.RenameEntityCaption(_entityType, _entityName));
      }

      [Observation]
      public void should_intitialize_the_view_with_the_entity_info_representing_the_object_to_clone()
      {
         A.CallTo(() => _view.BindTo(_renameObjectDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_view_to_allow_the_user_to_give_a_new_name_for_the_clone()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }
   }

   public class When_the_user_decides_to_cancel_the_rename_process : concern_for_RenameObjectBasePresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      [Observation]
      public void should_return_false()
      {
         sut.Edit(_objectToRename).ShouldBeFalse();
      }
   }

   public class When_the_user_confirms_the_rename_process : concern_for_RenameObjectBasePresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         _renameObjectDTO.Name = "new name";
         sut.Edit(_objectToRename);
      }

      [Observation]
      public void the_clone_presenter_should_return_then_name_choosen_by_the_user()
      {
         sut.Name.ShouldBeEqualTo(_renameObjectDTO.Name);
      }
   }
}