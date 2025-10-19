namespace CFCA_ADMIN
{
    partial class Subjects
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Subjects));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgSubjects = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbGradeLevel = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnAddSubject = new Guna.UI2.WinForms.Guna2Button();
            this.grade_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subject_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConfirm = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSubjects)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgSubjects
            // 
            this.dtgSubjects.AllowUserToAddRows = false;
            this.dtgSubjects.AllowUserToDeleteRows = false;
            this.dtgSubjects.AllowUserToResizeColumns = false;
            this.dtgSubjects.AllowUserToResizeRows = false;
            dataGridViewCellStyle33.BackColor = System.Drawing.Color.White;
            this.dtgSubjects.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle33;
            this.dtgSubjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgSubjects.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.dtgSubjects.ColumnHeadersHeight = 50;
            this.dtgSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.grade_level,
            this.subject_name,
            this.btnConfirm,
            this.btnDelete});
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle37.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle37.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle37.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle37.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle37.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgSubjects.DefaultCellStyle = dataGridViewCellStyle37;
            this.dtgSubjects.EnableHeadersVisualStyles = true;
            this.dtgSubjects.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSubjects.Location = new System.Drawing.Point(12, 86);
            this.dtgSubjects.Name = "dtgSubjects";
            this.dtgSubjects.ReadOnly = true;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle38.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSubjects.RowHeadersDefaultCellStyle = dataGridViewCellStyle38;
            this.dtgSubjects.RowHeadersVisible = false;
            this.dtgSubjects.RowHeadersWidth = 51;
            this.dtgSubjects.RowTemplate.Height = 45;
            this.dtgSubjects.Size = new System.Drawing.Size(845, 433);
            this.dtgSubjects.TabIndex = 6;
            this.dtgSubjects.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSubjects.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgSubjects.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgSubjects.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgSubjects.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgSubjects.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgSubjects.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSubjects.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgSubjects.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgSubjects.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSubjects.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgSubjects.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgSubjects.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgSubjects.ThemeStyle.ReadOnly = true;
            this.dtgSubjects.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSubjects.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgSubjects.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSubjects.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSubjects.ThemeStyle.RowsStyle.Height = 45;
            this.dtgSubjects.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSubjects.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSubjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgSubjects_CellContentClick);
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle39.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle39.NullValue")));
            dataGridViewCellStyle39.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle39;
            this.dataGridViewImageColumn1.FillWeight = 40F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 43;
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle40.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle40.NullValue")));
            dataGridViewCellStyle40.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle40;
            this.dataGridViewImageColumn2.FillWeight = 40F;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 6;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 43;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 525);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(871, 76);
            this.panel2.TabIndex = 11;
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.BorderRadius = 17;
            this.tbSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.DefaultText = "";
            this.tbSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbSearch.IconLeft = ((System.Drawing.Image)(resources.GetObject("tbSearch.IconLeft")));
            this.tbSearch.Location = new System.Drawing.Point(704, 46);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PlaceholderText = "Search";
            this.tbSearch.SelectedText = "";
            this.tbSearch.Size = new System.Drawing.Size(153, 36);
            this.tbSearch.TabIndex = 12;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged_1);
            // 
            // cbGradeLevel
            // 
            this.cbGradeLevel.AutoRoundedCorners = true;
            this.cbGradeLevel.BackColor = System.Drawing.Color.Transparent;
            this.cbGradeLevel.BorderRadius = 17;
            this.cbGradeLevel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGradeLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradeLevel.DropDownWidth = 155;
            this.cbGradeLevel.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbGradeLevel.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbGradeLevel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbGradeLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbGradeLevel.ItemHeight = 30;
            this.cbGradeLevel.Items.AddRange(new object[] {
            "All",
            "Nursery",
            "Kinder 1",
            "Kinder 2 (P)",
            "Grade 1",
            "Grade 2",
            "Grade 3",
            "Grade 4",
            "Grade 5",
            "Grade 6",
            "Grade 7",
            "Grade 8",
            "Grade 9",
            "Grade 10",
            "Grade 11 ABM",
            "Grade 11 STEM",
            "Grade 11 HUMSS",
            "Grade 12 ABM",
            "Grade 12 STEM",
            "Grade 12 HUMSS"});
            this.cbGradeLevel.Location = new System.Drawing.Point(12, 44);
            this.cbGradeLevel.Name = "cbGradeLevel";
            this.cbGradeLevel.Size = new System.Drawing.Size(152, 36);
            this.cbGradeLevel.TabIndex = 13;
            this.cbGradeLevel.SelectedIndexChanged += new System.EventHandler(this.cbGradeLevel_SelectedIndexChanged_1);
            // 
            // btnAddSubject
            // 
            this.btnAddSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSubject.BorderRadius = 17;
            this.btnAddSubject.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddSubject.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddSubject.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddSubject.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddSubject.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(130)))), ((int)(((byte)(254)))));
            this.btnAddSubject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSubject.ForeColor = System.Drawing.Color.White;
            this.btnAddSubject.Image = ((System.Drawing.Image)(resources.GetObject("btnAddSubject.Image")));
            this.btnAddSubject.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAddSubject.Location = new System.Drawing.Point(618, 47);
            this.btnAddSubject.Name = "btnAddSubject";
            this.btnAddSubject.Size = new System.Drawing.Size(80, 33);
            this.btnAddSubject.TabIndex = 14;
            this.btnAddSubject.Text = "New";
            this.btnAddSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnAddSubject.Click += new System.EventHandler(this.btnAddSubject_Click_1);
            // 
            // grade_level
            // 
            this.grade_level.FillWeight = 300F;
            this.grade_level.HeaderText = "Grade Level";
            this.grade_level.MinimumWidth = 6;
            this.grade_level.Name = "grade_level";
            this.grade_level.ReadOnly = true;
            // 
            // subject_name
            // 
            this.subject_name.FillWeight = 300F;
            this.subject_name.HeaderText = "Subject Name";
            this.subject_name.MinimumWidth = 6;
            this.subject_name.Name = "subject_name";
            this.subject_name.ReadOnly = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle35.NullValue")));
            dataGridViewCellStyle35.Padding = new System.Windows.Forms.Padding(10);
            this.btnConfirm.DefaultCellStyle = dataGridViewCellStyle35;
            this.btnConfirm.FillWeight = 40F;
            this.btnConfirm.HeaderText = "";
            this.btnConfirm.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.btnConfirm.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnConfirm.MinimumWidth = 6;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.ReadOnly = true;
            this.btnConfirm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnConfirm.Width = 45;
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle36.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle36.NullValue")));
            dataGridViewCellStyle36.Padding = new System.Windows.Forms.Padding(10);
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle36;
            this.btnDelete.FillWeight = 40F;
            this.btnDelete.HeaderText = "";
            this.btnDelete.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.btnDelete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnDelete.MinimumWidth = 6;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ReadOnly = true;
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.Width = 45;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this.dtgSubjects;
            // 
            // Subjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.cbGradeLevel);
            this.Controls.Add(this.btnAddSubject);
            this.Controls.Add(this.dtgSubjects);
            this.Controls.Add(this.panel2);
            this.Name = "Subjects";
            this.Size = new System.Drawing.Size(871, 601);
            this.Load += new System.EventHandler(this.Subjects_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSubjects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2DataGridView dtgSubjects;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2TextBox tbSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cbGradeLevel;
        private Guna.UI2.WinForms.Guna2Button btnAddSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject_name;
        private System.Windows.Forms.DataGridViewImageColumn btnConfirm;
        private System.Windows.Forms.DataGridViewImageColumn btnDelete;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
    }
}
