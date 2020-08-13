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
    public partial class FrmInventoryReport : Form
    {
        public FrmInventoryReport()
        {
            InitializeComponent();
        }

        Session4Entities db = null;
        private void FrmInventoryReport_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();
            cbWarehouse.DataSource = db.Warehouses.ToList();
            cbWarehouse.DisplayMember = "Name";
            cbWarehouse.ValueMember = "ID";

            long wareHouseID = long.Parse(cbWarehouse.SelectedValue.ToString());

            var list = db.Parts.ToList()
                .Select(m => new { 
                    PartName = m.Name,
                    CurrentStock = db.OrderItems.Where(i => i.PartID == m.ID).Count(),
                    ReceivedStock = db.OrderItems.Where(i => i.PartID == m.ID && i.Order.DestinationWarehouseID == wareHouseID).Count(),
                    Action = (m.BatchNumberHasRequired == true) ? "View Batch Numbers" : "",
                    PartID = m.ID
                });
            dataGridView.Rows.Clear();
            foreach(var r in list)
            {
                dataGridView.Rows.Add(r.PartName, r.CurrentStock, r.ReceivedStock, r.Action, r.PartID);
            }
        }

        private void cbWarehouse_SelectionChangeCommitted(object sender, EventArgs e)
        {
            long wareHouseID = long.Parse(cbWarehouse.SelectedValue.ToString());

            var list = db.Parts.ToList()
                .Select(m => new {
                    PartName = m.Name,
                    CurrentStock = db.OrderItems.Where(i => i.PartID == m.ID).Count(),
                    ReceivedStock = db.OrderItems.Where(i => i.PartID == m.ID && i.Order.DestinationWarehouseID == wareHouseID).Count(),
                    Action = (m.BatchNumberHasRequired == true) ? "View Batch Numbers" : "",
                    PartID = m.ID
                });
            dataGridView.Rows.Clear();
            foreach (var r in list)
            {
                dataGridView.Rows.Add(r.PartName, r.CurrentStock, r.ReceivedStock, r.Action, r.PartID);
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView.CurrentRow;
            FrmBatchNumbers frmBatchNumbers = new FrmBatchNumbers();
            frmBatchNumbers.Tag = row;
            frmBatchNumbers.ShowDialog();
            FrmInventoryReport_Load(sender, e);
        }
    }
}
