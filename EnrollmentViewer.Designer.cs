namespace CAPSTONE_PROJ
{
    partial class EnrollmentViewer
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cbDepartmentFilter;
        private System.Windows.Forms.DataGridView dtgEnrollees;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.DataGridView dtgDetails;
        private System.Windows.Forms.Label lblSelectedStudent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cbDepartmentFilter = new System.Windows.Forms.ComboBox();
            this.dtgEnrollees = new System.Windows.Forms.DataGridView();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lblSelectedStudent = new System.Windows.Forms.Label();
            this.dtgDetails = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtgEnrollees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgDetails)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDepartmentFilter
            // 
            this.cbDepartmentFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartmentFilter.Location = new System.Drawing.Point(12, 12);
            this.cbDepartmentFilter.Name = "cbDepartmentFilter";
            this.cbDepartmentFilter.Size = new System.Drawing.Size(250, 23);
            this.cbDepartmentFilter.SelectedIndexChanged += new System.EventHandler(this.cbDepartmentFilter_SelectedIndexChanged);
            // 
            // dtgEnrollees
            // 
            this.dtgEnrollees.AllowUserToAddRows = false;
            this.dtgEnrollees.AllowUserToDeleteRows = false;
            this.dtgEnrollees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgEnrollees.Location = new System.Drawing.Point(12, 45);
            this.dtgEnrollees.MultiSelect = false;
            this.dtgEnrollees.Name = "dtgEnrollees";
            this.dtgEnrollees.ReadOnly = true;
            this.dtgEnrollees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgEnrollees.Size = new System.Drawing.Size(700, 550);
            this.dtgEnrollees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgEnrollees_CellClick);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.lblSelectedStudent);
            this.pnlDetails.Controls.Add(this.dtgDetails);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetails.Width = 550;
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // lblSelectedStudent
            // 
            this.lblSelectedStudent.AutoSize = true;
            this.lblSelectedStudent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedStudent.Location = new System.Drawing.Point(15, 15);
            this.lblSelectedStudent.Name = "lblSelectedStudent";
            this.lblSelectedStudent.Text = "Select a student to view details";
            // 
            // dtgDetails
            // 
            this.dtgDetails.AllowUserToAddRows = false;
            this.dtgDetails.AllowUserToDeleteRows = false;
            this.dtgDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgDetails.Location = new System.Drawing.Point(10, 45);
            this.dtgDetails.Size = new System.Drawing.Size(520, 540);
            this.dtgDetails.ReadOnly = true;
            // 
            // EnrollmentViewer
            // 
            this.ClientSize = new System.Drawing.Size(1260, 610);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.dtgEnrollees);
            this.Controls.Add(this.cbDepartmentFilter);
            this.Load += new System.EventHandler(this.EnrollmentViewer_Load);
            this.Text = "Enrollment Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dtgEnrollees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgDetails)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
