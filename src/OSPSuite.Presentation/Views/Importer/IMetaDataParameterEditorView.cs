using OSPSuite.Presentation.Presenters.Importer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IMetaDataParameterEditorView : IView<IMetaDataParameterEditorPresenter>
   {
      string Input { get; }
   }
}
