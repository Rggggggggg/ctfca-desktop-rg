using MySql.Data.MySqlClient;
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
    public partial class EditStudent : Form
    {
        private string studentID;
        public EditStudent(string studentID)
        {
            InitializeComponent();
            this.studentID = studentID;

        }

        private void EditStudent_Load(object sender, EventArgs e)
        {
            LoadStudentData();
        }

        private void LoadStudentData()
        {
            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT 
                                    COALESCE(student_number, lrn, application_no) AS id,
                                    COALESCE(level_applied, grade_level) AS level_applied,
                                    COALESCE(contact, cellphone) AS contact,
                                    first_name, surname, middle_name,gender, age, strand,
                                    citizenship, religion, address, 
                                    COALESCE(pob, birth_place) AS pob, guardian_name,
                                    guardian_contact 
                                    FROM students          
                                    WHERE COALESCE(student_number, lrn, application_no) = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbID.Text = reader["id"].ToString();
                                tbFirstName.Text = reader["first_name"].ToString();
                                tbLastName.Text = reader["surname"].ToString();
                                tbMiddleName.Text = reader["middle_name"].ToString();
                                tbGradeLevel.Text = reader["level_applied"].ToString();
                                cbGender.Text = reader["gender"].ToString();
                                tbAge.Text = reader["age"].ToString();
                                tbCitizenship.Text = reader["citizenship"].ToString();
                                tbReligion.Text = reader["religion"].ToString();
                                tbAddress.Text = reader["address"].ToString();
                                tbPOB.Text = reader["pob"].ToString();
                                tbContact.Text = reader["contact"].ToString();
                                tbStrand.Text = reader["strand"] != DBNull.Value ? reader["strand"].ToString() : "N/A";
                                tbGuardian.Text = reader["guardian_name"].ToString();
                                tbGuardianContact.Text = reader["guardian_contact"].ToString();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE students SET 
                                    level_applied = IF(level_applied IS NOT NULL, @level_applied, level_applied),
                                    grade_level = IF(level_applied IS NULL AND grade_level IS NOT NULL, @level_applied, grade_level),
                                    contact = IF(contact IS NOT NULL, @contact, contact),
                                    cellphone = IF(contact IS NULL AND cellphone IS NOT NULL, @contact, cellphone),
                                    pob = IF(pob IS NOT NULL, @pob, pob),
                                    birth_place = IF(pob IS NULL AND birth_place IS NOT NULL, @pob, birth_place),
                                    first_name = @first_name,
                                    surname = @surname,
                                    middle_name = @middle_name,
                                    gender = @gender,
                                    age = @age,
                                    strand = @strand,
                                    citizenship = @citizenship,
                                    religion = @religion,
                                    address = @address,
                                    guardian_name = @guardian_name,
                                    guardian_contact = @guardian_contact
                                    WHERE COALESCE(student_number, lrn, application_no) = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@level_applied", tbGradeLevel.Text.Trim());
                        cmd.Parameters.AddWithValue("@grade_level", tbGradeLevel.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact", tbContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@cellphone", tbContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@first_name", tbFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@surname", tbLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@middle_name", tbMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@gender", cbGender.Text.Trim());
                        cmd.Parameters.AddWithValue("@age", tbAge.Text.Trim());
                        cmd.Parameters.AddWithValue("@strand", tbStrand.Text.Trim());
                        cmd.Parameters.AddWithValue("@citizenship", tbCitizenship.Text.Trim());
                        cmd.Parameters.AddWithValue("@religion", tbReligion.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", tbAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@pob", tbPOB.Text.Trim());
                        cmd.Parameters.AddWithValue("@birth_place", tbPOB.Text.Trim());
                        cmd.Parameters.AddWithValue("@guardian_name", tbGuardian.Text.Trim());
                        cmd.Parameters.AddWithValue("@guardian_contact", tbGuardianContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", studentID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student information updated successfully.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made or student not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
               
            }
        }
    }
}



