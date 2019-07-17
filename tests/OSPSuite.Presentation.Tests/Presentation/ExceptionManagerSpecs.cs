using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ExceptionManager : ContextSpecification<IExceptionManager>
   {
      protected IDialogCreator _dialogCreator;
      protected IExceptionView _exceptionView;
      protected IApplicationConfiguration _configuration;
      protected ILogger _logger;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _exceptionView = A.Fake<IExceptionView>();
         _configuration = A.Fake<IApplicationConfiguration>();
         _logger= A.Fake<ILogger>();
         sut = new ExceptionManager(_dialogCreator, _exceptionView, _configuration, _logger);
      }
   }

   public class When_showing_an_osp_suite_exception : concern_for_ExceptionManager
   {
      private Exception _exceptionToShow;
      private Exception _childException;
      private string _message;

      protected override void Context()
      {
         base.Context();
         _childException = new OSPSuiteException("ChildMessageError");
         _exceptionToShow = new OSPSuiteException("ParentMessageError", _childException);
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>.Ignored)).Invokes(
            x => _message = x.GetArgument<string>(0));
      }

      protected override void Because()
      {
         sut.LogException(_exceptionToShow);
      }

      [Observation]
      public void should_leverage_the_dialog_creator_to_display_an_error_message_containg_the_exception_message()
      {
         _message.Contains("ParentMessageError").ShouldBeTrue();
         _message.Contains("ChildMessageError").ShouldBeTrue();
      }
   }

   public class When_show_a_generic_exception : concern_for_ExceptionManager
   {
      private Exception _exceptionToShow;
      private string _message;
      private string _stackTrace;
      private string _clipboard;

      protected override void Context()
      {
         base.Context();
         _exceptionToShow = new Exception("Error");
         A.CallTo(() => _exceptionView.Display(A<string>._, A<string>._, A<string>._))
            .Invokes(x =>
            {
               _message = x.GetArgument<string>(0);
               _stackTrace = x.GetArgument<string>(1);
               _clipboard = x.GetArgument<string>(2);
            });
      }

      protected override void Because()
      {
         sut.LogException(_exceptionToShow);
      }

      [Observation]
      public void should_leverage_the_exception_view_to_display_an_error_message_containg_the_exception_message()
      {
         _message.ShouldBeEqualTo(_exceptionToShow.FullMessage());
         _stackTrace.ShouldBeEqualTo(_exceptionToShow.FullStackTrace());
         _clipboard.Contains($"```\n{_exceptionToShow.FullStackTrace()}\n```").ShouldBeTrue();
      }
   }
}