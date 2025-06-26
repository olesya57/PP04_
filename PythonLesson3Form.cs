using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Learning___Program
{

    /// Форма для урока по Pascal: "Переменные и ввод данных"

    public partial class PythonLesson3Form : Form
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
        private const int TotalPracticeTasks = 2;    // Всего практических заданий
        private const int TotalAdvancedTasks = 1;    // Всего дополнительных заданий
        private int completedQuestions = 0;          // Выполнено вопросов
        private int completedPractice = 0;           // Выполнено практических заданий
        private int completedAdvanced = 0;           // Выполнено дополнительных заданий

        /// Конструктор формы. Инициализирует данные пользователя и настраивает интерфейс.

        public PythonLesson3Form(string username, MainForm mainForm)
        {
            InitializeComponent(); // Инициализация компонентов формы (автогенерируемый код)

            // Сохраняем переданные данные
            _username = username;
            _mainForm = mainForm;

            // Получаем ID пользователя и урока из базы данных
            _userId = GetUserId(_username);
            _lessonId = GetLessonId("Циклы (for, while)", "Python");

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

        private void SetupQuestionsTab()
        {
            // Вопрос 1
            question1GroupBox.Text = "1. Какой цикл используется для перебора элементов списка?";
            answer1RadioButton1.Text = "for";
            answer1RadioButton2.Text = "while";
            answer1RadioButton3.Text = "loop";
            answer1RadioButton4.Text = "foreach";

            checkQuestion1Button.Click += (s, e) => {
                if (CheckAnswer(1, new[] { answer1RadioButton1, answer1RadioButton2, answer1RadioButton3, answer1RadioButton4 }, 0))
                {
                    correctAnswersCount++;
                }
                checkQuestion1Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 2
            question2GroupBox.Text = "2. Что делает break в цикле?";
            answer2RadioButton1.Text = "Прерывает цикл полностью";
            answer2RadioButton2.Text = "Пропускает текущую итерацию";
            answer2RadioButton3.Text = "Запускает цикл заново";
            answer2RadioButton4.Text = "Ничего";

            checkQuestion2Button.Click += (s, e) => {
                if (CheckAnswer(2, new[] { answer2RadioButton1, answer2RadioButton2, answer2RadioButton3, answer2RadioButton4 }, 0))
                {
                    correctAnswersCount++;
                }
                checkQuestion2Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 3
            question3GroupBox.Text = "3. Как заставить цикл for выполниться ровно 10 раз?";
            answer3RadioButton1.Text = "for i in range(10):";
            answer3RadioButton2.Text = "for i in range(1, 10):";
            answer3RadioButton3.Text = "for i in range(0, 10, 2):";
            answer3RadioButton4.Text = "for i in range(1, 11):";

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
            question4GroupBox.Text = "4. В чём разница между break и continue?";
            answer4RadioButton1.Text = "break прерывает цикл, continue пропускает итерацию";
            answer4RadioButton2.Text = "continue прерывает цикл, break пропускает итерацию";
            answer4RadioButton3.Text = "Оба прерывают цикл";
            answer4RadioButton4.Text = "Оба пропускают итерацию";


            checkQuestion4Button.Click += (s, e) => {
                if (CheckAnswer(4, new[] { answer4RadioButton1, answer4RadioButton2, answer4RadioButton3, answer4RadioButton4 }, 0))
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
            bool isCorrect = false;
            for (int i = 0; i < radioButtons.Length; i++)
            {
                if (radioButtons[i].Checked)
                {
                    isCorrect = (i == correctIndex);
                    break;
                }
            }

            if (isCorrect)
            {
                MessageBox.Show("Правильно!", $"Вопрос {questionNumber}");
                return true;
            }
            else
            {
                string correctAnswer = radioButtons[correctIndex].Text;
                MessageBox.Show($"Неправильно. Правильный ответ: {correctAnswer}", $"Вопрос {questionNumber}");
                return false;
            }
        }


        /// Настраивает вкладку с практическими заданиями

        private void SetupPracticeTab()
        {
            // Задание 1
            practiceTaskLabel.Text = "1. Напиши программу, которая:\n" +
                                   "- Выводит все чётные числа от 1 до 20\n" +
                                   "- Использует цикл for и проверку на чётность\n\n" +
                                   "Пример работы:\n" +
                                   "2 4 6 8 10 12 14 16 18 20";

            checkPracticeButton.Click += (s, e) => {
                if (CheckEvenNumbersProgram(practiceCodeTextBox.Text))
                {
                    correctAnswersCount++;
                }
                practiceCodeTextBox.ReadOnly = true;
                checkPracticeButton.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };

            // Задание 2
            task2Label.Text = "2. Напиши программу, которая:\n" +
                              "- Запрашивает число N\n" +
                              "- Считает сумму всех чисел от 1 до N\n\n" +
                              "Пример работы:\n" +
                              "Введите число: 5\n" +
                              "Сумма чисел от 1 до 5 = 15";

            checkTask2Button.Click += (s, e) => {
                if (CheckSumProgram(task2CodeBox.Text))
                {
                    correctAnswersCount++;
                }
                task2CodeBox.ReadOnly = true;
                checkTask2Button.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };
        }
        /// Проверяет практическое задание по ключевым словам


        private bool CheckEvenNumbersProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasForLoop = userCode.Contains("for") && userCode.Contains("range");
            bool hasIf = userCode.Contains("if") && userCode.Contains("% 2 ==");
            bool hasPrint = userCode.Contains("print");

            if (hasForLoop && hasIf && hasPrint)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "for num in range(1, 21):\n    if num % 2 == 0:\n        print(num, end=\" \")";
                MessageBox.Show($"Неправильно. Пример решения:\n\n{exampleSolution}", "Результат");
                return false;
            }
        }

        /// Проверяет программу расчета НДС

        private bool CheckSumProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasInput = userCode.Contains("input(");
            bool hasForLoop = userCode.Contains("for") && userCode.Contains("range");
            bool hasSum = userCode.Contains("sum") || userCode.Contains("+=");
            bool hasPrint = userCode.Contains("print");


            if (hasInput && hasForLoop && hasSum && hasPrint)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "N = int(input(\"Введите число: \"))\nsumma = 0\nfor i in range(1, N + 1):\n    summa += i\nprint(f\"Сумма чисел от 1 до {N} = {summa}\")";
                MessageBox.Show($"Неправильно. Пример решения:\n\n{exampleSolution}", "Результат");
                return false;
            }
        }


        /// Настраивает вкладку с дополнительными заданиями

        private void SetupAdvancedTab()
        {
            advancedTaskLabel.Text = "Дополнительное задание:\n\n" +
                                    "Напиши программу, которая:\n" +
                                    "- Запрашивает пароль\n" +
                                    "- Даёт 3 попытки на ввод\n" +
                                    "- При правильном пароле выводит \"Доступ открыт!\"\n" +
                                    "- При исчерпании попыток — \"Доступ запрещён!\"\n\n" +
                                    "Пример работы:\n" +
                                    "Введите пароль: 111\n" +
                                    "Неверно! Осталось 2 попытки.";

            checkAdvancedButton.Click += (s, e) => {
                if (CheckPasswordAttemptsProgram(advancedCodeTextBox.Text))
                {
                    correctAnswersCount++;
                }
                advancedCodeTextBox.ReadOnly = true;
                checkAdvancedButton.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };
        }


        /// Проверяет программу конвертера валют

        private bool CheckPasswordAttemptsProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasWhile = userCode.Contains("while");
            bool hasBreak = userCode.Contains("break");
            bool hasAttempts = userCode.Contains("attempt") || userCode.Contains("попыт");
            bool hasInput = userCode.Contains("input(");
            bool hasOutput = userCode.Contains("Доступ открыт") && userCode.Contains("Доступ запрещён");

            if (hasWhile && hasBreak && hasAttempts && hasInput && hasOutput)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "password = \"12345\"\nattempts = 3\n\nwhile attempts > 0:\n    user_input = input(\"Введите пароль: \")\n    if user_input == password:\n        print(\"Доступ открыт!\")\n        break\n    else:\n        attempts -= 1\n        print(f\"Неверно! Осталось {attempts} попытки.\")\nelse:\n    print(\"Доступ запрещён!\")";
                MessageBox.Show($"Неправильно. Пример решения:\n\n{exampleSolution}", "Результат");
                return false;
            }
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