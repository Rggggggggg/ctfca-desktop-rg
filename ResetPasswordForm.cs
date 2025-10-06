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
    public partial class ResetPasswordForm : Form
    {
        private int adminId;

        public ResetPasswordForm(int adminId)
        {
            InitializeComponent();
            this.adminId = adminId;
            tbNewPassword.Focus();
            tbNewPassword.KeyDown += PasswordField_KeyDown;
            tbConfirmPassword.KeyDown += PasswordField_KeyDown;
            this.AcceptButton = btnSubmit;
        }

        private void PasswordField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmit_Click(sender, e);
            }
        }

        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private bool ValidateInputs()
        {
            // Check if new password is empty
            if (string.IsNullOrWhiteSpace(tbNewPassword.Text))
            {
                MessageBox.Show("Please enter a new password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbNewPassword.Focus();
                return false;
            }

            // Check if confirm password is empty
            if (string.IsNullOrWhiteSpace(tbConfirmPassword.Text))
            {
                MessageBox.Show("Please confirm your password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbConfirmPassword.Focus();
                return false;
            }

            // Check password strength
            if (!IsPasswordStrong(tbNewPassword.Text))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain:\n" +
                    "• At least one uppercase letter\n" +
                    "• At least one lowercase letter\n" +
                    "• At least one number\n" +
                    "• At least one special character",
                    "Password Requirements", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbNewPassword.Focus();
                return false;
            }

            // Check if passwords match
            if (tbNewPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbConfirmPassword.Clear();
                tbConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void ClearPasswordFields()
        {
            tbNewPassword.Clear();
            tbConfirmPassword.Clear();
        }

        private async Task<bool> UpdatePasswordAsync(string hashedPassword)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE admin_accounts SET password=@pwd, updated_at=@updated WHERE id=@id";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@pwd", hashedPassword);
                            cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@id", adminId);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating password: {ex.Message}");
                    return false;
                }

            });
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate all inputs first
            if (!ValidateInputs())
                return;

            // Disable the submit button and show loading state
            btnSubmit.Enabled = false;
            btnSubmit.Text = "Updating Password...";
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Hash the password (this is CPU intensive, so we do it in background)
                string hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(tbNewPassword.Text));

                // Update the password in database
                bool success = await UpdatePasswordAsync(hashedPassword);

                if (success)
                {
                    MessageBox.Show("Password has been reset successfully!\nYou will now be redirected to the login page.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear password fields for security
                    ClearPasswordFields();

                    // Hide current form and show login form
                    this.Hide();

                    // Create and show login form
                    Form1 loginForm = new Form1();
                    loginForm.FormClosed += (s, args) => Application.Exit(); // Exit application when login form is closed
                    loginForm.Show();

                    // Close this form
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Admin account not found or could not be updated.\nPlease contact system administrator.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating your password:\n{ex.Message}\n\nPlease try again or contact system administrator.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Clear password fields for security
                ClearPasswordFields();
            }
            finally
            {
                // Restore button state
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Reset Password";
                this.Cursor = Cursors.Default;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel the password reset?",
               "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearPasswordFields();
                this.Close();
                Form1 loginForm = new Form1();
                loginForm.Show();

            }
        }
       


        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            tbNewPassword.UseSystemPasswordChar = true;
            tbConfirmPassword.UseSystemPasswordChar = true;

            newpasswordShown = false;
            confirmpasswordShown = false;
            tbNewPassword.Focus();
        }

        bool newpasswordShown = false;
        bool confirmpasswordShown = false;
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            if (newpasswordShown)
            {
                tbNewPassword.UseSystemPasswordChar = true;
                guna2PictureBox2.Image = Properties.Resources.hide;
                newpasswordShown = false;
            }
            else
            {
                tbNewPassword.UseSystemPasswordChar = false;
                guna2PictureBox2.Image = Properties.Resources.view;
                newpasswordShown = true;
            }
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
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