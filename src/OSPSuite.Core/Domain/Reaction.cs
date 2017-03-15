using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    A Reaction converts molecules to other
   /// </summary>
   public interface IReaction : IProcess
   {
      /// <summary>
      ///    Gets the educts of the reaction.
      /// </summary>
      /// <value>The educts.</value>
      IEnumerable<IReactionPartner> Educts { get; }

      /// <summary>
      ///    Gets the products of the reaction.
      /// </summary>
      /// <value>The products.</value>
      IEnumerable<IReactionPartner> Products { get; }

      /// <summary>
      ///    Gets the Modifier Names.
      /// </summary>
      /// <value>The products.</value>
      IEnumerable<string> ModifierNames { get; }

      /// <summary>
      ///    Adds the modifier name.
      /// </summary>
      /// <param name="modifierName">Name of the modifier.</param>
      void AddModifier(string modifierName);

      /// <summary>
      ///    Adds the educt to the reactions educt list.
      /// </summary>
      /// <param name="newEduct">The new educt.</param>
      void AddEduct(IReactionPartner newEduct);

      /// <summary>
      ///    Adds the product to the reactions product list.
      /// </summary>
      /// <param name="newProduct">The new product.</param>
      void AddProduct(IReactionPartner newProduct);
   }

   public class Reaction : Process, IReaction
   {
      private readonly IList<IReactionPartner> _educts;
      private readonly IList<IReactionPartner> _products;
      private readonly IList<string> _modifier;

      public Reaction()
      {
         _educts = new List<IReactionPartner>();
         _products = new List<IReactionPartner>();
         _modifier = new List<string>();
         ContainerType = ContainerType.Reaction;
         Icon = IconNames.REACTION;
      }

      public IEnumerable<IReactionPartner> Educts
      {
         get { return _educts; }
      }

      public void AddModifier(string modifierName)
      {
         _modifier.Add(modifierName);
      }

      /// <summary>
      ///    Adds the educt to the reactions educt list.
      /// </summary>
      /// <param name="newEduct">The new educt.</param>
      public void AddEduct(IReactionPartner newEduct)
      {
         _educts.Add(newEduct);
      }

      public IEnumerable<IReactionPartner> Products
      {
         get { return _products; }
      }

      public IEnumerable<string> ModifierNames
      {
         get { return _modifier; }
      }

      /// <summary>
      ///    Adds the product to the reactions product list.
      /// </summary>
      /// <param name="newProduct">The new product.</param>
      public void AddProduct(IReactionPartner newProduct)
      {
         _products.Add(newProduct);
      }

      public override bool Uses(IMoleculeAmount amount)
      {
         return
            _educts.Any(x => Equals(x.Partner, amount)) ||
            _products.Any(x => Equals(x.Partner, amount));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcReaction = source as IReaction;
         if (srcReaction == null) return;
         srcReaction.ModifierNames.Each(AddModifier);
         //Educts/Products should NOT be cloned
         //Instead, some Model-Finalizer must be called
      }
   }
}