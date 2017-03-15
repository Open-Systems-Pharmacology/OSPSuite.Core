using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Nodes
{
   public abstract class AbstractNode : ITreeNode
   {
      private readonly IList<ITreeNode> _children;
      public event Action<ITreeNode> TextChanged = delegate { };
      public event Action<ITreeNode> IconChanged = delegate { };
      public event Action<ITreeNode> ForeColorChanged = delegate { };
      public abstract string Id { get; }
      public IList<ToolTipPart> ToolTip { get; set; }
      
      public abstract object TagAsObject { get; }
      public virtual ITreeNode ParentNode { get; set; }
      private ApplicationIcon _icon;
      private string _text;
      private Color _color;

      protected AbstractNode()
      {
         _children = new List<ITreeNode>();
         _color = Color.Empty;
         ToolTip = new List<ToolTipPart>();
      }

      public ITreeNode RootNode
      {
         get { return ParentNode == null ? this : ParentNode.RootNode; }
      }

      public void AddToolTipPart(ToolTipPart toolTipPart)
      {
         if(ToolTip==null)
            ToolTip = new List<ToolTipPart>();
         ToolTip.Add(toolTipPart);
      }

      public ApplicationIcon Icon
      {
         get { return _icon; }
         set
         {
            _icon = value;
            IconChanged(this);
         }
      }


      public  string Text
      {
         get { return _text; }
         set
         {
            _text = value;
            TextChanged(this);
         }
      }

      public Color ForeColor
      {
         get { return _color; }
         set
         {
            _color = value;
            ForeColorChanged(this);
         }
      }

      public IEnumerable<ITreeNode> AllLeafNodes
      {
         get
         {
            //this is a leaf already: return 
            if (_children.Count == 0)
               return new[] {this};

            var allChildren = new List<ITreeNode>();
            foreach (var node in _children)
            {
               allChildren.AddRange(node.AllLeafNodes);
            }
            return allChildren;
         }
      }

      public IEnumerable<ITreeNode> AllNodes
      {
         get
         {
            var allChildren = new List<ITreeNode> {this};
            foreach (var node in _children)
            {
               allChildren.AddRange(node.AllNodes);
            }
            return allChildren;
         }
      }

      public bool HasChildren
      {
         get { return _children.Count > 0; }
      }

      public string FullPath()
      {
         return FullPath("\\");
      }

      public string FullPath(string delimiter)
      {
         var sb = new StringBuilder(Text);
         var parentNode = this.ParentNode;
         while (parentNode!=null)
         {
            sb.Insert(0, string.Format("{0}{1}", parentNode.Text, delimiter));
            parentNode = parentNode.ParentNode;
         }
         return sb.ToString();
      }

      public virtual IEnumerable<ITreeNode> Children
      {
         get { return _children; }
      }

      public void AddChild(ITreeNode childNode)
      {
         if (childNode == null || _children.Contains(childNode))
            return;

         _children.Add(childNode);
         childNode.ParentNode = this;
      }

      public void RemoveChild(ITreeNode childNode)
      {
         _children.Remove(childNode);
         childNode.ParentNode = null;
      }

      protected void OnTextChanged()
      {
         TextChanged(this);
      }

      public virtual void Delete()
      {
         if (ParentNode != null)
            ParentNode.RemoveChild(this);

         ParentNode = null;
         _children.ToList().Each(child => child.Delete());
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return Equals(obj as ITreeNode);
      }

      public bool Equals(ITreeNode other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(other.Id, Id);
      }

      public override int GetHashCode()
      {
         return (Id != null ? Id.GetHashCode() : 0);
      }
   }

   public abstract class AbstractNode<T> : AbstractNode, ITreeNode<T>
   {
      protected AbstractNode(T tag)
      {
         Tag = tag;
         var propertyChanged = tag as INotifyPropertyChanged;
         if(propertyChanged!=null)
            propertyChanged.PropertyChanged += propertyNameChanged;

      }

      private void propertyNameChanged(object sender, PropertyChangedEventArgs e)
      {
         if (!e.PropertyName.Equals(Constants.NAME_PROPERTY)) return;
         UpdateText();
      }

      protected virtual void UpdateText()
      {
         //nothing to do here
      }

      public T Tag { get; protected set; }

      public override object TagAsObject
      {
         get { return Tag; }
      }

      public override void Delete()
      {
         base.Delete();
         var propertyChanged = Tag as INotifyPropertyChanged;
         if (propertyChanged != null)
            propertyChanged.PropertyChanged -= propertyNameChanged;
      }
   }
}