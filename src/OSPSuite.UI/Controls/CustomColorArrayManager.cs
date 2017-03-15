using System;
using System.Drawing;
using DevExpress.XtraEditors.ColorPickEditControl;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public class CustomColorArrayManager
   {
      /// <summary>
      /// Pushes a color onto the front of the <paramref name="customColors"/> array. If the array already contains the color, it is removed
      /// from the existing location so that it does not appear twice (once at the front and once again in an existing location).
      /// </summary>
      /// <param name="customColors">The array being managed</param>
      /// <param name="newColor">The new color being inserted at the front of the array</param>
      public void PushRecentColor(Color[] customColors, Color newColor)
      {
         var duplicateColorIndex = Array.IndexOf(customColors, newColor);

         // Start iterating from the end to the beginning we need to offset by -1 to go from array length to index of the last element
         // a further -1 becuase we start shuffling the next-to-last color into the last spot.
         var removeFromIndex = customColors.Length - 2;

         if (duplicateColorIndex != -1)
            removeFromIndex = duplicateColorIndex - 1;

         for (var i = removeFromIndex; i >= 0; i--)
         {
            customColors[i + 1] = customColors[i];
         }

         customColors[0] = newColor;
      }

      public void PushRecentColor(Matrix recentColors, Color newColor)
      {
         var coordinatesOfDuplicateInMatrix = findDuplicateInMatrix(recentColors, newColor);
         
         // shuffle all the colors before the duplicate by one
         shuffleToFirstDuplicate(recentColors, coordinatesOfDuplicateInMatrix);

         // Now, the first color in the matrix can be replaced by the new color
         recentColors[0, 0] = newColor;
      }

      private static Point findDuplicateInMatrix(Matrix recentColors, Color newColor)
      {
         for (var x = 0; x < recentColors.ColumnCount; x++)
         {
            for (var y = 0; y < recentColors.RowCount; y++)
            {
               if (recentColors[y,x].IsEqualTo(newColor))
                  return new Point(x, y);
            }
         }
         return Point.Empty;
      }

      private void shuffleToFirstDuplicate(Matrix recentColors, Point duplicatePoint)
      {
         if(duplicatePoint.IsEmpty)
            duplicatePoint = new Point(recentColors.ColumnCount-1, recentColors.RowCount-1);

         for (var y = duplicatePoint.Y; y >= 0; y--)
         {
            for (var x = duplicatePoint.X-1; x >= 0; x--)
            {
               var newCoordinate = newColorCoordinateAfterShuffle(recentColors, new Point(x, y));
               if (newCoordinate.IsEmpty)
                  continue;
               recentColors[newCoordinate.Y, newCoordinate.X] = recentColors[y, x];
            }
         }
      }

      private Point newColorCoordinateAfterShuffle(Matrix recentColors, Point point)
      {
         var newPoint = new Point(point.X+1, point.Y);

         if (newPoint.X > recentColors.ColumnCount - 1)
         {
            newPoint.X = 0;
            newPoint.Y ++;
         }
         
         if (newPoint.Y > recentColors.RowCount - 1)
            return Point.Empty;
         return newPoint;
      }
   }
}
