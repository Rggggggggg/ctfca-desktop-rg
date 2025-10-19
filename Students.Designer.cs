namespace CFCA_ADMIN
{
    partial class Students
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle57 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle58 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle64 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle65 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle59 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Students));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle60 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle61 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle62 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle63 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle66 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle67 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle68 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle69 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle70 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgStudents = new Guna.UI2.WinForms.Guna2DataGridView();
            this.photo = new System.Windows.Forms.DataGridViewImageColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Grades = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Strand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payment_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.view_info = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnConfirm = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnGrades = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnPayment = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tbSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbGradeLevel = new Guna.UI2.WinForms.Guna2ComboBox();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.btnPrev = new Guna.UI2.WinForms.Guna2Button();
            this.lblPageInfo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dtgStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgStudents
            // 
            this.dtgStudents.AllowUserToAddRows = false;
            this.dtgStudents.AllowUserToDeleteRows = false;
            this.dtgStudents.AllowUserToResizeColumns = false;
            this.dtgStudents.AllowUserToResizeRows = false;
            dataGridViewCellStyle57.BackColor = System.Drawing.Color.White;
            this.dtgStudents.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle57;
            this.dtgStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgStudents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle58.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle58.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle58.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle58.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle58.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle58.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle58.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgStudents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle58;
            this.dtgStudents.ColumnHeadersHeight = 50;
            this.dtgStudents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.photo,
            this.id,
            this.name,
            this.Grades,
            this.gender,
            this.age,
            this.contact,
            this.Strand,
            this.payment_status,
            this.view_info,
            this.btnConfirm,
            this.btnDelete,
            this.btnGrades,
            this.btnPayment});
            dataGridViewCellStyle64.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle64.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle64.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle64.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle64.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle64.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle64.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgStudents.DefaultCellStyle = dataGridViewCellStyle64;
            this.dtgStudents.EnableHeadersVisualStyles = true;
            this.dtgStudents.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgStudents.Location = new System.Drawing.Point(12, 86);
            this.dtgStudents.Name = "dtgStudents";
            this.dtgStudents.ReadOnly = true;
            dataGridViewCellStyle65.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle65.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle65.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle65.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle65.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle65.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle65.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgStudents.RowHeadersDefaultCellStyle = dataGridViewCellStyle65;
            this.dtgStudents.RowHeadersVisible = false;
            this.dtgStudents.RowHeadersWidth = 51;
            this.dtgStudents.RowTemplate.Height = 60;
            this.dtgStudents.Size = new System.Drawing.Size(845, 433);
            this.dtgStudents.TabIndex = 4;
            this.dtgStudents.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgStudents.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgStudents.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgStudents.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgStudents.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgStudents.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgStudents.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgStudents.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgStudents.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgStudents.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgStudents.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgStudents.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgStudents.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgStudents.ThemeStyle.ReadOnly = true;
            this.dtgStudents.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgStudents.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgStudents.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgStudents.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgStudents.ThemeStyle.RowsStyle.Height = 60;
            this.dtgStudents.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgStudents.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgStudents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgStudents_CellContentClick);
            // 
            // photo
            // 
            dataGridViewCellStyle59.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle59.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle59.NullValue")));
            dataGridViewCellStyle59.Padding = new System.Windows.Forms.Padding(2);
            this.photo.DefaultCellStyle = dataGridViewCellStyle59;
            this.photo.FillWeight = 60F;
            this.photo.HeaderText = "Photo";
            this.photo.Image = global::CFCA_ADMIN.Properties.Resources.no_image;
            this.photo.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.photo.MinimumWidth = 6;
            this.photo.Name = "photo";
            this.photo.ReadOnly = true;
            // 
            // id
            // 
            this.id.FillWeight = 70F;
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // name
            // 
            this.name.FillWeight = 120F;
            this.name.HeaderText = "Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // Grades
            // 
            this.Grades.FillWeight = 80F;
            this.Grades.HeaderText = "Grade Level";
            this.Grades.MinimumWidth = 6;
            this.Grades.Name = "Grades";
            this.Grades.ReadOnly = true;
            // 
            // gender
            // 
            this.gender.FillWeight = 60F;
            this.gender.HeaderText = "Gender";
            this.gender.MinimumWidth = 6;
            this.gender.Name = "gender";
            this.gender.ReadOnly = true;
            // 
            // age
            // 
            this.age.FillWeight = 50F;
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
            // Strand
            // 
            this.Strand.FillWeight = 50F;
            this.Strand.HeaderText = "Strand";
            this.Strand.MinimumWidth = 6;
            this.Strand.Name = "Strand";
            this.Strand.ReadOnly = true;
            // 
            // payment_status
            // 
            this.payment_status.FillWeight = 50F;
            this.payment_status.HeaderText = "Payment";
            this.payment_status.MinimumWidth = 6;
            this.payment_status.Name = "payment_status";
            this.payment_status.ReadOnly = true;
            // 
            // view_info
            // 
            this.view_info.FillWeight = 60F;
            this.view_info.HeaderText = "View Info";
            this.view_info.Name = "view_info";
            this.view_info.ReadOnly = true;
            this.view_info.Text = "View Info";
            this.view_info.UseColumnTextForButtonValue = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle60.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle60.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle60.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle60.NullValue")));
            dataGridViewCellStyle60.Padding = new System.Windows.Forms.Padding(5);
            this.btnConfirm.DefaultCellStyle = dataGridViewCellStyle60;
            this.btnConfirm.FillWeight = 35F;
            this.btnConfirm.HeaderText = "Accept";
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
            dataGridViewCellStyle61.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle61.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle61.NullValue")));
            dataGridViewCellStyle61.Padding = new System.Windows.Forms.Padding(5);
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle61;
            this.btnDelete.FillWeight = 35F;
            this.btnDelete.HeaderText = "Decline";
            this.btnDelete.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.btnDelete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnDelete.MinimumWidth = 6;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ReadOnly = true;
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.Width = 45;
            // 
            // btnGrades
            // 
            this.btnGrades.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle62.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle62.NullValue = null;
            dataGridViewCellStyle62.Padding = new System.Windows.Forms.Padding(5);
            this.btnGrades.DefaultCellStyle = dataGridViewCellStyle62;
            this.btnGrades.FillWeight = 35F;
            this.btnGrades.HeaderText = "Grading";
            this.btnGrades.Image = ((System.Drawing.Image)(resources.GetObject("btnGrades.Image")));
            this.btnGrades.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnGrades.MinimumWidth = 6;
            this.btnGrades.Name = "btnGrades";
            this.btnGrades.ReadOnly = true;
            this.btnGrades.Width = 45;
            // 
            // btnPayment
            // 
            this.btnPayment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle63.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle63.NullValue = null;
            dataGridViewCellStyle63.Padding = new System.Windows.Forms.Padding(5);
            this.btnPayment.DefaultCellStyle = dataGridViewCellStyle63;
            this.btnPayment.FillWeight = 35F;
            this.btnPayment.HeaderText = "Payment";
            this.btnPayment.Image = ((System.Drawing.Image)(resources.GetObject("btnPayment.Image")));
            this.btnPayment.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnPayment.MinimumWidth = 6;
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.ReadOnly = true;
            this.btnPayment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnPayment.Width = 47;
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle66.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle66.NullValue = null;
            dataGridViewCellStyle66.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle66;
            this.dataGridViewImageColumn1.FillWeight = 40F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 43;
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle67.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle67.NullValue = null;
            dataGridViewCellStyle67.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle67;
            this.dataGridViewImageColumn2.FillWeight = 40F;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 6;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 43;
            // 
            // dataGridViewImageColumn3
            // 
            dataGridViewCellStyle68.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle68.NullValue = null;
            dataGridViewCellStyle68.Padding = new System.Windows.Forms.Padding(5);
            this.dataGridViewImageColumn3.DefaultCellStyle = dataGridViewCellStyle68;
            this.dataGridViewImageColumn3.FillWeight = 35F;
            this.dataGridViewImageColumn3.HeaderText = "";
            this.dataGridViewImageColumn3.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn3.Image")));
            this.dataGridViewImageColumn3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn3.MinimumWidth = 6;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn3.Width = 40;
            // 
            // dataGridViewImageColumn4
            // 
            dataGridViewCellStyle69.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle69.NullValue = null;
            dataGridViewCellStyle69.Padding = new System.Windows.Forms.Padding(5);
            this.dataGridViewImageColumn4.DefaultCellStyle = dataGridViewCellStyle69;
            this.dataGridViewImageColumn4.FillWeight = 35F;
            this.dataGridViewImageColumn4.HeaderText = "";
            this.dataGridViewImageColumn4.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.dataGridViewImageColumn4.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn4.MinimumWidth = 6;
            this.dataGridViewImageColumn4.Name = "dataGridViewImageColumn4";
            this.dataGridViewImageColumn4.ReadOnly = true;
            this.dataGridViewImageColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn4.Width = 40;
            // 
            // dataGridViewImageColumn5
            // 
            dataGridViewCellStyle70.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle70.NullValue = null;
            dataGridViewCellStyle70.Padding = new System.Windows.Forms.Padding(5);
            this.dataGridViewImageColumn5.DefaultCellStyle = dataGridViewCellStyle70;
            this.dataGridViewImageColumn5.FillWeight = 35F;
            this.dataGridViewImageColumn5.HeaderText = "payment";
            this.dataGridViewImageColumn5.Image = global::CFCA_ADMIN.Properties.Resources.payment;
            this.dataGridViewImageColumn5.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn5.MinimumWidth = 6;
            this.dataGridViewImageColumn5.Name = "dataGridViewImageColumn5";
            this.dataGridViewImageColumn5.ReadOnly = true;
            this.dataGridViewImageColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn5.Width = 125;
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
            this.tbSearch.Location = new System.Drawing.Point(685, 44);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PlaceholderText = "Search";
            this.tbSearch.SelectedText = "";
            this.tbSearch.Size = new System.Drawing.Size(172, 36);
            this.tbSearch.TabIndex = 9;
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
            "Grade 11 STEM",
            "Grade 11 ABM",
            "Grade 11 HUMSS",
            "Grade 12 STEM",
            "Grade 12 ABM",
            "Grade 12 HUMSS"});
            this.cbGradeLevel.Location = new System.Drawing.Point(12, 44);
            this.cbGradeLevel.Name = "cbGradeLevel";
            this.cbGradeLevel.Size = new System.Drawing.Size(152, 36);
            this.cbGradeLevel.TabIndex = 10;
            this.cbGradeLevel.SelectedIndexChanged += new System.EventHandler(this.cbGradeLevel_SelectedIndexChanged_1);
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this.dtgStudents;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BorderColor = System.Drawing.Color.Silver;
            this.btnNext.BorderRadius = 10;
            this.btnNext.BorderThickness = 1;
            this.btnNext.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNext.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNext.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNext.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNext.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(130)))), ((int)(((byte)(254)))));
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnNext.ImageOffset = new System.Drawing.Point(15, 0);
            this.btnNext.ImageSize = new System.Drawing.Size(15, 15);
            this.btnNext.Location = new System.Drawing.Point(757, 536);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 45);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "Next";
            this.btnNext.TextOffset = new System.Drawing.Point(-5, 0);
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.BorderColor = System.Drawing.Color.Silver;
            this.btnPrev.BorderRadius = 10;
            this.btnPrev.BorderThickness = 1;
            this.btnPrev.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrev.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPrev.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPrev.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPrev.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(130)))), ((int)(((byte)(254)))));
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.White;
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnPrev.ImageSize = new System.Drawing.Size(15, 15);
            this.btnPrev.Location = new System.Drawing.Point(514, 536);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(100, 45);
            this.btnPrev.TabIndex = 12;
            this.btnPrev.Text = "Previous";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageInfo.Location = new System.Drawing.Point(620, 551);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(90, 15);
            this.lblPageInfo.TabIndex = 13;
            this.lblPageInfo.Text = "guna2HtmlLabel1";
            // 
            // Students
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.cbGradeLevel);
            this.Controls.Add(this.dtgStudents);
            this.Name = "Students";
            this.Size = new System.Drawing.Size(871, 601);
            this.Load += new System.EventHandler(this.Students_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2DataGridView dtgStudents;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn4;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn5;
        private Guna.UI2.WinForms.Guna2TextBox tbSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cbGradeLevel;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.DataGridViewImageColumn photo;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Grades;
        private System.Windows.Forms.DataGridViewTextBoxColumn gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn age;
        private System.Windows.Forms.DataGridViewTextBoxColumn contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Strand;
        private System.Windows.Forms.DataGridViewTextBoxColumn payment_status;
        private System.Windows.Forms.DataGridViewButtonColumn view_info;
        private System.Windows.Forms.DataGridViewImageColumn btnConfirm;
        private System.Windows.Forms.DataGridViewImageColumn btnDelete;
        private System.Windows.Forms.DataGridViewImageColumn btnGrades;
        private System.Windows.Forms.DataGridViewImageColumn btnPayment;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnPrev;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPageInfo;
    }
}