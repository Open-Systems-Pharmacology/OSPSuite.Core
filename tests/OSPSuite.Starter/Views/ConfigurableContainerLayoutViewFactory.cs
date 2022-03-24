using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Views
{
   public interface IConfigurableContainerLayoutView : IView
   {
      /// <summary>
      ///    Adds finishing touches to the view once it's been configured
      /// </summary>
      void StartAddingViews();

      /// <summary>
      ///    Adds a view dynamically to the list of views shown
      /// </summary>
      /// <param name="view"></param>
      void AddView(IView view);

      /// <summary>
      ///    Adds finishing touches to the view once it's been configured
      /// </summary>
      void FinishedAddingViews();
   }

   public interface IAccordionLayoutView : IConfigurableContainerLayoutView
   {
   }

   public interface ITabbedLayoutView : IConfigurableContainerLayoutView
   {
   }

   public interface IConfigurableContainerLayoutViewFactory
   {
      IConfigurableContainerLayoutView Create();
   }

   public class ConfigurableContainerLayoutViewFactory : IConfigurableContainerLayoutViewFactory
   {
      private readonly IContainer _container;

      public ConfigurableContainerLayoutViewFactory(IContainer container)
      {
         _container = container;
      }

      public IConfigurableContainerLayoutView Create()
      {
         return _container.Resolve<IConfigurableContainerLayoutView>();
      }
   }
}