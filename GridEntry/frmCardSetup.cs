using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DataAccess;

namespace GridEntry
{
    public partial class frmCardSetup : Form
    {

      public  String catchID = "";

        public frmCardSetup()
        {
            InitializeComponent();
        }



        private void frmCardSetup_Load(object sender, EventArgs e)
        {

            CardType.DisplayMember = "CardType";
            CardType.ValueMember = "CardID";
            CardType.DataSource = DataAccess.DataAccessController.getCard();
            getGenerateVOU();
           
        }

        //Generate Vou No
        private void getGenerateVOU()
        {
            int InvNo = DataAccess.DataAccessController.getInvCount();
            txtVouNo.Text = string.Format("INV-" + "{0:00000}", ++InvNo);
        }

        private void dgvPurchase_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvPurchase.Rows.Count; i++)
                {
                    if (dgvPurchase.Rows[i].Cells["CardType"].Value != null)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            //DataTable dt = (DataTable)CardType.DataSource;
                            int getid = Convert.ToInt16(dgvPurchase.Rows[i].Cells[1].Value);
                            dgvPurchase.Rows[i].Cells["Amount"].Value = DataAccess.DataAccessController.getPrice(getid);
                        }
                    }

                    if (e.ColumnIndex == 3)
                    {
                        dgvPurchase.Rows[i].Cells["TotalAmount"].Value = Convert.ToInt32(dgvPurchase.Rows[i].Cells["Amount"].Value) * Convert.ToInt32(dgvPurchase.Rows[i].Cells["QTY"].Value);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtVouNo.Text.Trim() != "" && txtSupplier.Text.Trim() != "" && dgvPurchase.Rows[0].Cells["CardType"].Value != null && dgvPurchase.Rows[0].Cells["Qty"].Value != null && dgvPurchase.Rows[0].Cells["TotalAmount"].Value != null)
                {
                   // DataTable dt = (DataTable)dgvPurchase.DataSource;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("No");
                    dt.Columns.Add("CardTypeID");
                    dt.Columns.Add("Amount");
                    dt.Columns.Add("QTY");
                    dt.Columns.Add("TotalAmount");


                    for (int i = 0; i < dgvPurchase.Rows.Count; i++)
                    {
                        if (dgvPurchase.Rows[i].Cells[1].Value == null)
                        {
                            break;
                        }
                        DataRow dr = dt.NewRow();
                        dr["No"] = dgvPurchase.Rows[i].Cells[0].Value.ToString();
                        dr["CardTypeId"] = dgvPurchase.Rows[i].Cells[1].Value.ToString();
                        dr["Amount"] = dgvPurchase.Rows[i].Cells[2].Value.ToString();
                        dr["QTY"] = dgvPurchase.Rows[i].Cells[3].Value.ToString();
                        dr["TotalAmount"] = dgvPurchase.Rows[i].Cells[4].Value.ToString();                        
                        dt.Rows.Add(dr);

                      //  getInfo.GetCardID = Convert.ToInt32(dt.Rows[i]["CardTypeID"]);
                    }

                    if (btnSave.Text == "Save")
                    {
                        Pur_Inv_His InvHis = new Pur_Inv_His();
                        InvHis.VouNo = txtVouNo.Text.Trim();
                        InvHis.PurDate = dtpDate.Value;
                        InvHis.SupplierName = txtSupplier.Text.Trim();
                        InvHis.Remark = txtRemark.Text.Trim();

                        int l_return = DataAccess.DataAccessController.SaveInvDAO(InvHis, dt);
                        if (l_return > 0)
                        {
                            MessageBox.Show("Save Successfully", "Save");
                        }
                    }

                    ClearData();
                }
                else
                {
                    MessageBox.Show("No Data found to Save", "Unsuccess Process", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearData()
        {
            txtVouNo.Text = "";
            txtSupplier.Text = "";
            txtRemark.Text = "";
            dtpDate.Value = DateTime.Now;
            dgvPurchase.Rows.Clear();
            dgvPurchase.Refresh();
            txtQty.Text = "";
            txtTotal.Text = "";
            getGenerateVOU();
        }


        private void txtVouNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtSupplier.Focus();
                txtSupplier.BackColor = Color.GhostWhite;
            }
        }

        private void txtSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                txtRemark.Focus();
            txtRemark.BackColor = Color.GhostWhite;

        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < dgvPurchase.Rows.Count; i++)
            {
                if (e.KeyData == Keys.Enter)
                {
                    dgvPurchase.Rows[i].Cells[0].Selected = true;
                }
            }
        }



        private void txtSupplier_Leave(object sender, EventArgs e)
        {
            txtSupplier.BackColor = Color.WhiteSmoke;
        }

        private void txtVouNo_Leave(object sender, EventArgs e)
        {
            txtVouNo.BackColor = Color.WhiteSmoke;
        }

        private void txtRemark_Leave(object sender, EventArgs e)
        {
            txtRemark.BackColor = Color.WhiteSmoke;
        }

        private void dgvPurchase_CurrentCellChanged(object sender, EventArgs e)
        {
            decimal totalAmt = 0;
            int Count = 0;
            for (int i = 0; i < dgvPurchase.Rows.Count; i++)
            {
                if (dgvPurchase.Rows[i].Cells["QTY"].Value != null)
                {
                    string str = dgvPurchase.Rows[i].Cells["QTY"].Value.ToString();
                    if (str.Trim() == "") return;
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (!char.IsNumber(str[j]))
                        {
                            MessageBox.Show("Please Number Qty", "Informaion");
                            dgvPurchase.Rows[i].Cells["QTY"].Value = null;
                            dgvPurchase.Rows[i].Cells["TotalAmount"].Value = null;
                        }
                        else
                        {
                            Count = Count + Convert.ToInt32(dgvPurchase.Rows[i].Cells["QTY"].Value);
                            totalAmt = totalAmt + Convert.ToDecimal(dgvPurchase.Rows[i].Cells["TotalAmount"].Value);
                            dgvPurchase.Rows[i].Cells["No"].Value = i + 1;
                        }
                    }
                    txtQty.Text = Count.ToString();
                    txtTotal.Text = totalAmt.ToString();
                }

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPurchase.Rows.Count > 1)
            {
                DialogResult msg =MessageBox.Show("Are your sure want to delete","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    if (dgvPurchase.CurrentRow.Cells[1].Value != null && dgvPurchase.CurrentRow.Cells[2].Value != null && dgvPurchase.CurrentRow.Cells[3] != null && dgvPurchase.CurrentRow.Cells[4].Value != null)
                    {
                        dgvPurchase.Rows.RemoveAt(dgvPurchase.CurrentRow.Index);
                        dgvPurchase.Refresh();
                        txtQty.Text = "";
                        txtTotal.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("This Row is  data Record", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }               
            }
            else
            {
                MessageBox.Show("There is no Data Sources", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmPurchaseList frmList = new frmPurchaseList();
            frmList.ShowDialog();
            btnSave.Text = "Update";

            this.catchID = frmList.CatchID;
        }

    }
}
