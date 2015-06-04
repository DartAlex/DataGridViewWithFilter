﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace DataGridWithFilter
{
    public class DataGridFilterHeader : DataGridViewColumnHeaderCell
    {
        PushButtonState currentState = PushButtonState.Normal;
        Point cellLocation;
        Rectangle buttonRect;

        public event EventHandler<ColumnFilterClickedEventArg> FilterButtonClicked;

        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates dataGridViewElementState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds,
                       cellBounds, rowIndex,
                       dataGridViewElementState, value,
                       formattedValue, errorText,
                       cellStyle, advancedBorderStyle, paintParts);

            int width = 20; // 20 px
            buttonRect = new Rectangle(cellBounds.X + cellBounds.Width - width, cellBounds.Y, width, cellBounds.Height);

            cellLocation = cellBounds.Location;
            ButtonRenderer.DrawButton(graphics,
                                      buttonRect,
                                      "V",
                                      this.DataGridView.Font,
                                      false,
                                      currentState);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
                currentState = PushButtonState.Pressed;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsMouseOverButton(e.Location))
            {
                currentState = PushButtonState.Normal;
                this.OnFilterButtonClicked();
            }
            base.OnMouseUp(e);
        }
        private bool IsMouseOverButton(Point e)
        {
            Point p = new Point(e.X + cellLocation.X, e.Y + cellLocation.Y);
            if (p.X >= buttonRect.X && p.X <= buttonRect.X + buttonRect.Width &&
                p.Y >= buttonRect.Y && p.Y <= buttonRect.Y + buttonRect.Height)
            {
                return true;
            }
            return false;
        }
        protected virtual void OnFilterButtonClicked()
        {
            if (this.FilterButtonClicked != null)
                this.FilterButtonClicked(this, new ColumnFilterClickedEventArg(this.ColumnIndex, this.buttonRect));
        }
    }
}
