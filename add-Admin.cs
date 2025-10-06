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
    public partial class add_Admin : Form
    {
       
        private string editingAdminId = null;

        public add_Admin(string adminId)
        {
            InitializeComponent();
            editingAdminId = adminId;

            LoadAdminData(adminId);

            if (editingAdminId != null)
            {
                lblTitle.Text = "Edit Admin";
                btnSave.Text = "Update";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (!ValidateInput())
                return;

            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string query;
                    MySqlCommand cmd;

                    if (editingAdminId == null)
                    {
                        // INSERT logic with hashed password
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(tbPassword.Text);

                        query = "INSERT INTO admin_accounts (firstname, middlename, lastname, gender, role, contact_no, email, photo, username, password) " +
                                "VALUES (@fname, @mname, @lname, @gender, @role, @contact, @email, @photo, @username, @password)";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                    }
                    else
                    {
                        // UPDATE logic (password optional)
                        query = "UPDATE admin_accounts SET firstname=@fname, middlename=@mname, lastname=@lname, gender=@gender, " +
                                "role=@role, contact_no=@contact, email=@email, photo=@photo, username=@username";

                        if (!string.IsNullOrEmpty(tbPassword.Text))
                        {
                            query += ", password=@password";
                        }

                        query += " WHERE id=@id";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", editingAdminId);

                        if (!string.IsNullOrEmpty(tbPassword.Text))
                        {
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(tbPassword.Text); // Fixed: single declaration
                            cmd.Parameters.AddWithValue("@password", hashedPassword);
                        }
                    }

                    // Common parameters
                    cmd.Parameters.AddWithValue("@fname", tbFirstname.Text);
                    cmd.Parameters.AddWithValue("@mname", tbMiddlename.Text);
                    cmd.Parameters.AddWithValue("@lname", tbLastname.Text);
                    cmd.Parameters.AddWithValue("@gender", cbGender.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@role", cbRole.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@contact", tbContact.Text);
                    cmd.Parameters.AddWithValue("@email", tbEmail.Text);
                    cmd.Parameters.AddWithValue("@username", tbUsername.Text);

                    // Image processing
                    Image img = btnChooseImage.Image;
                    byte[] imageBytes = img != null ? ImageToByteArray(img) : new byte[0];
                    cmd.Parameters.AddWithValue("@photo", imageBytes);

                    // Execute
                    cmd.ExecuteNonQuery();

                    string message = editingAdminId == null ? "Admin successfully added!" : "Admin successfully updated!";
                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving admin: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // First Name validation
            if (string.IsNullOrWhiteSpace(tbFirstname.Text))
            {
                MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbFirstname.Focus();
                return false;
            }

            // Last Name validation
            if (string.IsNullOrWhiteSpace(tbLastname.Text))
            {
                MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbLastname.Focus();
                return false;
            }

            // Gender ComboBox validation
            if (cbGender.SelectedItem == null || string.IsNullOrWhiteSpace(cbGender.SelectedItem.ToString()))
            {
                MessageBox.Show("Please select a gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbGender.Focus();
                return false;
            }

            // Role ComboBox validation
            if (cbRole.SelectedItem == null || string.IsNullOrWhiteSpace(cbRole.SelectedItem.ToString()))
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbRole.Focus();
                return false;
            }

            // Contact Number validation
            if (string.IsNullOrWhiteSpace(tbContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbContact.Focus();
                return false;
            }
            // Contact number must start with 09
            if (!tbContact.Text.StartsWith("09"))
            {
                MessageBox.Show("Contact number must start with '09'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbContact.Focus();
                return false;
            }

            // Contact number length validation (11 digits)
            if (tbContact.Text.Length != 11)
            {
                MessageBox.Show("Contact number must be exactly 11 digits (09xxxxxxxxx).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbContact.Focus();
                return false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Email address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEmail.Focus();
                return false;
            }

            // Email format validation
            if (!IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEmail.Focus();
                return false;
            }

            // Photo/Image validation
            if (btnChooseImage.Image == null)
            {
                MessageBox.Show("Please select a photo for the admin.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChooseImage.Focus();
                return false;
            }

            // Username validation
            if (string.IsNullOrWhiteSpace(tbUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return false;
            }

            // Username length validation
            if (tbUsername.Text.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbUsername.Focus();
                return false;
            }

            // Password validation for new admin
            if (editingAdminId == null && string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Password is required for new admin.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPassword.Focus();
                return false;
            }

            if (!IsPasswordStrong(tbPassword.Text))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain:\n" +
                    "• At least one uppercase letter\n" +
                    "• At least one lowercase letter\n" +
                    "• At least one number\n" +
                    "• At least one special character",
                    "Password Requirements", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                tbPassword.Focus();
                return false;
            }

            // Confirm Password validation
            if (!string.IsNullOrWhiteSpace(tbPassword.Text) && string.IsNullOrWhiteSpace(tbConfirmPassword.Text))
            {
                MessageBox.Show("Please confirm your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbConfirmPassword.Focus();
                return false;
            }

            // Password match validation
            if (!string.IsNullOrWhiteSpace(tbPassword.Text) && tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbConfirmPassword.Focus();
                return false;
            }

            return true;
        }
        

        // Helper method for email validation
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

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private void LoadAdminData(string adminId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM admin_accounts WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", adminId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tbFirstname.Text = reader["firstname"].ToString();
                        tbMiddlename.Text = reader["middlename"].ToString();
                        tbLastname.Text = reader["lastname"].ToString();
                        tbContact.Text = reader["contact_no"].ToString();
                        tbEmail.Text = reader["email"].ToString();
                        tbUsername.Text = reader["username"].ToString();
                        cbGender.SelectedItem = reader["gender"].ToString();
                        cbRole.SelectedItem = reader["role"].ToString();

                        if (!(reader["photo"] is DBNull))
                        {
                            byte[] imageBytes = (byte[])reader["photo"];
                            using (var ms = new System.IO.MemoryStream(imageBytes))
                            {
                                btnChooseImage.Image = Image.FromStream(ms);
                                btnChooseImage.Text = ""; // clear text
                            }
                        }
                    }
                }
            }
        }
       
        private byte[] ImageToByteArray(Image image)
        {
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
                        btnChooseImage.Image = new Bitmap(img); // clone the image
                        btnChooseImage.Text = "";
                    }

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbContact_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, backspace, and delete
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Cancel the key press
            }
        }

        private void add_Admin_Load(object sender, EventArgs e)
        {
            tbPassword.UseSystemPasswordChar = true;
            tbConfirmPassword.UseSystemPasswordChar = true;

            newpasswordShown = false;
            confirmpasswordShown = false;
        }
        bool newpasswordShown = false;
        bool confirmpasswordShown = false;

        private void guna2PictureBox2_Click_1(object sender, EventArgs e)
        {
            if (newpasswordShown)
            {
                tbPassword.UseSystemPasswordChar = true;
                guna2PictureBox2.Image = Properties.Resources.hide;
                newpasswordShown = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = false;
                guna2PictureBox2.Image = Properties.Resources.view;
                newpasswordShown = true;
            }
        }

        private void guna2PictureBox3_Click_1(object sender, EventArgs e)
        {
            if (confirmpasswordShown)
            {
                tbConfirmPassword.UseSystemPasswordChar = true;
                guna2PictureBox3.Image = Properties.Resources.hide;
                confirmpasswordShown = false;
            }
            else
            {
                tbConfirmPassword.UseSystemPasswordChar = false;
                guna2PictureBox3.Image = Properties.Resources.view;
                confirmpasswordShown = true;
            }
        }
    }
}
