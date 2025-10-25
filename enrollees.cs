using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Enrollees : UserControl
    {
        public Enrollees()
        {
            InitializeComponent();
        }

        private void LoadStudentData(string statusFilter = "Pending")
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT student_number, level_applied, surname, 
                             first_name, middle_name, gender, age, contact, 
                             submitted_at, email, enrollment_status, student_photo
                             FROM basic_ed_enrollment";

                    if (statusFilter != "All")
                    {
                        query += $" WHERE enrollment_status = '{statusFilter}'";
                    }

                    query += " ORDER BY submitted_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgEnrollees.Rows.Clear();

                        // ✅ Ensure Photo column exists (only once)
                        if (!dtgEnrollees.Columns.Contains("colPhoto"))
                        {
                            DataGridViewImageColumn colPhoto = new DataGridViewImageColumn();
                            colPhoto.Name = "colPhoto";
                            colPhoto.HeaderText = "Photo";
                            colPhoto.ImageLayout = DataGridViewImageCellLayout.Zoom;
                            colPhoto.Width = 80;
                            dtgEnrollees.Columns.Insert(0, colPhoto);
                        }

                        dtgEnrollees.RowTemplate.Height = 60; // make room for the photo

                        while (reader.Read())
                        {
                            // ✅ Convert BLOB to image
                            Image studentImage = null;
                            byte[] photoBytes = null;
                            if (reader["student_photo"] != DBNull.Value)
                            {
                                try
                                {
                                    photoBytes = (byte[])reader["student_photo"];
                                    if (photoBytes.Length > 0)
                                    {
                                        using (var ms = new System.IO.MemoryStream(photoBytes))
                                        {
                                            studentImage = Image.FromStream(ms);
                                        }
                                    }
                                }
                                catch
                                {
                                    studentImage = Properties.Resources.no_image; // fallback image
                                }
                            }

                            // Use placeholder if no photo exists
                            if (studentImage == null)
                            {
                                studentImage = Properties.Resources.no_image;
                            }

                            string fullName = $"{reader["surname"]}, {reader["first_name"]} {reader["middle_name"]}";
                            string status = reader["enrollment_status"].ToString();

                            // ✅ Add new photo cell as first column
                            int rowIndex = dtgEnrollees.Rows.Add(
                                studentImage, // new column for photo
                                reader["student_number"].ToString(),
                                fullName,
                                reader["level_applied"].ToString(),
                                reader["gender"].ToString(),
                                reader["age"].ToString(),
                                reader["contact"].ToString(),
                                Convert.ToDateTime(reader["submitted_at"]).ToString("MM/dd/yyyy"),
                                reader["email"].ToString()
                            );

                            // Store the full-size photo bytes in the row's Tag for later use
                            if (photoBytes != null && photoBytes.Length > 0)
                            {
                                dtgEnrollees.Rows[rowIndex].Tag = photoBytes;
                            }

                            // ✅ Color code rows based on status
                            if (status == "Confirmed")
                                dtgEnrollees.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 230);
                            else if (status == "Rejected")
                                dtgEnrollees.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Enrollees_Load(object sender, EventArgs e)
        {
            cbStatusFilter.SelectedIndex = 0;
            LoadStudentData("Pending");

            if (dtgEnrollees.Columns.Contains("colEmail"))
                dtgEnrollees.Columns["colEmail"].Visible = false;

            // ✅ Add "View Info" button column (if not already added)
            if (!dtgEnrollees.Columns.Contains("btnViewInfo"))
            {
                DataGridViewButtonColumn btnViewInfo = new DataGridViewButtonColumn();
                btnViewInfo.HeaderText = "Action";
                btnViewInfo.Name = "btnViewInfo";
                btnViewInfo.Text = "View Info";
                btnViewInfo.UseColumnTextForButtonValue = true;

                // Insert after "Submitted At" (index 8 because of photo)
                dtgEnrollees.Columns.Insert(8, btnViewInfo);
            }
        }

        private void DtgEnrollees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dtgEnrollees.Rows[e.RowIndex];
            if (row.Cells.Count <= 0) return;

            string studentNumber = row.Cells[1].Value?.ToString();
            string name = row.Cells[2].Value?.ToString();
            string email = row.Cells[8].Value?.ToString();
            string yearLevel = row.Cells[3].Value?.ToString();

            string columnName = dtgEnrollees.Columns[e.ColumnIndex].Name;
            if (string.IsNullOrEmpty(studentNumber)) return;

            if (columnName == "btnConfirm")
            {
                ConfirmEnrollment(studentNumber, name, email, yearLevel);
            }
            else if (columnName == "btnDelete")
            {
                RejectEnrollment(studentNumber, name, email, yearLevel);
            }
            else if (columnName == "btnViewInfo")
            {
                Form2 parentForm = this.ParentForm as Form2;
                parentForm?.LoadBasicDetails(studentNumber);
            }
            else if (columnName == "colPhoto")
            {
                // Show full-size photo when photo cell is clicked
                ShowFullSizePhoto(row);
            }
        }

        private void ShowFullSizePhoto(DataGridViewRow row)
        {
            if (row.Tag is byte[] photoBytes && photoBytes.Length > 0)
            {
                try
                {
                    using (var ms = new System.IO.MemoryStream(photoBytes))
                    {
                        Image fullSizeImage = Image.FromStream(ms);

                        // Create a form to display the full-size image
                        Form imageForm = new Form();
                        imageForm.Text = $"Student Photo - {row.Cells[1].Value}";
                        imageForm.Size = new Size(600, 700);
                        imageForm.StartPosition = FormStartPosition.CenterScreen;
                        imageForm.BackColor = Color.White;
                        imageForm.Icon = SystemIcons.Information;

                        // PictureBox to display the image
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Dock = DockStyle.Fill;
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Image = fullSizeImage;

                        // Add student info below the photo
                        Panel infoPanel = new Panel();
                        infoPanel.Dock = DockStyle.Bottom;
                        infoPanel.Height = 80;
                        infoPanel.BackColor = Color.LightGray;
                        infoPanel.Padding = new Padding(10);

                        Label infoLabel = new Label();
                        infoLabel.Text = $"Student: {row.Cells[2].Value}\nStudent #: {row.Cells[1].Value}";
                        infoLabel.Dock = DockStyle.Fill;
                        infoLabel.TextAlign = ContentAlignment.MiddleCenter;
                        infoLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                        // Close button
                        Button closeButton = new Button();
                        closeButton.Text = "Close";
                        closeButton.Size = new Size(80, 30);
                        closeButton.Location = new Point(260, 45);
                        closeButton.BackColor = Color.FromArgb(220, 53, 69);
                        closeButton.ForeColor = Color.White;
                        closeButton.FlatStyle = FlatStyle.Flat;
                        closeButton.Click += (s, e) => imageForm.Close();

                        // Add controls
                        infoPanel.Controls.Add(infoLabel);
                        infoPanel.Controls.Add(closeButton);
                        imageForm.Controls.Add(pictureBox);
                        imageForm.Controls.Add(infoPanel);

                        imageForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying photo: " + ex.Message, "Photo Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No photo available for this student.", "No Photo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Rest of your existing methods remain the same...
        private async void ConfirmEnrollment(string studentNumber, string name, string email, string yearLevel)
        {
            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Invalid student record selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to approve this student: {name}?",
                "Confirm Approval",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            bool success = false; // ✅ track success
            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string transferQuery = @"
                            INSERT INTO students (
                                student_number, level_for_registrar, lrn, level_applied, surname, first_name, middle_name, age, gender, dob,
                                pob, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                                father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, status, guardian_name, guardian_relation, 
                                guardian_occupation, guardian_contact, guardian_email, has_siblings, prev_ctfcai, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                                health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date
                            )
                            SELECT 
                                student_number, level_for_registrar, lrn, level_applied, surname, first_name, middle_name, age, gender, dob,
                                pob, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                                father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, status, guardian_name, guardian_relation, 
                                guardian_occupation, guardian_contact, guardian_email, has_siblings, prev_ctfcai, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                                health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date
                            FROM basic_ed_enrollment 
                            WHERE student_number = @student_number";

                    MySqlCommand transferCmd = new MySqlCommand(transferQuery, conn);
                    transferCmd.Parameters.AddWithValue("@student_number", studentNumber);

                    int rowsAffected = transferCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        UpdateStatus(conn, studentNumber, "Confirmed");
                        success = true; // ✅ mark as successful
                    }
                    else
                    {
                        MessageBox.Show("Student record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } // ✅ Connection automatically closes here
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ✅ Now the connection is closed before sending email
            if (success)
            {
                bool notificationSent = await NotifyStudent(studentNumber, name, email, "Approved", yearLevel);

                MessageBox.Show($"Student {name} enrolled successfully" +
                    (notificationSent ? "" : "\nNote: Email notification failed to send"),
                    "Enrollment Confirmed", MessageBoxButtons.OK,
                    notificationSent ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                LoadStudentData(cbStatusFilter.Text);
            }
        }


        private async void RejectEnrollment(string studentNumber, string name, string email, string yearLevel)
        {
            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Invalid student record selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to reject this student: {name}?",
                                                 "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE basic_ed_enrollment SET enrollment_status = 'Rejected' WHERE student_number = @student_number";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@student_number", studentNumber);
                    updateCmd.ExecuteNonQuery();

                    bool notificationSent = await NotifyStudent(studentNumber, name, email, "Rejected", yearLevel);

                    MessageBox.Show($"Student {name} rejected successfully" +
                        (notificationSent ? "" : "\nNote: Email notification failed to send"),
                        "Enrollment Rejected", MessageBoxButtons.OK,
                        notificationSent ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    LoadStudentData(cbStatusFilter.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateStatus(MySqlConnection conn, string studentNumber, string status)
        {
            string updateQuery = "UPDATE basic_ed_enrollment SET enrollment_status = @status WHERE student_number = @student_number";
            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
            updateCmd.Parameters.AddWithValue("@student_number", studentNumber);
            updateCmd.Parameters.AddWithValue("@status", status);
            updateCmd.ExecuteNonQuery();
        }

        private async Task<bool> NotifyStudent(string studentNumber, string name, string email, string status, string yearLevel)
        {
            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    student_number = studentNumber,
                    name,
                    email,
                    status,
                    year_level = yearLevel,
                    enrollment_type = "Basic Education"
                };

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    //HttpResponseMessage response = await client.PostAsync("http://localhost/CAPSTONE_PROJ/api/cs/notify-student.php", content);
                    HttpResponseMessage response = await client.PostAsync("https://ctfca-enrollment.onrender.com/api/cs/notify-student.php", content);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Email notification failed: " + result);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                    return false;
                }
            }
        }

        private void TbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void CbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pbStudentPhoto_Click(object sender, EventArgs e)
        {

        }

        private void cbStatusFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string filterText = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgEnrollees.Rows)
            {
                if (row.Cells.Count < 3) continue;

                string studentNumber = row.Cells[1].Value?.ToString().ToLower() ?? "";
                string levelApplied = row.Cells[3].Value?.ToString().ToLower() ?? "";
                string fullName = row.Cells[2].Value?.ToString().ToLower() ?? "";

                bool visible = studentNumber.Contains(filterText) || fullName.Contains(filterText) || levelApplied.Contains(filterText);
                row.Visible = visible;
            }
        }
    }
}