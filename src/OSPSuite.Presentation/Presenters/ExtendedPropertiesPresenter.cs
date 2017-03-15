using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IExtendedPropertiesPresenter : IPresenter<IExtendedPropertiesView>
   {
      void Edit(ExtendedProperties extendedProperties);
      void SetPropertyValue(IExtendedProperty property, object newValue);
      bool ReadOnly { set; }
   }

   public class ExtendedPropertiesPresenter : AbstractPresenter<IExtendedPropertiesView, IExtendedPropertiesPresenter>, IExtendedPropertiesPresenter
   {
      public ExtendedPropertiesPresenter(IExtendedPropertiesView view) : base(view)
      {
      }

      public void Edit(ExtendedProperties extendedProperties)
      {
         if (extendedProperties == null) return;
         _view.BindTo(extendedProperties);
      }

      public void SetPropertyValue(IExtendedProperty property, object newValue)
      {
         property.ValueAsObject = newValue;
      }

      public bool ReadOnly
      {
         set { View.ReadOnly = value; }
      }
   }
}