using System;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class RootNodeType : IClassification, IViewItem
   {
      public ApplicationIcon Icon { get; private set; }
      public string Id { get; set; }
      public string Name { get; set; }
      private readonly Classification _classification;

      public RootNodeType(string name, ApplicationIcon icon, ClassificationType classificationType = ClassificationType.Unknown)
      {
         Id = ShortGuid.NewGuid().ToString();
         Name = name;
         Icon = icon;
         _classification = new Classification {ClassificationType = classificationType};
      }

      /// <summary>
      ///    Should never be set on RootNodeType
      /// </summary>
      public IClassification Parent
      {
         get { return null; }
         set { throw new NotSupportedException(); }
      }

      public string Path => _classification.Path;

      public bool HasEquivalentClassification(IClassification compared)
      {
         return _classification.HasEquivalentClassification(compared);
      }

      public ClassificationType ClassificationType => _classification.ClassificationType;
   }
}