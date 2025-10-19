namespace CFCA_ADMIN
{
    partial class Gradingform
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgGrades = new Guna.UI2.WinForms.Guna2DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.First = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Second = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Third = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fourth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Average = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dtgSHSGrades = new Guna.UI2.WinForms.Guna2DataGridView();
            this.IDSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubjectSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Semester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemarksSHS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionSHS = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cbSemester = new Guna.UI2.WinForms.Guna2ComboBox();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblname = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtgGrades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSHSGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgGrades
            // 
            this.dtgGrades.AllowUserToAddRows = false;
            this.dtgGrades.AllowUserToDeleteRows = false;
            this.dtgGrades.AllowUserToResizeColumns = false;
            this.dtgGrades.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dtgGrades.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgGrades.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgGrades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgGrades.ColumnHeadersHeight = 50;
            this.dtgGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dtgGrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.subject,
            this.First,
            this.Second,
            this.Third,
            this.Fourth,
            this.Average,
            this.Remarks,
            this.Action});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgGrades.DefaultCellStyle = dataGridViewCellStyle6;
            this.dtgGrades.EnableHeadersVisualStyles = true;
            this.dtgGrades.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgGrades.Location = new System.Drawing.Point(19, 127);
            this.dtgGrades.Name = "dtgGrades";
            this.dtgGrades.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgGrades.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dtgGrades.RowHeadersVisible = false;
            this.dtgGrades.RowHeadersWidth = 51;
            this.dtgGrades.RowTemplate.Height = 45;
            this.dtgGrades.Size = new System.Drawing.Size(842, 433);
            this.dtgGrades.TabIndex = 3;
            this.dtgGrades.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgGrades.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgGrades.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgGrades.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgGrades.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgGrades.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgGrades.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgGrades.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgGrades.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgGrades.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgGrades.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgGrades.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dtgGrades.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgGrades.ThemeStyle.ReadOnly = true;
            this.dtgGrades.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgGrades.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgGrades.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgGrades.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgGrades.ThemeStyle.RowsStyle.Height = 45;
            this.dtgGrades.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgGrades.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgGrades.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgGrades_CellContentClick);
            // 
            // ID
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ID.DefaultCellStyle = dataGridViewCellStyle3;
            this.ID.FillWeight = 80F;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // subject
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subject.DefaultCellStyle = dataGridViewCellStyle4;
            this.subject.FillWeight = 80F;
            this.subject.HeaderText = "Subject";
            this.subject.MinimumWidth = 6;
            this.subject.Name = "subject";
            this.subject.ReadOnly = true;
            // 
            // First
            // 
            this.First.FillWeight = 60F;
            this.First.HeaderText = "First";
            this.First.MinimumWidth = 6;
            this.First.Name = "First";
            this.First.ReadOnly = true;
            // 
            // Second
            // 
            this.Second.FillWeight = 50F;
            this.Second.HeaderText = "Second";
            this.Second.MinimumWidth = 6;
            this.Second.Name = "Second";
            this.Second.ReadOnly = true;
            // 
            // Third
            // 
            this.Third.FillWeight = 110F;
            this.Third.HeaderText = "Third";
            this.Third.MinimumWidth = 6;
            this.Third.Name = "Third";
            this.Third.ReadOnly = true;
            // 
            // Fourth
            // 
            this.Fourth.FillWeight = 110F;
            this.Fourth.HeaderText = "Fourth";
            this.Fourth.MinimumWidth = 6;
            this.Fourth.Name = "Fourth";
            this.Fourth.ReadOnly = true;
            // 
            // Average
            // 
            this.Average.HeaderText = "Average";
            this.Average.MinimumWidth = 6;
            this.Average.Name = "Average";
            this.Average.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 6;
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            // 
            // Action
            // 
            this.Action.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(93)))), ((int)(((byte)(158)))));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Action.DefaultCellStyle = dataGridViewCellStyle5;
            this.Action.HeaderText = "Action";
            this.Action.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.Action.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(144)))), ((int)(((byte)(255)))));
            this.Action.MinimumWidth = 6;
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.Text = "Add grades";
            this.Action.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // dtgSHSGrades
            // 
            this.dtgSHSGrades.AllowUserToAddRows = false;
            this.dtgSHSGrades.AllowUserToDeleteRows = false;
            this.dtgSHSGrades.AllowUserToResizeColumns = false;
            this.dtgSHSGrades.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            this.dtgSHSGrades.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dtgSHSGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgSHSGrades.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSHSGrades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dtgSHSGrades.ColumnHeadersHeight = 50;
            this.dtgSHSGrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDSHS,
            this.SubjectSHS,
            this.Semester,
            this.FirstSHS,
            this.SecondSHS,
            this.AverageSHS,
            this.RemarksSHS,
            this.ActionSHS});
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgSHSGrades.DefaultCellStyle = dataGridViewCellStyle13;
            this.dtgSHSGrades.EnableHeadersVisualStyles = true;
            this.dtgSHSGrades.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHSGrades.Location = new System.Drawing.Point(19, 127);
            this.dtgSHSGrades.Name = "dtgSHSGrades";
            this.dtgSHSGrades.ReadOnly = true;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgSHSGrades.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dtgSHSGrades.RowHeadersVisible = false;
            this.dtgSHSGrades.RowHeadersWidth = 51;
            this.dtgSHSGrades.RowTemplate.Height = 45;
            this.dtgSHSGrades.Size = new System.Drawing.Size(842, 433);
            this.dtgSHSGrades.TabIndex = 7;
            this.dtgSHSGrades.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHSGrades.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgSHSGrades.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgSHSGrades.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgSHSGrades.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgSHSGrades.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHSGrades.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgSHSGrades.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgSHSGrades.ThemeStyle.ReadOnly = true;
            this.dtgSHSGrades.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgSHSGrades.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgSHSGrades.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgSHSGrades.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSHSGrades.ThemeStyle.RowsStyle.Height = 45;
            this.dtgSHSGrades.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgSHSGrades.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgSHSGrades.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgSHSGrades_CellContentClick);
            // 
            // IDSHS
            // 
            this.IDSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDSHS.DefaultCellStyle = dataGridViewCellStyle10;
            this.IDSHS.FillWeight = 80F;
            this.IDSHS.HeaderText = "ID";
            this.IDSHS.MinimumWidth = 6;
            this.IDSHS.Name = "IDSHS";
            this.IDSHS.ReadOnly = true;
            this.IDSHS.Visible = false;
            // 
            // SubjectSHS
            // 
            this.SubjectSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubjectSHS.DefaultCellStyle = dataGridViewCellStyle11;
            this.SubjectSHS.FillWeight = 80F;
            this.SubjectSHS.HeaderText = "Subject";
            this.SubjectSHS.MinimumWidth = 6;
            this.SubjectSHS.Name = "SubjectSHS";
            this.SubjectSHS.ReadOnly = true;
            // 
            // Semester
            // 
            this.Semester.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Semester.HeaderText = "Semester";
            this.Semester.MinimumWidth = 6;
            this.Semester.Name = "Semester";
            this.Semester.ReadOnly = true;
            this.Semester.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Semester.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FirstSHS
            // 
            this.FirstSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FirstSHS.FillWeight = 60F;
            this.FirstSHS.HeaderText = "First";
            this.FirstSHS.MinimumWidth = 6;
            this.FirstSHS.Name = "FirstSHS";
            this.FirstSHS.ReadOnly = true;
            // 
            // SecondSHS
            // 
            this.SecondSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SecondSHS.FillWeight = 50F;
            this.SecondSHS.HeaderText = "Second";
            this.SecondSHS.MinimumWidth = 6;
            this.SecondSHS.Name = "SecondSHS";
            this.SecondSHS.ReadOnly = true;
            // 
            // AverageSHS
            // 
            this.AverageSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AverageSHS.HeaderText = "Average";
            this.AverageSHS.MinimumWidth = 6;
            this.AverageSHS.Name = "AverageSHS";
            this.AverageSHS.ReadOnly = true;
            // 
            // RemarksSHS
            // 
            this.RemarksSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RemarksSHS.HeaderText = "Remarks";
            this.RemarksSHS.MinimumWidth = 6;
            this.RemarksSHS.Name = "RemarksSHS";
            this.RemarksSHS.ReadOnly = true;
            // 
            // ActionSHS
            // 
            this.ActionSHS.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(93)))), ((int)(((byte)(158)))));
            this.ActionSHS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionSHS.DefaultCellStyle = dataGridViewCellStyle12;
            this.ActionSHS.HeaderText = "Action";
            this.ActionSHS.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.ActionSHS.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(144)))), ((int)(((byte)(255)))));
            this.ActionSHS.MinimumWidth = 6;
            this.ActionSHS.Name = "ActionSHS";
            this.ActionSHS.ReadOnly = true;
            this.ActionSHS.Text = "Add grades";
            this.ActionSHS.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // cbSemester
            // 
            this.cbSemester.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSemester.BackColor = System.Drawing.Color.Transparent;
            this.cbSemester.BorderRadius = 17;
            this.cbSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSemester.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbSemester.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbSemester.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbSemester.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbSemester.ItemHeight = 30;
            this.cbSemester.Items.AddRange(new object[] {
            "1st Semester",
            "2nd Semester"});
            this.cbSemester.Location = new System.Drawing.Point(721, 75);
            this.cbSemester.Name = "cbSemester";
            this.cbSemester.Size = new System.Drawing.Size(140, 36);
            this.cbSemester.TabIndex = 232;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(63)))), ((int)(((byte)(75)))));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(19, 84);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(182, 27);
            this.guna2HtmlLabel1.TabIndex = 233;
            this.guna2HtmlLabel1.Text = "Student Subjects";
            // 
            // lblname
            // 
            this.lblname.BackColor = System.Drawing.Color.Transparent;
            this.lblname.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(63)))), ((int)(((byte)(75)))));
            this.lblname.Location = new System.Drawing.Point(19, 19);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(84, 27);
            this.lblname.TabIndex = 231;
            this.lblname.Text = "Student";
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this.dtgGrades;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 20;
            this.guna2Elipse2.TargetControl = this.dtgSHSGrades;
            // 
            // Gradingform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbSemester);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.lblname);
            this.Controls.Add(this.dtgSHSGrades);
            this.Controls.Add(this.dtgGrades);
            this.Name = "Gradingform";
            this.Size = new System.Drawing.Size(871, 601);
            this.Load += new System.EventHandler(this.Gradingform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgGrades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSHSGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dtgGrades;
        private Guna.UI2.WinForms.Guna2DataGridView dtgSHSGrades;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn First;
        private System.Windows.Forms.DataGridViewTextBoxColumn Second;
        private System.Windows.Forms.DataGridViewTextBoxColumn Third;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fourth;
        private System.Windows.Forms.DataGridViewTextBoxColumn Average;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewLinkColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDSHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectSHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Semester;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstSHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondSHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageSHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemarksSHS;
        private System.Windows.Forms.DataGridViewLinkColumn ActionSHS;
        private Guna.UI2.WinForms.Guna2ComboBox cbSemester;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblname;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
    }
}
