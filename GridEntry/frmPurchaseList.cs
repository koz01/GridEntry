using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GridEntry
{
    public partial class frmPurchaseList : Form
    {
        public string CatchID;
        private DataTable l_Datable;
        private Dictionary<int, bool> checkState;

        public frmPurchaseList()
        {

            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPurchaseList_Load(object sender, EventArgs e)
        {

            cboCardType.DisplayMember = "CardType";
            cboCardType.ValueMember = "CardID";
            cboCardType.Items.Insert(0, "-SELECT OPERATIONS-");
            cboCardType.DataSource = DataAccess.DataAccessController.getCard();

            getHistoryBindData();
        }

        private void getHistoryBindData()
        {

            dgvPurchaseList.DataSource = DataAccess.DataAccessController.getPurchaseItemList(dtpFDate.Text, dtpTdate.Text, 0);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            getHistoryBindData();
        }

        private void dgvPurchaseList_DoubleClick(object sender, EventArgs e)
        {
            CatchID = dgvPurchaseList.SelectedRows[0].Cells["PurID"].Value.ToString();
            this.Close();

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DataTable lstCustomer = new DataTable();
            lstCustomer.Columns.Add("SrNo");
            lstCustomer.Columns.Add("CardType");
            lstCustomer.Columns.Add("Price");
            lstCustomer.Columns.Add("QTY");
            lstCustomer.Columns.Add("TotalAmount");

            foreach (DataGridViewRow row in dgvPurchaseList.SelectedRows)
            {

                DataRow dr = lstCustomer.NewRow();

                string str = row.Cells[1].Value.ToString();

                dr["SrNo"] = row.Cells[dgvPurchaseList.Columns[2].Index].Value.ToString();
                dr["CardType"] = row.Cells[3].Value.ToString();
                dr["Price"] = row.Cells[4].Value.ToString();
                dr["QTY"] = row.Cells[5].Value.ToString();
                dr["TotalAmount"] = row.Cells[6].Value.ToString();
                lstCustomer.Rows.Add(dr.ItemArray);
                dr = null;
            }

            //foreach (DataGridViewRow row in dgvPurchaseList.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells[ChkSelect.Name].Value) == true)
            //    {
            //        DataRow dr = lstCustomer.NewRow();

                 
            //        dr["SrNo"] = row.Cells[dgvPurchaseList.Columns[2].Index].Value.ToString();
            //        dr["CardType"] = row.Cells[3].Value.ToString();
            //        dr["Price"] = row.Cells[4].Value.ToString();
            //        dr["QTY"] = row.Cells[5].Value.ToString();
            //        dr["TotalAmount"] = row.Cells[6].Value.ToString();
            //        lstCustomer.Rows.Add(dr.ItemArray);
            //        dr = null;

            //    }
            //}
            frmReport r = new frmReport();
            r.Show();
            ReportViewer v = r.Controls.Find("reportViewer1", true).FirstOrDefault() as ReportViewer;
            ReportDataSource dataset = new ReportDataSource("DataSet1", lstCustomer);
            v.LocalReport.ReportEmbeddedResource = "GridEntry.Report1.rdlc";
            v.LocalReport.DataSources.Clear();
            v.LocalReport.DataSources.Add(dataset);
            v.LocalReport.Refresh();
            dataset.Value = lstCustomer;
            v.RefreshReport();
            v.LocalReport.Refresh();
        }

    }
}
