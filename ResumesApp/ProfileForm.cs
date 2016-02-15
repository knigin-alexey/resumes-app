using ResumesApp.DAL;
using ResumesApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResumesApp
{
    public partial class ProfileForm : Form
    {
        private DataTable gridTable;
        private int profileId;
        private string workerName;
        public ProfileForm(DataTable dt, int id, string fullName)
        {
            gridTable = dt;
            InitializeComponent();
            profileId = id;
            workerName = fullName;
            lblFullName.Text = workerName;
            PopulateDataGrid(dataGridView1);
        }
        private void PopulateDataGrid(DataGridView dgv)
        {
            FormHelper.PopulateDataGrid(dgv, gridTable);
            dgv.Columns["Id"].Visible = false;
            dgv.Columns["ProfileId"].Visible = false;
            dgv.Columns["FullName"].Visible = false;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            JobForm jForm = new JobForm(profileId, workerName);
            jForm.Show();
            jForm.updateDGW += this.updateDGVfromDb;
        }

        private void updateDGVfromDb(int id)
        {
            gridTable = QueryDb.GetJobsByProfile(id);
            dataGridView1.DataSource = gridTable;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToInt32(((DataTable)dataGridView1.DataSource).Rows[e.RowIndex]["Id"]) > 0)
            {
                RecentJob job = new RecentJob(((DataTable)dataGridView1.DataSource).Rows[e.RowIndex]);

                JobForm jForm = new JobForm(job, workerName);
                jForm.Show();
                jForm.updateDGW += this.updateDGVfromDb;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int[] jobIds = new int[dataGridView1.SelectedRows.Count];
                int i = 0;
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    jobIds[i] = Convert.ToInt32(row.Cells["Id"].Value);
                    i++;
                }
                QueryDb.DeleteRecentJobsByJobId(jobIds);
                updateDGVfromDb(profileId);
            }
        }
    }
}
