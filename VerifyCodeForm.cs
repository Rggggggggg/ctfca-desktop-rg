using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Security.Cryptography;

namespace CFCA_ADMIN
{
    public partial class VerifyCodeForm : Form
    {
        private string expectedCode;
        private string userEmail;
        private int adminId;
        private DateTime codeExpiry;
        private int attemptCount = 0;
        private const int MAX_ATTEMPTS = 3;
        private const int CODE_EXPIRY_MINUTES = 10;

        public VerifyCodeForm(string email, string code, int adminId)
        {
            InitializeComponent();
            expectedCode = code;
            userEmail = email;
            this.adminId = adminId;
            codeExpiry = DateTime.Now.AddMinutes(CODE_EXPIRY_MINUTES);

            InitializeForm();
        }

        private void InitializeForm()
        {
            // Set max length for each textbox
            SetTextBoxProperties(tb1);
            SetTextBoxProperties(tb2);
            SetTextBoxProperties(tb3);
            SetTextBoxProperties(tb4);
            SetTextBoxProperties(tb5);
            SetTextBoxProperties(tb6);

            // Start timer to check expiry
            StartExpiryTimer();
        }

        private void SetTextBoxProperties(Guna.UI2.WinForms.Guna2TextBox textBox)
        {
            textBox.MaxLength = 1;
            textBox.CharacterCasing = CharacterCasing.Upper; // If using letters
            // Only allow digits
            textBox.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private Timer expiryTimer;

        private void StartExpiryTimer()
        {
            expiryTimer = new Timer();
            expiryTimer.Interval = 30000;
            expiryTimer.Tick += ExpiryTimer_Tick;
            expiryTimer.Start();
        }

        private void ExpiryTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > codeExpiry)
            {
                expiryTimer.Stop();
                MessageBox.Show("Verification code has expired. Please request a new one.",
                    "Code Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            expiryTimer?.Stop();
            expiryTimer?.Dispose();
            base.OnFormClosing(e);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            string enteredCode = tb1.Text + tb2.Text + tb3.Text + tb4.Text + tb5.Text + tb6.Text;

            // Check if code is complete
            if (enteredCode.Length != 6)
            {
                MessageBox.Show("Please enter the complete 6-digit verification code.",
                    "Incomplete Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check expiry
            if (DateTime.Now > codeExpiry)
            {
                MessageBox.Show("Verification code has expired. Please request a new one.",
                    "Code Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (enteredCode == expectedCode)
            {
                try
                {
                    btnContinue.Enabled = false;
                    btnContinue.Text = "Verifying...";
                    this.Cursor = Cursors.WaitCursor;

                    ResetPasswordForm resetForm = new ResetPasswordForm(adminId);
                    resetForm.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening reset form: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnContinue.Enabled = true;
                    btnContinue.Text = "Continue";
                }
            }
            else
            {
                attemptCount++;
                int remainingAttempts = MAX_ATTEMPTS - attemptCount;

                if (remainingAttempts <= 0)
                {
                    MessageBox.Show("Too many incorrect attempts. Form will close.",
                        "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                MessageBox.Show($"Incorrect verification code. {remainingAttempts} attempts remaining.",
                    "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                ClearAllTextBoxes();
                tb1.Focus();
            }
        }

        private void VerifyCodeForm_Load(object sender, EventArgs e)
        {
            // Set up navigation between textboxes
            tb1.TextChanged += (s, ev) => MoveNext(tb1, tb2);
            tb2.TextChanged += (s, ev) => MoveNext(tb2, tb3);
            tb3.TextChanged += (s, ev) => MoveNext(tb3, tb4);
            tb4.TextChanged += (s, ev) => MoveNext(tb4, tb5);
            tb5.TextChanged += (s, ev) => MoveNext(tb5, tb6);
            tb6.TextChanged += (s, ev) => CheckIfComplete();

            // Set up backspace navigation
            tb2.KeyDown += (s, ev) => MovePrev(tb2, tb1, ev);
            tb3.KeyDown += (s, ev) => MovePrev(tb3, tb2, ev);
            tb4.KeyDown += (s, ev) => MovePrev(tb4, tb3, ev);
            tb5.KeyDown += (s, ev) => MovePrev(tb5, tb4, ev);
            tb6.KeyDown += (s, ev) => MovePrev(tb6, tb5, ev);

            // Set up paste functionality
            foreach (var tb in new[] { tb1, tb2, tb3, tb4, tb5, tb6 })
            {
                tb.KeyDown += HandlePaste;
            }

            tb1.Focus(); // Focus on first textbox
        }

        private async void CheckIfComplete()
        {
            string code = tb1.Text + tb2.Text + tb3.Text + tb4.Text + tb5.Text + tb6.Text;

            if (code.Length == 6)
            {
                if (code == expectedCode && DateTime.Now <= codeExpiry)
                {
                    await ShowSuccessAnimationAsync();

                    // Auto open ResetPasswordForm
                    ResetPasswordForm resetForm = new ResetPasswordForm(adminId);
                    resetForm.Show();
                    this.Close();
                }
                else
                {
                    attemptCount++;
                    int remainingAttempts = MAX_ATTEMPTS - attemptCount;

                    if (remainingAttempts <= 0)
                    {
                        MessageBox.Show("Too many incorrect attempts. Form will close.",
                            "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    MessageBox.Show($"Incorrect verification code. {remainingAttempts} attempts remaining.",
                        "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    ClearAllTextBoxes();
                    tb1.Focus();
                }
            }
        }


        private void HandlePaste(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string clipboardText = Clipboard.GetText();
                    if (clipboardText.Length == 6 && clipboardText.All(char.IsDigit))
                    {
                        tb1.Text = clipboardText[0].ToString();
                        tb2.Text = clipboardText[1].ToString();
                        tb3.Text = clipboardText[2].ToString();
                        tb4.Text = clipboardText[3].ToString();
                        tb5.Text = clipboardText[4].ToString();
                        tb6.Text = clipboardText[5].ToString();
                        tb6.Focus();
                        e.Handled = true;
                    }
                }
                catch
                {
                    // Ignore paste errors
                }
            }
        }

        private void MoveNext(Guna.UI2.WinForms.Guna2TextBox current, Guna.UI2.WinForms.Guna2TextBox next)
        {
            if (current.Text.Length == current.MaxLength && !string.IsNullOrEmpty(current.Text))
            {
                next.Focus();
                next.SelectAll(); // Select text in case there's existing content
            }
        }

        private void MovePrev(Guna.UI2.WinForms.Guna2TextBox current, Guna.UI2.WinForms.Guna2TextBox previous, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && string.IsNullOrEmpty(current.Text))
            {
                previous.Focus();
                previous.SelectAll();
                e.Handled = true; // Prevent further processing
            }
        }

        private void ClearAllTextBoxes()
        {
            tb1.Clear(); tb2.Clear(); tb3.Clear();
            tb4.Clear(); tb5.Clear(); tb6.Clear();
        }

        private async void linkResendCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkResendCode.Enabled = false;
                linkResendCode.Text = "Sending...";

                string newCode = GenerateSecureCode();
                expectedCode = newCode;
                codeExpiry = DateTime.Now.AddMinutes(CODE_EXPIRY_MINUTES); // Reset expiry

                ClearAllTextBoxes();
                attemptCount = 0;
                tb1.Focus();

                bool emailSent = await SendVerificationEmailAsync(userEmail, newCode);

                if (emailSent)
                {
                    MessageBox.Show("Verification code has been resent to your email.",
                        "Code Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to send verification code. Please try again.",
                        "Send Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resending code: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                linkResendCode.Enabled = true;
                linkResendCode.Text = "Resend Code";
            }
        }

        private string GenerateSecureCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint randomValue = BitConverter.ToUInt32(bytes, 0);
                return (randomValue % 900000 + 100000).ToString();
            }
        }

        private async Task<bool> SendVerificationEmailAsync(string toEmail, string code)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
                string senderPassword = ConfigurationManager.AppSettings["SenderPassword"];

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail, "CTFCA Admin System");
                    mail.To.Add(toEmail);
                    mail.Subject = "CTFCA - Password Reset Verification Code";
                    mail.Body = CreateEmailBody(code);
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000; // 10 second timeout

                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (you might want to add proper logging)
                System.Diagnostics.Debug.WriteLine($"Email send error: {ex.Message}");
                return false;
            }
        }

        private string CreateEmailBody(string code)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
            <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                
                <div style='text-align: center; margin-bottom: 30px;'>
                    <h2 style='color: #2A82FE; margin: 0;'>CTFCA Password Reset</h2>
                    <h3 style='color: #555; margin: 10px 0;'>Secure verification code below</h3>
                </div>

                <p style='margin-bottom: 20px;'>You have requested to reset your password. Please use the verification code below:</p>

                <div style='background-color: #f8f9fa; border: 2px solid #2A82FE; padding: 25px; text-align: center; margin: 30px 0; border-radius: 8px;'>
                    <div style='font-size: 32px; font-weight: bold; letter-spacing: 8px; color: #2A82FE; font-family: monospace;'>
                        {code}
                    </div>
                </div>

                <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0;'>
                    <p style='margin: 0; color: #856404;'>
                        <strong>Important:</strong> This code will expire in {CODE_EXPIRY_MINUTES} minutes.
                    </p>
                </div>

                <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; font-size: 12px; color: #666;'>
                    <p style='margin: 0;'>If you did not request this password reset, please ignore this email.</p>
                    <p style='margin: 10px 0 0 0;'>This is an automated message from CTFCA System.</p>
                </div>
            </div>
        </body>
        </html>";
        }

        private async Task ShowSuccessAnimationAsync()
        {
            Color successColor = Color.FromArgb(94, 148, 255);

            // Save original border colors
            var originalColors = new Dictionary<Guna.UI2.WinForms.Guna2TextBox, Color>();
            foreach (var tb in new[] { tb1, tb2, tb3, tb4, tb5, tb6 })
            {
                originalColors[tb] = tb.BorderColor;
                tb.BorderColor = successColor;
            }

            await Task.Delay(800); // wait 0.8 sec so user can see success

            // Restore original border colors
            foreach (var tb in new[] { tb1, tb2, tb3, tb4, tb5, tb6 })
            {
                tb.BorderColor = originalColors[tb];
            }
        }

    }
}