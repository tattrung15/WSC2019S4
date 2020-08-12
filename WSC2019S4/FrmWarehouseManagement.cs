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

        private void FrmWarehouseManagement_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();
            cbSourceWarehouse.DataSource = db.Warehouses.ToList();
            cbSourceWarehouse.DisplayMember = "Name";
            cbSourceWarehouse.ValueMember = "ID";

            cbDestinationWarehouse.DataSource = db.Warehouses.ToList();
            cbDestinationWarehouse.DisplayMember = "Name";
            cbDestinationWarehouse.ValueMember = "ID";

            cbPartName.DataSource = db.Parts.ToList();
            cbPartName.DisplayMember = "Name";
            cbPartName.ValueMember = "ID";

            if (isEdit)
            {

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
