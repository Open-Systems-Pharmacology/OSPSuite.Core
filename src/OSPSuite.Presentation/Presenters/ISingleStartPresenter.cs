using System;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters
{
   
   /// <summary>
   ///    A presenter that should be started only once (edit presenter for a given entity for instance)
   /// </summary>
   public interface ISingleStartPresenter : IListener<RenamedEvent>, IEditPresenter
   {
      /// <summary>
      ///    This event is fired whenever the view is closing
      /// </summary>
      event EventHandler Closing;

      /// <summary>
      ///    this method is called from the view, whenever the form closes, either after 'x' or when CloseView is called from
      ///    the presenter itself
      /// </summary>
      void OnFormClosed();

      /// <summary>
      ///    This function should be called when the presenter is being closed programatically
      /// </summary>
      void Close();

     
      /// <summary>
      ///    Settings representing the current state of the presenter (e.g. which tab has been selected, colors etc..)
      /// </summary>
      IPresentationSettings GetSettings();

      /// <summary>
      ///    Restore the serialized settings in the presenter
      /// </summary>
      void RestoreSettings(IPresentationSettings settings);

    
      /// <summary>
      ///    Ensure that any value beind edited in the current presenter is being saved in the underlying model
      /// </summary>
      void SaveChanges();

      /// <summary>
      ///    This function should be called when the presenter is being activated
      /// </summary>
      void Activated();
   }

   //Marker interface 
   public interface ISingleStartPresenter<TSubject> : ISingleStartPresenter, IEditPresenter<TSubject>
   {
   }
}