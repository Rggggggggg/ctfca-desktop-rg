using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class StudentDetailsFormSHS : UserControl
    {
        public event EventHandler BackButtonClicked;
        public string StudentNumber { get; set; }
        public StudentDetailsFormSHS()
        {
            InitializeComponent();

        }

        private void StudentDetailsFormSHS_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(StudentNumber))
            {
                ShowStudentInfo(StudentNumber);
            }
        }

        private void ShowStudentInfo(string studentNumber)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM shs_enrollments WHERE student_number = @studentNumber";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentNumber", studentNumber);
                MySqlDataReader dr = cmd.ExecuteReader();

                int enrollmentId = 0;

                if (dr.Read())
                {
                    enrollmentId = Convert.ToInt32(dr["id"]);

                    fullname.Text = $"{dr["surname"]}, {dr["first_name"]} {dr["middle_name"]}";
                    studenNumber.Text = $"{dr["student_number"]}";
                    applicantNumber.Text = dr["application_no"].ToString();
                    status.Text = dr["enrollment_status"].ToString();

                    // Personal Info
                    grdlvl.Text = dr["grade_level"].ToString();
                    strand.Text = dr["strand"].ToString();
                    nickname.Text = dr["nickname"].ToString();
                    gender.Text = dr["gender"].ToString();
                    age.Text = dr["age"].ToString();
                    birthdate.Text = Convert.ToDateTime(dr["birth_date"]).ToString("MM/dd/yyyy");
                    religion.Text = dr["religion"].ToString();
                    churchname.Text = dr["church_name"].ToString();
                    pastorname.Text = dr["pastor_name"].ToString();
                    email.Text = dr["shsEmail"].ToString();
                    cellphone.Text = dr["shsCellphone"].ToString();
                    telephone.Text = dr["shsTelephone"].ToString();
                    homeaddress.Text = dr["home_address"].ToString();

                    // ============ FAMILY INFORMATION - FATHER ============
                    fathersname.Text = dr["father_name"].ToString();
                    fathersoccupation.Text = dr["father_occupation"].ToString();
                    fatherscompany.Text = dr["father_company"].ToString();
                    fathersincome.Text = dr["father_income"].ToString();
                    fathersoffice.Text = dr["father_office"].ToString();
                    fathersTel.Text = dr["father_tel"].ToString();
                    fathersMobile.Text = dr["father_mobile"].ToString();
                    fathersreligion.Text = dr["father_religion"].ToString();

                    // ============ FAMILY INFORMATION - MOTHER ============
                    mothersName.Text = dr["mother_name"].ToString();
                    mothersOccupation.Text = dr["mother_occupation"].ToString();
                    mothersCompany.Text = dr["mother_company"].ToString();
                    mothersIncome.Text = dr["mother_income"].ToString();
                    mothersOffice.Text = dr["mother_office"].ToString();
                    mothersTel.Text = dr["mother_tel"].ToString();
                    mothersMobile.Text = dr["mother_mobile"].ToString();
                    mothersReligion.Text = dr["mother_religion"].ToString();

                    // ============ FAMILY INFORMATION - STEP PARENT ============
                    stepPName.Text = dr["step_parent_name"].ToString();
                    stepPOccup.Text = dr["step_parent_occupation"].ToString();
                    stepPCompany.Text = dr["step_parent_company"].ToString();
                    stepPIncome.Text = dr["step_parent_income"].ToString();
                    stepPOffice.Text = dr["step_parent_office"].ToString();
                    stepPTel.Text = dr["step_parent_tel"].ToString();
                    stepPMobile.Text = dr["step_parent_mobile"].ToString();
                    stepPReligion.Text = dr["step_parent_religion"].ToString();

                    // ============ FAMILY INFORMATION - GUARDIAN ============
                    guardianName.Text = dr["guardian_name"].ToString();
                    guardianOccup.Text = dr["guardian_occupation"].ToString();
                    guardianCompany.Text = dr["guardian_company"].ToString();
                    guardianIncome.Text = dr["guardian_income"].ToString();
                    guardianOffice.Text = dr["guardian_office"].ToString();
                    guardianTel.Text = dr["guardian_tel"].ToString();
                    guardianMobile.Text = dr["guardian_mobile"].ToString();
                    guardianReligion.Text = dr["guardian_religion"].ToString();

                    // ============ EDUCATIONAL BACKGROUND ============
                    lastSchool.Text = dr["last_school"].ToString();
                    schoolAdd.Text = dr["last_school_address"].ToString();
                    ncaeResult.Text = dr["ncae_result"].ToString();

                    // ============ HEALTH INFORMATION ============
                    genHealth.Text = dr["health"].ToString();
                    eyeCond.Text = dr["health_eye"].ToString();
                    earCond.Text = dr["health_ear"].ToString();
                    allergies.Text = dr["health_allergies"].ToString();
                    otherHealthCond.Text = dr["health_others"].ToString();

                    // ============ APPLICATION INFORMATION ============
                    applicationDate.Text = Convert.ToDateTime(dr["application_date"]).ToString("MM/dd/yyyy");
                    dateCreated.Text = Convert.ToDateTime(dr["created_at"]).ToString("MM/dd/yyyy");
                    schoolYear.Text = dr["school_year"].ToString();

                    // Load Photo
                    if (dr["id_photo_filename"] != DBNull.Value)
                    {
                        try
                        {
                            byte[] photoBytes = (byte[])dr["id_photo_filename"];
                            if (photoBytes.Length > 0)
                            {
                                using (var ms = new System.IO.MemoryStream(photoBytes))
                                {
                                    Image originalImage = Image.FromStream(ms);
                                    studentPicture.Image = new Bitmap(originalImage);
                                    originalImage.Dispose();
                                }
                                studentPicture.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            else
                            {
                                studentPicture.Image = Properties.Resources.no_image;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading photo: {ex.Message}");
                            studentPicture.Image = Properties.Resources.no_image;
                        }
                    }
                    else
                    {
                        studentPicture.Image = Properties.Resources.no_image;
                    }
                }

                // ✅ CLOSE THE DATAREADER BEFORE LOADING SIBLINGS
                dr.Close();

                // ============ LOAD SIBLINGS DATA ============
                if (enrollmentId > 0)
                {
                    LoadSiblingsData(conn, enrollmentId);
                }
            }
        }

        private void LoadSiblingsData(MySqlConnection conn, int enrollmentId)
        {
            try
            {
                string siblingQuery = "SELECT name, age, grade, occupation FROM shs_siblings WHERE enrollment_id = @enrollmentId ORDER BY id";
                MySqlCommand siblingCmd = new MySqlCommand(siblingQuery, conn);
                siblingCmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);

                MySqlDataReader siblingReader = siblingCmd.ExecuteReader();

                List<string> names = new List<string>();
                List<string> ages = new List<string>();
                List<string> grades = new List<string>();
                List<string> occupations = new List<string>();

                while (siblingReader.Read())
                {
                    names.Add(siblingReader["name"].ToString());
                    ages.Add(siblingReader["age"].ToString());
                    grades.Add(siblingReader["grade"].ToString());
                    occupations.Add(siblingReader["occupation"].ToString());
                }

                siblingReader.Close();

                // Display all siblings separated by commas or line breaks
                if (names.Count > 0)
                {
                    siblingsName.Text = string.Join("\n", names);
                    siblingsAge.Text = string.Join("\n", ages);
                    siblingsGrdLvl.Text = string.Join("\n", grades);
                    siblingsOccup.Text = string.Join("\n", occupations);
                }
                else
                {
                    siblingsName.Text = "N/A";
                    siblingsAge.Text = "N/A";
                    siblingsGrdLvl.Text = "N/A";
                    siblingsOccup.Text = "N/A";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading siblings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}