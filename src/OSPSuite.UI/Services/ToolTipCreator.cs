using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Services
{
   public interface IToolTipCreator
   {
      SuperToolTip CreateToolTip(string content, string title = null, Image image = null);
      SuperToolTip ToolTipFor(JournalPageDTO journalPageDTO);
      SuperToolTip ToolTipFor(ValueOrigin valueOrigin);
      SuperToolTip ToolTipFor(RelatedItem relatedItem);
      SuperToolTip ToolTipFor(ValidationMessageDTO validationMessage);
      SuperToolTip ToolTipFor(IdentificationParameterDTO identificationParameterDTO);
      ToolTipControlInfo ToolTipControlInfoFor(object objectWithToolTip, SuperToolTip superToolTip);
      SuperToolTip ToolTipFor(Image tooltipImage);
      SuperToolTip AddSubItemTo(SuperToolTip toolTip, string title, string text);

   }

   public class ToolTipCreator : IToolTipCreator
   {
      public SuperToolTip CreateToolTip(string content, string title = null, Image image = null)
      {
         // Create an object to initialize the SuperToolTip.
         var superToolTip = CreateToolTip();
         var setupArgs = new SuperToolTipSetupArgs();
         if (!string.IsNullOrEmpty(title))
            setupArgs.Title.Text = title;

         setupArgs.Contents.Text = convertHtml(content);
         setupArgs.Contents.Image = image;
         superToolTip.Setup(setupArgs);
         return superToolTip;
      }

      protected SuperToolTip CreateToolTip()
      {
         return new SuperToolTip {AllowHtmlText = DefaultBoolean.True, MaxWidth = 1000};
      }

      public SuperToolTip ToolTipFor(ValidationMessageDTO validationMessage)
      {
         var toolTip = CreateToolTip(validationMessage.Message, validationMessage.Status.ToString(),
            validationMessage.Icon);

         if (!validationMessage.Details.Any())
            return toolTip;

         toolTip.Items.AddSeparator();
         toolTip.WithTitle(Captions.Details);
         validationMessage.Details.Each(s => toolTip.WithText(s));
         return toolTip;
      }

      public SuperToolTip ToolTipFor(IdentificationParameterDTO identificationParameterDTO)
      {
         var toolTip = CreateToolTip(string.Empty, identificationParameterDTO.Name);
         foreach (var linkedParameter in identificationParameterDTO.IdentificationParameter.AllLinkedParameters)
         {
            toolTip.WithText(linkedParameter.FullQuantityPath);
         }
         return toolTip;
      }

      private string replaceHtmlTag(string oldTag, string newTag, string description)
      {
         var html = description.Replace($"<{oldTag}>", $"<{newTag}>");
         return html.Replace($"</{oldTag}>", $"</{newTag}>");
      }

      private string convertHtml(string htmlString)
      {
         if (string.IsNullOrEmpty(htmlString))
            return htmlString;

         var html = htmlString;
         html = replaceHtmlTag("STRONG", "b", html);
         html = replaceHtmlTag("EM", "i", html);
         html = replaceHtmlTag("P", "br", html);
         html = html.Replace("&nbsp;", string.Empty);
         html = html.Replace("<FONT color", "<color");
         html = replaceHtmlTag("FONT", "color", html);
         return html;
      }

      public SuperToolTip ToolTipFor(JournalPageDTO journalPageDTO)
      {
         var toolTip = CreateToolTip(string.Empty, journalPageDTO.Title);
         toolTip.WithText(journalPageDTO.CreatedAtBy);
         toolTip.WithText(journalPageDTO.UpdatedAtBy);

         var relatedItems = journalPageDTO.RelatedItems;
         if (relatedItems.Any())
         {
            toolTip.Items.AddSeparator();
            toolTip.WithTitle(Captions.Journal.RelatedItems);
            foreach (var relatedItem in relatedItems)
            {
               var item = toolTip.Items.Add(relatedItem.Display);
               item.Image = ApplicationIcons.IconByName(relatedItem.IconName);
            }
         }

         if (journalPageDTO.Tags.Any())
            AddSubItemTo(toolTip, Captions.Journal.Tags, journalPageDTO.TagsDisplay);

         return toolTip;
      }

      public SuperToolTip ToolTipFor(ValueOrigin valueOrigin)
      {
         if (valueOrigin.Source == ValueOriginSources.Undefined)
            return null;

         var title =  new[]
         {
            valueOrigin.Source.Display, valueOrigin.Method.Display
         }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString(" - ");

         if (string.IsNullOrEmpty(valueOrigin.Description))
            return CreateToolTip(title, image: valueOrigin.Source.Icon);

         return CreateToolTip(valueOrigin.Description, title, valueOrigin.Source.Icon);
      }

      public SuperToolTip ToolTipFor(RelatedItem relatedItem)
      {
         var toolTip = CreateToolTip(relatedItem.Display, Captions.Journal.RelatedItem, ApplicationIcons.IconByName(relatedItem.IconName));
         if(relatedItem.IsFile)
            return toolTip;

         return AddSubItemTo(toolTip, Captions.Journal.Project, relatedItem.FullPath);
      }

      public ToolTipControlInfo ToolTipControlInfoFor(object objectWithToolTip, SuperToolTip superToolTip)
      {
         return new ToolTipControlInfo(objectWithToolTip, string.Empty) {SuperTip = superToolTip, ToolTipType = ToolTipType.SuperTip};
      }

      public SuperToolTip ToolTipFor(Image tooltipImage)
      {
         var tooltip = CreateToolTip();
         return addImageToToolTip(tooltipImage, tooltip);
      }

      public SuperToolTip AddSubItemTo(SuperToolTip toolTip, string title, string text)
      {
         toolTip.Items.AddSeparator();
         toolTip.WithTitle(title);
         toolTip.WithText(text);
         return toolTip;
      }

      private static SuperToolTip addImageToToolTip(Image tooltipImage, SuperToolTip tooltip)
      {
         var toolTipItem = new ToolTipItem {Image = tooltipImage};
         tooltip.Items.Add(toolTipItem);
         return tooltip;
      }
   }
}