using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class ModuleNode : ObjectWithIdAndNameNode<ClassifiableModule>
   {
      public ModuleNode(ClassifiableModule classifiable) : base(classifiable)
      {
         Icon = ApplicationIcons.IconByName(classifiable.Icon);
      }
   }
}