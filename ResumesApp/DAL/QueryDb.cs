using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumesApp.Model;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Data;

namespace ResumesApp.DAL
{
    public static class QueryDb
    {
        static string connStr = string.Format("Datasource=\"{0}\"; Password=\"{1}\"", "Resumes.sdf", "resumes");

        public static DataTable GetProfiles()
        {
            string sql = "SELECT Id, Fullname, BirthDate, BirthPlace, PassportData, PersonalQualities," +
                " Characteristics, EntryDate FROM Profiles ORDER BY FullName";
            return SelectData(sql);
        }

        public static DataTable QueryWParams(string sql, Dictionary<string, object> parameters, bool isSelect)
        {
            DataTable result = new DataTable();
            using (SqlCeConnection cn = new SqlCeConnection(connStr))
            {
                SqlCeCommand cmd;
                SqlCeDataAdapter dAdapt;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                try
                {
                    cmd = new SqlCeCommand(sql, cn);
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }                   
                    if (isSelect)
                    {
                        dAdapt = new SqlCeDataAdapter(cmd);
                        dAdapt.Fill(result);
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlCeException sqlexception)
                {
                    MessageBox.Show(sqlexception.Message, "Sql Exception"
                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception"
                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                cn.Close();
            }

            return result;

        }

        public static DataTable GetJobsByProfile(int profileId)
        {
            string sql = "SELECT r.Id, r.ProfileId, p.FullName, r.JobName, r.ReceiptDate, r.DismissDate, r.DismissReason" +
                " FROM RecentJobs r JOIN Profiles p ON r.profileId = p.Id WHERE ProfileId = @ProfileId ORDER BY ReceiptDate";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ProfileId", profileId);
            return QueryWParams(sql, parameters, true);
        }
        public static void InsertRecentJob(RecentJob job)
        {
            string sql = "insert into RecentJobs " +
                "(ProfileId, JobName, ReceiptDate, DismissDate, DismissReason) " +
                "values (@ProfileId, @JobName, @ReceiptDate, @DismissDate, @DismissReason)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ProfileId", job.ProfileId);
            parameters.Add("JobName", job.JobName);
            parameters.Add("ReceiptDate", job.ReceiptDate);
            if (job.DismissDate != DateTime.MinValue)
            {
                parameters.Add("DismissDate", job.DismissDate);
            }
            else
            {
                parameters.Add("DismissDate", DBNull.Value);
            }
            parameters.Add("DismissReason", job.DismissReason ?? Convert.DBNull);
            QueryWParams(sql, parameters, false);
        }
        public static void UpdateRecentJob(RecentJob job)
        {
            string sql = "UPDATE RecentJobs SET JobName = @JobName, ReceiptDate = @ReceiptDate, " + 
                "DismissDate = @DismissDate, DismissReason = @DismissReason WHERE " + 
                "Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();            
            parameters.Add("JobName", job.JobName);
            parameters.Add("ReceiptDate", job.ReceiptDate);
            if (job.DismissDate != DateTime.MinValue)
            {
                parameters.Add("DismissDate", job.DismissDate);
            }
            else
            {
                parameters.Add("DismissDate", DBNull.Value);
            }
            parameters.Add("DismissReason", job.DismissReason ?? Convert.DBNull);
            parameters.Add("Id", job.Id);
            QueryWParams(sql, parameters, false);
        }
        public static DataTable SelectProfilesByIds(int[] profileIds)
        {
            var sqlParams = new string[profileIds.Length];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < profileIds.Length; i++)
            {
                sqlParams[i] = string.Format("@Id{0}", i);
                parameters.Add(sqlParams[i], profileIds[i]);
            }
            string sql = String.Format("SELECT * FROM Profiles WHERE Id IN ({0})", string.Join(", ", sqlParams));

            return QueryWParams(sql, parameters, true);
        }
        public static DataTable SelectJobsByProfileIds(int[] profileIds)
        {
            var sqlParams = new string[profileIds.Length];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < profileIds.Length; i++)
            {
                sqlParams[i] = string.Format("@Id{0}", i);
                parameters.Add(sqlParams[i], profileIds[i]);
            }
            string sql = String.Format("SELECT * FROM RecentJobs WHERE ProfileId IN ({0})", string.Join(", ", sqlParams));
            return QueryWParams(sql, parameters, true);
        }
        public static void DeleteRecentJobsByJobId(int[] jobIds)
        {
            var sqlParams = new string[jobIds.Length];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < jobIds.Length; i++)
            {
                sqlParams[i] = string.Format("@Id{0}", i);
                parameters.Add(sqlParams[i], jobIds[i]);
            }
            string sql = String.Format("DELETE FROM RecentJobs WHERE Id IN ({0})", string.Join(", ", sqlParams));
            QueryWParams(sql, parameters, false);
        }

        public static void InsertProfile(Profile profile)
        {
            string sql = "insert into Profiles " +
                "(FullName, BirthDate, BirthPlace, PassportData, " +
                " PersonalQualities, Characteristics, EntryDate) " +
                "values (@FullName, @BirthDate, @BirthPlace, @PassportData," +
                " @PersonalQualities, @Characteristics, @EntryDate)";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@FullName", profile.FullName);
            parameters.Add("@BirthDate", profile.BirthDate);
            parameters.Add("@BirthPlace", profile.BirthPlace);
            parameters.Add("@PassportData", profile.PassportData);
            parameters.Add("@PersonalQualities", profile.PersonalQualities);
            parameters.Add("@Characteristics", profile.Characteristics);
            parameters.Add("@EntryDate", profile.EntryDate);

            QueryWParams(sql, parameters, false);
        }
        public static void UpdateProfile(Profile profile)
        {
            string sql = "UPDATE Profiles SET FullName = @FullName, BirthDate = @BirthDate, " +
               "BirthPlace = @BirthPlace, PassportData = @PassportData, PersonalQualities = @PersonalQualities, " +
               "Characteristics = @Characteristics WHERE Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@FullName", profile.FullName);
            parameters.Add("@BirthDate", profile.BirthDate);
            parameters.Add("@BirthPlace", profile.BirthPlace);
            parameters.Add("@PassportData", profile.PassportData);
            parameters.Add("@PersonalQualities", profile.PersonalQualities);
            parameters.Add("@Characteristics", profile.Characteristics);
            parameters.Add("@Id", profile.Id);

            QueryWParams(sql, parameters, false);
        }
        public static void DeleteRecentJobsByProfileId(int profileId)
        {
            string sql = "DELETE FROM RecentJobs WHERE ProfileId = @ProfileId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProfileId", profileId);
            QueryWParams(sql, parameters, false);
        }
        public static void DeleteProfileByIds(int[] profileIds)
        {
            var sqlParams = new string[profileIds.Length];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < profileIds.Length; i++)
            {
                DeleteRecentJobsByProfileId(profileIds[i]);
                sqlParams[i] = string.Format("@Id{0}", i);
                parameters.Add(sqlParams[i], profileIds[i]);
            }
            string sql = String.Format("DELETE FROM Profiles WHERE Id IN ({0})", string.Join(", ", sqlParams));
            QueryWParams(sql, parameters, false);
        }
        private static DataTable SelectData(string sql)
        {
            DataTable result = new DataTable();
            using (SqlCeDataAdapter dAdapt = new SqlCeDataAdapter(sql, connStr))
            {
                dAdapt.Fill(result);
            }
            return result;
        }

    }
}
