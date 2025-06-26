using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Learning___Program
{
    public partial class MainForm : Form
    {
        public string Username { get; }
        public string UserRole { get; }
        private readonly int _userId;

        private readonly Dictionary<string, string[]> CourseLessons = new Dictionary<string, string[]>
        {
            { "Python", new[]
                {
                    "[Python] Основы программирования: переменные",
                    "[Python] Условные операторы (if, else)",
                    "[Python] Циклы (for, while)"
                }
            },
            { "Pascal", new[]
                {
                    "[Pascal] Структура программы",
                    "[Pascal] Переменные и ввод данных",
                    "[Pascal] Условные операторы (if, else)"
                }
            }
        };

        public MainForm(string username, string role)
        {
            InitializeComponent();
            Username = username;
            UserRole = role;
            _userId = GetUserId(username);

            InitializeUI();
            InitializeDatabaseLessons();
            LoadUserProgress();
            if (role == "Учитель")
            {
                var teacherMenu = new ToolStripMenuItem("Панель учителя");
                menuStrip1.Items.Add(teacherMenu);
                teacherMenu.Click += (s, e) =>
                {
                    new TeacherForm().Show();
                    this.Hide();
                };
            }
            }

        private void InitializeUI()
        {
            lblWelcome.Text = $"{Username} ({UserRole})";

            ConfigureListView(listViewPython);
            ConfigureListView(listViewPascal);

            LoadLessonsToListView(listViewPython, CourseLessons["Python"]);
            LoadLessonsToListView(listViewPascal, CourseLessons["Pascal"]);

            listViewPython.DoubleClick += ListViewPython_DoubleClick;
            listViewPascal.DoubleClick += ListViewPascal_DoubleClick;
            btnRefresh.Click += btnRefresh_Click;
            //   btnLogout.Click += btnLogout_Click;
            // Обновляем статистику при загрузке
            UpdateProgress();
        }
        //
        private void UpdateProgress()
        {
            // Обновление статистики Python
            int pythonCompleted = CountCompletedLessons(listViewPython);
            lblPythonStats.Text = $"Python: {pythonCompleted}/{listViewPython.Items.Count} уроков\n" +
                                $"Последний вход: {DateTime.Now:dd.MM.yyyy}";

            // Обновление статистики Pascal
            int pascalCompleted = CountCompletedLessons(listViewPascal);
            lblPascalStats.Text = $"Pascal: {pascalCompleted}/{listViewPascal.Items.Count} уроков\n" +
                                $"Последний вход: {DateTime.Now:dd.MM.yyyy}";
        }

        private int CountCompletedLessons(ListView listView)
        {
            int completed = 0;
            foreach (ListViewItem item in listView.Items)
                if (item.Text == "✔️") completed++;
            return completed;
        }
        //
        private void ConfigureListView(ListView listView)
        {
            listView.View = View.Details;
            listView.Columns.Clear();
            listView.Columns.AddRange(new[]
            {
                new ColumnHeader { Text = "Статус", Width = 70 },
                new ColumnHeader { Text = "Урок", Width = 300 },
                new ColumnHeader { Text = "Прогресс", Width = 150 }
            });
            listView.FullRowSelect = true;
        }

        private void LoadLessonsToListView(ListView listView, string[] lessons)
        {
            listView.Items.Clear();
            foreach (var lesson in lessons)
            {
                var item = new ListViewItem(new[] { "✖", lesson, "Не начат" })
                {
                    BackColor = Color.White
                };
                listView.Items.Add(item);
            }
        }

        private void InitializeDatabaseLessons()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    InsertLessonIfNotExists(connection, "Основы программирования: переменные", "Python");
                    InsertLessonIfNotExists(connection, "Условные операторы (if, else)", "Python");
                    InsertLessonIfNotExists(connection, "Циклы (for, while)", "Python");

                    InsertLessonIfNotExists(connection, "Структура программы", "Pascal");
                    InsertLessonIfNotExists(connection, "Переменные и ввод данных", "Pascal");
                    InsertLessonIfNotExists(connection, "Условные операторы (if, else)", "Pascal");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка инициализации уроков: " + ex.Message);
            }
        }

        private void InsertLessonIfNotExists(SqlConnection connection, string lessonName, string language)
        {
            string query = @"
        IF NOT EXISTS (
            SELECT 1 
            FROM Lessons 
            WHERE LessonName = @name 
              AND Language = @lang
        ) 
        INSERT INTO Lessons (LessonName, Language) 
        VALUES (@name, @lang)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@name", lessonName);
                cmd.Parameters.AddWithValue("@lang", language); // Добавлен параметр языка
                cmd.ExecuteNonQuery();
            }
        }

        // 1. Измените SQL-запрос в методе LoadUserProgress()
        public void LoadUserProgress()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    // Используем LEFT JOIN вместо обычного JOIN
                    string query = @"SELECT 
                l.LessonName, 
                l.Language, 
                ISNULL(up.IsCompleted, 0) AS IsCompleted,
                ISNULL(up.Score, 0) AS Score
            FROM Lessons l
            LEFT JOIN UserProgress up 
                ON l.LessonID = up.LessonID 
                AND up.UserID = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", _userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Очищаем текущий прогресс перед обновлением
                            ResetListViewProgress(listViewPython);
                            ResetListViewProgress(listViewPascal);

                            while (reader.Read())
                            {
                                string lessonName = reader["LessonName"].ToString();
                                string language = reader["Language"].ToString();
                                bool isCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                int score = Convert.ToInt32(reader["Score"]);

                                UpdateLessonDisplay(lessonName, language, isCompleted, score);
                            }
                        }
                    }
                    UpdateProgress();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки прогресса: " + ex.Message);
            }
        }
        // 2. Добавьте метод сброса визуального прогресса
        private void ResetListViewProgress(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Text = "✖";
                item.SubItems[2].Text = "Не завершён";
                item.BackColor = Color.White;
            }
        }

        // 3. Обновите метод UpdateLessonDisplay
        private void UpdateLessonDisplay(string lessonName, string language, bool isCompleted, int score)
        {
            ListView targetListView = language == "Python" ? listViewPython : listViewPascal;

            foreach (ListViewItem item in targetListView.Items)
            {
                if (item.SubItems[1].Text.Contains(lessonName))
                {
                    // Обновляем статус даже если нет записи в UserProgress
                    bool hasProgress = isCompleted || score > 0;

                    item.Text = hasProgress ? "✔️" : "✖️";
                    item.SubItems[2].Text = hasProgress ? $"Оценка: {score}%" : "Не завершён";
                    item.BackColor = hasProgress ? Color.LightGreen : Color.White;
                    break;
                }
            }
        }

        private void ListViewPython_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedLesson(listViewPython);
        }

        private void ListViewPascal_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedLesson(listViewPascal);

        }

        private void OpenSelectedLesson(ListView listView)
        {
            if (listView.SelectedItems.Count == 0) return;

            string lessonName = listView.SelectedItems[0].SubItems[1].Text;
            Form lessonForm = null;

            if (lessonName.StartsWith("[Python]"))
            {
                switch (lessonName)
                {
                    case "[Python] Основы программирования: переменные":
                        lessonForm = new PythonLesson1Form(Username, this);
                        break;
                    case "[Python] Условные операторы (if, else)":
                        lessonForm = new PythonLesson2Form(Username, this);
                        break;
                    case "[Python] Циклы (for, while)":
                        lessonForm = new PythonLesson3Form(Username, this);
                        break;
                }
            }
            else if (lessonName.StartsWith("[Pascal]"))
            {
                switch (lessonName)
                {
                    case "[Pascal] Структура программы":
                        lessonForm = new PascalLesson1Form(Username, this);
                        break;

                    case "[Pascal] Переменные и ввод данных":
                        lessonForm = new PascalLesson2Form(Username, this);
                        break;
                    case "[Pascal] Условные операторы (if, else)":
                        lessonForm = new PascalLesson3Form(Username, this);
                        break;
                }
            }

            if (lessonForm != null)
            {
                lessonForm.ShowDialog();
                LoadUserProgress(); // Обновляем прогресс после закрытия урока
            }
        }

        private int GetUserId(string username)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT UserID FROM Users WHERE Username = @username", connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUserProgress();
            UpdateProgress(); // Добавляем обновление статистики
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выйти из аккаунта?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close(); // Закрываем текущую форму
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }

        private void pascalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlCourses.SelectedTab = tabPagePascal;
        }

        private void pythonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlCourses.SelectedTab = tabPagePython;
        }

        private void сменитьТемуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Реализация смены темы
            MessageBox.Show("Функция смены темы будет реализована в следующей версии");
        }

        private void языкИнтерфейсаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Реализация смены языка
            MessageBox.Show("Функция смены языка будет реализована в следующей версии");
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Справка будет доступна потом", "Справка");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Обучающая платформа v1.0\n© 2025 ", "О программе");
        }

        private void btnResetProgress_Click(object sender, EventArgs e)
        {
            var currentTab = tabControlCourses.SelectedTab;
            ListView currentListView = currentTab == tabPagePython ? listViewPython : listViewPascal;

            if (currentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите урок для снятия отметки");
                return;
            }

            var selectedItem = currentListView.SelectedItems[0];
            string lessonName = selectedItem.SubItems[1].Text.Replace("[Python] ", "").Replace("[Pascal] ", "");
            string language = currentTab == tabPagePython ? "Python" : "Pascal";

            if (MessageBox.Show($"Снять отметку с урока '{lessonName}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetLessonProgress(lessonName, language);
                LoadUserProgress();
                UpdateProgress();
            }

        }
        // 4. Измените метод ResetLessonProgress
        private void ResetLessonProgress(string lessonName, string language)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                DELETE FROM UserProgress
                WHERE UserID = @userId
                AND LessonID IN (
                    SELECT LessonID 
                    FROM Lessons 
                    WHERE LessonName = @lessonName 
                    AND Language = @language
                )";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userId", _userId);
                        cmd.Parameters.AddWithValue("@lessonName", lessonName);
                        cmd.Parameters.AddWithValue("@language", language);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Определяем нужный ListView на основе языка
                            ListView targetListView = language == "Python"
                                ? listViewPython
                                : listViewPascal;

                            // Обновляем данные и интерфейс
                            LoadUserProgress();
                            UpdateProgress();
                            targetListView.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сброса прогресса: {ex.Message}");
            }
        }
    }
}