using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Importer
{
   public partial class MetaDataEditControl : XtraUserControl, IDataEditControl
   {
      private readonly MetaDataTable _data;
      private SimpleButton _copyButton;
      private const int TEXT_LENGTH_FOR_MEMO_EDITOR = 40;
      private const string INFINITY_SIGN = "∞";
      /// <summary>
      /// This is a property which can be used to enable auto acceptance of changes.
      /// </summary>
      public bool AutoAcceptChanges = false;

      public MetaDataEditControl(MetaDataTable data)
      {
         InitializeComponent();
         _data = data;
         initialize(0, true);
      }

      public MetaDataEditControl(MetaDataTable data, int rowIndex, bool applyToAllButton)
      {
         InitializeComponent();
         _data = data;
         initialize(rowIndex, applyToAllButton);
      }

      private void initialize(int rowIndex, bool applyToAllButton)
      {
         IsDataValid = false;
         createEditors(rowIndex, applyToAllButton);
         checkData();
         if (IsDataValid) AcceptDataChanges();
      }

      private ImageList getImageList(Dictionary<string, ApplicationIcon> icons, IconSize iconSize)
      {
         if (icons == null || icons.Count == 0) return null;
         var imageList = new ImageList { ImageSize = iconSize };
         foreach (var image in icons)
            imageList.Images.Add(image.Key, image.Value.WithSize(iconSize));
         return imageList;
      }

      private int getImageIndex(ImageList imageList, string valueKey)
      {
         int imageIndex = -1;
         if (imageList == null) return imageIndex;
         if (imageList.Images.ContainsKey(valueKey))
            imageIndex = imageList.Images.IndexOfKey(valueKey);
         return imageIndex;
      }

      private void createEditors(int rowIndex, bool applyToAllButton)
      {
         var layoutControl = new LayoutControl();
         Controls.Add(layoutControl);
         layoutControl.Dock = DockStyle.Fill;
         layoutControl.AllowCustomization = false;

         foreach (MetaDataColumn col in _data.Columns)
         {
            LayoutControlItem colItem = layoutControl.Root.AddItem();
            colItem.Name = col.DisplayName.FormatForLabel();
            colItem.ControlName = col.DisplayName;
            BaseEdit editor;

            if (col.DataType == typeof(string))
            {
               if (col.ListOfValues != null && col.ListOfValues.Count > 0)
               {
                  if (col.IsListOfValuesFixed)
                  {
                     editor = new ImageComboBoxEdit();
                     var cb = editor as ImageComboBoxEdit;
                     var imageListSmall = getImageList(col.ListOfImages, IconSizes.Size16x16);
                     var imageListLarge = getImageList(col.ListOfImages, IconSizes.Size32x32);
                     cb.Properties.SmallImages = imageListSmall;
                     cb.Properties.LargeImages = imageListLarge;
                     foreach (var s in col.ListOfValues)
                        cb.Properties.Items.Add(new ImageComboBoxItem(s.Value, s.Key, getImageIndex(imageListLarge, s.Key)));
                     cb.Properties.AutoComplete = true;
                     cb.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
                     setEditorProperties(cb.Properties, col);
                     cb.Properties.CloseUpKey = new KeyShortcut(Keys.Enter);
                     cb.SelectedValueChanged += onSelectedValueChanged;
                  }
                  else
                  {
                     editor = new MRUEdit();
                     var cb = editor as MRUEdit;
                     foreach (var s in col.ListOfValues)
                        cb.Properties.Items.Add(s.Value);
                     cb.Properties.AutoComplete = true;
                     cb.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
                     setEditorProperties(cb.Properties, col);
                     cb.Properties.CloseUpKey = new KeyShortcut(Keys.Enter);
                     cb.SelectedValueChanged += onSelectedValueChanged;
                     cb.Properties.AllowRemoveMRUItems = false;
                  }
               }
               else
               {
                  if (col.MaxLength > TEXT_LENGTH_FOR_MEMO_EDITOR)
                  {
                     editor = new MemoExEdit();
                     var me = editor as MemoExEdit;
                     me.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
                     me.Properties.MaxLength = maxLengthFor(col);
                     setEditorProperties(me.Properties, col);
                     me.Properties.CloseUpKey = new KeyShortcut(Keys.Enter);
                  }
                  else
                  {
                     editor = new TextEdit();
                     var te = editor as TextEdit;
                     te.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
                     te.Properties.MaxLength = maxLengthFor(col);
                     setEditorProperties(te.Properties, col);
                  }
               }
            }
            else if (col.DataType == typeof(DateTime))
            {
               editor = new DateEdit();
               var de = editor as DateEdit;
               de.Properties.ShowClear = !col.Required;
               setEditorProperties(de.Properties, col);
               de.Properties.CloseUpKey = new KeyShortcut(Keys.Enter);
            }

            else if (col.DataType == typeof(bool))
            {
               editor = new CheckEdit();
               var ce = editor as CheckEdit;
               ce.Properties.Caption = col.DisplayName;
               ce.Properties.GlyphAlignment = HorzAlignment.Far;
               ce.Properties.NullStyle = StyleIndeterminate.Inactive;
               ce.Properties.AllowGrayed = !col.Required;
               setEditorProperties(ce.Properties, col);
            }
            else if (col.DataType == typeof(double))
            {
               editor = new TextEdit();
               var te = editor as TextEdit;
               te.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
               // te.Properties.MaxLength = col.MaxLength;
               setEditorProperties(te.Properties, col);
               te.Properties.Mask.MaskType = MaskType.RegEx;
               te.Properties.Mask.EditMask = RegularExpression.Numeric;
            }
            else if (col.DataType == typeof(int))
            {
               editor = new SpinEdit();
               var se = editor as SpinEdit;
               if (col.MinValue != null)
                  if (col.MinValueAllowed)
                     se.Properties.MinValue = (decimal)col.MinValue;
                  else
                     se.Properties.MinValue = (decimal)col.MinValue + 1;
               if (col.MaxValue != null)
                  if (col.MaxValueAllowed)
                     se.Properties.MaxValue = (decimal)col.MaxValue;
                  else
                     se.Properties.MaxValue = (decimal)col.MaxValue - 1;
               se.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
               setEditorProperties(se.Properties, col);
            }
            else
            {
               editor = new TextEdit();
               var te = editor as TextEdit;
               te.Properties.AllowNullInput = col.Required ? DefaultBoolean.False : DefaultBoolean.True;
               te.Properties.MaxLength = col.MaxLength;
               setEditorProperties(te.Properties, col);
            }

            if (col.Required)
               editor.BackColor = Color.LightYellow;
            editor.Validating += onEditorValidating;
            editor.TextChanged += OnEditorTextChanged;
            editor.EditValueChanged += OnEditorTextChanged;
            editor.Validated += onEditorValidated;

            editor.ToolTipController = new ToolTipController();
            editor.SuperTip = new SuperToolTip();
            editor.SuperTip.Items.AddTitle(col.DisplayName);
            editor.SuperTip.Items.Add(col.Description);

            //add information about ranges to the tool tip
            if (col.MinValue != null || col.MaxValue != null)
            {
               var lowerBound = col.MinValue == null ? INFINITY_SIGN : (col.MinValueAllowed) ? "[" : "]";
               var lowerValue = col.MinValue == null ? String.Empty : col.MinValue.ToString();
               var upperValue = col.MaxValue == null ? String.Empty : col.MaxValue.ToString();
               var upperBound = col.MaxValue == null ? INFINITY_SIGN : (col.MaxValueAllowed) ? "]" : "[";
               var text = $"Valid values must be within range {lowerBound}{lowerValue};{upperValue}{upperBound}.";
               editor.SuperTip.Items.Add(text);
            }

            editor.Name = col.ColumnName;
            editor.DataBindings.Add(new Binding("EditValue", _data, col.ColumnName));
            editor.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            if (shouldSetDefaultValue(col))
               editor.EditValue = col.DefaultValue;

            colItem.Control = editor;

            var btnColItem = layoutControl.Root.AddItem();
            btnColItem.Move(colItem, InsertType.Right);
            var button = createThisMetaDataValueApplyToAllButton(editor);

            button.Enabled = !col.IsColumnUsedForGrouping;

            btnColItem.Control = button;

            btnColItem.TextVisible = false;
         }

         // bind to given row index
         if (_data.Rows.Count > 0 && rowIndex >= 0 && rowIndex < _data.Rows.Count)
            BindingContext[_data].Position = rowIndex;

         if (!applyToAllButton) return;
         _copyButton = new UxSimpleButton();
         var copyItem = layoutControl.Root.AddItem();
         copyItem.Control = _copyButton;
         _copyButton.Name = "btnApplyToAll";
         _copyButton.Text = Captions.Importer.ApplyToAll;
         _copyButton.ToolTip = Captions.Importer.ApplyToAll;
         _copyButton.Image = ApplicationIcons.ApplyAll;
         _copyButton.Enabled = IsDataValid;
         _copyButton.Click += onCopyButtonClick;
         copyItem.TextVisible = false;
      }

      private static bool shouldSetDefaultValue(MetaDataColumn metaDataColumn)
      {
         var hasDefaultValue = metaDataColumn.DefaultValue != null && metaDataColumn.DefaultValue != DBNull.Value;
         return hasDefaultValue && (!metaDataColumn.IsListOfValuesFixed || metaDataColumn.ListOfValues.ContainsValue(metaDataColumn.DefaultValue.ToString()));
      }

      private int maxLengthFor(MetaDataColumn col)
      {
         return col.MaxLength > 0 ? col.MaxLength : 0;
      }

      private Control createThisMetaDataValueApplyToAllButton(BaseEdit editor)
      {
         var button = new SimpleButton { Image = ApplicationIcons.ApplyAll };
         button.MaximumSize = button.CalcBestSize();
         button.MinimumSize = button.MaximumSize;
         button.ToolTip = Captions.Importer.ApplyToAll;
         button.Click += (sender, args) => updateAllOtherDataTables(editor.Name, editor.EditValue);
         return button;
      }



      private void updateAllOtherDataTables(string name, object editValue)
      {
         OnBroadCastMetaData?.Invoke(this, new BroadcastMetaDataEventArgs { Name = name, Value = editValue });
      }

      private void onSelectedValueChanged(object sender, EventArgs e)
      {
         var editor = sender as BaseEdit;
         editor?.DoValidate();
      }

      private void onCopyButtonClick(object sender, EventArgs e)
      {
         AcceptDataChanges();
         OnCopyMetaData?.Invoke(this, new EventArgs());
      }

      /// <summary>
      /// Method reacting on validating event.
      /// </summary>
      private void onEditorValidating(object sender, CancelEventArgs e)
      {
         var editor = sender as BaseEdit;
         if (editor == null) return;

         checkEditorInput(editor);
      }

      /// <summary>
      /// Method for auto accept values. After validation is finished and all requirements are fullfilled we can accept data.
      /// </summary>
      private void onEditorValidated(object sender, EventArgs e)
      {
         checkData();

         if (AutoAcceptChanges && IsDataValid)
            AcceptDataChanges();
      }

      /// <summary>
      /// Method reacting on data changes.
      /// </summary>
      protected virtual void OnEditorTextChanged(object sender, EventArgs e)
      {
         checkData();
      }

      /// <summary>
      /// Method for setting common properties of editors.
      /// </summary>
      private static void setEditorProperties(RepositoryItem properties, DataColumn column)
      {
         properties.ReadOnly = column.ReadOnly;
      }

      /// <summary>
      /// Method for retrieving the bound meta data column of a given editor.
      /// </summary>
      private MetaDataColumn getEditorColumn(IBindableComponent editor)
      {
         if (editor.DataBindings.Count == 0) return null;
         var colName = editor.DataBindings[0].BindingMemberInfo.BindingField;
         return !_data.Columns.ContainsName(colName) ? null : _data.Columns.ItemByName(colName);
      }

      /// <summary>
      /// Method for checking the input of editor against column settings.
      /// </summary>
      private bool checkEditorInput(BaseEdit editor)
      {
         var col = getEditorColumn(editor);

         editor.ErrorText = String.Empty;

         if (col.Required && (editor.EditValue == null || editor.EditValue == DBNull.Value || String.IsNullOrEmpty(editor.EditValue.ToString())))
         {
            editor.ErrorText = $"The meta data information <{col.DisplayName}> can not be left empty!";
            return false;
         }

         if (col.DataType != typeof(double)) return true;
         var editorValue = getEditorValue(editor);
         editor.EditValue = editorValue.ToString();

         if (editorValue == null && !String.IsNullOrEmpty(editor.EditValue.ToString()))
         {
            editor.ErrorText = $"The input '{editor.EditValue}' is not valid for meta data information <{col.DisplayName}>!";
            return false;
         }

         if (col.MinValue == null && col.MaxValue == null) return true;

         if (editorValue == null) return true;

         if (col.IsValueValid((double)editorValue)) return true;

         editor.ErrorText = $"The meta data information <{col.DisplayName}> is out of range!";
         return false;
      }

      /// <summary>
      /// Method for getting the current value of an editor.
      /// </summary>
      private static double? getEditorValue(BaseEdit editor)
      {
         if (String.IsNullOrEmpty(editor.EditValue?.ToString())) return null;

         try
         {
            return editor.EditValue.ConvertedTo<double>();
         }
         catch (Exception)
         {
            return null;
         }
      }

      /// <summary>
      /// Method for getting a complete list of all editors.
      /// </summary>
      private IList<BaseEdit> getEditors()
      {
         var retVal = new List<BaseEdit>();

         foreach (Control control in Controls[0].Controls)
         {
            var editor = control as BaseEdit;
            if (editor == null) continue;
            retVal.Add(editor);
         }

         return retVal;
      }

      /// <summary>
      /// Handler for event OnCopyMetaData.
      /// </summary>
      public delegate void CopyMetaDataHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when user clicks on copy button.
      /// </summary>
      public event CopyMetaDataHandler OnCopyMetaData;


      public delegate void BroadcastMetaDataHandler(object sender, BroadcastMetaDataEventArgs e);

      public event BroadcastMetaDataHandler OnBroadCastMetaData;

      /// <summary>
      /// Handler for event OnMetaDataChanged.
      /// </summary>
      public delegate void MetaDataChangedHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when meta data has been changed.
      /// </summary>
      public event MetaDataChangedHandler OnMetaDataChanged;

      /// <summary>
      /// Event raised when the validity status of the data has changed.
      /// </summary>
      public event IsDataValidChangeHandler OnIsDataValidChanged;

      /// <summary>
      /// Property indicating if the data is valid.
      /// </summary>
      public bool IsDataValid { get; set; }

      /// <summary>
      /// Method for checking the validity of the data and raising event OnIsDataValidChanged if neccessary.
      /// </summary>
      private void checkData()
      {
         var editors = getEditors();
         var isDataValid = (editors.Count > 0);

         foreach (BaseEdit editor in editors)
            isDataValid &= checkEditorInput(editor);

         if (isDataValid != IsDataValid)
         {
            IsDataValid = isDataValid;
            OnIsDataValidChanged?.Invoke(this, new EventArgs());
         }
         if (_copyButton != null)
            _copyButton.Enabled = IsDataValid;
      }

      public void SetEditorValue(string metaDataColumn, object value)
      {
         if (_data.Rows.Count == 0)
         {

            foreach (BaseEdit editor in getEditors())
            {
               var col = getEditorColumn(editor);
               if (col.ColumnName != metaDataColumn)
                  continue;
               editor.EditValue = value;
               checkData();
               break;
            }
         }
         else
         {
            var row = _data.Rows.ItemByIndex(0);
            if (_data.Columns.ContainsName(metaDataColumn))
               row[metaDataColumn] = value;
         }
      }

      /// <summary>
      /// Method for accepting the meta data.
      /// </summary>
      public void AcceptDataChanges()
      {
         if (_data.Rows.Count == 0)
         {
            var row = (MetaDataRow)_data.NewRow();
            foreach (var editor in getEditors())
            {
               var col = getEditorColumn(editor);
               if (col == null) continue;
               if (col.DataType == typeof(double))
               {
                  var editorValue = getEditorValue(editor);
                  row[col] = editorValue ?? (object)DBNull.Value;
               }
               else
                  row[col] = editor.EditValue ?? DBNull.Value;
            }
            _data.Rows.Add(row);
         }

         _data.AcceptChanges();
         OnMetaDataChanged?.Invoke(this, new EventArgs());
      }

      public void RejectDataChanges()
      {
         _data.RejectChanges();
      }

      /// <summary>
      /// Method for refreshing the changed data in the used editors.
      /// </summary>
      /// <remarks>Should be called if the meta data has been changed outside of the control.</remarks>
      public void RefreshData(int rowindex)
      {
         if (_data.Rows.Count <= rowindex) return;

         var row = _data.Rows.ItemByIndex(rowindex);
         foreach (BaseEdit editor in getEditors())
         {
            var col = getEditorColumn(editor);
            if (col == null) continue;
            editor.EditValue = row[col] ?? DBNull.Value;
         }
      }

      private void cleanMemory()
      {
         _data.Dispose();
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

   }

   public class BroadcastMetaDataEventArgs : EventArgs
   {
      public string Name { set; get; }
      public object Value { set; get; }
   }
}


