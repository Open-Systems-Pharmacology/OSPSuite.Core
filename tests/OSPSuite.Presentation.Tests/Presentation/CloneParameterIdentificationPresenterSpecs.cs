using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CloneParameterIdentificationPresenter : ContextSpecification<CloneObjectBasePresenter<ParameterIdentification>>
   {
      protected ICloneManagerForModel _cloneManager;
      private IRenameObjectDTOFactory _renameObjectDTOFactory;
      private IObjectTypeResolver _objectTypeResolver;
      private IObjectBaseView _view;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {

         _cloneManager = A.Fake<ICloneManagerForModel>();
         _renameObjectDTOFactory = A.Fake<IRenameObjectDTOFactory>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _view = A.Fake<IObjectBaseView>();
         _parameterIdentification = A.Fake<ParameterIdentification>();

         sut = new CloneObjectBasePresenter<ParameterIdentification>(_view, _objectTypeResolver, _renameObjectDTOFactory, _cloneManager);
      }
   }

   public class When_cloning_a_loaded_parameter_identification_using_the_clone_presenter : concern_for_CloneParameterIdentificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         _parameterIdentification.IsLoaded = true;
      }

      protected override void Because()
      {
         sut.CreateCloneFor(_parameterIdentification);
      }

      [Observation]
      public void the_clone_manager_must_be_used_to_clone_the_identification()
      {
         A.CallTo(() => _cloneManager.Clone(_parameterIdentification)).MustHaveHappened();
      }
   }
}
