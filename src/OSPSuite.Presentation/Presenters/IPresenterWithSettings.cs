using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters
{
   public interface IPresenterWithSettings
   {
      /// <summary>
      ///    Load presenter settings for this subject into the presenter
      /// </summary>
      /// <param name="subject">The subject of this presenter</param>
      void LoadSettingsForSubject(IWithId subject);

      /// <summary>
      ///    The unique key used to identify the presenter type.
      /// </summary>
      string PresentationKey { get; }
   }
}