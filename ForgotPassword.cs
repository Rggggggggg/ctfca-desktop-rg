using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
            this.AcceptButton = btnSubmit;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            string email = tbEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.");
                return;
            }

            // Change button state
            btnSubmit.Text = "Verifying...";
            btnSubmit.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id FROM admin_accounts WHERE email=@Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int adminId = Convert.ToInt32(result);

                        if (!VerificationStore.CanRequestNewCode(email))
                        {
                            MessageBox.Show("You have recently requested a code. Please wait before trying again.");
                            return;
                        }

                        string code = GenerateVerificationCode();
                        VerificationStore.SaveCode(email, code, TimeSpan.FromMinutes(10));

                        try
                        {
                            await SendVerificationEmailAsync(email, code);
                            MessageBox.Show($"Verification code sent to {email}");

                            VerifyCodeForm verifyForm = new VerifyCodeForm(email, code, adminId);
                            verifyForm.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to send email: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email not found.");
                    }
                }
            }
            finally
            {
                // Reset button state
                btnSubmit.Text = "Submit";
                btnSubmit.Enabled = true;
            }
        }


        // Store verification codes with expiry + rate limiting
        public static class VerificationStore
        {
            private class CodeInfo
            {
                public string Code { get; set; }
                public DateTime Expiry { get; set; }
                public DateTime LastRequest { get; set; }
            }

            private static Dictionary<string, CodeInfo> codeMap = new Dictionary<string, CodeInfo>();

            public static void SaveCode(string email, string code, TimeSpan expiryDuration)
            {
                codeMap[email] = new CodeInfo
                {
                    Code = code,
                    Expiry = DateTime.Now.Add(expiryDuration),
                    LastRequest = DateTime.Now
                };
            }

            public static string GetCode(string email)
            {
                if (codeMap.TryGetValue(email, out var info))
                {
                    if (DateTime.Now <= info.Expiry)
                        return info.Code;
                }
                return null;
            }

            public static bool CanRequestNewCode(string email)
            {
                if (codeMap.TryGetValue(email, out var info))
                {
                    // Allow new request only if 1 minute has passed
                    return (DateTime.Now - info.LastRequest).TotalSeconds > 60;
                }
                return true;
            }
        }

        private string GenerateVerificationCode()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString(); // 6-digit code
        }

        private async Task SendVerificationEmailAsync(string toEmail, string code)
        {
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            string senderPassword = ConfigurationManager.AppSettings["SenderPassword"];

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmail, "CTFCA Admin System"),
                Subject = "CTFCA - Password Reset Verification Code",
                Body = CreateEmailBody(code),
                IsBodyHtml = true // ✅ Make sure HTML is used
            };
            mail.To.Add(toEmail);

            using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
        private string CreateEmailBody(string code)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h2 style='color: #2A82FE; margin: 0;'>CTFCA Admin System</h2>
                            <h3 style='color: #555; margin: 10px 0;'>Password Reset Request</h3>
                        </div>
                        
                        <p style='margin-bottom: 20px;'>Hello,</p>
                        <p style='margin-bottom: 20px;'>You have requested to reset your password. Please use the verification code below to continue:</p>
                        
                        <div style='background-color: #f8f9fa; border: 2px solid #2A82FE; padding: 25px; text-align: center; margin: 30px 0; border-radius: 8px;'>
                            <div style='font-size: 32px; font-weight: bold; letter-spacing: 8px; color: #2A82FE; font-family: monospace;'>
                                {code}
                            </div>
                        </div>
                        
                        <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0;'>
                            <p style='margin: 0; color: #856404;'><strong>Important:</strong> This code will expire in 10 minutes for security reasons.</p>
                        </div>
                        
                        <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; font-size: 12px; color: #666;'>
                            <p style='margin: 0;'>If you did not request this password reset, please ignore this email and contact your system administrator.</p>
                            <p style='margin: 10px 0 0 0;'>This is an automated message from CTFCA Admin System.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {
            tbEmail.Focus();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
