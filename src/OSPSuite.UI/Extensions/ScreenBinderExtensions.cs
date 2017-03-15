using System.Collections.Generic;
using OSPSuite.DataBinding;
using DevExpress.XtraEditors;
using OSPSuite.UI.Binders;

namespace OSPSuite.UI.Extensions
{
   public static class ScreenBinderExtensions
   {
      public static TokenEditBinder<TObjectType, TValue> To<TObjectType, TValue>(this IScreenToElementBinder<TObjectType, IEnumerable<TValue>> screenToElementBinder, TokenEdit tokenEditControl)
      {
         var element = new TokenEditBinder<TObjectType, TValue>(screenToElementBinder.PropertyBinder, tokenEditControl);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }
   }
}