using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumesApp.Model
{
    public class RecentJob
    {
        /*"CREATE TABLE [RecentJobs] (" +
         "[Id] int IDENTITY (1,1) NOT NULL" +
        ", [ProfileId] int NOT NULL" +
        ", [JobName] nvarchar(300) NOT NULL" +
        ", [ReceiptDate] datetime NOT NULL" +
        ", [DismissDate] datetime NULL" +
        ", [DismissReason] nvarchar(300) NOT NULL" +*/

        int id;
        int profileId;
        string jobName;
        DateTime receiptDate;
        DateTime dismissDate;
        string dismissReason;
        public RecentJob() { }
        public RecentJob(System.Data.DataRow row)
        {
            this.Id = Convert.ToInt32(row["Id"]);
            this.JobName = row["JobName"].ToString();
            this.ProfileId = Convert.ToInt32(row["ProfileId"]);
            this.DismissReason = row["DismissReason"].ToString();
            this.ReceiptDate = DateTime.Parse(row["ReceiptDate"].ToString());
            if (row["DismissDate"] != System.DBNull.Value)
            {
                this.DismissDate = DateTime.Parse(row["DismissDate"].ToString());
            }
            else
            {
                this.DismissDate = DateTime.MinValue;
            }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int ProfileId
        {
            get { return profileId; }
            set { profileId = value; }
        }
        public string JobName
        {
            get { return jobName; }
            set { jobName = value; }
        }
        public DateTime ReceiptDate
        {
            get { return receiptDate; }
            set { receiptDate = value; }
        }
        public DateTime DismissDate
        {
            get { return dismissDate; }
            set { dismissDate = value; }
        }
        public string DismissReason
        {
            get { return dismissReason; }
            set { dismissReason = value; }
        }
    }
}
