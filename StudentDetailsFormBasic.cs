using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CFCA_ADMIN
{
    public partial class StudentDetailsFormBasic : UserControl
    {
        private string studentNumber;
        public StudentDetailsFormBasic(string studentNumber)
        {
            InitializeComponent();
            this.studentNumber = studentNumber;
            lblStudentNumber.Text = studentNumber;
        }

        private void StudentDetailsFormBasic_Load(object sender, EventArgs e)
        {
            ShowStudentInfo(studentNumber);
        }

        private void ShowStudentInfo(string studentNumber)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM basic_ed_enrollment WHERE student_number = @studentNumber";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentNumber", studentNumber);
                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // ============ BASIC INFO ============
                    lblFullName.Text = $"{dr["surname"]}, {dr["first_name"]} {dr["middle_name"]}";
                    lblLRN.Text = dr["lrn"].ToString();
                    lblStatus.Text = dr["enrollment_status"].ToString();

                    // ============ PERSONAL INFORMATION ============
                    lblLevel.Text = dr["level_applied"].ToString();
                    lblRegistrar.Text = dr["level_for_registrar"].ToString();
                    lblAge.Text = dr["age"].ToString();
                    lblGender.Text = dr["gender"].ToString();
                    lblDOB.Text = Convert.ToDateTime(dr["dob"]).ToString("MM/dd/yyyy");
                    lblPOB.Text = dr["pob"].ToString();
                    lblCitizenship.Text = dr["citizenship"].ToString();
                    lblReligion.Text = dr["religion"].ToString();
                    lblAddress.Text = dr["address"].ToString();
                    lblContact.Text = dr["contact"].ToString();
                    lblEmail.Text = dr["email"].ToString();

                    // ============ FAMILY INFORMATION - FATHER ============
                    fathersname.Text = dr["father_name"].ToString();
                    fathersoccupation.Text = dr["father_occupation"].ToString();
                    fathersoffice.Text = dr["father_office"].ToString();
                    fatherscontact.Text = dr["father_contact"].ToString();
                    fathersemail.Text = dr["father_email"].ToString();

                    // ============ FAMILY INFORMATION - MOTHER ============
                    mothersname.Text = dr["mother_name"].ToString();
                    mothersoccupation.Text = dr["mother_occupation"].ToString();
                    mothersoffice.Text = dr["mother_office"].ToString();
                    motherscontact.Text = dr["mother_contact"].ToString();
                    mothersemail.Text = dr["mother_email"].ToString();

                    // ============ FAMILY INFORMATION - GUARDIAN ============
                    guardiansname.Text = dr["guardian_name"].ToString();
                    guardiansrelation.Text = dr["guardian_relation"].ToString();
                    guardiansoccupation.Text = dr["guardian_occupation"].ToString();
                    guardianscontact.Text = dr["guardian_contact"].ToString();
                    guardiansemail.Text = dr["guardian_email"].ToString();

                    // ============ EDUCATIONAL BACKGROUND ============
                    lblPrevStudent.Text = dr["prev_ctfcai"].ToString();
                    lblPrevGrade.Text = dr["prev_grade"].ToString();
                    lblPrevSchool.Text = dr["prev_school"].ToString();
                    lblSchoolYear.Text = dr["prev_school_sy"].ToString();
                    lblSchoolAdd.Text = dr["prev_school_addr"].ToString();

                    // ============ HEALTH / EMERGENCY ============
                    lblHealth.Text = dr["health_conditions"].ToString();
                    lblEmergencyName.Text = dr["emergency_name"].ToString();
                    lblEmergencyAddress.Text = dr["emergency_address"].ToString();
                    lblEmergencyContact.Text = dr["emergency_contact"].ToString();
                    lblSchoolYearDetails.Text = dr["school_year"].ToString();
                    lblSubmitted.Text = Convert.ToDateTime(dr["submitted_at"]).ToString("MM/dd/yyyy");

                    // ============ PHOTO ============
                    if (dr["student_photo"] != DBNull.Value)
                    {
                        try
                        {
                            byte[] photoBytes = (byte[])dr["student_photo"];
                            using (var ms = new MemoryStream(photoBytes))
                            {
                                studentPicture.Image = new Bitmap(ms);
                            }
                            studentPicture.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        catch
                        {
                            studentPicture.Image = Properties.Resources.no_image;
                        }
                    }
                    else
                    {
                        studentPicture.Image = Properties.Resources.no_image;
                    }
                }

                    dr.Close();


                // ============ LOAD SIBLINGS DATA ============
                string siblingQuery = "SELECT name, grade_occupation FROM basic_ed_siblings WHERE enrollment_id = (SELECT id FROM basic_ed_enrollment WHERE student_number = @studentNumber)";
                MySqlCommand siblingCmd = new MySqlCommand(siblingQuery, conn);
                siblingCmd.Parameters.AddWithValue("@studentNumber", studentNumber);
                MySqlDataReader siblingReader = siblingCmd.ExecuteReader();

                List<string> siblingNames = new List<string>();
                List<string> siblingGrades = new List<string>();

                while (siblingReader.Read())
                {
                    siblingNames.Add(siblingReader["name"].ToString());
                    siblingGrades.Add(siblingReader["grade_occupation"].ToString());
                }

                siblingReader.Close();

                // ✅ Display siblings (each on a new line)
                if (siblingNames.Count > 0)
                {
                    lblsiblingsName.Text = string.Join("\n", siblingNames);
                    lblsiblingsOccup.Text = string.Join("\n", siblingGrades);
                }
                else
                {
                    lblsiblingsName.Text = "N/A";
                    lblsiblingsOccup.Text = "N/A";
                }
            }
        }

        // Add this event to the StudentDetailsFormBasic class
        public event EventHandler BackButtonClicked;

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            BackButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
