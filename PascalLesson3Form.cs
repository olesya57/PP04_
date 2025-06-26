using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Learning___Program
{

    /// Форма для урока по Pascal: "Переменные и ввод данных"

    public partial class PascalLesson3Form : Form
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

        public PascalLesson3Form(string username, MainForm mainForm)
        {
            InitializeComponent(); // Инициализация компонентов формы (автогенерируемый код)

            // Сохраняем переданные данные
            _username = username;
            _mainForm = mainForm;

            // Получаем ID пользователя и урока из базы данных
            _userId = GetUserId(_username);
            _lessonId = GetLessonId("Условные операторы (if, else)", "Pascal");

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
            // Пример кода на Pascal с базовым вводом/выводом
          //  exampleCodeTextBox.Text = "program Example;\nvar\n  name: string;\nbegin\n  write('Введите ваше имя: ');\n  readln(name);\n  writeln('Привет, ', name, '!');\nend.";
        }

        /// Настраивает вкладку с тестовыми вопросами

        private void SetupQuestionsTab()
        {
            // Вопрос 1
            question1GroupBox.Text = "1. Как работает оператор if?";
            answer1RadioButton1.Text = "Выполняет код, если условие истинно";
            answer1RadioButton2.Text = "Всегда выполняет код в блоке";
            answer1RadioButton3.Text = "Используется только для вывода текста";
            answer1RadioButton4.Text = "Повторяет действие несколько раз";

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
            question2GroupBox.Text = "2. Какие логические операторы используются в условиях?";
            answer2RadioButton1.Text = "Только > и <";
            answer2RadioButton2.Text = ">, <, =, <>, >=, <=";
            answer2RadioButton3.Text = "Только = и <>";
            answer2RadioButton4.Text = "+, -, *, /";

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
            question3GroupBox.Text = "3. Что делает else?";
            answer3RadioButton1.Text = "Выполняет код, если условие в if ложно";
            answer3RadioButton2.Text = "Завершает программу";
            answer3RadioButton3.Text = "Повторяет условие if";
            answer3RadioButton4.Text = "Выводит текст на экран";

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
            question4GroupBox.Text = "4. Как проверить, что число чётное?";
            answer4RadioButton1.Text = "if num mod 2 = 0 then";
            answer4RadioButton2.Text = "if num / 2 = 0 then";
            answer4RadioButton3.Text = "if num * 2 = 0 then";
            answer4RadioButton4.Text = "if num + 2 = 0 then";

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
            practiceTaskLabel.Text = "1. Напиши программу, которая:\n" +
                                   "- Запрашивает возраст\n" +
                                   "- Выводит \"Доступ разрешён\", если возраст ≥ 18\n" +
                                   "- Выводит \"Доступ запрещён\", если возраст < 18\n\n" +
                                   "Пример работы:\n" +
                                   "Введите ваш возраст: 20\n" +
                                   "Доступ разрешён";

            checkPracticeButton.Click += (s, e) => {
                if (CheckAgeProgram(practiceCodeTextBox.Text))
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
                              "- Запрашивает три числа\n" +
                              "- Выводит наибольшее из них\n\n" +
                              "Пример работы:\n" +
                              "Введите первое число: 5\n" +
                              "Введите второе число: 12\n" +
                              "Введите третье число: 7\n" +
                              "Наибольшее число: 12";

            checkTask2Button.Click += (s, e) => {
                if (CheckMaxNumberProgram(task2CodeBox.Text))
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


        private bool CheckAgeProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasIf = userCode.Contains("if") && userCode.Contains("then");
            bool hasElse = userCode.Contains("else");
            bool hasCondition = userCode.Contains(">=") || userCode.Contains(">");
            bool hasOutput = userCode.Contains("Доступ разрешён") && userCode.Contains("Доступ запрещён");

            if (hasIf && hasElse && hasCondition && hasOutput)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "program AgeCheck;\nvar\n  age: integer;\nbegin\n  write('Введите ваш возраст: ');\n  readln(age);\n  \n  if age >= 18 then\n    writeln('Доступ разрешён')\n  else\n    writeln('Доступ запрещён');\nend.";
                MessageBox.Show($"Неправильно. Пример решения:\n\n{exampleSolution}", "Результат");
                return false;
            }
        }

        private bool CheckMaxNumberProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasThreeInputs = userCode.Contains("readln(") &&
                                 userCode.Split(new[] { "readln(" }, StringSplitOptions.None).Length >= 4;
            bool hasIfConditions = userCode.Contains("if") &&
                                 (userCode.Contains(">") || userCode.Contains("<"));
            bool hasOutput = userCode.Contains("Наибольшее число");

            if (hasThreeInputs && hasIfConditions && hasOutput)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "program MaxNumber;\nvar\n  a, b, c, max: integer;\nbegin\n  write('Введите первое число: ');\n  readln(a);\n  write('Введите второе число: ');\n  readln(b);\n  write('Введите третье число: ');\n  readln(c);\n  \n  if (a >= b) and (a >= c) then\n    max := a\n  else if (b >= a) and (b >= c) then\n    max := b\n  else\n    max := c;\n  \n  writeln('Наибольшее число: ', max);\nend.";
                MessageBox.Show($"Неправильно. Пример решения:\n\n{exampleSolution}", "Результат");
                return false;
            }
        }

        /// Настраивает вкладку с дополнительными заданиями

        private void SetupAdvancedTab()
        {
            advancedTaskLabel.Text = "Дополнительное задание:\n\n" +
                                    "Напиши программу, которая определяет, является ли год високосным.\n\n" +
                                    "Правила:\n" +
                                    "- Год високосный, если он делится на 4, но не делится на 100\n" +
                                    "- Исключение: Если год делится на 400, он всё равно високосный\n\n" +
                                    "Пример работы:\n" +
                                    "Введите год: 2024\n" +
                                    "2024 — високосный год!";

            checkAdvancedButton.Click += (s, e) => {
                if (CheckLeapYearProgram(advancedCodeTextBox.Text))
                {
                    correctAnswersCount++;
                }
                advancedCodeTextBox.ReadOnly = true;
                checkAdvancedButton.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };
        }


        private bool CheckLeapYearProgram(string userCode)
        {
            userCode = userCode.Trim();
            bool hasMod = userCode.Contains("mod");
            bool hasComplexCondition = (userCode.Contains("mod 4") && userCode.Contains("mod 100")) ||
                                     userCode.Contains("mod 400");
            bool hasOutput = userCode.Contains("високосный");

            if (hasMod && hasComplexCondition && hasOutput)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            else
            {
                string exampleSolution = "program LeapYear;\nvar\n  year: integer;\nbegin\n  write('Введите год: ');\n  readln(year);\n  \n  if ((year mod 4 = 0) and (year mod 100 <> 0)) or (year mod 400 = 0) then\n    writeln(year, ' — високосный год!')\n  else\n    writeln(year, ' — не високосный год.');\nend.";
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