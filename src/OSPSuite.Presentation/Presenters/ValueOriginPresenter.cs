using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IValueOriginPresenter : IDisposablePresenter
   {
      void Edit(ValueOrigin valueOrigin);
      void Save();
      IEnumerable<ValueOriginType> AllValueOrigins { get; }
   }

   public class ValueOriginPresenter : AbstractDisposablePresenter<IValueOriginView, IValueOriginPresenter>, IValueOriginPresenter
   {
      private ValueOrigin _valueOriginToEdit;
      private readonly ValueOrigin _valueOriginDTO;
      private readonly List<ValueOriginType> _allValueOrigins = new List<ValueOriginType>();

      public ValueOriginPresenter(IValueOriginView view) : base(view)
      {
         _valueOriginDTO = new ValueOrigin();
         _allValueOrigins.AddRange(ValueOriginTypes.AllValueOrigins.Except(new[] {ValueOriginTypes.Undefined}));
      }

      public void Edit(ValueOrigin valueOrigin)
      {
         _valueOriginToEdit = valueOrigin;
         _valueOriginDTO.UpdateFrom(_valueOriginToEdit);
         _view.BindTo(_valueOriginDTO);
      }

      public void Save()
      {
         _view.Save();
         updateUndefinedValueOriginType();
         _valueOriginToEdit.UpdateFrom(_valueOriginDTO);
      }

      private void updateUndefinedValueOriginType()
      {
         if (_valueOriginDTO.Type != ValueOriginTypes.Undefined)
            return;

         if (string.IsNullOrWhiteSpace(_valueOriginDTO.Description))
            return;

         _valueOriginDTO.Type = ValueOriginTypes.Unknown;
      }

      public IEnumerable<ValueOriginType> AllValueOrigins => _allValueOrigins;
   }
}