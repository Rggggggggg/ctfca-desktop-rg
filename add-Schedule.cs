using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class add_Schedule : Form
    {
        private string editingSchedId = null;
        private bool isEditMode = false;

        public add_Schedule(string editingSchedId)
        {
            InitializeComponent();
            this.editingSchedId = editingSchedId;
            this.isEditMode = !string.IsNullOrEmpty(editingSchedId);
        }

        private void add_Schedule_Load(object sender, EventArgs e)
        {
            LoadInstructors();
            cbInstructor.SelectedIndexChanged += cbInstructor_SelectedIndexChanged;
            cbGradeLevel.SelectedIndexChanged += cbGradeLevel_SelectedIndexChanged;

            // If in edit mode, load the existing schedule data
            if (isEditMode)
            {
                LoadScheduleForEdit();
                // Change form title to indicate edit mode
                lblSchedule.Text = "Edit Schedule";
                this.Text = "Edit Schedule";
                btnSave.Text = "Update";
            }
            else
            {
                this.Text = "Add New Schedule";
                btnSave.Text = "Save";
            }
        }

        private void LoadScheduleForEdit()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT 
                                    instructor,
                                    grade_level,
                                    section,
                                    school_year,
                                    semester,
                                    subject,
                                    days,
                                    start_time,
                                    end_time,
                                    room
                                FROM instructor_schedule 
                                WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", editingSchedId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Store the values we need to set
                            string instructorName = reader["instructor"].ToString();
                            string gradeLevel = reader["grade_level"].ToString();
                            string section = reader["section"].ToString();
                            string schoolYear = reader["school_year"].ToString();
                            string semester = reader["semester"].ToString();
                            string subject = reader["subject"].ToString();
                            string days = reader["days"].ToString();
                            string startTime = reader["start_time"].ToString();
                            string endTime = reader["end_time"].ToString();
                            string room = reader["room"].ToString();

                            // Set instructor first - this will populate grade levels
                            SetInstructorByName(instructorName);

                            // Wait a moment for the instructor change to complete, then set grade level
                            // This will trigger the grade level change event and populate subjects
                            SetGradeLevelByText(gradeLevel);

                            // Set other fields
                            tbSection.Text = section;
                            cbSchoolYear.Text = schoolYear;

                            // Set semester
                            if (semester != "N/A")
                            {
                                cbSemester.Enabled = true;
                                cbSemester.Text = semester;
                            }
                            else
                            {
                                cbSemester.Enabled = false;
                                cbSemester.SelectedIndex = -1;
                            }

                            // Set subject after grade level is set
                            SetSubjectByText(subject);

                            // Set days
                            SetSelectedDays(days);

                            // Set times
                            tbStart.Text = startTime;
                            tbEnd.Text = endTime;

                            // Set room
                            tbRoom.Text = room;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading schedule for edit: " + ex.Message, "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SetInstructorByName(string instructorName)
        {
            for (int i = 0; i < cbInstructor.Items.Count; i++)
            {
                DataRowView row = cbInstructor.Items[i] as DataRowView;
                if (row != null && row["fullname"].ToString() == instructorName)
                {
                    cbInstructor.SelectedIndex = i;
                    break;
                }
            }
        }
        private void SetGradeLevelByText(string gradeLevelText)
        {
            if (cbGradeLevel.DataSource != null)
            {
                var items = cbGradeLevel.DataSource as List<string>;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == gradeLevelText)
                    {
                        cbGradeLevel.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        private void SetSubjectByText(string subjectText)
        {
            if (cbSubject.DataSource != null)
            {
                var items = cbSubject.DataSource as List<string>;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == subjectText)
                    {
                        cbSubject.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        private void SetSelectedDays(string daysString)
        {
            // Clear all checkboxes first
            checkMonday.Checked = false;
            CheckTuesday.Checked = false;
            CheckWednesday.Checked = false;
            CheckThursday.Checked = false;
            CheckFriday.Checked = false;
            CheckSaturday.Checked = false;
            CheckSunday.Checked = false;

            if (!string.IsNullOrEmpty(daysString))
            {
                string[] days = daysString.Split(',');
                foreach (string day in days)
                {
                    string trimmedDay = day.Trim();
                    switch (trimmedDay)
                    {
                        case "Monday":
                            checkMonday.Checked = true;
                            break;
                        case "Tuesday":
                            CheckTuesday.Checked = true;
                            break;
                        case "Wednesday":
                            CheckWednesday.Checked = true;
                            break;
                        case "Thursday":
                            CheckThursday.Checked = true;
                            break;
                        case "Friday":
                            CheckFriday.Checked = true;
                            break;
                        case "Saturday":
                            CheckSaturday.Checked = true;
                            break;
                        case "Sunday":
                            CheckSunday.Checked = true;
                            break;
                    }
                }
            }
        }

        private void LoadInstructors()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT instructors_id, firstname, middlename, lastname FROM instructor_accounts";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbInstructor.DisplayMember = "fullname";
                    cbInstructor.ValueMember = "instructors_id";

                    dt.Columns.Add("fullname", typeof(string), "firstname + ' ' + middlename + ' ' + lastname");
                    cbInstructor.DataSource = dt;
                    cbInstructor.SelectedIndex = -1; // No selection initially
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading instructors: " + ex.Message);
                }
            }
        }

        private void cbInstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInstructor.SelectedValue == null)
                return;

            string instructorId = cbInstructor.SelectedValue.ToString();
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT subject_assigned, grades_level_handled FROM instructor_accounts WHERE instructors_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", instructorId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string subjects = reader["subject_assigned"].ToString();
                            string gradeLevels = reader["grades_level_handled"].ToString();

                            // Store the subjects for later filtering
                            cbInstructor.Tag = subjects;

                            // Populate grade levels
                            cbGradeLevel.DataSource = gradeLevels.Split(',').Select(g => g.Trim()).ToList();

                            // In edit mode, don't reset the selection
                            if (!isEditMode)
                            {
                                cbGradeLevel.SelectedIndex = -1;
                                // Clear subjects until grade level is selected
                                cbSubject.DataSource = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading instructor data: " + ex.Message);
                }
            }
        }

        private void cbGradeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGradeLevel.SelectedItem == null || cbInstructor.Tag == null)
                return;

            string selectedGradeLevel = cbGradeLevel.SelectedItem.ToString();

            // Enable only for Grade 11 and 12
            if (selectedGradeLevel != "Grade 11 STEM" && selectedGradeLevel != "Grade 11 ABM" && selectedGradeLevel != "Grade 11 HUMSS" && selectedGradeLevel != "Grade 12 STEM" && selectedGradeLevel != "Grade 12 ABM" && selectedGradeLevel != "Grade 12 HUMSS")
            {
                cbSemester.Enabled = false;
                if (!isEditMode) // Only clear if not in edit mode
                {
                    cbSemester.SelectedIndex = -1;
                }
            }
            else
            {
                cbSemester.Enabled = true;
            }

            string instructorSubjects = cbInstructor.Tag.ToString();
            PopulateSubjectsForGradeLevel(instructorSubjects, selectedGradeLevel);
        }

        private void PopulateSubjectsForGradeLevel(string instructorSubjects, string gradeLevel)
        {
            try
            {
                List<string> filteredSubjects = new List<string>();

                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT DISTINCT subject_name 
                                    FROM subjects 
                                    WHERE grade_level = @grade_level 
                                    ORDER BY subject_name";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@grade_level", gradeLevel);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string subjectName = reader["subject_name"].ToString();
                            filteredSubjects.Add(subjectName);
                        }
                    }
                }

                cbSubject.DataSource = filteredSubjects;

                // In edit mode, don't reset the selection
                if (!isEditMode)
                {
                    cbSubject.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error populating subjects: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate form first
            if (!ValidateForm())
                return;

            string conflictDetails;
            if (CheckScheduleConflict(out conflictDetails))
            {
                DialogResult conflictResult = MessageBox.Show(
                    conflictDetails + "\n\nA schedule conflict has been detected with existing schedules. Do you want to continue anyway?",
                    "Schedule Conflict Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (conflictResult == DialogResult.No)
                    return;
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query;
                    if (isEditMode)
                    {
                        // Update existing schedule
                        query = @"UPDATE instructor_schedule SET
                                instructor = @instructor,
                                grade_level = @grade_level,
                                section = @section,
                                school_year = @school_year,
                                semester = @semester,
                                subject = @subject,
                                days = @days,
                                start_time = @start_time,
                                end_time = @end_time,
                                room = @room
                                WHERE id = @id";
                    }
                    else
                    {
                        // Insert new schedule
                        query = @"INSERT INTO instructor_schedule
                                (instructor, grade_level, section, school_year, semester, subject, days, start_time, end_time, room)
                                VALUES
                                (@instructor, @grade_level, @section, @school_year, @semester, @subject, @days, @start_time, @end_time, @room)";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Selected Days
                    List<string> selectedDays = new List<string>();
                    if (checkMonday.Checked) selectedDays.Add("Monday");
                    if (CheckTuesday.Checked) selectedDays.Add("Tuesday");
                    if (CheckWednesday.Checked) selectedDays.Add("Wednesday");
                    if (CheckThursday.Checked) selectedDays.Add("Thursday");
                    if (CheckFriday.Checked) selectedDays.Add("Friday");
                    if (CheckSaturday.Checked) selectedDays.Add("Saturday");
                    if (CheckSunday.Checked) selectedDays.Add("Sunday");

                    DateTime startTime = DateTime.Parse(tbStart.Text);
                    DateTime endTime = DateTime.Parse(tbEnd.Text);

                    cmd.Parameters.AddWithValue("@instructor", cbInstructor.Text);
                    cmd.Parameters.AddWithValue("@grade_level", cbGradeLevel.Text);
                    cmd.Parameters.AddWithValue("@section", tbSection.Text);
                    cmd.Parameters.AddWithValue("@school_year", cbSchoolYear.Text);
                    cmd.Parameters.AddWithValue("@semester", cbSemester.Enabled ? cbSemester.Text : "N/A");
                    cmd.Parameters.AddWithValue("@subject", cbSubject.Text);
                    cmd.Parameters.AddWithValue("@days", string.Join(",", selectedDays));
                    cmd.Parameters.AddWithValue("@start_time", startTime.ToString("hh:mm tt"));
                    cmd.Parameters.AddWithValue("@end_time", endTime.ToString("hh:mm tt"));
                    cmd.Parameters.AddWithValue("@room", tbRoom.Text);

                    // Add ID parameter for update
                    if (isEditMode)
                    {
                        cmd.Parameters.AddWithValue("@id", editingSchedId);
                    }

                    cmd.ExecuteNonQuery();

                    string successMessage = isEditMode ? "Schedule updated successfully!" : "Schedule saved successfully!";
                    MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (Exception ex)
                {
                    string errorMessage = isEditMode ? "Error updating schedule: " : "Error saving schedule: ";
                    MessageBox.Show(errorMessage + ex.Message);
                }
            }
        }

        private int previousStartLength = 0;
        private int previousEndLength = 0;

        private static readonly Regex strictTimeRegex =
            new Regex(@"^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$", RegexOptions.Compiled);

        private void tbStart_TextChanged(object sender, EventArgs e)
        {
            tbStart.TextChanged -= tbStart_TextChanged;

            string original = tbStart.Text;
            string input = original.ToUpper().Replace(" ", "");

            // Force uppercase + strip spaces
            if (original != input)
            {
                tbStart.Text = input;
                tbStart.SelectionStart = input.Length;
            }

            // Allow only digits, colon, A, P, M
            if (!Regex.IsMatch(input, @"^[0-9:APM]*$"))
            {
                tbStart.Text = new string(input.Where(c => "0123456789:APM".Contains(c)).ToArray());
                tbStart.SelectionStart = tbStart.Text.Length;
                tbStart.TextChanged += tbStart_TextChanged;
                return;
            }

            bool isDeleting = tbStart.Text.Length < previousStartLength;

            if (!isDeleting)
            {
                // Case: user typed a single digit
                if (input.Length == 1 && char.IsDigit(input[0]))
                {
                    char first = input[0];

                    if (first >= '3' && first <= '9')
                    {
                        // Auto-pad 3–9 -> 03:, 04:, ...
                        tbStart.Text = "0" + first + ":";
                        tbStart.SelectionStart = tbStart.Text.Length;
                        input = tbStart.Text;
                    }
                    // If 1 or 2 -> wait for next digit (do nothing yet)
                }
                // Case: user typed two digits - handle 12-hour format conversion
                else if (input.Length == 2 && !input.Contains(":"))
                {
                    int hourValue;
                    if (int.TryParse(input, out hourValue))
                    {
                        // Convert 24-hour format to 12-hour format
                        if (hourValue >= 13 && hourValue <= 23)
                        {
                            int convertedHour = hourValue - 12;
                            tbStart.Text = convertedHour.ToString("D2") + ":";
                        }
                        else if (hourValue == 0)
                        {
                            // Convert 00 to 12 (midnight in 12-hour format)
                            tbStart.Text = "12:";
                        }
                        else
                        {
                            // Valid 12-hour format (01-12)
                            tbStart.Text = input + ":";
                        }
                    }
                    else
                    {
                        tbStart.Text = input + ":";
                    }
                    tbStart.SelectionStart = tbStart.Text.Length;
                    input = tbStart.Text;
                }
                // Case: user typed AM/PM partially
                else if (input.Length == 6 && (input.EndsWith("A") || input.EndsWith("P")) &&
                         !input.EndsWith("AM") && !input.EndsWith("PM"))
                {
                    tbStart.Text = input.Insert(5, " ") + "M";
                    tbStart.SelectionStart = tbStart.Text.Length;
                    input = tbStart.Text;
                }
            }

            // Validate strictly when full length reached
            if (tbStart.Text.Length == 8)
            {
                if (!strictTimeRegex.IsMatch(tbStart.Text))
                {
                    MessageBox.Show("Please enter a valid time in hh:mm AM/PM format. ex.(12:30 PM)",
                                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbStart.Text = "";
                    tbStart.SelectionStart = 0;
                    previousStartLength = 0;
                    tbStart.TextChanged += tbStart_TextChanged;
                    return;
                }
            }

            previousStartLength = tbStart.Text.Length;
            tbStart.TextChanged += tbStart_TextChanged;
        }

        private void tbEnd_TextChanged(object sender, EventArgs e)
        {
            tbEnd.TextChanged -= tbEnd_TextChanged;

            string original = tbEnd.Text;
            string input = original.ToUpper().Replace(" ", "");

            if (original != input)
            {
                tbEnd.Text = input;
                tbEnd.SelectionStart = input.Length;
            }

            if (!Regex.IsMatch(input, @"^[0-9:APM]*$"))
            {
                tbEnd.Text = new string(input.Where(c => "0123456789:APM".Contains(c)).ToArray());
                tbEnd.SelectionStart = tbEnd.Text.Length;
                tbEnd.TextChanged += tbEnd_TextChanged;
                return;
            }

            bool isDeleting = tbEnd.Text.Length < previousEndLength;

            if (!isDeleting)
            {
                if (input.Length == 1 && char.IsDigit(input[0]))
                {
                    char first = input[0];

                    if (first >= '3' && first <= '9')
                    {
                        tbEnd.Text = "0" + first + ":";
                        tbEnd.SelectionStart = tbEnd.Text.Length;
                        input = tbEnd.Text;
                    }
                    // Leave 1 and 2 alone (wait for second digit)
                }
                // Case: user typed two digits - handle 12-hour format conversion
                else if (input.Length == 2 && !input.Contains(":"))
                {
                    int hourValue;
                    if (int.TryParse(input, out hourValue))
                    {
                        // Convert 24-hour format to 12-hour format
                        if (hourValue >= 13 && hourValue <= 23)
                        {
                            int convertedHour = hourValue - 12;
                            tbEnd.Text = convertedHour.ToString("D2") + ":";
                        }
                        else if (hourValue == 0)
                        {
                            // Convert 00 to 12 (midnight in 12-hour format)
                            tbEnd.Text = "12:";
                        }
                        else
                        {
                            // Valid 12-hour format (01-12)
                            tbEnd.Text = input + ":";
                        }
                    }
                    else
                    {
                        tbEnd.Text = input + ":";
                    }
                    tbEnd.SelectionStart = tbEnd.Text.Length;
                    input = tbEnd.Text;
                }
                else if (input.Length == 6 && (input.EndsWith("A") || input.EndsWith("P")) &&
                         !input.EndsWith("AM") && !input.EndsWith("PM"))
                {
                    tbEnd.Text = input.Insert(5, " ") + "M";
                    tbEnd.SelectionStart = tbEnd.Text.Length;
                    input = tbEnd.Text;
                }
            }

            if (tbEnd.Text.Length == 8)
            {
                if (!strictTimeRegex.IsMatch(tbEnd.Text))
                {
                    MessageBox.Show("Please enter a valid time in hh:mm AM/PM format. ex.(12:30 PM)",
                                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbEnd.Text = "";
                    tbEnd.SelectionStart = 0;
                    previousEndLength = 0;
                    tbEnd.TextChanged += tbEnd_TextChanged;
                    return;
                }
            }

            previousEndLength = tbEnd.Text.Length;
            tbEnd.TextChanged += tbEnd_TextChanged;
        }




        private bool IsValid12HourFormat(string time)
        {
            return DateTime.TryParseExact(
                time,
                "hh:mm tt",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _
            );
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog before canceling
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel? Any unsaved changes will be lost.",
                "Cancel Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();
               
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog before clearing
            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all fields?",
                "Clear Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Clear all form controls
                ClearAllFields();
            }
        }

        private void ClearAllFields()
        {
            // Clear ComboBoxes
            cbInstructor.SelectedIndex = -1;
            cbInstructor.Tag = null;
            cbGradeLevel.DataSource = null;
            cbGradeLevel.SelectedIndex = -1;
            cbSubject.DataSource = null;
            cbSubject.SelectedIndex = -1;
            cbSchoolYear.SelectedIndex = -1;
            cbSemester.SelectedIndex = -1;
            cbSemester.Enabled = false;

            // Clear TextBoxes
            tbSection.Text = "";
            tbStart.Text = "";
            tbEnd.Text = "";
            tbRoom.Text = "";

            // Clear CheckBoxes (Days)
            checkMonday.Checked = false;
            CheckTuesday.Checked = false;
            CheckWednesday.Checked = false;
            CheckThursday.Checked = false;
            CheckFriday.Checked = false;
            CheckSaturday.Checked = false;
            CheckSunday.Checked = false;

            // Reset text length trackers
            previousStartLength = 0;
            previousEndLength = 0;

            cbInstructor.Select();
        }

        private bool ValidateForm()
        {
            // Validate instructor selection
            if (cbInstructor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an instructor.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbInstructor.Focus();
                return false;
            }

            // Validate grade level selection
            if (cbGradeLevel.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a grade level.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbGradeLevel.Focus();
                return false;
            }

            // Validate subject selection
            if (cbSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSubject.Focus();
                return false;
            }

            // Validate section input
            if (string.IsNullOrWhiteSpace(tbSection.Text))
            {
                MessageBox.Show("Please enter a section.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbSection.Focus();
                return false;
            }

            // Validate school year selection
            if (cbSchoolYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a school year.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSchoolYear.Focus();
                return false;
            }

            // Validate semester for Grade 11 and 12 (if semester is enabled)
            if (cbSemester.Enabled && cbSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a semester.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbSemester.Focus();
                return false;
            }

            // Check if at least one day is selected
            if (!checkMonday.Checked && !CheckTuesday.Checked && !CheckWednesday.Checked &&
                !CheckThursday.Checked && !CheckFriday.Checked && !CheckSaturday.Checked && !CheckSunday.Checked)
            {
                MessageBox.Show("Please select at least one day.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate start time
            if (string.IsNullOrWhiteSpace(tbStart.Text))
            {
                MessageBox.Show("Please enter a start time.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbStart.Focus();
                return false;
            }

            // Validate end time
            if (string.IsNullOrWhiteSpace(tbEnd.Text))
            {
                MessageBox.Show("Please enter an end time.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Validate time format and parsing
            if (!DateTime.TryParse(tbStart.Text, out DateTime startTime))
            {
                MessageBox.Show("Please enter a valid start time in hh:mm AM/PM format.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbStart.Focus();
                return false;
            }

            if (!DateTime.TryParse(tbEnd.Text, out DateTime endTime))
            {
                MessageBox.Show("Please enter a valid end time in hh:mm AM/PM format.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Check if end time is after start time
            if (endTime <= startTime)
            {
                MessageBox.Show("End time must be after start time.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Validate minimum class duration (e.g., at least 30 minutes)
            TimeSpan duration = endTime - startTime;
            if (duration.TotalMinutes < 30)
            {
                MessageBox.Show("Class duration must be at least 30 minutes.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Validate maximum class duration (e.g., not more than 4 hours)
            if (duration.TotalHours > 4)
            {
                MessageBox.Show("Class duration cannot exceed 4 hours.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Validate reasonable time range (e.g., between 6:00 AM and 10:00 PM)
            TimeSpan minTime = new TimeSpan(6, 0, 0);  // 6:00 AM
            TimeSpan maxTime = new TimeSpan(22, 0, 0); // 10:00 PM

            if (startTime.TimeOfDay < minTime || startTime.TimeOfDay > maxTime)
            {
                MessageBox.Show("Start time must be between 6:00 AM and 10:00 PM.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbStart.Focus();
                return false;
            }

            if (endTime.TimeOfDay < minTime || endTime.TimeOfDay > maxTime)
            {
                MessageBox.Show("End time must be between 6:00 AM and 10:00 PM.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbEnd.Focus();
                return false;
            }

            // Validate room input
            if (string.IsNullOrWhiteSpace(tbRoom.Text))
            {
                MessageBox.Show("Please enter a room.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbRoom.Focus();
                return false;
            }

            return true;
        }
        // Helper method to get selected days as a list
        private List<string> GetSelectedDays()
        {
            List<string> selectedDays = new List<string>();

            if (checkMonday.Checked) selectedDays.Add("Monday");
            if (CheckTuesday.Checked) selectedDays.Add("Tuesday");
            if (CheckWednesday.Checked) selectedDays.Add("Wednesday");
            if (CheckThursday.Checked) selectedDays.Add("Thursday");
            if (CheckFriday.Checked) selectedDays.Add("Friday");
            if (CheckSaturday.Checked) selectedDays.Add("Saturday");
            if (CheckSunday.Checked) selectedDays.Add("Sunday");

            return selectedDays;
        }

        private bool CheckScheduleConflict(out string conflictDetails)
        {
            conflictDetails = "";
            List<string> conflicts = new List<string>();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Get selected days
                    List<string> selectedDays = GetSelectedDays();

                    // Parse times
                    DateTime startTime = DateTime.Parse(tbStart.Text.Trim());
                    DateTime endTime = DateTime.Parse(tbEnd.Text.Trim());

                    // Check for conflicts with each selected day
                    foreach (string day in selectedDays)
                    {
                        // Check INSTRUCTOR conflicts
                        string instructorQuery = @"SELECT section, subject, start_time, end_time, room 
                                         FROM instructor_schedule 
                                         WHERE school_year = @school_year
                                         AND instructor = @instructor
                                         AND FIND_IN_SET(@day, REPLACE(days, ' ', '')) > 0
                                         AND (
                                             (TIME(STR_TO_DATE(@start_time, '%h:%i %p')) < TIME(STR_TO_DATE(end_time, '%h:%i %p')) AND 
                                              TIME(STR_TO_DATE(@end_time, '%h:%i %p')) > TIME(STR_TO_DATE(start_time, '%h:%i %p')))
                                         )";

                        if (isEditMode)
                        {
                            instructorQuery += " AND id != @id";
                        }

                        MySqlCommand instructorCmd = new MySqlCommand(instructorQuery, conn);
                        instructorCmd.Parameters.AddWithValue("@instructor", cbInstructor.Text.Trim());
                        instructorCmd.Parameters.AddWithValue("@school_year", cbSchoolYear.Text.Trim());
                        instructorCmd.Parameters.AddWithValue("@day", day);
                        instructorCmd.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm tt"));
                        instructorCmd.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm tt"));

                        if (isEditMode)
                        {
                            instructorCmd.Parameters.AddWithValue("@id", editingSchedId);
                        }

                        using (MySqlDataReader reader = instructorCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                conflicts.Add($"INSTRUCTOR CONFLICT on {day}:\n" +
                                            $"  {cbInstructor.Text} is already teaching {reader["subject"]} " +
                                            $"to {reader["section"]} in {reader["room"]}\n" +
                                            $"  from {reader["start_time"]} to {reader["end_time"]}");
                            }
                        }

                        // Check ROOM conflicts
                        string roomQuery = @"SELECT instructor, section, subject, start_time, end_time 
                                   FROM instructor_schedule 
                                   WHERE school_year = @school_year
                                   AND room = @room
                                   AND FIND_IN_SET(@day, REPLACE(days, ' ', '')) > 0
                                   AND (
                                       (TIME(STR_TO_DATE(@start_time, '%h:%i %p')) < TIME(STR_TO_DATE(end_time, '%h:%i %p')) AND 
                                        TIME(STR_TO_DATE(@end_time, '%h:%i %p')) > TIME(STR_TO_DATE(start_time, '%h:%i %p')))
                                   )";

                        if (isEditMode)
                        {
                            roomQuery += " AND id != @id";
                        }

                        MySqlCommand roomCmd = new MySqlCommand(roomQuery, conn);
                        roomCmd.Parameters.AddWithValue("@room", tbRoom.Text.Trim());
                        roomCmd.Parameters.AddWithValue("@school_year", cbSchoolYear.Text.Trim());
                        roomCmd.Parameters.AddWithValue("@day", day);
                        roomCmd.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm tt"));
                        roomCmd.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm tt"));

                        if (isEditMode)
                        {
                            roomCmd.Parameters.AddWithValue("@id", editingSchedId);
                        }

                        using (MySqlDataReader reader = roomCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                conflicts.Add($"ROOM CONFLICT on {day}:\n" +
                                            $"  Room {tbRoom.Text} is already occupied by {reader["instructor"]}\n" +
                                            $"  teaching {reader["subject"]} to {reader["section"]}\n" +
                                            $"  from {reader["start_time"]} to {reader["end_time"]}");
                            }
                        }
                    }

                    if (conflicts.Any())
                    {
                        conflictDetails = "Schedule conflicts detected:\n\n" + string.Join("\n\n", conflicts);
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    conflictDetails = "Error checking schedule conflicts: " + ex.Message;
                    return true;
                }
            }
        }

        }
}