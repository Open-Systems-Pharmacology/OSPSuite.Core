using OSPSuite.Presentation.Views.Importer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IMetaDataParameterEditorPresenter : IDisposablePresenter
   {
      string Input { get; }
   }

   public class MetaDataParameterEditorPresenter : AbstractDisposablePresenter<IMetaDataParameterEditorView, IMetaDataParameterEditorPresenter>, IMetaDataParameterEditorPresenter
   {
      public string Input => View.Input;

      public void InitView()
      {
         
      }

      public MetaDataParameterEditorPresenter(
         IMetaDataParameterEditorView view
      ) : base(view)
      {
      }
   }
}
