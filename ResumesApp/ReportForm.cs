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
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        public ReportForm(List<ProfileJobs> pjList)
        {
            InitializeComponent();
            Point nextlocation = new Point(13, 13);
            foreach (var profjobs in pjList)
            {
                Label lblFullName = new Label();
                lblFullName.AutoSize = true;
                lblFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                lblFullName.Location = nextlocation;
                lblFullName.Size = new System.Drawing.Size(70, 25);
                lblFullName.TabIndex = 0;
                lblFullName.Text = profjobs.Profile.FullName;
                this.Controls.Add(lblFullName);

                nextlocation = new Point(nextlocation.X, nextlocation.Y + lblFullName.Height);
                foreach (var job in profjobs.RecentJobs)
                {
                    Label lblJobName = new Label();
                    lblJobName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    lblJobName.AutoSize = true;
                    lblJobName.Text = job.JobName;
                    lblJobName.Location = nextlocation;
                    nextlocation = new Point(nextlocation.X, nextlocation.Y + lblJobName.Height);
                    this.Controls.Add(lblJobName);

                    Label lblDetails = new Label();
                    lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    lblDetails.AutoSize = true;
                    StringBuilder details = new StringBuilder();
                    details.Append("Дата принятия: " + job.ReceiptDate.ToString("d"));
                    if (job.DismissDate != DateTime.MinValue)
                    {
                        details.Append(", дата увольнения: " + job.DismissDate.ToString("d") + ". Причина увольнения: " + job.DismissReason + ".");
                    }
                    else
                    {
                        details.Append(" по настоящее время.");
                    }
                    lblDetails.Text = details.ToString();
                    lblDetails.Location = nextlocation;
                    nextlocation = new Point(nextlocation.X, nextlocation.Y + lblDetails.Height);
                    this.Controls.Add(lblDetails);
                }
            }
        }
    }
}
