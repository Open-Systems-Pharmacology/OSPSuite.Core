using OSPSuite.Utility;
using DevExpress.XtraBars.Ribbon;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Mappers
{
   public interface IButtonGroupToRibbonPageGroupMapper : IMapper<IButtonGroup, RibbonPageGroup>
   {
   }

   public class ButtonGroupToRibbonPageGroupMapper : IButtonGroupToRibbonPageGroupMapper
   {
      public RibbonPageGroup MapFrom(IButtonGroup buttonGroup)
      {
         return new RibbonPageGroup(buttonGroup.Caption) { ShowCaptionButton = false };
      }
   }
}