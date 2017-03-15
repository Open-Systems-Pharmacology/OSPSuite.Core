using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ActivateSkinCommand : ContextSpecification<ActivateSkinCommand>
   {
      protected ISkinManager _skinManager;
      protected IPresentationUserSettings _userSettings;

      protected override void Context()
      {
         _skinManager = A.Fake<ISkinManager>();
         _userSettings = A.Fake<IPresentationUserSettings>();
         sut = new ActivateSkinCommand(_userSettings, _skinManager);
      }
   }

   public class When_changing_the_active_skin : concern_for_ActivateSkinCommand
   {
      protected override void Context()
      {
         base.Context();
         sut.SkinName = "tralala";
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_skin_manager_to_activate_the_skin()
      {
         A.CallTo(() => _skinManager.ActivateSkin(_userSettings, sut.SkinName)).MustHaveHappened();
      }
   }
}