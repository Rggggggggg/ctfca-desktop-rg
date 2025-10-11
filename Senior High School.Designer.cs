namespace CFCA_ADMIN
{
    partial class Senior_High_School
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Senior_High_School));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgSHS = new Guna.UI2.WinForms.Guna2DataGridView();
            this.tbSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbStatusFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.student_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grade_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.submitted_at = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConfirm = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSHS)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtgSHS
            // 
            this.dtgSHS.AllowUserToAddRows = false;
            this.dtgSHS.AllowUserToDeleteRows = false;
            this.dtgSHS.AllowUserToResizeColumns = false;
            this.dtgSHS.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dtgSHS.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgSHS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtgSHS.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSHS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgSHS.ColumnHeadersHeight = 50;
            this.dtgSHS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dtgSHS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.student_number,
            this.name,
            this.grade_level,
            this.gender,
            this.age,
            this.contact,
            this.submitted_at,
            this.btnConfirm,
            this.btnDelete});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgSHS.DefaultCellStyle = dataGridViewCellStyle5;
            this.dtgSHS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgSHS.EnableHeadersVisualStyles = true;
            this.dtgSHS.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHS.Location = new System.Drawing.Point(0, 80);
            this.dtgSHS.Margin = new System.Windows.Forms.Padding(4);
            this.dtgSHS.Name = "dtgSHS";
            this.dtgSHS.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSHS.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dtgSHS.RowHeadersVisible = false;
            this.dtgSHS.RowHeadersWidth = 51;
            this.dtgSHS.RowTemplate.Height = 45;
            this.dtgSHS.Size = new System.Drawing.Size(1275, 484);
            this.dtgSHS.TabIndex = 0;
            this.dtgSHS.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHS.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgSHS.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgSHS.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgSHS.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgSHS.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHS.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHS.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgSHS.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgSHS.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSHS.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgSHS.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dtgSHS.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgSHS.ThemeStyle.ReadOnly = true;
            this.dtgSHS.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHS.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgSHS.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSHS.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSHS.ThemeStyle.RowsStyle.Height = 45;
            this.dtgSHS.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHS.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSHS.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgSHS_CellContentClick);
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.BorderRadius = 17;
            this.tbSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.DefaultText = "";
            this.tbSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbSearch.Location = new System.Drawing.Point(1119, 31);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PlaceholderText = "Search";
            this.tbSearch.SelectedText = "";
            this.tbSearch.Size = new System.Drawing.Size(153, 36);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // cbStatusFilter
            // 
            this.cbStatusFilter.AutoRoundedCorners = true;
            this.cbStatusFilter.BackColor = System.Drawing.Color.Transparent;
            this.cbStatusFilter.BorderRadius = 17;
            this.cbStatusFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatusFilter.FocusedColor = System.Drawing.Color.Empty;
            this.cbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbStatusFilter.ItemHeight = 30;
            this.cbStatusFilter.Items.AddRange(new object[] {
            "Pending",
            "Confirmed",
            "Rejected",
            "All"});
            this.cbStatusFilter.Location = new System.Drawing.Point(3, 31);
            this.cbStatusFilter.Name = "cbStatusFilter";
            this.cbStatusFilter.Size = new System.Drawing.Size(152, 36);
            this.cbStatusFilter.TabIndex = 0;
            this.cbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cbStatusFilter_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.tbSearch);
            this.panel1.Controls.Add(this.cbStatusFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1275, 80);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 564);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1275, 100);
            this.panel2.TabIndex = 3;
            // 
            // student_number
            // 
            this.student_number.HeaderText = "Student No.";
            this.student_number.MinimumWidth = 6;
            this.student_number.Name = "student_number";
            this.student_number.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // grade_level
            // 
            this.grade_level.HeaderText = "Level Applied";
            this.grade_level.MinimumWidth = 6;
            this.grade_level.Name = "grade_level";
            this.grade_level.ReadOnly = true;
            // 
            // gender
            // 
            this.gender.HeaderText = "Gender";
            this.gender.MinimumWidth = 6;
            this.gender.Name = "gender";
            this.gender.ReadOnly = true;
            // 
            // age
            // 
            this.age.HeaderText = "Age";
            this.age.MinimumWidth = 6;
            this.age.Name = "age";
            this.age.ReadOnly = true;
            // 
            // contact
            // 
            this.contact.HeaderText = "Contact";
            this.contact.MinimumWidth = 6;
            this.contact.Name = "contact";
            this.contact.ReadOnly = true;
            // 
            // submitted_at
            // 
            this.submitted_at.HeaderText = "Submitted At";
            this.submitted_at.MinimumWidth = 6;
            this.submitted_at.Name = "submitted_at";
            this.submitted_at.ReadOnly = true;
            // 
            // btnConfirm
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle3.NullValue")));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10);
            this.btnConfirm.DefaultCellStyle = dataGridViewCellStyle3;
            this.btnConfirm.FillWeight = 40F;
            this.btnConfirm.HeaderText = "Accept";
            this.btnConfirm.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.btnConfirm.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnConfirm.MinimumWidth = 6;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.ReadOnly = true;
            this.btnConfirm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // btnDelete
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle4.NullValue")));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(10);
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle4;
            this.btnDelete.FillWeight = 40F;
            this.btnDelete.HeaderText = "Decline";
            this.btnDelete.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.btnDelete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnDelete.MinimumWidth = 6;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ReadOnly = true;
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Senior_High_School
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtgSHS);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Senior_High_School";
            this.Size = new System.Drawing.Size(1275, 664);
            this.Load += new System.EventHandler(this.Senior_High_School_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSHS)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dtgSHS;
        private Guna.UI2.WinForms.Guna2TextBox tbSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cbStatusFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn age;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn submitted_at;
        private System.Windows.Forms.DataGridViewImageColumn btnConfirm;
        private System.Windows.Forms.DataGridViewImageColumn btnDelete;
    }
}
