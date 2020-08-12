using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSC2019S4
{
    public partial class FrmWarehouseManagement : Form
    {
        public FrmWarehouseManagement()
        {
            InitializeComponent();
        }
        Session4Entities db = null;
        public bool isEdit;
        DataGridViewRow row;
        int orderID;
        private void FrmWarehouseManagement_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();
            cbSourceWarehouse.DataSource = db.Warehouses.ToList();
            cbSourceWarehouse.DisplayMember = "Name";
            cbSourceWarehouse.ValueMember = "ID";

            cbDestinationWarehouse.DataSource = db.Warehouses.ToList();
            cbDestinationWarehouse.DisplayMember = "Name";
            cbDestinationWarehouse.ValueMember = "ID";

            cbPartName.DataSource = db.OrderItems.Where(i => i.Order.SourceWarehouseID != null).Select(m => m.Part).ToList();
            cbPartName.DisplayMember = "Name";
            cbPartName.ValueMember = "ID";

            if (isEdit)
            {
                row = this.Tag as DataGridViewRow;
                orderID = int.Parse(row.Cells["OrderID"].Value.ToString());
                Order order = db.Orders.SingleOrDefault(m => m.ID == orderID);
                cbSourceWarehouse.SelectedValue = order.SourceWarehouseID;
                cbDestinationWarehouse.SelectedValue = order.DestinationWarehouseID;
                date.Value = order.Date;
                var list = db.OrderItems.Where(m => m.OrderID == orderID).Select(x => new
                {
                    PartName = x.Part.Name,
                    BatchNumber = x.BatchNumber,
                    Amount = x.Amount,
                    Action = "Remove",
                    PartID = x.PartID
                }).ToList();
                dataGridView.Rows.Clear();
                int index = 0;
                foreach(var r in list)
                {
                    dataGridView.Rows.Add(r.PartName, r.BatchNumber, r.Amount, r.Action, r.PartID);
                    dataGridView.Rows[index].DefaultCellStyle.BackColor = Color.Beige;
                    index++;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
