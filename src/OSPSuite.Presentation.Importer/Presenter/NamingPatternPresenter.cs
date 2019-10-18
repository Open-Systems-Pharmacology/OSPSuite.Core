using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.View;

namespace OSPSuite.Presentation.Presenter
{
   public interface INamingPatternPresenter : IPresenter<INamingPatternView>
   {
      void AddPresetNamingPatterns(string namePattern);

      /// <summary>
      /// The pattern to be used to rename repositories
      /// </summary>
      string Pattern { set; get; }

      string NamingPatternDescription { get; }

      /// <summary>
      /// Sets the <paramref name="token"/> for this instance of the naming pattern presenter.
      /// Used to set the description
      /// </summary>
      void WithToken(string token);
   }

   public class NamingPatternPresenter : AbstractPresenter<INamingPatternView, INamingPatternPresenter>, INamingPatternPresenter
   {
      private readonly List<string> _patterns;
      private string _token;

      public string Pattern { get; set; }

      public string NamingPatternDescription
      {
         get { return Captions.Importer.NamingPatternDescription(_token); }
      }

      public void WithToken(string token)
      {
         _token = token;
         _view.SetNamingPatternDescriptiveText();
      }

      public NamingPatternPresenter(INamingPatternView view) : base(view)
      {
         _patterns = new List<string>();
      }

      public void AddPresetNamingPatterns(string namePattern)
      {
         if (_patterns.Contains(namePattern)) return;

         _patterns.Add(namePattern);
         _view.UpdateNamingPatterns(_patterns);
      }
   }
}
