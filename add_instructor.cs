using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class add_instructor : Form
    {
        private string currentInstructorId = null;
        private Dictionary<string, List<string>> gradeToSubjects = new Dictionary<string, List<string>>();
        public add_instructor(string instructorId = null)
        {
            InitializeComponent();
            currentInstructorId = instructorId;
            gradeToSubjects = LoadSubjectsFromDB();

            if (!string.IsNullOrEmpty(instructorId))
            {
                LoadInstructorData(instructorId);
            }
            tbAge.KeyPress += NumericOnly_KeyPress;
            tbContact.KeyPress += NumericOnly_KeyPress;
            dtpDOB.ValueChanged += dtpDOB_ValueChanged;
        }

        private void instructor_Load(object sender, EventArgs e)
        {
            clbGradesLevelHandled.ItemCheck += clbGradesLevelHandled_ItemCheck;
            // Only apply these defaults if not in edit mode
            // Only apply these defaults if not in edit mode
            if (string.IsNullOrEmpty(currentInstructorId))
            {
                GenerateNewInstructorID();
                SetDefaultValues();
            }
        }

        private void GenerateNewInstructorID()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MAX(CAST(SUBSTRING(instructors_id, 4) AS UNSIGNED)) FROM instructor_accounts WHERE instructors_id LIKE 'INS%'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    object result = cmd.ExecuteScalar();
                    int nextNumber = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;

                    tbInstructorID.Text = "INS" + nextNumber.ToString("D3"); // INS001, INS002, etc.
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating instructor ID: " + ex.Message);
                    tbInstructorID.Text = "INS001"; // Fallback
                }
            }
        }

        private void SetDefaultValues()
        {
            if (cbGender.Items.Count > 0)
                cbGender.SelectedIndex = 0;
            if (cbCivilStatus.Items.Count > 0)
                cbCivilStatus.SelectedIndex = 0;
            if (cbEmployment.Items.Count > 0)
                cbEmployment.SelectedIndex = 0;
        }
        private byte[] ImageToByteArray(Image image)
        {
            if (image == null) return null;

            int maxWidth = 300;  // Resize width
            int maxHeight = 300; // Resize height

            int newWidth = image.Width;
            int newHeight = image.Height;

            // Maintain aspect ratio
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                float ratioX = (float)maxWidth / image.Width;
                float ratioY = (float)maxHeight / image.Height;
                float ratio = Math.Min(ratioX, ratioY);
                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);
            }

            using (Bitmap resized = new Bitmap(newWidth, newHeight))
            {
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                using (var ms = new System.IO.MemoryStream())
                {
                    resized.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // JPEG = smaller file
                    return ms.ToArray();
                }
            }
        }

        // Add this method to handle loading instructor data
        private void LoadInstructorData(string instructorId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM instructor_accounts WHERE instructors_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", instructorId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tbInstructorID.Text = reader["instructors_id"].ToString();
                            tbFirstname.Text = reader["firstname"].ToString();
                            tbMiddlename.Text = reader["middlename"].ToString();
                            tbLastname.Text = reader["lastname"].ToString();
                            cbGender.SelectedItem = reader["gender"].ToString();
                            tbAge.Text = reader["age"].ToString();
                            dtpDOB.Value = Convert.ToDateTime(reader["date_of_birth"]);
                            cbCivilStatus.SelectedItem = reader["civil_status"].ToString();
                            tbContact.Text = reader["contact_no"].ToString();
                            tbEmail.Text = reader["email"].ToString();
                            cbEmployment.SelectedItem = reader["employment"].ToString();

                            // Load image
                            if (reader["image"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["image"];
                                using (var ms = new System.IO.MemoryStream(imgBytes))
                                {
                                    Image loadedImage = Image.FromStream(ms);
                                    btnChooseImage.Image = MakeCircularImage(loadedImage);
                                    btnChooseImage.Text = "";
                                    btnChooseImage.ImageSize = new Size(btnChooseImage.Width, btnChooseImage.Height);
                                }
                            }

                            // Load subject_assigned (assumes comma-separated)
                            if (reader["subject_assigned"] != DBNull.Value)
                            {
                                string[] subjects = reader["subject_assigned"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < clbSubjectAssigned.Items.Count; i++)
                                {
                                    clbSubjectAssigned.SetItemChecked(i, subjects.Any(s => s.Trim() == clbSubjectAssigned.Items[i].ToString()));
                                }
                            }

                            // Load grades_level_handled (assumes comma-separated)
                            if (reader["grades_level_handled"] != DBNull.Value)
                            {
                                string[] grades = reader["grades_level_handled"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < clbGradesLevelHandled.Items.Count; i++)
                                {
                                    clbGradesLevelHandled.SetItemChecked(i, grades.Any(g => g.Trim() == clbGradesLevelHandled.Items[i].ToString()));
                                }
                                UpdateSubjectsBasedOnGrades();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading instructor data: " + ex.Message);
                }
            }
        }

        private Dictionary<string, List<string>> LoadSubjectsFromDB()
        {
            var result = new Dictionary<string, List<string>>();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT grade_level, subject_name FROM subjects ORDER BY grade_level, subject_name";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string gradeLevel = reader["grade_level"].ToString();
                            string subjectName = reader["subject_name"].ToString();

                            if (!result.ContainsKey(gradeLevel))
                                result[gradeLevel] = new List<string>();

                            result[gradeLevel].Add(subjectName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading subjects: " + ex.Message);
                }
            }

            return result;
        }

        private void clbGradesLevelHandled_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    UpdateSubjectsBasedOnGrades();
                });
            }

        }

        private void UpdateSubjectsBasedOnGrades()
        {
            // Clear and repopulate the subject list, grouped by grade with headers
            clbSubjectAssigned.Items.Clear();

            var checkedGrades = clbGradesLevelHandled.CheckedItems.Cast<string>().ToList();
            var addedSubjects = new HashSet<string>();

            foreach (string grade in checkedGrades)
            {
                if (gradeToSubjects.ContainsKey(grade))
                {
                    // Add a header for the grade (disabled, not checkable)
                    int headerIndex = clbSubjectAssigned.Items.Add($"--- {grade} ---");
                    clbSubjectAssigned.SetItemCheckState(headerIndex, CheckState.Indeterminate);

                    foreach (string subject in gradeToSubjects[grade])
                    {
                        // Only add subject if not already added under another grade
                        if (!addedSubjects.Contains(subject))
                        {
                            clbSubjectAssigned.Items.Add(subject, true);
                            addedSubjects.Add(subject);
                        }
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            // Image validation
            if (btnChooseImage.Image == null)
            {
                MessageBox.Show("Please upload an instructor image.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChooseImage.Focus();
                return false;
            }

            // Required field validation
            if (string.IsNullOrWhiteSpace(tbFirstname.Text) || tbFirstname.Text == "First name")
            {
                MessageBox.Show("First name is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbFirstname.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbLastname.Text) || tbLastname.Text == "Last name")
            {
                MessageBox.Show("Last name is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbLastname.Focus();
                return false;
            }

            // Gender validation (check if placeholder is selected)
            if (cbGender.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a gender.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbGender.Focus();
                return false;
            }

            // Civil Status validation
            if (cbCivilStatus.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a civil status.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCivilStatus.Focus();
                return false;
            }

            // Date of Birth validation
            DateTime selectedDate = dtpDOB.Value.Date;
            DateTime today = DateTime.Today;

            // Check if date is in the future
            if (selectedDate > today)
            {
                MessageBox.Show("Date of birth cannot be in the future.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDOB.Focus();
                return false;
            }

            // Calculate age from date of birth
            int calculatedAge = today.Year - selectedDate.Year;
            if (selectedDate.Date > today.AddYears(-calculatedAge))
                calculatedAge--;

            // Check minimum age (18 for instructors)
            if (calculatedAge < 18)
            {
                MessageBox.Show("Instructor must be at least 18 years old.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDOB.Focus();
                return false;
            }

            // Check maximum reasonable age (e.g., 100 years old)
            if (calculatedAge > 100)
            {
                MessageBox.Show("Please enter a valid date of birth.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDOB.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbInstructorID.Text))
            {
                MessageBox.Show("Instructor ID is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbInstructorID.Focus();
                return false;
            }

            // Age validation
            if (!string.IsNullOrWhiteSpace(tbAge.Text))
            {
                if (!int.TryParse(tbAge.Text, out int age) || age < 18 || age > 100)
                {
                    MessageBox.Show("Please enter a valid age between 18 and 100.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbAge.Focus();
                    return false;
                }
            }

            // Phone validation (basic)
            if (!string.IsNullOrWhiteSpace(tbContact.Text) || tbContact.Text != "09XX XXX XXXX")
            {
                if (tbContact.Text.Length < 10)
                {
                    MessageBox.Show("Please enter a valid contact number.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbContact.Focus();
                    return false;
                }
            }

            // Email validation (basic)
            if (!string.IsNullOrWhiteSpace(tbEmail.Text) || tbEmail.Text != "email@example.com")
            {
                if (!IsValidEmail(tbEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbEmail.Focus();
                    return false;
                }
            }

          
            // Employment validation
            if (cbEmployment.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an employment status.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbEmployment.Focus();
                return false;
            }


            // Check if at least one grade level is selected
            if (clbGradesLevelHandled.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one grade level.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if at least one subject is selected
            var selectedSubjects = clbSubjectAssigned.CheckedItems.Cast<string>()
                                 .Where(item => !item.StartsWith("---")).ToList();
            if (selectedSubjects.Count == 0)
            {
                MessageBox.Show("Please select at least one subject.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool InstructorIdExists(MySqlConnection conn, string instructorId, string currentId = null)
        {
            string checkQuery = "SELECT COUNT(*) FROM instructor_accounts WHERE instructors_id = @id";
            if (!string.IsNullOrEmpty(currentId))
            {
                checkQuery += " AND instructors_id != @currentId";
            }

            using (var cmd = new MySqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", instructorId);
                if (!string.IsNullOrEmpty(currentId))
                    cmd.Parameters.AddWithValue("@currentId", currentId);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (e.g., Backspace, Delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block non-numeric input
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog before canceling
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel? Any unsaved changes will be lost.",
                "Cancel Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs first
            if (!ValidateInputs())
            {
                return;
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check if instructor ID already exists (for new entries)
                    bool isEdit = !string.IsNullOrEmpty(currentInstructorId);

                    if (!isEdit && InstructorIdExists(conn, tbInstructorID.Text))
                    {
                        MessageBox.Show("Instructor ID already exists. Please use a different ID.",
                                      "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbInstructorID.Focus();
                        return;
                    }

                    string query;
                    if (isEdit)
                    {
                        // UPDATE if editing
                        query = @"UPDATE instructor_accounts SET
                            firstname = @firstname,
                            middlename = @middlename,
                            lastname = @lastname,
                            gender = @gender,
                            age = @age,
                            date_of_birth = @dob,
                            civil_status = @civil_status,
                            contact_no = @contact_no,
                            email = @email,
                            image = @image,
                            employment = @employment,
                            subject_assigned = @subject_assigned,
                            grades_level_handled = @grades_level_handled
                        WHERE instructors_id = @instructors_id";
                    }
                    else
                    {
                        // INSERT if adding
                        query = @"INSERT INTO instructor_accounts
                            (instructors_id, firstname, middlename, lastname, gender, age, date_of_birth, 
                             civil_status, contact_no, email, image, employment, subject_assigned, grades_level_handled)
                            VALUES
                            (@instructors_id, @firstname, @middlename, @lastname, @gender, @age, @dob, 
                             @civil_status, @contact_no, @email, @image, @employment, @subject_assigned, @grades_level_handled)";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@instructors_id", tbInstructorID.Text.Trim());
                        cmd.Parameters.AddWithValue("@firstname", tbFirstname.Text.Trim());
                        cmd.Parameters.AddWithValue("@middlename", tbMiddlename.Text.Trim());
                        cmd.Parameters.AddWithValue("@lastname", tbLastname.Text.Trim());
                        cmd.Parameters.AddWithValue("@gender", cbGender.Text);
                        cmd.Parameters.AddWithValue("@age", string.IsNullOrWhiteSpace(tbAge.Text) ? (object)DBNull.Value : int.Parse(tbAge.Text));
                        cmd.Parameters.AddWithValue("@dob", dtpDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@civil_status", cbCivilStatus.Text);
                        cmd.Parameters.AddWithValue("@contact_no", tbContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", tbEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@employment", cbEmployment.Text);

                        // Handle image - only save if there's an image
                        if (btnChooseImage.Image != null)
                        {
                            byte[] imageBytes = ImageToByteArray(btnChooseImage.Image);
                            cmd.Parameters.AddWithValue("@image", imageBytes);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@image", DBNull.Value);
                        }

                        // Get only non-header subjects
                        var selectedSubjects = clbSubjectAssigned.CheckedItems.Cast<string>()
                                             .Where(item => !item.StartsWith("---"))
                                             .ToList();
                        cmd.Parameters.AddWithValue("@subject_assigned", string.Join(", ", selectedSubjects));

                        var selectedGrades = clbGradesLevelHandled.CheckedItems.Cast<string>().ToList();
                        cmd.Parameters.AddWithValue("@grades_level_handled", string.Join(", ", selectedGrades));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(isEdit ? "Instructor updated successfully!" : "Instructor added successfully!",
                                          "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Operation failed. Please try again.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    MessageBox.Show($"Database error: {mysqlEx.Message}", "Database Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChooseImage_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an Image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var img = Image.FromFile(ofd.FileName))
                    {
                        btnChooseImage.Image = MakeCircularImage(img); // clone the image
                        btnChooseImage.Text = "";

                        btnChooseImage.ImageSize = new Size(btnChooseImage.Width, btnChooseImage.Height);
                    }

                }
            }
        }

        private Image MakeCircularImage(Image original)
        {
            int size = Math.Min(original.Width, original.Height);
            Bitmap circularBitmap = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(circularBitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, size, size);
                    g.SetClip(path);

                    // This line zooms and centers the image so it fills the circle completely
                    g.DrawImage(original, -((original.Width - size) / 2), -((original.Height - size) / 2), original.Width, original.Height);
                }
            }
            return circularBitmap;
        }

        private void dtpDOB_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpDOB.Value.Date;
            DateTime today = DateTime.Today;

            // Calculate age
            int age = today.Year - selectedDate.Year;
            if (selectedDate.Date > today.AddYears(-age))
                age--;

            // Only update if age is reasonable
            if (age >= 0 && age <= 100)
            {
                tbAge.Text = age.ToString();
            }
        }
    }
}
