using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class ModuleNode : ObjectWithIdAndNameNode<Module>
   {
      public ModuleNode(Module module) : base(module)
      {
         Icon = ApplicationIcons.IconByName(module.Icon);
      }
   }
}