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
    public partial class FrmBatchNumbers : Form
    {
        public FrmBatchNumbers()
        {
            InitializeComponent();
        }

        Session4Entities db = null;
        DataGridViewRow row;
        int partID;
        private void FrmBatchNumbers_Load(object sender, EventArgs e)
        {
            db = new Session4Entities();
            row = this.Tag as DataGridViewRow;
            partID = int.Parse(row.Cells["PartID"].Value.ToString());
            var list = db.OrderItems.Where(m => m.PartID == partID)
                .Select(m => new {
                    BatchNumber = m.BatchNumber
                }).Distinct().ToList();
            dataGridView.Rows.Clear();
            foreach(var r in list)
            {
                dataGridView.Rows.Add(r.BatchNumber);
            }
        }
    }
}
