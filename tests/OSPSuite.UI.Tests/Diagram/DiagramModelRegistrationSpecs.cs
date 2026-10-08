using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Container;
using OSPSuite.Presentation.Services;
using ContainerBaseLayouter = OSPSuite.Presentation.Diagram.Services.ContainerBaseLayouter;
using DiagramLayoutTask = OSPSuite.Presentation.Diagram.Services.DiagramLayoutTask;
using DiagramModelToXmlMapper = OSPSuite.Presentation.Diagram.Services.DiagramModelToXmlMapper;

namespace OSPSuite.UI.Diagram
{
   public abstract class concern_for_DiagramModelRegistration : ContextSpecification<IContainer>
   {
      protected override void Context()
      {
         sut = new CastleWindsorContainer();
         sut.RegisterImplementationOf(sut);
      }

      public override void Cleanup()
      {
         sut.Dispose();
      }

      protected void ShouldResolveTheUIFreeDiagram()
      {
         sut.Resolve<IDiagramModel>().ShouldBeAnInstanceOf<DiagramModel>();
         sut.Resolve<IDiagramModelToXmlMapper>().ShouldBeAnInstanceOf<DiagramModelToXmlMapper>();
         sut.Resolve<IContainerBaseLayouter>().ShouldBeAnInstanceOf<ContainerBaseLayouter>();
         sut.Resolve<IDiagramLayoutTask>().ShouldBeAnInstanceOf<DiagramLayoutTask>();
      }
   }

   public class When_resolving_the_diagram_model_of_the_application : concern_for_DiagramModelRegistration
   {
      protected override void Context()
      {
         base.Context();
         sut.AddRegister(x => x.FromType<PresenterRegister>());
         sut.AddRegister(x => x.FromType<UIRegister>());
      }

      [Observation]
      public void should_create_the_ui_free_diagram()
      {
         ShouldResolveTheUIFreeDiagram();
      }
   }

   public class When_resolving_the_diagram_model_of_an_application_registering_the_user_interface_first : concern_for_DiagramModelRegistration
   {
      protected override void Context()
      {
         base.Context();
         sut.AddRegister(x => x.FromType<UIRegister>());
         sut.AddRegister(x => x.FromType<PresenterRegister>());
      }

      [Observation]
      public void should_create_the_ui_free_diagram()
      {
         ShouldResolveTheUIFreeDiagram();
      }
   }
}
