using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public interface IModel : IObjectBase
   {
      IContainer Root { get; set; }
      IContainer Neighborhoods { set; get; }

      /// <summary>
      ///    Returns the value of the molweight <see cref="IParameter" /> defined in the model. If the parameter is not found for
      ///    the given <paramref name="quantity" />, returns <c>null</c>.
      ///    We use the following logic: MolWeight is only available for <see cref="IMoleculeAmount" /> and
      ///    <see cref="IObserver" />. Also the MolWeight parameter is defined in the global container
      /// </summary>
      /// <param name="quantity">Quantity for which the molweight parameter should be retrieved</param>
      /// <returns></returns>
      double? MolWeightFor(IQuantity quantity);
   }

   public class Model : ObjectBase, IModel
   {
      private IContainer _neighborhoods;

      private IContainer _root;

      public IContainer Neighborhoods
      {
         set
         {
            if (Root != null && _neighborhoods != null)
            {
               Root.RemoveChild(_neighborhoods);
            }
            _neighborhoods = value;
            if (Root != null)
            {
               Root.Add(_neighborhoods);
            }
         }
         get { return _neighborhoods; }
      }

      public double? MolWeightFor(IQuantity quantity)
      {
         if (Root == null)
            return null;

         //Mol weight only available for molecule amount and observer
         string moleculeName;
         if (quantity.IsAnImplementationOf<IMoleculeAmount>())
            moleculeName = quantity.Name;
         else if (quantity.IsAnImplementationOf<IObserver>())
            moleculeName = quantity.ParentContainer.Name;
         else
            return null;

         //try to find the molweight parameter in the global molecule container
         var molWeightParameter = Root.EntityAt<IParameter>(moleculeName, Constants.Parameters.MOL_WEIGHT);
         if (molWeightParameter != null)
            return molWeightParameter.Value;

         return null;
      }

      public IContainer Root
      {
         get { return _root; }
         set
         {
            _root = value;
            if (_neighborhoods != null && !_root.ContainsName(Constants.NEIGHBORHOODS))
            {
               _root.Add(_neighborhoods);
            }
         }
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         Root.AcceptVisitor(visitor);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcModel = source as IModel;
         if (srcModel == null) return;

         //root container must be cloned BEFORE neighborhoods
         Root = cloneManager.Clone(srcModel.Root);

         //Neighborhoods-subnode is already cloned (as one of the Root-children)
         // so only internal property must be set
         _neighborhoods = Root.GetSingleChildByName<IContainer>(srcModel.Neighborhoods.Name);
      }
   }
}