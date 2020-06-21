using DevExpress.XtraRichEdit;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.UI.Services
{
   public interface IRichEditDocumentServerFactory
   {
      IRichEditDocumentServer Create();
   }

   class RichEditDocumentServerFactory : DynamicFactory<IRichEditDocumentServer>, IRichEditDocumentServerFactory
   {
      public RichEditDocumentServerFactory(IContainer container) : base(container)
      {
      }
   }
}