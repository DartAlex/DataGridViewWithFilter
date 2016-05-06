using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 

namespace DataGridWithFilter
{
    public class ColumnFilterClickedEventArg : EventArgs
    {
        public int ColumnIndex { get; private set; }
        public Rectangle ButtonRectangle { get; private set; }
        public ColumnFilterClickedEventArg(int colIndex, Rectangle btnRect)
        {
            this.ColumnIndex = colIndex;
            this.ButtonRectangle = btnRect;
        }
    }
}
