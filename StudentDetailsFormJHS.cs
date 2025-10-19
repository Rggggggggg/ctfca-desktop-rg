using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class StudentDetailsFormJHS : UserControl
    {
        private string studentNumber;
        public StudentDetailsFormJHS(string studentNumber)
        {
            InitializeComponent();
            this.studentNumber = studentNumber;
            lblStudentNumber.Text = studentNumber;
        }

        private void StudentDetailsFormJHS_Load(object sender, EventArgs e)
        {
            ShowStudentInfo(studentNumber);
        }

        private void ShowStudentInfo(string studentNumber)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM jhs_enrollments WHERE student_number = @studentNumber";
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
                    lblRegistrar.Text = dr["level_applied"].ToString(); // no "level_for_registrar" in this table
                    lblAge.Text = dr["age"].ToString();
                    lblGender.Text = dr["gender"].ToString();
                    lblDOB.Text = Convert.ToDateTime(dr["birth_date"]).ToString("MM/dd/yyyy");
                    lblPOB.Text = dr["birth_place"].ToString();
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
                    lblPrevStudent.Text = dr["previously_enrolled"].ToString();
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
                    lblSubmitted.Text = Convert.ToDateTime(dr["date_created"]).ToString("MM/dd/yyyy");

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

                // ✅ Load siblings if you have a JHS sibling table
                string siblingQuery = "SELECT name, grade_occupation FROM jhs_siblings WHERE enrollment_id = (SELECT id FROM jhs_enrollments WHERE student_number = @studentNumber)";
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
    }
}


   