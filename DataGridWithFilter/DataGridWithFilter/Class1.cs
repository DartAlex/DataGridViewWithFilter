using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace DataGridWithFilter
{
    public class DataGridViewWithFilter : DataGridView
    {
        List<FilterStatus> Filter = new List<FilterStatus>();
        string StrFilter = "";

        TextBox textBoxCtrl = new TextBox();
        CheckedListBox CheckCtrl = new CheckedListBox();
        Button ApplyButtonCtrl = new Button();
        Button ClearFilterCtrl = new Button();
        ToolStripDropDown popup = new ToolStripDropDown();

        private int columnIndex { get; set; }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            var header = new DataGridFilterHeader();
            header.FilterButtonClicked += new EventHandler<ColumnFilterClickedEventArg>(header_FilterButtonClicked);
            e.Column.HeaderCell = header;
            base.OnColumnAdded(e);
        }

        void header_FilterButtonClicked(object sender, ColumnFilterClickedEventArg e)
        {
            int widthTool = GetWhithColumn(e.ColumnIndex) + 70;
            if (widthTool < 50) widthTool = 50;

            columnIndex = e.ColumnIndex;

            textBoxCtrl.Clear();
            CheckCtrl.Items.Clear();

            textBoxCtrl.Size = new System.Drawing.Size(widthTool, 30);
            textBoxCtrl.TextChanged -= textBoxCtrl_TextChanged;
            textBoxCtrl.TextChanged += textBoxCtrl_TextChanged;

            CheckCtrl.CheckOnClick = true;
            CheckCtrl.Items.Add("All", CheckState.Indeterminate);
            CheckCtrl.ItemCheck -= CheckCtrl_ItemCheck;
            CheckCtrl.ItemCheck += CheckCtrl_ItemCheck;

            GetChkFilter();

            CheckCtrl.MaximumSize = new System.Drawing.Size(widthTool, GetHeightTable() - 100);
            CheckCtrl.Size = new System.Drawing.Size(widthTool, (CheckCtrl.Items.Count + 1) * 18);

            ApplyButtonCtrl.Text = "Apply";
            ApplyButtonCtrl.Size = new System.Drawing.Size(widthTool, 30);
            ApplyButtonCtrl.Click -= ApplyButtonCtrl_Click;
            ApplyButtonCtrl.Click += ApplyButtonCtrl_Click;

            ClearFilterCtrl.Text = "Clear filtres";
            ClearFilterCtrl.Size = new System.Drawing.Size(widthTool, 30);
            ClearFilterCtrl.Click -= ClearFilterCtrl_Click;
            ClearFilterCtrl.Click += ClearFilterCtrl_Click;

            popup.Items.Clear();
            popup.AutoSize = true;
            popup.Margin = Padding.Empty;
            popup.Padding = Padding.Empty;

            ToolStripControlHost host1 = new ToolStripControlHost(textBoxCtrl);
            host1.Margin = Padding.Empty;
            host1.Padding = Padding.Empty;
            host1.AutoSize = false;
            host1.Size = textBoxCtrl.Size;

            ToolStripControlHost host2 = new ToolStripControlHost(CheckCtrl);
            host2.Margin = Padding.Empty;
            host2.Padding = Padding.Empty;
            host2.AutoSize = false;
            host2.Size = CheckCtrl.Size;

            ToolStripControlHost host3 = new ToolStripControlHost(ApplyButtonCtrl);
            host3.Margin = Padding.Empty;
            host3.Padding = Padding.Empty;
            host3.AutoSize = false;
            host3.Size = ApplyButtonCtrl.Size;

            ToolStripControlHost host4 = new ToolStripControlHost(ClearFilterCtrl);
            host4.Margin = Padding.Empty;
            host4.Padding = Padding.Empty;
            host4.AutoSize = false;
            host4.Size = ClearFilterCtrl.Size;

            popup.Items.Add(host1);
            popup.Items.Add(host2);
            popup.Items.Add(host3);
            popup.Items.Add(host4);

            popup.Show(this, e.ButtonRectangle.X, e.ButtonRectangle.Bottom);
        }

        void CheckCtrl_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    for (int i = 1; i < CheckCtrl.Items.Count; i++)
                        CheckCtrl.SetItemChecked(i, true);
                }
                else
                {
                    for (int i = 1; i < CheckCtrl.Items.Count; i++)
                        CheckCtrl.SetItemChecked(i, false);
                }
            }
        }

        void ClearFilterCtrl_Click(object sender, EventArgs e)
        {
            Filter.Clear();
            StrFilter = "";
            ApllyFilter();
            popup.Close();
        }

        void textBoxCtrl_TextChanged(object sender, EventArgs e)
        {
            (this.DataSource as DataTable).DefaultView.RowFilter = string.Format("[" + this.Columns[columnIndex].Name + "] LIKE '%{0}%'", textBoxCtrl.Text);
        }

        void ApplyButtonCtrl_Click(object sender, EventArgs e)
        {
            StrFilter = "";
            SaveChkFilter();
            ApllyFilter();
            popup.Close();
        }

        private List<string> GetDataColumns(int e)
        {
            List<string> ValueCellList = new List<string>();
            string Value;

            foreach (DataGridViewRow row in this.Rows)
            {
                try
                {
                    Value = row.Cells[e].Value.ToString();
                    if (!ValueCellList.Contains(Value))
                        ValueCellList.Add(row.Cells[e].Value.ToString());
                }
                catch { }
                
            }
            return ValueCellList;
        }

        private int GetHeightTable()
        {
            return this.Height;
        }

        private int GetWhithColumn(int e)
        {
            return this.Columns[e].Width;
        }

        private void SaveChkFilter()
        {
            string col = this.Columns[columnIndex].Name;
            string itemChk;
            bool statChk;

            Filter.RemoveAll(x => x.columnName == col);

            for (int i = 0; i < CheckCtrl.Items.Count; i++)
            {
                itemChk = CheckCtrl.Items[i].ToString();
                statChk = CheckCtrl.GetItemChecked(i);
                if (itemChk != "All")
                {
                    Filter.Add(new FilterStatus() { columnName = col, valueName = itemChk, check = statChk });
                }
            }
        }

        private void GetChkFilter()
        {
            int Count = 0;
            foreach (FilterStatus val in Filter)
            {
                if (this.Columns[columnIndex].Name == val.columnName)
                {
                    Count++;
                    if (val.check == true)
                    {
                        CheckCtrl.Items.Add(val.valueName, CheckState.Checked);
                    }
                    else
                    {
                        CheckCtrl.Items.Add(val.valueName, CheckState.Unchecked);
                    }
                }
            }

            if (Count == 0)
            {
                foreach (string ValueCell in GetDataColumns(columnIndex))
                {
                    CheckCtrl.Items.Add(ValueCell, CheckState.Checked);
                }
            }
        }

        private void ApllyFilter()
        {
            foreach (FilterStatus val in Filter)
            {
                if (val.check == false)
                {
                    if (StrFilter.Length == 0)
                    {
                        StrFilter = StrFilter + ("[" + val.columnName + "] <> '" + val.valueName + "' ");
                    }
                    else
                    {
                        StrFilter = StrFilter + (" AND [" + val.columnName + "] <> '" + val.valueName + "' ");
                    }
                }
            }
            (this.DataSource as DataTable).DefaultView.RowFilter = StrFilter;
        }
    }
}
