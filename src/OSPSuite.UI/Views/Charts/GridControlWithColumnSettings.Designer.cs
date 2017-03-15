using System.ComponentModel;
using DevExpress.XtraGrid.Views.Grid;

namespace OSPSuite.UI.Views.Charts
{
  partial class GridControlWithColumnSettings
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      _gridView = new GridView();
      ((ISupportInitialize)(_gridView)).BeginInit();
      ((ISupportInitialize)(this)).BeginInit();
      SuspendLayout();
      // 
      // _gridView
      // 
      _gridView.GridControl = this;
      _gridView.Name = "_gridView";
      // 
      // GridControlWithColumnSettings
      // 
      MainView = _gridView;
      ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[]{ _gridView });
      ((ISupportInitialize)(_gridView)).EndInit();
      ((ISupportInitialize)(this)).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private GridView _gridView;
  }
}
