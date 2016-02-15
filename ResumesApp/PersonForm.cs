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
    public partial class PersonForm : Form
    {
        public delegate void MethodContainer();
        public event MethodContainer updateDGW;
        Profile currProfile = new Profile();

        public PersonForm()
        {
            InitializeComponent();
        }
        public PersonForm(Profile profile)
        {
            currProfile = profile;
            InitializeComponent();
            dtpBirthDay.Value = profile.BirthDate;
            tbxBirthPlace.Text = profile.BirthPlace;
            tbxCharacteristics.Text = profile.Characteristics;
            tbxFullName.Text = profile.FullName;
            tbxPassportData.Text = profile.PassportData;
            tbxPersonalQualities.Text = profile.PersonalQualities;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (tbxFullName.Text != "" && tbxBirthPlace.Text != "" && tbxPassportData.Text != "" &&
                tbxPersonalQualities.Text != "" && tbxCharacteristics.Text != "")
            {
                currProfile.FullName = tbxFullName.Text;
                currProfile.BirthDate = dtpBirthDay.Value;
                currProfile.BirthPlace = tbxBirthPlace.Text;
                currProfile.PassportData = tbxPassportData.Text;
                currProfile.PersonalQualities = tbxPersonalQualities.Text;
                currProfile.Characteristics = tbxCharacteristics.Text;
                if (currProfile.EntryDate == DateTime.MinValue)
                {
                    currProfile.EntryDate = DateTime.Today;
                }
                if (currProfile.Id == 0)
                {
                    QueryDb.InsertProfile(currProfile);
                }
                else
                {
                    QueryDb.UpdateProfile(currProfile);
                }

                updateDGW();
                this.Close();
            }
            else 
            {
                MessageBox.Show("Заполните все поля.");
            }
        }
    }
}
