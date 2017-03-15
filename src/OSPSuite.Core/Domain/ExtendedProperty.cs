using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Domain
{
   public interface IExtendedProperty : INotifier, IWithName
   {
      bool ReadOnly { get; set; }
      object ValueAsObject { get; set; }
      IReadOnlyList<object> ListOfValuesAsObjects { get; }
      IExtendedProperty Clone();
      Type Type { get; }
      string Description { set; get; }
      string FullName { get; set; }
      string DisplayName { get; }
   }

   public interface IExtendedProperty<T> : IExtendedProperty
   {
      T Value { get; set; }
      IReadOnlyList<T> ListOfValues { get; }
   }

   public class ExtendedProperty<T> : Notifier, IExtendedProperty<T>
   {
      private readonly List<T> _listOfValues = new List<T>();
      private string _name;
      private bool _readOnly;
      private string _description;
      private string _fullName;

      public string Description
      {
         set
         {
            _description = value;
            OnPropertyChanged();
         }
         get { return _description; }
      }

      public string DisplayName => FullName?? Name;

      public string FullName
      {
         get { return _fullName; }
         set
         {
            _fullName = value;
            OnPropertyChanged();
            OnPropertyChanged(() => DisplayName);
         }
      }

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged();
            if(string.IsNullOrEmpty(FullName))
               OnPropertyChanged(() => DisplayName);
         }
      }

      public IReadOnlyList<T> ListOfValues => _listOfValues;
      public IReadOnlyList<object> ListOfValuesAsObjects => _listOfValues.Cast<object>().ToList();

      public void AddToListOfValues(T value)
      {
         _listOfValues.Add(value);
      }

      public object ValueAsObject
      {
         get
         {
            return Value;
         }
         set
         {
            Value = value.ConvertedTo<T>();
         }
      }

      public IExtendedProperty Clone()
      {
         var extendedProperty = new ExtendedProperty<T> { Name = Name, Value = Value, ReadOnly = ReadOnly, Description = Description, FullName = FullName};
         ListOfValues.Each(property => extendedProperty.AddToListOfValues(property));
         return extendedProperty;
      }

      public Type Type => typeof (T);

      public bool ReadOnly
      {
         set
         {
            _readOnly = value;
            OnPropertyChanged(() => ReadOnly);
         }
         get { return _readOnly; }
      }

      private T _value;

      public T Value
      {
         get { return _value; }
         set
         {
            _value = value;
            OnPropertyChanged(() => Value);
         }
      }
   }
}