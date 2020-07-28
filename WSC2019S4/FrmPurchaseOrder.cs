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
    public partial class FrmPurchaseOrder : Form
    {
        public FrmPurchaseOrder()
        {
            InitializeComponent();
        }
        Session4Entities db = null;
        public bool isEdit;
        DataGridViewRow row;
        int orderID;
        private void FrmPurchaseOrder_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();

            cbSuppliers.DataSource = db.Suppliers.ToList();
            cbSuppliers.DisplayMember = "Name";
            cbSuppliers.ValueMember = "ID";

            cbWarehouse.DataSource = db.Warehouses.ToList();
            cbWarehouse.DisplayMember = "Name";
            cbWarehouse.ValueMember = "ID";

            cbPartName.DataSource = db.Parts.ToList();
            cbPartName.DisplayMember = "Name";
            cbPartName.ValueMember = "ID";

            if (isEdit)
            {
                row = this.Tag as DataGridViewRow;
                orderID = int.Parse(row.Cells["OrderID"].Value.ToString());
                Order order = db.Orders.SingleOrDefault(m => m.ID == orderID);
                cbSuppliers.SelectedValue = order.SupplierID;
                cbWarehouse.SelectedValue = order.SourceWarehouseID;
                date.Value = order.Date.Value;

                var list = db.OrderItems.Where(m => m.Order.ID == orderID).Select(x => new
                {
                    PartName = x.Part.Name,
                    BatchNumber = x.BatchNumber,
                    Amount = x.Amount,
                    Action = "Remove",
                    PartID = x.PartID
                }).ToList();

                dataGridView.Rows.Clear();
                int index = 0;
                foreach(var i in list)
                {
                    dataGridView.Rows.Add(i.PartName, i.BatchNumber, i.Amount, i.Action, i.PartID);
                    dataGridView.Rows[index].DefaultCellStyle.BackColor = Color.Beige;
                }
            }
        }
    }
}
