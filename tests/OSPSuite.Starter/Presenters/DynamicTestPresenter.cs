using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Starter.Presenters
{
   public interface IDynamicTestPresenter : IDisposablePresenter
   {
      void Start();
      void LoadViews();
   }

   public class DynamicTestPresenter : AbstractDisposablePresenter<IDynamicTestView, IDynamicTestPresenter>, IDynamicTestPresenter
   {
      private readonly IConfigurableLayoutPresenter _configurableLayoutPresenter;
      private readonly IContainer _container;
      private readonly IQuantityToQuantitySelectionDTOMapper _quantitySelectionDTOMapper;

      public DynamicTestPresenter(
         IDynamicTestView view, 
         IConfigurableLayoutPresenter configurableLayoutPresenter,
         IContainer container,
         IQuantityToQuantitySelectionDTOMapper quantitySelectionDTOMapper
         ) : base(view)
      {
         _configurableLayoutPresenter = configurableLayoutPresenter;
         _container = container;
         _quantitySelectionDTOMapper = quantitySelectionDTOMapper;
         _view.AddCollectorView(_configurableLayoutPresenter.BaseView);
      }

      public void Start()
      {
         View.Display();
      }

      public void LoadViews()
      {
         var views = new List<IResizableView>();
         var model = new TestEnvironment().Model;
         for (int i = 0; i < 2; i++)
         {
            var quantitySelectionDTOs = model.Root.GetAllChildren<IQuantity>(x=>x.Persistable)
               .MapAllUsing(_quantitySelectionDTOMapper);
           
            var view = _container.Resolve<ITestResizableView>();
            view.BindTo(quantitySelectionDTOs);
            views.Add(view);
         }
         _configurableLayoutPresenter.AddViews(views);


         views.Each(x=>x.AdjustHeight());
         views.Each(x=>x.Repaint());
      }
   }
}