using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumesApp.Model
{
    public class Profile
    {
        /*"[Id] int IDENTITY (1,1) NOT NULL" +
                ", [FullName] nvarchar(300) NOT NULL" +
                ", [BirthDate] datetime NOT NULL" +
                ", [BirthPlace] nvarchar(300) NOT NULL" +
                ", [PassportData] nvarchar(50) NOT NULL" +
                ", [PersonalQualities] nvarchar(300) NULL" +
                ", [Characteristics] nvarchar(300) NULL" +
                ", [EntryDate] datetime NOT NULL" +*/
        int id;
        string fullName;
        DateTime birthDate;
        string birthPlace;
        string passportData;
        string personalQualities;
        string characteristics;
        DateTime entryDate;

        public Profile() { }

        public Profile(DataRow row)
        {
            this.Id = Convert.ToInt32(row["Id"]);
            this.FullName = row["FullName"].ToString();
            this.BirthDate = DateTime.Parse(row["BirthDate"].ToString());
            this.BirthPlace = row["BirthPlace"].ToString();
            this.PassportData = row["PassportData"].ToString();
            this.PersonalQualities = row["PersonalQualities"].ToString();
            this.Characteristics = row["Characteristics"].ToString();
            this.EntryDate = DateTime.Parse(row["EntryDate"].ToString());
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string FullName 
        {
            get { return fullName; }
            set { fullName = value; }
        }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }
        public string BirthPlace
        {
            get { return birthPlace; }
            set { birthPlace = value; }
        }
        public string PassportData
        {
            get { return passportData; }
            set { passportData = value; }
        }
        public string PersonalQualities
        {
            get { return personalQualities; }
            set { personalQualities = value; }
        }
        public string Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
    }
}
