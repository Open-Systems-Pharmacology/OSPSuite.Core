using DevExpress.XtraRichEdit;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.UI
{
   public class UIRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<UIRegister>();

            //Exclude types that should be singleton or not at all
            scan.ExcludeType<ImageListRetriever>();
            scan.ExcludeType<SkinManager>();
            scan.ExcludeType<ExceptionView>();
            scan.ExcludeType<ToolTipCreator>();
         });

         //Register singleton objects
         container.Register<IExceptionView, ExceptionView>(LifeStyle.Singleton);
         container.Register<ISkinManager, SkinManager>(LifeStyle.Singleton);
         container.Register<IImageListRetriever, ImageListRetriever>(LifeStyle.Singleton);
         container.Register<IRichEditDocumentServer, RichEditDocumentServer>();

         //Register open types
         container.Register(typeof(PathElementsBinder<>), typeof(PathElementsBinder<>));
         container.Register(typeof(ValueOriginBinder<>), typeof(ValueOriginBinder<>));
      }
   }
}