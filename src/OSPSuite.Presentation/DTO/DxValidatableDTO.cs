using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using DevExpress.XtraEditors.DXErrorProvider;

namespace OSPSuite.Presentation.DTO
{
     public abstract class DxValidatableDTO : ValidatableDTO , IDXDataErrorInfo
   {
      public virtual void GetPropertyError(string propertyName, ErrorInfo info)
      {
         this.UpdatePropertyError(propertyName, info);
      }

      public virtual void GetError(ErrorInfo info)
      {
         this.UpdateError(info);
      }     
   }

   public abstract class DxValidatableDTO<T> : DxValidatableDTO where T : IValidatable, INotifier
   {
      protected DxValidatableDTO(T underlyingObject)
      {
         this.AddRulesFrom(underlyingObject);
         underlyingObject.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
      }
   }
}