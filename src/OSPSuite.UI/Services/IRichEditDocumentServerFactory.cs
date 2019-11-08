using DevExpress.XtraRichEdit;
using OSPSuite.Core.Services;

namespace OSPSuite.UI.Services
{
   public interface IRichEditDocumentServerFactory
   {
      IRichEditDocumentServer Create();
   }

   class RichEditDocumentServerFactory : DynamicFactory<IRichEditDocumentServer>, IRichEditDocumentServerFactory
   {
   }
}