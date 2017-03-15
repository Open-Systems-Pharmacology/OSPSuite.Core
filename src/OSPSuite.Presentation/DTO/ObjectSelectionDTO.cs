using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class ObjectSelectionDTO<TObjectBase> : Notifier where TObjectBase : IObjectBase
   {
      public TObjectBase Object { get; private set; }

      public ObjectSelectionDTO(TObjectBase objectBase)
      {
         Object = objectBase;
      }

      private bool _selected;

      public virtual bool Selected
      {
         get { return _selected; }
         set
         {
            if (_selected == value)
               return;

            _selected = value;
            OnPropertyChanged(() => Selected);
         }
      }

      public string Name
      {
         get { return Object.Name; }
      }
   }

   public class ObjectSelectionDTO : ObjectSelectionDTO<IObjectBase>
   {
      public ObjectSelectionDTO(IObjectBase objectBase)
         : base(objectBase)
      {
      }
   }

}