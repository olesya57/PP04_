using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Learning___Program
{
    public partial class TeacherForm : Form
    {
        public TeacherForm()
        {
            InitializeComponent();
            LoadStudentProgress();
            ConfigureGrid();
        }

        private void LoadStudentProgress()
        {
            try
            {
                string query = @"SELECT 
                    u.Username AS 'Ученик',
                    l.LessonName AS 'Урок',
                    l.Language AS 'Язык',
                    up.Score AS 'Оценка (%)',
                    up.CompletedDate AS 'Дата завершения'
                FROM UserProgress up
                JOIN Users u ON up.UserID = u.UserID
                JOIN Lessons l ON up.LessonID = l.LessonID
                WHERE u.Role = 'Ученик' AND up.IsCompleted = 1
                ORDER BY u.Username, up.CompletedDate DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void ConfigureGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

            // Форматирование столбца с датой
            dataGridView1.Columns["Дата завершения"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudentProgress();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // Реализация экспорта в Excel может быть добавлена позже
            MessageBox.Show("Экспорт в Excel будет реализован в следующей версии");
        }

        // Обработчик кнопки "Назад"
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Возвращаемся к форме 
            Form1 Form1 = new Form1();
            Form1.Show();
            this.Hide();
        }
    }
}