using System;
using System.Drawing;
using System.Windows.Forms;

namespace OSPSuite.UI.Extensions
{
   public static class FormExtensions
   {
      /// <summary>
      ///    Resizes the given <paramref name="view" /> to fit the screen. <c>Width</c> and <c>Height</c> will be adjusted
      ///    according to the given fraction for width and height respectively
      /// </summary>
      /// <param name="view">The view to resize</param>
      /// <param name="fractionWidth">
      ///    A value in ]0;1] representing the fraction of the current screen width that should be used
      ///    as witdh for the given view
      /// </param>
      /// <param name="fractionHeight">
      ///    A value in ]0;1] representing the fraction of the current screen height that should be
      ///    used as height for the given view
      /// </param>
      /// <param name="resizeOnlyIfOutOfBound">
      ///    If set to <c>true</c> the <paramref name="view" /> will only be resized if its dimension are bigger than the resized
      ///    dimension.
      ///    If set to <c>false</c> the view will always be resized
      /// </param>
      public static void ReziseForCurrentScreen(this Form view, double fractionWidth = 1, double fractionHeight = 1, bool resizeOnlyIfOutOfBound = true)
      {
         var workingArea = Screen.PrimaryScreen.WorkingArea;
         ReziseForCurrentScreen(view, workingArea, fractionWidth, fractionHeight, resizeOnlyIfOutOfBound);
      }

      internal static void ReziseForCurrentScreen(Form view, Rectangle workingArea, double fractionWidth, double fractionHeight, bool resizeOnlyIfOutOfBound)
      {
         int resizedWidth = Convert.ToInt32(workingArea.Width * fractionWidth);
         int resizeHeight = Convert.ToInt32(workingArea.Height * fractionHeight);

         int targetWidth = resizedWidth;
         int targetHeight = resizeHeight;

         if (resizeOnlyIfOutOfBound)
         {
            targetWidth = Math.Min(view.Width, resizedWidth);
            targetHeight = Math.Min(view.Height, resizeHeight);
         }

         Resize(view, targetWidth, targetHeight);
      }

      public static void Resize(this Form view, int width, int height)
      {
         view.Width = width;
         view.Height = height;
      }

      /// <summary>
      /// Ensures that the view is brought to the front of the screen, even it was minimized or is hidden behing another view of the application
      /// </summary>
      public static void EnsureVisible(this Form view)
      {
         view.Visible = true;
         if (view.WindowState == FormWindowState.Minimized)
            view.WindowState = FormWindowState.Normal;
         else
         {
            //according to http://stackoverflow.com/questions/5282588/how-can-i-bring-my-application-window-to-the-front, this is the way that works in all cases
            view.TopMost = true;
            view.Focus();
            view.BringToFront();
            view.TopMost = false;
         }
      }

      /// <summary>
      /// Applies the <paramref name="formArea" /> dimensions to the <paramref name="form" /> and adjusts for boundaries of <paramref name="workingArea" />
      /// </summary>
      public static void FitToScreen(this Form form, Rectangle formArea, Rectangle workingArea)
      {
         //we want the form to fit completely in the window
         if(!formFitsWithinScreen(workingArea, formArea))
         {
            if (formIsOutsideScreen(workingArea, formArea))
            {
               formArea.X = workingArea.X;
               formArea.Y = workingArea.Y;
            }

            if (leftEdgeOutsideWorkingArea(workingArea, formArea))
               formArea.X = shiftLeft(workingArea, formArea);

            if (topEdgeOutsideWorkingArea(workingArea, formArea))
               formArea.Y = shiftDown(workingArea, formArea);

            if (rightEdgeOutsideWorkingArea(workingArea, formArea))
               formArea.X = shiftRight(workingArea, formArea);

            if (lowerEdgeOutsideWorkingArea(workingArea, formArea))
               formArea.Y = shiftUp(workingArea, formArea);

            // After adjusting in 4 directions if necessary, put the location to top left and clip the form
            if (formArea.X < workingArea.X)
            {
               formArea.X = workingArea.X;
               formArea.Width = workingArea.Width;
            }

            if (formArea.Y < workingArea.Y)
            {
               formArea.Y = workingArea.Y;
               formArea.Height = workingArea.Height;
            }
         }

         form.Location = formArea.Location;
         form.Height = formArea.Height;
         form.Width = formArea.Width;
      }

      private static int shiftDown(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.Y + (workingArea.Top - formArea.Top);
      }

      private static bool topEdgeOutsideWorkingArea(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.Top < workingArea.Top;
      }

      private static int shiftLeft(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.X + (workingArea.X - formArea.X);
      }

      private static bool leftEdgeOutsideWorkingArea(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.X < workingArea.X;
      }

      private static int shiftUp(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.Y + (workingArea.Bottom - formArea.Bottom);
      }

      private static bool lowerEdgeOutsideWorkingArea(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.Bottom > workingArea.Bottom;
      }

      private static int shiftRight(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.X + (workingArea.Right - formArea.Right);
      }

      private static bool rightEdgeOutsideWorkingArea(Rectangle workingArea, Rectangle formArea)
      {
         return formArea.Right > workingArea.Right;
      }

      private static bool formIsOutsideScreen(Rectangle workingArea, Rectangle formArea)
      {
         return !workingArea.IntersectsWith(formArea);
      }

      private static bool formFitsWithinScreen(Rectangle workingArea, Rectangle formArea)
      {
         return workingArea.Contains(formArea);
      }
   }
}