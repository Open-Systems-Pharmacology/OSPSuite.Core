using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTabbedMdi;

namespace OSPSuite.Starter
{
   public class DxContainer
   {
      public RibbonBarManager RibbonBarManager { get; set; }  
      public BarManager BarManager { get; set; }  
      public UserLookAndFeel UserLookAndFeel { get; set; }  
      public XtraTabbedMdiManager XtraTabbedMdiManager { get; set; }  
      public ApplicationMenu ApplicationMenu { get; set; }  
      public PanelControl PanelControl { get; set; }  
      public RibbonControl RibbonControl { get; set; }  
   }
}