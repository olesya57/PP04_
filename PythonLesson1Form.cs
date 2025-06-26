using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Learning___Program
{

    /// Форма для урока по Pascal: "Переменные и ввод данных"

    public partial class PythonLesson1Form : Form
    {
        // Результаты прохождения урока
        public int Score { get; private set; }    // Процент правильных ответов (0-100%)
        public bool IsCompleted { get; private set; } // Флаг завершения урока

        // Приватные поля для работы с данными
        private int _userId;      // ID пользователя из таблицы Users
        private int _lessonId;    // ID урока из таблицы Lessons
        private string _username; // Имя пользователя для идентификации
        private MainForm _mainForm; // Ссылка на главную форму для обновления прогресса

        // Счетчики прогресса
        private int correctAnswersCount = 0;         // Количество правильных ответов
        private const int TotalQuestions = 4;        // Всего вопросов в уроке
        private const int TotalPracticeTasks = 3;    // Всего практических заданий
        private const int TotalAdvancedTasks = 2;    // Всего дополнительных заданий
        private int completedQuestions = 0;          // Выполнено вопросов
        private int completedPractice = 0;           // Выполнено практических заданий
        private int completedAdvanced = 0;           // Выполнено дополнительных заданий

        /// Конструктор формы. Инициализирует данные пользователя и настраивает интерфейс.

        public PythonLesson1Form(string username, MainForm mainForm)
        {
            InitializeComponent(); // Инициализация компонентов формы (автогенерируемый код)

            // Сохраняем переданные данные
            _username = username;
            _mainForm = mainForm;

            // Получаем ID пользователя и урока из базы данных
            _userId = GetUserId(_username);
            _lessonId = GetLessonId("Основы программирования: переменные", "Python");

            // Настройка интерфейса
            SetupTheoryTab();     // Инициализация вкладки с теорией
            SetupQuestionsTab();  // Настройка вопросов
            SetupPracticeTab();   // Настройка практических заданий
            SetupAdvancedTab();  // Настройка дополнительных заданий
        }


        /// Получает UserID из базы данных по имени пользователя


        private int GetUserId(string username)
        {
            // SQL-запрос с параметром для защиты от инъекций
            string query = "SELECT UserID FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = { new SqlParameter("@Username", username) };

            // Выполнение запроса через вспомогательный класс
            object result = DatabaseHelper.ExecuteScalar(query, parameters);

            return result != null ? Convert.ToInt32(result) : -1;
        }


        /// Получает LessonID из базы данных по названию и языку

        public static int GetLessonId(string lessonName, string language)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open(); // Открываем соединение

                    // Параметризованный запрос для поиска урока
                    string query = @"SELECT LessonID FROM Lessons 
                                   WHERE LessonName = @LessonName 
                                   AND Language = @Language";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Добавляем параметры для безопасной подстановки значений
                        cmd.Parameters.AddWithValue("@LessonName", lessonName);
                        cmd.Parameters.AddWithValue("@Language", language);

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок с выводом сообщения пользователю
                MessageBox.Show($"Ошибка получения ID урока: {ex.Message}");
                return -1;
            }
        }


        /// Сохраняет прогресс пользователя в базу данных

        private void SaveProgress(int score)
        {
            try
            {
                // Используем MERGE для атомарного обновления или вставки записи
                string query = @"
        MERGE UserProgress AS target
        USING (VALUES (@userId, @lessonId)) AS source (UserID, LessonID)
        ON target.UserID = source.UserID AND target.LessonID = source.LessonID
        WHEN MATCHED THEN
            UPDATE SET 
                IsCompleted = @completed,
                Score = @score,
                LastUpdated = GETDATE(),
                CompletedDate = CASE WHEN @completed = 1 THEN GETDATE() ELSE CompletedDate END
        WHEN NOT MATCHED THEN
            INSERT (UserID, LessonID, IsCompleted, Score, LastUpdated, CompletedDate)
            VALUES (@userId, @lessonId, @completed, @score, GETDATE(), 
                   CASE WHEN @completed = 1 THEN GETDATE() ELSE NULL END);";

                // Параметры запроса
                SqlParameter[] parameters = {
            new SqlParameter("@userId", _userId),
            new SqlParameter("@lessonId", _lessonId),
            new SqlParameter("@completed", IsCompleted),
            new SqlParameter("@score", score)
        };

                // Выполнение запроса
                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        /// Настраивает вкладку с теоретическим материалом

        private void SetupTheoryTab()
        {
            exampleCodeTextBox.ReadOnly = true;
        }

        /// Настраивает вкладку с тестовыми вопросами

        private void SetupQuestionsTab()
        {
            // Вопрос 1
            question1GroupBox.Text = "1. Для чего компьютеры используют переменные?";
            answer1RadioButton1.Text = "Для выполнения математических операций";
            answer1RadioButton2.Text = "Для хранения информации";
            answer1RadioButton3.Text = "Для создания графики";
            answer1RadioButton4.Text = "Для подключения к интернету";

            checkQuestion1Button.Click += (s, e) => {
                if (CheckAnswer(1, new[] { answer1RadioButton1, answer1RadioButton2, answer1RadioButton3, answer1RadioButton4 }, 1))
                {
                    correctAnswersCount++;
                }
                checkQuestion1Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 2
            question2GroupBox.Text = "2. Какое значение у переменной name = \"Karin\"?";
            answer2RadioButton1.Text = "name";
            answer2RadioButton2.Text = "\"Karin\"";
            answer2RadioButton3.Text = "Karin";
            answer2RadioButton4.Text = "=";

            checkQuestion2Button.Click += (s, e) => {
                if (CheckAnswer(2, new[] { answer2RadioButton1, answer2RadioButton2, answer2RadioButton3, answer2RadioButton4 }, 1))
                {
                    correctAnswersCount++;
                }
                checkQuestion2Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 3
            question3GroupBox.Text = "3. Какое имя у переменной hobby = \"Tree shaping\"?";
            answer3RadioButton1.Text = "hobby";
            answer3RadioButton2.Text = "\"Tree shaping\"";
            answer3RadioButton3.Text = "Tree";
            answer3RadioButton4.Text = "shaping";

            checkQuestion3Button.Click += (s, e) => {
                if (CheckAnswer(3, new[] { answer3RadioButton1, answer3RadioButton2, answer3RadioButton3, answer3RadioButton4 }, 0))
                {
                    correctAnswersCount++;
                }
                checkQuestion3Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 4
            question4GroupBox.Text = "4. Что делает этот код: song_name = \"Hey Ya!\"?";
            answer4RadioButton1.Text = "Сравнивает song_name с \"Hey Ya!\"";
            answer4RadioButton2.Text = "Переменная хранит значение \"Hey Ya!\"";
            answer4RadioButton3.Text = "Удаляет переменную";
            answer4RadioButton4.Text = "Выводит на экран";

            checkQuestion4Button.Click += (s, e) => {
                if (CheckAnswer(4, new[] { answer4RadioButton1, answer4RadioButton2, answer4RadioButton3, answer4RadioButton4 }, 1))
                {
                    correctAnswersCount++;
                }
                checkQuestion4Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };
        }



        /// Проверяет правильность ответа на вопрос

        private bool CheckAnswer(int questionNumber, RadioButton[] radioButtons, int correctIndex)
        {
            // Проверяем выбранный вариант
            for (int i = 0; i < radioButtons.Length; i++)
            {
                if (radioButtons[i].Checked)
                {
                    if (i == correctIndex)
                    {
                        MessageBox.Show("Правильно!", $"Вопрос {questionNumber}");
                        return true;
                    }
                    else
                    {
                        // Показываем правильный ответ при ошибке
                        MessageBox.Show($"Неправильно. Правильный ответ: {radioButtons[correctIndex].Text}",
                                        $"Вопрос {questionNumber}");
                        return false;
                    }
                }
            }
            return false; // Если ни один вариант не выбран
        }


        /// Настраивает вкладку с практическими заданиями

        private void SetupPracticeTab()
        {
            // Задание 1
            practiceTaskLabel.Text = "1. Создай переменную job и присвой ей значение \"Plumber\".";
            checkPracticeButton.Click += (s, e) => {
                if (CheckPracticeAnswer(practiceCodeTextBox.Text, "job = \"Plumber\""))
                {
                    correctAnswersCount++;
                }
                practiceCodeTextBox.ReadOnly = true;
                checkPracticeButton.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };

            // Задание 2
            task2Label.Text = "2. Напишите программу, которая:\n- Создаёт переменную city со значением \"Miami\"\n- Выводит её на экран";
            checkTask2Button.Click += (s, e) => {
                if (CheckPracticeAnswer(task2CodeBox.Text, "city = \"Miami\""))
                {
                    correctAnswersCount++;
                }
                task2CodeBox.ReadOnly = true;
                checkTask2Button.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };

            // Задание 3
            task3Label.Text = "3. Объясните, что делает этот код:\nname = \"Mario\"\njob = \"Plumber\"\nprint(name, \"is a\", job)";
            checkTask3Button.Click += (s, e) => {
                if (CheckPracticeExplanation(task3ExplanationBox.Text))
                {
                    correctAnswersCount++;
                }
                task3ExplanationBox.ReadOnly = true;
                checkTask3Button.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };
        }
        /// Проверяет практическое задание по ключевым словам


        private bool CheckPracticeAnswer(string userCode, string correctAnswer)
        {
            userCode = userCode.Trim();
            if (userCode.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase) ||
                userCode.Equals(correctAnswer.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                MessageBox.Show($"Неправильно. Пример решения:\n{correctAnswer}", "Результат");
                return false;
            }
        }

        /// Проверяет программу расчета НДС

        private bool CheckPracticeExplanation(string explanation)
        {
            explanation = explanation.Trim();
            if (!string.IsNullOrEmpty(explanation) &&
                (explanation.Contains("создаёт") || explanation.Contains("создает")) &&
                (explanation.Contains("переменные") || explanation.Contains("переменная")) &&
                explanation.Contains("выводит"))
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                MessageBox.Show("Неправильно. Пример объяснения:\nПрограмма создаёт две переменные и выводит результат", "Результат");
                return false;
            }
        }

        /// Настраивает вкладку с дополнительными заданиями

        private void SetupAdvancedTab()
        {
            // Доп. задание 1
            advancedTask1Group.Text = "1. Напишите программу, которая:\n- Создаёт переменную 'movie'\n- Создаёт переменную 'rating'\n- Выводит информацию";
            checkTask1Button.Click += (s, e) => {
                if (CheckAdvancedTask(advancedCodeTextBox1.Text, "movie", "rating"))
                {
                    correctAnswersCount++;
                }
                advancedCodeTextBox1.ReadOnly = true;
                checkTask1Button.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };

            // Доп. задание 2
            advancedTask2Group.Text = "2. Создайте программу для расчёта площади прямоугольника:\n- length = 5\n- width = 3\n- Выведите площадь";
            checkTask2Button1.Click += (s, e) => {
                if (CheckAdvancedTask(advancedCodeTextBox2.Text, "length = 5", "width = 3", "area = length * width"))
                {
                    correctAnswersCount++;
                }
                advancedCodeTextBox2.ReadOnly = true;
                checkTask2Button1.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };

        }


        /// Проверяет программу конвертера валют

        private bool CheckAdvancedTask(string userCode, params string[] requiredParts)
        {
            userCode = userCode.Trim();
            foreach (var part in requiredParts)
            {
                if (!userCode.Contains(part))
                {
                    MessageBox.Show($"Неправильно. В решении должно быть: {string.Join(", ", requiredParts)}", "Результат");
                    return false;
                }
            }
            MessageBox.Show("Правильно!", "Результат");
            return true;
        }


        /// Обновляет статус выполнения всех заданий

        private void UpdateCompletionStatus()
        {
            // Считаем общее количество выполненных заданий
            int totalCompleted = completedQuestions + completedPractice + completedAdvanced;

        }


        /// Обработчик завершения урока

        private void finishButton_Click(object sender, EventArgs e)
        {
            // Рассчитываем процент правильных ответов
            int totalTasks = TotalQuestions + TotalPracticeTasks + TotalAdvancedTasks;
            int incorrectAnswers = totalTasks - correctAnswersCount;
            int percentage = (int)Math.Round((double)correctAnswersCount / totalTasks * 100);

            // Устанавливаем флаг завершения (если >= 0%)
            IsCompleted = percentage >= 0;
            Score = percentage;

            // Сохраняем прогресс в базе данных
            SaveProgress(Score);

            // Обновляем данные в главной форме
            if (_mainForm != null)
            {
                _mainForm.LoadUserProgress();
            }

            // Формируем подробное сообщение
            string resultMessage = $@"Результаты тестирования:

Правильных ответов: {correctAnswersCount} из {totalTasks}
Неправильных ответов: {incorrectAnswers}
Процент выполнения: {percentage}%";

            MessageBox.Show(resultMessage, "Результат урока");
            this.Close();
        }
    }
}