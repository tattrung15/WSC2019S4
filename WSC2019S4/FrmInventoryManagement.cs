﻿using System;
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
    public partial class FrmInventoryManagement : Form
    {
        Session4Entities db = null;
        public FrmInventoryManagement()
        {
            InitializeComponent();
        }

        private void purchaseOrderManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPurchaseOrder frmPurchaseOrder = new FrmPurchaseOrder();
            frmPurchaseOrder.ShowDialog();
        }

        private void warehouseManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmWarehouseManagement frmWarehouse = new FrmWarehouseManagement();
            frmWarehouse.ShowDialog();
        }

        private void inventoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInventoryReport frmInventoryReport = new FrmInventoryReport();
            frmInventoryReport.ShowDialog();
        }

        private void FrmInventoryManagement_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();
            var list = db.OrderItems.ToList<OrderItem>()
                .Select(m => new
                {
                    PartName = m.Part.Name,
                    TransactionType = m.Order.TransactionType.Name,
                    Date = m.Order.Date,
                    Amount = m.Amount,
                    Source = (db.Warehouses.Where(x => x.ID == m.Order.SourceWarehouseID).SingleOrDefault() != null) ? db.Warehouses.Where(x => x.ID == m.Order.SourceWarehouseID).SingleOrDefault().Name : "",
                    Destination = (db.Warehouses.Where(x => x.ID == m.Order.DestinationWarehouseID).SingleOrDefault() != null) ? db.Warehouses.Where(x => x.ID == m.Order.DestinationWarehouseID).SingleOrDefault().Name : "",
                    ActionEdit = "Edit",
                    ActionRemove = "Remove",
                    OrderItemID = m.ID,
                    PartID = m.PartID,
                    OrderID = m.OrderID
                }).OrderBy(m => m.Date).ThenBy(m => m.TransactionType).ToList();
            dataGridView.Rows.Clear();
            dataGridView.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
            foreach (var i in list)
            {
                dataGridView.Rows.Add(i.PartName, i.TransactionType, i.Date, i.Amount, i.Source, i.Destination, i.ActionEdit, i.ActionRemove, i.OrderItemID, i.PartID, i.OrderID);
                if (i.TransactionType == "Purchase Order")
                {
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Cells["Amount"].Style.BackColor = Color.Green;
                }
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo);
                
                if(dialogResult == DialogResult.Yes)
                {
                    int partID = int.Parse(dataGridView.CurrentRow.Cells["PartID"].Value.ToString());
                    double minAmount = db.Parts.SingleOrDefault(m => m.ID == partID).MinimumAmount.Value;
                    double amountSelected = double.Parse(dataGridView.CurrentRow.Cells["Amount"].Value.ToString());
                    double totalAmount = double.Parse(db.OrderItems.Where(m => m.PartID == partID).Sum(m => m.Amount).ToString());
                    if (totalAmount - amountSelected < minAmount)
                    {
                        MessageBox.Show("This record can't remove");
                    }
                    else
                    {
                        int orderItemID = int.Parse(dataGridView.CurrentRow.Cells["OrderItemID"].Value.ToString());
                        db.OrderItems.Remove(db.OrderItems.SingleOrDefault(m => m.ID == orderItemID));
                        db.SaveChanges();
                        FrmInventoryManagement_Load(sender, e);
                        MessageBox.Show("Done");
                    }
                }
            }
            if (e.ColumnIndex == 6)
            {
                DataGridViewRow row = dataGridView.CurrentRow;
                if (row.Cells["TransactionType"].Value.ToString().Trim() == "Purchase Order")
                {
                    FrmPurchaseOrder frmPurchaseOrder = new FrmPurchaseOrder();
                    frmPurchaseOrder.Tag = row;
                    frmPurchaseOrder.isEdit = true;
                    frmPurchaseOrder.ShowDialog();
                    FrmInventoryManagement_Load(sender, e);
                }
                else
                {
                    FrmWarehouseManagement frmWarehouse = new FrmWarehouseManagement();
                    frmWarehouse.Tag = row;
                    frmWarehouse.isEdit = true;
                    frmWarehouse.ShowDialog();
                    FrmInventoryManagement_Load(sender, e);
                }
            }
        }
    }
}
