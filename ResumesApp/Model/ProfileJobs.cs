using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumesApp.Model
{
    public class ProfileJobs
    {
        private Profile profile;
        private List<RecentJob> recentJobs = new List<RecentJob>();

        public Profile Profile
        {
            get { return profile; }
            set { profile = value; }
        }
        public List<RecentJob> RecentJobs
        {
            get { return recentJobs; }
            set { recentJobs = value; }
        }
        public void AddRecentJob(RecentJob job)
        {
            recentJobs.Add(job);
        }
    }
}
