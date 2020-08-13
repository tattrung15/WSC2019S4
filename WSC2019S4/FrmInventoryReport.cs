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
        }
    }
}
