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
    }
}
