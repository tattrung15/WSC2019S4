﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

            cbPartName.DataSource = db.OrderItems.Where(i => i.Order.SourceWarehouseID != null).Select(m => m.Part).Distinct().ToList();
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
            try
            {
                bool isExists = false;
                string batchNumber = cbBatchNumber.Text.Trim();
                try
                {
                    if (cbSourceWarehouse.Text == cbDestinationWarehouse.Text)
                    {
                        MessageBox.Show("Source Warehouse must be different Destination Warehouse !");
                        return;
                    }
                    if (batchNumber == "")
                    {
                        MessageBox.Show("You need to enter batch number !");
                        return;
                    }
                    else if (txtAmount.Text.Trim() == "")
                    {
                        MessageBox.Show("You need to enter Amount !");
                        return;
                    }
                    else if (txtAmount.Text.Trim() != "")
                    {
                        double.Parse(txtAmount.Text.Trim());
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid data");
                    return;
                }

                if (db.OrderItems.SingleOrDefault(m => m.BatchNumber == batchNumber) != null)
                {
                    MessageBox.Show("Batch Number is exists!");
                    return;
                }
                foreach (DataGridViewRow r in dataGridView.Rows)
                {
                    try
                    {
                        string cellBatchNumber = (r.Cells["BatchNumber"].Value != null) ? r.Cells["BatchNumber"].Value.ToString() : "";
                        if (cbBatchNumber.Text.Trim() == cellBatchNumber)
                        {
                            MessageBox.Show("Batch Number is exists!");
                            isExists = true;
                            return;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
                if (!isExists)
                {
                    int partID = int.Parse(cbPartName.SelectedValue.ToString());
                    if (db.Parts.SingleOrDefault(m => m.ID == partID && m.BatchNumberHasRequired == true) != null)
                    {
                        dataGridView.Rows.Add(cbPartName.Text, cbBatchNumber.Text, txtAmount.Text, "Remove", cbPartName.SelectedValue);
                    }
                    else
                    {
                        dataGridView.Rows.Add(cbPartName.Text, null, txtAmount.Text, "Remove", cbPartName.SelectedValue);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Add to List Fail !");
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {

                DialogResult dialogResult = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo);

                if(dialogResult == DialogResult.Yes)
                {
                    if (isEdit && dataGridView.CurrentRow.DefaultCellStyle.BackColor == Color.Beige)
                    {
                        int partID = int.Parse(dataGridView.CurrentRow.Cells["PartID"].Value.ToString());
                        double minAmount = db.Parts.SingleOrDefault(m => m.ID == partID).MinimumAmount.Value;
                        double amountSelected = double.Parse(dataGridView.CurrentRow.Cells["Amount"].Value.ToString());
                        double totalAmount = double.Parse(db.OrderItems.Where(m => m.PartID == partID).Sum(m => m.Amount).ToString());
                        if (totalAmount - amountSelected < minAmount)
                        {
                            MessageBox.Show("This record can't remove");
                            return;
                        }
                        else
                        {
                            dataGridView.Rows.Remove(dataGridView.CurrentRow);
                            MessageBox.Show("Done");
                        }
                    }
                    else
                    {
                        dataGridView.Rows.Remove(dataGridView.CurrentRow);
                        MessageBox.Show("Done");
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo);

            if(dialogResult == DialogResult.Yes)
            {
                if (dataGridView.Rows.Count > 0)
                {
                    Order order = new Order()
                    {
                        TransactionTypeID = 2,
                        SupplierID = null,
                        SourceWarehouseID = int.Parse(cbSourceWarehouse.SelectedValue.ToString()),
                        DestinationWarehouseID = int.Parse(cbDestinationWarehouse.SelectedValue.ToString()),
                        Date = date.Value
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    OrderItem orderItem = new OrderItem();
                    db.Database.ExecuteSqlCommand("DELETE dbo.OrderItems WHERE OrderID = @OrderID", new SqlParameter("@OrderID", orderID));
                    db.SaveChanges();

                    foreach (DataGridViewRow r in dataGridView.Rows)
                    {
                        orderItem = new OrderItem()
                        {
                            OrderID = db.Orders.ToList().LastOrDefault<Order>().ID,
                            PartID = int.Parse(r.Cells["PartID"].Value.ToString()),
                            BatchNumber = (r.Cells["BatchNumber"].Value != null) ? r.Cells["BatchNumber"].Value.ToString() : "",
                            Amount = decimal.Parse(r.Cells["Amount"].Value.ToString())
                        };
                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Submit success !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("At least one part needs to be added");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
