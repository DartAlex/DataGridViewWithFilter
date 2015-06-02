using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataGridWithFilter;

namespace DataGridWithFilterDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(380, 480);
            
            DataGridViewWithFilter DG = new DataGridViewWithFilter();

            DG.Bounds = new Rectangle(10, 10, 345, 420);
            DG.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right | AnchorStyles.Bottom)));

            this.Controls.Add(DG);
            
            DataTable DT = new DataTable();
            DT.Columns.Add("Column1");
            DT.Columns.Add("Column2");
            DT.Columns.Add("Column3");
            DT.Rows.Add("AAA", "CCC", "1");
            DT.Rows.Add("AAA", "CCC", "2");
            DT.Rows.Add("AAA", "DDD", "3");
            DT.Rows.Add("AAA", "DDD", "4");
            DT.Rows.Add("BBB", "EEE", "5");
            DT.Rows.Add("BBB", "EEE", "6");
            DT.Rows.Add("BBB", "FFF", "7");
            DT.Rows.Add("BBB", "FFF", "8");


            DataSet DS = new DataSet();
            DS.Tables.Add(DT);
            
            DG.DataSource = DS.Tables[0];
        }
    }
}
