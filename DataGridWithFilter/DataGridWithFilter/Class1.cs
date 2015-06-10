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
        TextBox textBoxCtrl = new TextBox();
        DateTimePicker DateTimeCtrl = new DateTimePicker();
        CheckedListBox CheckCtrl = new CheckedListBox();
        Button ApplyButtonCtrl = new Button();
        Button ClearFilterCtrl = new Button();
        ToolStripDropDown popup = new ToolStripDropDown();

        string StrFilter = "";
        string ButtonCtrlText = "Apply";
        string ClearFilterCtrlText = "Clear filters";
        string CheckCtrlAllText = "All";

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
            int widthTool = GetWhithColumn(e.ColumnIndex) + 50;
            if (widthTool < 110) widthTool = 110;

            columnIndex = e.ColumnIndex;

            textBoxCtrl.Clear();
            CheckCtrl.Items.Clear();

            textBoxCtrl.Size = new System.Drawing.Size(widthTool, 30);
            textBoxCtrl.TextChanged -= textBoxCtrl_TextChanged;
            textBoxCtrl.TextChanged += textBoxCtrl_TextChanged;

            DateTimeCtrl.Size = new System.Drawing.Size(widthTool, 30);
            DateTimeCtrl.Format = DateTimePickerFormat.Custom;
            DateTimeCtrl.CustomFormat = "dd.MM.yyyy";
            DateTimeCtrl.TextChanged -= DateTimeCtrl_TextChanged;
            DateTimeCtrl.TextChanged += DateTimeCtrl_TextChanged;

            CheckCtrl.ItemCheck -= CheckCtrl_ItemCheck;
            CheckCtrl.ItemCheck += CheckCtrl_ItemCheck;
            CheckCtrl.CheckOnClick = true;

            GetChkFilter();

            CheckCtrl.MaximumSize = new System.Drawing.Size(widthTool, GetHeightTable() - 120);
            CheckCtrl.Size = new System.Drawing.Size(widthTool, (CheckCtrl.Items.Count + 1) * 18);

            ApplyButtonCtrl.Text = ButtonCtrlText;
            ApplyButtonCtrl.Size = new System.Drawing.Size(widthTool, 30);
            ApplyButtonCtrl.Click -= ApplyButtonCtrl_Click;
            ApplyButtonCtrl.Click += ApplyButtonCtrl_Click;

            ClearFilterCtrl.Text = ClearFilterCtrlText;
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

            ToolStripControlHost host5 = new ToolStripControlHost(DateTimeCtrl);
            host5.Margin = Padding.Empty;
            host5.Padding = Padding.Empty;
            host5.AutoSize = false;
            host5.Size = DateTimeCtrl.Size;

            switch (this.Columns[columnIndex].ValueType.ToString())
            {
                case "System.DateTime":
                    popup.Items.Add(host5);
                    break;
                default:
                    popup.Items.Add(host1);
                    break;
            }
            popup.Items.Add(host2);
            popup.Items.Add(host3);
            popup.Items.Add(host4);

            popup.Show(this, e.ButtonRectangle.X, e.ButtonRectangle.Bottom);
            host2.Focus();
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
            (this.DataSource as DataTable).DefaultView.RowFilter = string.Format("convert([" + this.Columns[columnIndex].Name + "], 'System.String') LIKE '%{0}%'", textBoxCtrl.Text);
        }
        void DateTimeCtrl_TextChanged(object sender, EventArgs e)
        {
            (this.DataSource as DataTable).DefaultView.RowFilter = string.Format("convert([" + this.Columns[columnIndex].Name + "], 'System.String') LIKE '%{0}%'", DateTimeCtrl.Text);
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
                Value = row.Cells[e].Value.ToString();

                if (!ValueCellList.Contains(Value))
                    ValueCellList.Add(row.Cells[e].Value.ToString());
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

            for (int i = 1; i < CheckCtrl.Items.Count; i++)
            {
                itemChk = CheckCtrl.Items[i].ToString();
                statChk = CheckCtrl.GetItemChecked(i);
                Filter.Add(new FilterStatus() { columnName = col, valueString = itemChk, check = statChk });
            }
        }

        private void GetChkFilter()
        {
            List<FilterStatus> CheckList = new List<FilterStatus>();
            List<FilterStatus> CheckListSort = new List<FilterStatus>();

            foreach (FilterStatus val in Filter)
            {
                if (this.Columns[columnIndex].Name == val.columnName)
                {
                    CheckList.Add(new FilterStatus() { columnName = "", valueString = val.valueString, check = val.check });
                }
            }

            foreach (string ValueCell in GetDataColumns(columnIndex))
            {
                int index = CheckList.FindIndex(item => item.valueString == ValueCell);
                if (index == -1)
                {
                    CheckList.Add(new FilterStatus { valueString = ValueCell, check = true });
                }
            }

            CheckCtrl.Items.Add(CheckCtrlAllText, CheckState.Indeterminate);

            switch (this.Columns[columnIndex].ValueType.ToString())
            {
                case "System.Int32":
                    CheckListSort = CheckList.OrderBy(x => Int32.Parse(x.valueString)).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(val.valueString, CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(val.valueString, CheckState.Unchecked);
                    }
                    break;

                case "System.DateTime":
                    CheckListSort = CheckList.OrderBy(x => DateTime.Parse(x.valueString)).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(DateTime.Parse(val.valueString).ToString("dd.MM.yyyy"), CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(DateTime.Parse(val.valueString).ToString("dd.MM.yyyy"), CheckState.Unchecked);
                    }
                    break;
                default:

                    CheckListSort = CheckList.OrderBy(x => x.valueString).ToList();
                    foreach (FilterStatus val in CheckListSort)
                    {
                        if (val.check == true)
                            CheckCtrl.Items.Add(val.valueString, CheckState.Checked);
                        else
                            CheckCtrl.Items.Add(val.valueString, CheckState.Unchecked);
                    }
                    break;
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
                        StrFilter = StrFilter + ("[" + val.columnName + "] <> '" + val.valueString + "' ");
                    }
                    else
                    {
                        StrFilter = StrFilter + (" AND [" + val.columnName + "] <> '" + val.valueString + "' ");
                    }
                }
            }
            (this.DataSource as DataTable).DefaultView.RowFilter = StrFilter;
        }
    }
}
