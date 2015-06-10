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
            this.Size = new Size(480, 480);
            
            DataGridViewWithFilter DG = new DataGridViewWithFilter();

            DG.Bounds = new Rectangle(10, 10, 445, 420);
            DG.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right | AnchorStyles.Bottom)));
            DG.AllowUserToAddRows = false;

            this.Controls.Add(DG);
            
            DataTable DT = new DataTable();
            DT.Columns.Add("Number", typeof(int));
            DT.Columns.Add("Name");
            DT.Columns.Add("Ver");
            DT.Columns.Add("Date", typeof(DateTime));
            DT.Rows.Add("1", "Ubuntu", "11.10", "13.10.2011");
            DT.Rows.Add("2", "Ubuntu LTS", "12.04", "18.10.2012");
            DT.Rows.Add("3", "Ubuntu", "12.10", "18.10.2012");
            DT.Rows.Add("4", "Ubuntu", "13.04", "25.04.2012");
            DT.Rows.Add("5", "Ubuntu", "13.10", "17.10.2013");
            DT.Rows.Add("6", "Ubuntu LTS", "14.04", "23.04.2014");
            DT.Rows.Add("7", "Ubuntu", "14.10", "23.10.2014");
            DT.Rows.Add("8", "Ubuntu", "15.04", "23.04.2015");

            DataSet DS = new DataSet();
            DS.Tables.Add(DT);
            
            DG.DataSource = DS.Tables[0];
        }
    }
}
