using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public interface IValidatableDTO : IValidatable, INotifier
   {
      void AddNotifiableFor<TValidatable, TResult>(Expression<Func<TValidatable, TResult>> notifier) where TValidatable : IValidatableDTO;
   }

   public abstract class ValidatableDTO : Notifier, IValidatableDTO
   {
      public IBusinessRuleSet Rules { get; } = new BusinessRuleSet();

      public void AddNotifiableFor<TValidatable, TResult>(Expression<Func<TValidatable, TResult>> notifier) where TValidatable : IValidatableDTO
      {
         RaisePropertyChanged(notifier.Name());
      }
   }

   public abstract class ValidatableDTO<T> : ValidatableDTO where T : IValidatable, INotifier
   {
      protected ValidatableDTO(T underlyingObject)
      {
         this.AddRulesFrom(underlyingObject);
         underlyingObject.PropertyChanged += (o, e) => RaisePropertyChanged(e.PropertyName);
      }
   }
}