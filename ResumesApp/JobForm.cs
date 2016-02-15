using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResumesApp.Model;
using ResumesApp.DAL;

namespace ResumesApp
{
    public partial class JobForm : Form
    {
        public delegate void MethodContainer(int profileId);
        public event MethodContainer updateDGW;

        RecentJob currentJob = new RecentJob();
        public JobForm(int profileId, string worker)
        {
            InitializeComponent();
            this.Text = worker;
            currentJob.ProfileId = profileId;            
        }
        public JobForm(RecentJob job, string worker)
        {
            currentJob = job;
            InitializeComponent();
            this.Text = worker;
            tBxJobName.Text = currentJob.JobName;
            dTPReceiptDate.Value = currentJob.ReceiptDate;
            if (currentJob.DismissDate != DateTime.MinValue)
            {
                dTPDissmissDate.Value = currentJob.DismissDate;
            }
            else
            {
                dTPDissmissDate.Value = DateTime.Today;
                checkBox1.Checked = true;
            }
            tBxDismissReason.Text = currentJob.DismissReason;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bntConfirm_Click(object sender, EventArgs e)
        {
            if (tBxJobName.Text != "")
            {
                currentJob.JobName = tBxJobName.Text;
                currentJob.ReceiptDate = dTPReceiptDate.Value;
                if (!checkBox1.Checked)
                {
                    currentJob.DismissDate = dTPDissmissDate.Value;
                }
                else
                {
                    currentJob.DismissDate = DateTime.MinValue;
                }
                currentJob.DismissReason = tBxDismissReason.Text;
                if (currentJob.Id == 0)
                {
                    QueryDb.InsertRecentJob(currentJob);
                }
                else
                {
                    QueryDb.UpdateRecentJob(currentJob);
                }

                updateDGW(currentJob.ProfileId);
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите название места работы.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                dTPDissmissDate.Enabled = false;
                tBxDismissReason.Text = "";
            }
            else 
            {
                dTPDissmissDate.Enabled = true;
                tBxDismissReason.Text = currentJob.DismissReason;
            }
        }
    }
}
