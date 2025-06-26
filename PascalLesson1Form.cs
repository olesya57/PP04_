using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Learning___Program
{

    /// Форма для урока по Pascal: "Переменные и ввод данных"

    public partial class PascalLesson1Form : Form
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
        private const int TotalQuestions = 3;        // Всего вопросов в уроке
        private const int TotalPracticeTasks = 2;    // Всего практических заданий
        private const int TotalAdvancedTasks = 1;    // Всего дополнительных заданий
        private int completedQuestions = 0;          // Выполнено вопросов
        private int completedPractice = 0;           // Выполнено практических заданий
        private int completedAdvanced = 0;           // Выполнено дополнительных заданий

        /// Конструктор формы. Инициализирует данные пользователя и настраивает интерфейс.

        public PascalLesson1Form(string username, MainForm mainForm)
        {
            InitializeComponent(); // Инициализация компонентов формы (автогенерируемый код)

            // Сохраняем переданные данные
            _username = username;
            _mainForm = mainForm;

            // Получаем ID пользователя и урока из базы данных
            _userId = GetUserId(_username);
            _lessonId = GetLessonId("Структура программы", "Pascal");

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
            exampleCodeTextBox.ReadOnly = true; // Запрещаем редактирование
            }

        /// Настраивает вкладку с тестовыми вопросами

        private void SetupQuestionsTab()
        {
            question1GroupBox.Text = "1. Для чего нужна команда writeln?";
            answer1RadioButton1.Text = "Для ввода данных с клавиатуры";
            answer1RadioButton2.Text = "Для вывода текста на экран";
            answer1RadioButton3.Text = "Для математических вычислений";
            answer1RadioButton4.Text = "Для создания переменных";

            checkQuestion1Button.Click += (s, e) =>
            {
                if (CheckAnswer(1, new[] { answer1RadioButton1, answer1RadioButton2, answer1RadioButton3, answer1RadioButton4 }, 1))
                    correctAnswersCount++;

                checkQuestion1Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            question2GroupBox.Text = "2. Что обозначает begin и end. в программе?";
            answer2RadioButton1.Text = "Начало и конец комментария";
            answer2RadioButton2.Text = "Начало и конец программы";
            answer2RadioButton3.Text = "Условия выполнения программы";
            answer2RadioButton4.Text = "Объявление переменных";

            checkQuestion2Button.Click += (s, e) =>
            {
                if (CheckAnswer(2, new[] { answer2RadioButton1, answer2RadioButton2, answer2RadioButton3, answer2RadioButton4 }, 1))
                    correctAnswersCount++;

                checkQuestion2Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            question3GroupBox.Text = "3. Как бы выглядел код, выводящий твоё имя на экран?";
            answer3RadioButton1.Text = "writeln(Моё имя);";
            answer3RadioButton2.Text = "writeln('Моё имя');";
            answer3RadioButton3.Text = "print('Моё имя');";
            answer3RadioButton4.Text = "write('Моё имя')";

            checkQuestion3Button.Click += (s, e) =>
            {
                if (CheckAnswer(3, new[] { answer3RadioButton1, answer3RadioButton2, answer3RadioButton3, answer3RadioButton4 }, 1))
                    correctAnswersCount++;

                checkQuestion3Button.Enabled = false;
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
            practiceTaskLabel.Text = "1. Напиши программу, которая выводит на экран:\n\nМеня зовут [ваше имя].\nЯ начинаю изучать Pascal!";

            checkPracticeButton.Click += (s, e) =>
            {
                if (CheckPracticeAnswer(practiceCodeTextBox.Text, "writeln('Меня зовут"))
                    correctAnswersCount++;

                practiceCodeTextBox.ReadOnly = true;
                checkPracticeButton.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };

            task2Label.Text = "2. Напиши программу, которая рисует прямоугольник из символов *, например:\n\n********\n*      *\n*      *\n********";

            checkTask2Button.Click += (s, e) =>
            {
                if (CheckRectangleAnswer(task2CodeBox.Text))
                    correctAnswersCount++;

                task2CodeBox.ReadOnly = true;
                checkTask2Button.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };
        }

        /// Проверяет практическое задание по ключевым словам


        private bool CheckPracticeAnswer(string userCode, string requiredPart)
        {
            if (userCode.Trim().Contains(requiredPart))
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            MessageBox.Show("Неправильно. Пример решения:\nprogram MyFirstProgram;\nbegin\n  writeln('Меня зовут Алексей.');\n  writeln('Я начинаю изучать Pascal!');\nend.",
                          "Результат");
            return false;
        }

        /// Проверяет 

        private bool CheckRectangleAnswer(string userCode)
        {
            if (userCode.Contains("writeln('********')") && userCode.Contains("writeln('*      *')"))
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            MessageBox.Show("Неправильно. Пример решения:\nprogram Rectangle;\nbegin\n  writeln('********');\n  writeln('*      *');\n  writeln('*      *');\n  writeln('********');\nend.",
                          "Результат");
            return false;
        }

        /// Настраивает вкладку с дополнительными заданиями

        private void SetupAdvancedTab()
        {
            advancedTaskLabel.Text = "Дополнительное задание:\n\nНапишите программу, которая выводит на экран ASCII-арт котика:\n" +
                                   " /\\_/\\\n( 0.0 )\n > ^ <\n\nИ добавьте подпись \"Мой первый ASCII-арт в Pascal!\"";

            checkAdvancedButton.Click += (s, e) =>
            {
                if (CheckAdvancedAnswer(advancedCodeTextBox.Text))
                    correctAnswersCount++;

                advancedCodeTextBox.ReadOnly = true;
                checkAdvancedButton.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };
        }


        /// Проверяет программу 

        private bool CheckAdvancedAnswer(string userCode)
        {
            bool hasCat = userCode.Contains("/\\_/\\") || userCode.Contains("/\\ /\\");
            bool hasSignature = userCode.Contains("Мой первый ASCII-арт в Pascal!");

            if (hasCat && hasSignature)
            {
                MessageBox.Show("Отлично! Вы создали ASCII-арт котика!", "Результат");
                return true;
            }
            MessageBox.Show("Почти получилось! Пример решения:\nprogram CatArt;\nbegin\n  writeln(' /\\_/\\');\n  writeln('( 0.0 )');\n  writeln(' > ^ <');\n  writeln;\n  writeln('Мой первый ASCII-арт в Pascal!');\nend.",
                          "Результат");
            return false;
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