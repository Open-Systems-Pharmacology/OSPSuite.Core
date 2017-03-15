using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Xml;
using Northwoods.Go.Xml;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Services
{
   public class DiagramModelToXmlMapper : IDiagramModelToXmlMapper, IContainerBaseXmlSerializer
   {
      public virtual string ElementName { get { return "DiagramModel"; } }
      public List<GoXmlBindingTransformer> Transformers { set; get; }
      private const string IsLayouted = "IsLayouted";
      private const string LocationY = "LocationY";
      private const string LocationX = "LocationX";


      public DiagramModelToXmlMapper()
      {
         Transformers = new List<GoXmlBindingTransformer>();
         CreateTransformers();
      }

      public XmlDocument DiagramModelToXmlDocument(IDiagramModel diagramModel)
      {
         GoXmlWriter writer = new GoXmlWriter();
         RegisterTransformers(writer);
         writer.Objects = (IEnumerable)diagramModel;
         var xmlDoc = writer.Generate();

         xmlDoc.DocumentElement.SetAttribute(IsLayouted, diagramModel.IsLayouted.ToString());
         return xmlDoc;
      }


      public XmlDocument ContainerToXmlDocument(IContainerBase containerBase)
      {
         GoXmlWriter writer = new GoXmlWriter();
         RegisterTransformers(writer);

         // containerBase can be Document or GoNode
         var diagramModel = containerBase as IDiagramModel;
         if (diagramModel != null)
            writer.Objects = (IEnumerable)diagramModel;
         else // containerBase is IContainerBaseNode
            writer.Objects = containerBase.GetDirectChildren<IBaseNode>();

         writer.RootElementName = ElementName;
         var xmlDoc = writer.Generate();
         xmlDoc.DocumentElement.SetAttribute(LocationX, containerBase.Location.X.ToString());
         xmlDoc.DocumentElement.SetAttribute(LocationY, containerBase.Location.Y.ToString());
         return xmlDoc;
      }

      public void Deserialize(IDiagramModel diagramModel, XmlDocument xmlDoc)
      {
         GoXmlReader reader = new GoXmlReader();
         RegisterTransformers(reader);
         reader.RootObject = diagramModel;
         reader.Consume(xmlDoc);

         if (xmlDoc.DocumentElement.HasAttribute(LocationX) && xmlDoc.DocumentElement.HasAttribute(LocationY))
         {
            float x = Convert.ToSingle(xmlDoc.DocumentElement.GetAttribute(LocationX), CultureInfo.InvariantCulture);
            float y = Convert.ToSingle(xmlDoc.DocumentElement.GetAttribute(LocationY), CultureInfo.InvariantCulture);
            diagramModel.Location = new PointF(x, y);
         }

         if (xmlDoc.DocumentElement.HasAttribute(IsLayouted))
            diagramModel.IsLayouted = Convert.ToBoolean(xmlDoc.DocumentElement.GetAttribute(IsLayouted));

         PostReadStep(diagramModel);
      }

      public IDiagramModel XmlDocumentToDiagramModel(XmlDocument xmlDoc)
      {
         var diagramModel = new DiagramModel();
         Deserialize(diagramModel, xmlDoc);
         return diagramModel;
      }

      // necessary, because during deserialization container hierarchy seems to be assembled bottom up, 
      // however, diagramModel seems to be unknown at ContainerBaseNode.Add
      // therefore add nodeIDs, when not yet available
      virtual protected void PostReadStep(IDiagramModel diagramModel)
      {
         foreach (var topNode in diagramModel.GetDirectChildren<IContainerNode>())
            foreach (var baseNode in topNode.GetAllChildren<IBaseNode>())
            {
               diagramModel.AddNodeId(baseNode);
            }
      }

      virtual protected void RegisterTransformers(GoXmlReaderWriterBase rw)
      {
         foreach (var bt in Transformers) rw.AddTransformer(bt);
      }

      virtual protected void CreateTransformers()
      {
         GoXmlBindingTransformer bt;

         bt = new GoXmlBindingTransformer(ElementName, new DiagramModel());
         Transformers.Add(bt);


         bt = new GoXmlBindingTransformer(new SimpleNeighborhoodNode());
         AddNeighborhoodNodeBindings(bt);
         Transformers.Add(bt);

         var btc = new GoXmlBindingTransformer(new SimpleContainerNode());
         AddContainerBaseNodeBindings(btc);
         Transformers.Add(btc);

         bt = new GoXmlBindingTransformer(new MultiPortContainerNode());
         bt.HandlesChildAttributes = true;
         bt.HandlesSubGraphCollapsedChildren = true;
         
         Transformers.Add(bt);

         bt = new GoXmlBindingTransformer(new ReactionNode());
         AddReactionNodeBindings(bt);
         Transformers.Add(bt);

         AddElementBaseNodeBindingFor(new MoleculeNode());

         AddElementBaseNodeBindingFor(new ObserverNode());

         addExpandableElementBaseNodeBindingFor(new JournalPageNode());

         AddElementBaseNodeBindingFor(new RelatedItemNode());

         // Links do not need to be serialized, because they have no individual properties,
         // so the automatic creation is sufficient.
      }

      private void addExpandableElementBaseNodeBindingFor<T>(T node)
      {
         var bt = new GoXmlBindingTransformer(node);
         bt.AddBinding("IsExpanded");
         AddElementBaseNodeBindings(bt);
         Transformers.Add(bt);
      }

      public void AddElementBaseNodeBindingFor<T>(T node)
      {
         var bt = new GoXmlBindingTransformer(node);
         AddElementBaseNodeBindings(bt);
         Transformers.Add(bt);
      }

      protected void AddWithLocationBindings(GoXmlBindingTransformer bt)
      {
         bt.AddBinding("Location");
         bt.AddBinding("Size");
         // Center must not be serialized, because is determined by Location and Size
      }

      protected void AddWithLayoutInfoBindings(GoXmlBindingTransformer bt)
      {
         AddWithLocationBindings(bt);
         bt.AddBinding("Hidden");
         bt.AddBinding("IsVisible");
         bt.AddBinding("LocationFixed");
         bt.AddBinding("UserFlags");
      }

      protected void AddBaseNodeBindings(GoXmlBindingTransformer bt)
      {
         bt.AddBinding("Id");
         bt.AddBinding("Name");
         AddWithLayoutInfoBindings(bt);
         bt.AddBinding("Description");
      }

      public void AddElementBaseNodeBindings(GoXmlBindingTransformer bt)
      {
         AddBaseNodeBindings(bt);
         bt.AddBinding("NodeSize");
         // NodeBaseSize must not be serialized, because is determined in Constructor / copy
      }

      protected void AddContainerBaseNodeBindings(GoXmlBindingTransformer bt)
      {
         bt.HandlesChildren = true;
         bt.HandlesChildAttributes = true;
         bt.HandlesSubGraphCollapsedChildren = true;
         AddBaseNodeBindings(bt);
         bt.AddBinding("IsLogical");
         bt.AddBinding("IsExpanded");
         bt.AddBinding("IsExpandedByDefault");
      }

      protected void AddNeighborhoodNodeBindings(GoXmlBindingTransformer bt)
      {
         AddElementBaseNodeBindings(bt);
         bt.AddBinding("FirstNeighbor");
         bt.AddBinding("SecondNeighbor");
      }

      protected void AddReactionNodeBindings(GoXmlBindingTransformer bt)
      {
         AddElementBaseNodeBindings(bt);
         bt.AddBinding("DisplayEductsRight");
      }

   }
}