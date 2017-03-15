using System;
using System.Linq.Expressions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Chart
{
   public interface INotifying
   {
      void DoRaisePropertyChanged(string propertyName);
   }

   public class ErrorInfos<TObject>
   {
      private readonly INotifying _obj;
      private readonly Cache<string, string> _propertyErrorInfos;

      public ErrorInfos(INotifying obj)
      {
         _obj = obj;
         _propertyErrorInfos = new Cache<string, string>(onMissingKey: s => string.Empty);
      }

      public string Global
      {
         get
         {
            if (_propertyErrorInfos.Count != 0)
               return "Inconsistent to axes dimensions";

            return string.Empty;
         }
      }

      public string this[Expression<Func<TObject, object>> property]
      {
         set { this[ReflectionHelper.PropertyFor(property).Name] = value; }
         get { return this[ReflectionHelper.PropertyFor(property).Name]; }
      }

      public string this[string propertyName]
      {
         set
         {
            if (value == string.Empty && _propertyErrorInfos.Contains(propertyName))
            {
               _propertyErrorInfos.Remove(propertyName);
               _obj.DoRaisePropertyChanged(propertyName);
            }
            else
            {
               if (string.Equals(_propertyErrorInfos[propertyName], value))
                  return;

               _propertyErrorInfos[propertyName] = value;
               _obj.DoRaisePropertyChanged(propertyName);
            }
         }
         get { return _propertyErrorInfos[propertyName]; }
      }
   }
}