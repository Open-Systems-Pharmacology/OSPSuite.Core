using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class ConcernForSourceFilePresenter : ContextSpecification<SourceFilePresenter>
   {
      protected IDialogCreator _dialogCreator;
      protected ISourceFileControl _view;
      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _view = A.Fake<ISourceFileControl>();
         sut = new SourceFilePresenter(_dialogCreator, _view);
      }
   }

   public class When_show_dialog : ConcernForSourceFilePresenter
   {
      [Observation]
      public void could_be_intercepted()
      {
         sut.CheckBeforeSelectFile = () => true;
         sut.OpenFileDialog("");

         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

         sut.CheckBeforeSelectFile = () => false;
         sut.OpenFileDialog("");

         A.CallTo(() => _dialogCreator.AskForFileToOpen(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
      }
   }
}
