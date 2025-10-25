namespace CFCA_ADMIN
{
    partial class EnrollmentControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxMessage = new System.Windows.Forms.GroupBox();
            this.btnUpdateMessage = new System.Windows.Forms.Button();
            this.txtEnrollmentMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.btnCloseEnrollment = new System.Windows.Forms.Button();
            this.btnOpenEnrollment = new System.Windows.Forms.Button();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lblLastUpdatedValue = new System.Windows.Forms.Label();
            this.lblLastUpdated = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.groupBoxMessage.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.groupBoxMessage);
            this.panelMain.Controls.Add(this.groupBoxControl);
            this.panelMain.Controls.Add(this.groupBoxStatus);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(800, 600);
            this.panelMain.TabIndex = 0;
            // 
            // groupBoxMessage
            // 
            this.groupBoxMessage.Controls.Add(this.btnUpdateMessage);
            this.groupBoxMessage.Controls.Add(this.txtEnrollmentMessage);
            this.groupBoxMessage.Controls.Add(this.lblMessage);
            this.groupBoxMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxMessage.Location = new System.Drawing.Point(23, 300);
            this.groupBoxMessage.Name = "groupBoxMessage";
            this.groupBoxMessage.Size = new System.Drawing.Size(754, 180);
            this.groupBoxMessage.TabIndex = 2;
            this.groupBoxMessage.TabStop = false;
            this.groupBoxMessage.Text = "Enrollment Message";
            // 
            // btnUpdateMessage
            // 
            this.btnUpdateMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnUpdateMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdateMessage.ForeColor = System.Drawing.Color.White;
            this.btnUpdateMessage.Location = new System.Drawing.Point(24, 130);
            this.btnUpdateMessage.Name = "btnUpdateMessage";
            this.btnUpdateMessage.Size = new System.Drawing.Size(200, 35);
            this.btnUpdateMessage.TabIndex = 2;
            this.btnUpdateMessage.Text = "Update Message";
            this.btnUpdateMessage.UseVisualStyleBackColor = false;
            this.btnUpdateMessage.Click += new System.EventHandler(this.btnUpdateMessage_Click);
            // 
            // txtEnrollmentMessage
            // 
            this.txtEnrollmentMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEnrollmentMessage.Location = new System.Drawing.Point(24, 65);
            this.txtEnrollmentMessage.Multiline = true;
            this.txtEnrollmentMessage.Name = "txtEnrollmentMessage";
            this.txtEnrollmentMessage.Size = new System.Drawing.Size(706, 50);
            this.txtEnrollmentMessage.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMessage.Location = new System.Drawing.Point(20, 35);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(291, 19);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message to display when enrollment is closed:";
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.btnCloseEnrollment);
            this.groupBoxControl.Controls.Add(this.btnOpenEnrollment);
            this.groupBoxControl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxControl.Location = new System.Drawing.Point(23, 160);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Size = new System.Drawing.Size(754, 120);
            this.groupBoxControl.TabIndex = 1;
            this.groupBoxControl.TabStop = false;
            this.groupBoxControl.Text = "Enrollment Control";
            // 
            // btnCloseEnrollment
            // 
            this.btnCloseEnrollment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCloseEnrollment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseEnrollment.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseEnrollment.ForeColor = System.Drawing.Color.White;
            this.btnCloseEnrollment.Location = new System.Drawing.Point(390, 45);
            this.btnCloseEnrollment.Name = "btnCloseEnrollment";
            this.btnCloseEnrollment.Size = new System.Drawing.Size(340, 50);
            this.btnCloseEnrollment.TabIndex = 1;
            this.btnCloseEnrollment.Text = "CLOSE ENROLLMENT";
            this.btnCloseEnrollment.UseVisualStyleBackColor = false;
            this.btnCloseEnrollment.Click += new System.EventHandler(this.btnCloseEnrollment_Click);
            // 
            // btnOpenEnrollment
            // 
            this.btnOpenEnrollment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnOpenEnrollment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenEnrollment.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnOpenEnrollment.ForeColor = System.Drawing.Color.White;
            this.btnOpenEnrollment.Location = new System.Drawing.Point(24, 45);
            this.btnOpenEnrollment.Name = "btnOpenEnrollment";
            this.btnOpenEnrollment.Size = new System.Drawing.Size(340, 50);
            this.btnOpenEnrollment.TabIndex = 0;
            this.btnOpenEnrollment.Text = "OPEN ENROLLMENT";
            this.btnOpenEnrollment.UseVisualStyleBackColor = false;
            this.btnOpenEnrollment.Click += new System.EventHandler(this.btnOpenEnrollment_Click);
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.lblLastUpdatedValue);
            this.groupBoxStatus.Controls.Add(this.lblLastUpdated);
            this.groupBoxStatus.Controls.Add(this.lblStatusValue);
            this.groupBoxStatus.Controls.Add(this.lblCurrentStatus);
            this.groupBoxStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.groupBoxStatus.Location = new System.Drawing.Point(23, 23);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(754, 120);
            this.groupBoxStatus.TabIndex = 0;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Current Enrollment Status";
            // 
            // lblLastUpdatedValue
            // 
            this.lblLastUpdatedValue.AutoSize = true;
            this.lblLastUpdatedValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLastUpdatedValue.Location = new System.Drawing.Point(125, 75);
            this.lblLastUpdatedValue.Name = "lblLastUpdatedValue";
            this.lblLastUpdatedValue.Size = new System.Drawing.Size(45, 19);
            this.lblLastUpdatedValue.TabIndex = 3;
            this.lblLastUpdatedValue.Text = "Never";
            // 
            // lblLastUpdated
            // 
            this.lblLastUpdated.AutoSize = true;
            this.lblLastUpdated.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLastUpdated.Location = new System.Drawing.Point(20, 75);
            this.lblLastUpdated.Name = "lblLastUpdated";
            this.lblLastUpdated.Size = new System.Drawing.Size(94, 19);
            this.lblLastUpdated.TabIndex = 2;
            this.lblLastUpdated.Text = "Last Updated:";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatusValue.ForeColor = System.Drawing.Color.Red;
            this.lblStatusValue.Location = new System.Drawing.Point(153, 40);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(61, 19);
            this.lblStatusValue.TabIndex = 1;
            this.lblStatusValue.Text = "CLOSED";
            // 
            // lblCurrentStatus
            // 
            this.lblCurrentStatus.AutoSize = true;
            this.lblCurrentStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCurrentStatus.Location = new System.Drawing.Point(20, 40);
            this.lblCurrentStatus.Name = "lblCurrentStatus";
            this.lblCurrentStatus.Size = new System.Drawing.Size(120, 19);
            this.lblCurrentStatus.TabIndex = 0;
            this.lblCurrentStatus.Text = "Enrollment Status:";
            // 
            // EnrollmentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "EnrollmentControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.EnrollmentControl_Load);
            this.panelMain.ResumeLayout(false);
            this.groupBoxMessage.ResumeLayout(false);
            this.groupBoxMessage.PerformLayout();
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lblLastUpdatedValue;
        private System.Windows.Forms.Label lblLastUpdated;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblCurrentStatus;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.Button btnCloseEnrollment;
        private System.Windows.Forms.Button btnOpenEnrollment;
        private System.Windows.Forms.GroupBox groupBoxMessage;
        private System.Windows.Forms.Button btnUpdateMessage;
        private System.Windows.Forms.TextBox txtEnrollmentMessage;
        private System.Windows.Forms.Label lblMessage;
    }
}