using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views
{
   public partial class ObjectBaseView : BaseModalView, IObjectBaseView
   {
      private readonly ScreenBinder<ObjectBaseDTO> _screenBinder;

      public ObjectBaseView(IShell shell): base(shell)
      {
         InitializeComponent();
         MaximizeBox = false;
         MinimizeBox = false;
         Load += (o, e) => activateText();
         _screenBinder = new ScreenBinder<ObjectBaseDTO>();
      }

      public string NameDescription
      {
         set { layoutItemName.Text = value.FormatForLabel(checkCase: false); }
         get { return layoutItemName.Text; }
      }

      public void AttachPresenter(IObjectBasePresenter presenter)
      {
         //nothing to do here
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(dto => dto.Name).To(tbName);
         _screenBinder.Bind(x => x.Description).To(tbDescription);
         RegisterValidationFor(_screenBinder);
      }

      public virtual void BindTo(ObjectBaseDTO objectBaseDTO)
      {
         _screenBinder.BindToSource(objectBaseDTO);
         SetOkButtonEnable();
      }

      public bool DescriptionVisible
      {
         set
         {
            layoutItemDescription.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            if(value) return;
            Height -= (layoutItemDescription.Height-layoutItemDescription.Padding.All);
         }
         get { return LayoutVisibilityConvertor.ToBoolean(layoutItemDescription.Visibility); }
      }

      public bool NameVisible
      {
         set
         {
            layoutItemName.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            if (value) return;
            Height -= (layoutItemName.Height - layoutItemName.Padding.All);
         }
         get { return LayoutVisibilityConvertor.ToBoolean(layoutItemName.Visibility); }
      }

      public bool NameEditable
      {
         set
         {
            tbName.Enabled = value;
            tbName.Properties.ReadOnly = !value;
         }
         get {return  tbName.Enabled; }
      }

      public bool NameDescriptionVisible
      {
         get { return layoutItemName.TextVisible; }
         set { layoutItemName.TextVisible = value; }
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Icon = ApplicationIcons.Rename;
         layoutItemName.Text = Captions.Rename.FormatForLabel();
         layoutItemDescription.Text = Captions.Description.FormatForLabel();
      }

      protected override void SetActiveControl()
      {
         ActiveControl = tbName;
      }

      private void activateText()
      {
         tbName.Select();
         tbName.Select(tbName.Text.Length, 0);
      }
   }
}