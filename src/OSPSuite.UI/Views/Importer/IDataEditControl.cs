using System;

namespace OSPSuite.UI.Views.Importer
{
   /// <summary>
   /// Handler definition for IsDataValidChange event.
   /// </summary>
   public delegate void IsDataValidChangeHandler(object sender, EventArgs e);

   /// <summary>
   /// Interface definition for data editing controls.
   /// </summary>
   public interface IDataEditControl
   {
      /// <summary>
      /// Event raised if the data has been changed and validity status might get influenced. 
      /// </summary>
      event IsDataValidChangeHandler OnIsDataValidChanged;

      /// <summary>
      /// Property indicating if the data is valid.
      /// </summary>
      bool IsDataValid { get; set; }

      /// <summary>
      /// Method for accepting the entered data.
      /// </summary>
      void AcceptDataChanges();
   }
}