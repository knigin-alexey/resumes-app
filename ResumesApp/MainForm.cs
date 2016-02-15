using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResumesApp.DAL;
using ResumesApp.Model;

namespace ResumesApp
{
    public partial class MainForm : Form
    {
        private DataTable gridTable = QueryDb.GetProfiles();
        public MainForm()
        {
            InitializeComponent();
            PopulateDataGrid(profileDataGridView);
            InitBookmarks((DataTable)profileDataGridView.DataSource);
        }

        private void PopulateDataGrid(DataGridView dgv)
        {
            FormHelper.PopulateDataGrid(profileDataGridView, gridTable);
            dgv.Columns["Id"].Visible = false;
        }
        private void filterDGView(object sender, EventArgs e)
        {
            DataTable dt = gridTable;
            var byLetter = dt.AsEnumerable().Where(x => x["Fullname"].
                ToString().StartsWith(((LinkLabel)sender).Text))
                .CopyToDataTable();
            this.profileDataGridView.DataSource = byLetter;
        }
        private void selectJobsByProfile(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToInt32(((DataTable)profileDataGridView.DataSource).Rows[e.RowIndex]["Id"]) > 0)
            {
                int id = Convert.ToInt32(((DataTable)profileDataGridView.DataSource).Rows[e.RowIndex]["Id"]);
                string fullName = ((DataTable)profileDataGridView.DataSource).Rows[e.RowIndex]["FullName"].ToString();
                DataTable dt = QueryDb.GetJobsByProfile(id);
                ShowProfileForm(dt, id, fullName);
            }
        }
        private void ShowProfileForm(DataTable dt, int id, string fullName)
        {
            ProfileForm pForm = new ProfileForm(dt, id, fullName);
            pForm.Show();
        }
        private void InitBookmarks(DataTable dt)
        {
            var letters = dt.AsEnumerable()
                .Select(c => c["FullName"].ToString().Substring(0, 1))
                .GroupBy(r => r)
                .Select(x => x.Key).ToList();
            Point prevlocation = new Point(12, 27);

            foreach (var letter in letters)
            {
                LinkLabel newLink = new LinkLabel();
                newLink.Location = new Point(
                    prevlocation.X, prevlocation.Y + 15);
                prevlocation = newLink.Location;
                newLink.Text = letter;
                newLink.Width = 15;
                newLink.Height = 13;
                this.Controls.Add(newLink);
                newLink.BringToFront();
                newLink.Click += new System.EventHandler(this.filterDGView);
            }
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateDataGrid(profileDataGridView);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            PersonForm pForm = new PersonForm();
            pForm.Show();
            pForm.updateDGW += this.updateDGVfromDb;
        }
        private void updateDGVfromDb()
        {
            gridTable = QueryDb.GetProfiles();
            profileDataGridView.DataSource = gridTable;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = profileDataGridView.SelectedCells[0].RowIndex;
            DataRow row = ((DataTable)profileDataGridView.DataSource).Rows[index];
            Profile profile = new Profile(row);

            PersonForm pForm = new PersonForm(profile);
            pForm.Show();
            pForm.updateDGW += this.updateDGVfromDb;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (profileDataGridView.SelectedRows.Count > 0)
            {
                int[] profileIds = new int[profileDataGridView.SelectedRows.Count];
                int i = 0;
                foreach (DataGridViewRow row in profileDataGridView.SelectedRows)
                {
                    profileIds[i] = Convert.ToInt32(row.Cells["Id"].Value);
                    i++;
                }
                QueryDb.DeleteProfileByIds(profileIds);
                updateDGVfromDb();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            profileDataGridView.DataSource = gridTable.AsEnumerable()
                .Where(r => r.Field<string>("FullName").Contains(tbxSearch.Text)).CopyToDataTable();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (profileDataGridView.SelectedRows.Count > 0)
            {
                int[] profileIds = new int[profileDataGridView.SelectedRows.Count];
                int i = 0;
                foreach (DataGridViewRow row in profileDataGridView.SelectedRows)
                {
                    profileIds[i] = Convert.ToInt32(row.Cells["Id"].Value);
                    i++;
                }
                DataTable profiles = QueryDb.SelectProfilesByIds(profileIds);
                DataTable recentJobs = QueryDb.SelectJobsByProfileIds(profileIds);
                List<ProfileJobs> profJobList = new List<ProfileJobs>();
                foreach (var profile in profiles.AsEnumerable())
                {
                    ProfileJobs pj = new ProfileJobs();
                    pj.Profile = new Profile(profile);
                    DataTable jobs = recentJobs.AsEnumerable()
                        .Where(r => r.Field<int>("ProfileId") == pj.Profile.Id).CopyToDataTable();
                    foreach (var job in jobs.AsEnumerable())
                    {
                        RecentJob rj = new RecentJob(job);
                        pj.AddRecentJob(rj);
                    }

                    profJobList.Add(pj);
                }
                ReportForm report = new ReportForm(profJobList);
                report.Show();
            }
        }
    }
}
