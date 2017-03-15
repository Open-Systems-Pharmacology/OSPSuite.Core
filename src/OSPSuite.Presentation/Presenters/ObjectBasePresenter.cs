using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IObjectBasePresenter : IPresenter<IObjectBaseView>, IDisposablePresenter
   {
   }

   public interface IObjectBasePresenter<TObject> : IObjectBasePresenter
   {
      /// <summary>
      ///   Name entered by the user
      /// </summary>
      string Name { get; }

      /// <summary>
      ///   Descripton entered by the user
      /// </summary>
      string Description { get; }

      /// <summary>
      ///   Create a name for the object context: It could be a new name for an alternative in the context of a parameter group
      ///   or another name for an entity in the context of the entity itself (clone)
      /// </summary>
      /// <returns>true if the edit was completed successfuly otherwise false</returns>
      bool Edit(TObject objectContext);
   }

   public abstract class ObjectBasePresenter<TObject> : AbstractDisposablePresenter<IObjectBaseView, IObjectBasePresenter>, IObjectBasePresenter<TObject>
   {
      private ObjectBaseDTO _objectBaseDTO;

      /// <summary>
      /// Constructor setting the name and description visible by default
      /// </summary>
      protected ObjectBasePresenter(IObjectBaseView view)
         : this(view, true, true)
      {
      }

      /// <summary>
      ///  Constructor setting the name visible by default
      /// </summary>
      protected ObjectBasePresenter(IObjectBaseView view, bool descriptionVisible)
         : this(view, descriptionVisible, true)
      {
      }

      protected ObjectBasePresenter(IObjectBaseView view, bool descriptionVisible, bool nameVisible)
         : base(view)
      {
         //Default object base to ensure a define state at all time
         _objectBaseDTO = new ObjectBaseDTO();
         _view.DescriptionVisible = descriptionVisible;
         _view.NameVisible = nameVisible;
      }

      public bool Edit(TObject objectContext)
      {
         _objectBaseDTO = CreateDTOFor(objectContext);
         InitializeResourcesFor(objectContext);
         _view.BindTo(_objectBaseDTO);
         _view.Display();

         return !_view.Canceled;
      }

      public string Name
      {
         get { return _objectBaseDTO.Name; }
      }

      public string Description
      {
         get { return _objectBaseDTO.Description; }
      }

      protected abstract void InitializeResourcesFor(TObject objectContext);

      protected abstract ObjectBaseDTO CreateDTOFor(TObject entity);
   }
}