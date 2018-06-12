using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IValueOriginPresenter : IDisposablePresenter
   {
      ValueOrigin UpdatedValueOrigin { get; }
      bool ValueOriginChanged { get; }
      void Edit(ValueOrigin valueOrigin);
      void Save();
      IReadOnlyList<ValueOriginSource> AllValueOriginSources { get; }
      IReadOnlyList<ValueOriginDeterminationMethod> AllValueOriginDeterminationMethods { get; }
   }

   public class ValueOriginPresenter : AbstractDisposablePresenter<IValueOriginView, IValueOriginPresenter>, IValueOriginPresenter
   {
      private readonly ValueOrigin _valueOriginDTO;
      private readonly List<ValueOriginSource> _allValueOriginSources = new List<ValueOriginSource>();
      private readonly List<ValueOriginDeterminationMethod> _allValueOriginDeterminationMethods = new List<ValueOriginDeterminationMethod>();
      private ValueOrigin _valueOrigin;

      public IReadOnlyList<ValueOriginSource> AllValueOriginSources => _allValueOriginSources;

      public IReadOnlyList<ValueOriginDeterminationMethod> AllValueOriginDeterminationMethods => _allValueOriginDeterminationMethods;

      public ValueOriginPresenter(IValueOriginView view) : base(view)
      {
         _valueOriginDTO = new ValueOrigin();
         _allValueOriginSources.AddRange(ValueOriginSources.All.Except(new[] {ValueOriginSources.Undefined}));
         _allValueOriginDeterminationMethods.AddRange(ValueOriginDeterminationMethods.All.Except(new[] {ValueOriginDeterminationMethods.Undefined}));
      }

      public void Edit(ValueOrigin valueOrigin)
      {
         _valueOrigin = valueOrigin;
         _valueOriginDTO.UpdateFrom(valueOrigin);
         _view.BindTo(_valueOriginDTO);
      }

      public void Save()
      {
         _view.Save();
         updateUndefinedValueOriginSource();
      }

      private void updateUndefinedValueOriginSource()
      {
         if (_valueOriginDTO.Source != ValueOriginSources.Undefined)
            return;

         if (string.IsNullOrWhiteSpace(_valueOriginDTO.Description))
            return;

         _valueOriginDTO.Source = ValueOriginSources.Unknown;
      }

      public ValueOrigin UpdatedValueOrigin
      {
         get
         {
            Save();
            return _valueOriginDTO;
         }
      }

      public bool ValueOriginChanged
      {
         get
         {
            Save();
            return !areEquivalent(_valueOrigin, _valueOriginDTO);
         }
      }

      private bool areEquivalent(ValueOrigin valueOrigin, ValueOrigin valueOriginDTO)
      {
         return sameDescription(valueOrigin.Description, valueOriginDTO.Description) &&
                valueOrigin.Source == valueOriginDTO.Source &&
                valueOrigin.Method == valueOriginDTO.Method;
      }

      private bool sameDescription(string description1, string description2)
      {
         if (string.IsNullOrWhiteSpace(description1) && string.IsNullOrWhiteSpace(description2))
            return true;

         return string.Equals(description1, description2);
      }
   }
}