using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Learning___Program
{
  
    /// Форма для урока по Pascal: "Переменные и ввод данных"

    public partial class PascalLesson2Form : Form
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

        public PascalLesson2Form(string username, MainForm mainForm)
        {
            InitializeComponent(); // Инициализация компонентов формы (автогенерируемый код)

            // Сохраняем переданные данные
            _username = username;
            _mainForm = mainForm;

            // Получаем ID пользователя и урока из базы данных
            _userId = GetUserId(_username);
            _lessonId = GetLessonId("Переменные и ввод данных", "Pascal");

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
           // exampleCodeTextBox.Text = "program Example;\nvar\n  name: string;\nbegin\n  write('Введите ваше имя: ');\n  readln(name);\n  writeln('Привет, ', name, '!');\nend.";
        }

        /// Настраивает вкладку с тестовыми вопросами
   
        private void SetupQuestionsTab()
        {
            // Вопрос 1: Назначение раздела var
            question1GroupBox.Text = "1. Для чего нужен раздел var?";
            answer1RadioButton1.Text = "Для вывода текста";
            answer1RadioButton2.Text = "Для объявления переменных"; // Правильный ответ
            answer1RadioButton3.Text = "Для ввода данных";
            answer1RadioButton4.Text = "Для условий";

            // Обработчик проверки ответа
            checkQuestion1Button.Click += (s, e) => {
                // Проверяем ответ (индекс правильного варианта: 1)
                if (CheckAnswer(1, new[] { answer1RadioButton1, answer1RadioButton2, answer1RadioButton3, answer1RadioButton4 }, 1))
                    correctAnswersCount++;

                // Блокируем кнопку после ответа
                checkQuestion1Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus(); // Обновляем статус выполнения
            };

            // Вопрос 2: Тип данных для дробных чисел
            question2GroupBox.Text = "2. Какой тип для дробных чисел?";
            answer2RadioButton1.Text = "integer";
            answer2RadioButton2.Text = "real"; // Правильный ответ
            answer2RadioButton3.Text = "string";
            answer2RadioButton4.Text = "boolean";

            checkQuestion2Button.Click += (s, e) => {
                if (CheckAnswer(2, new[] { answer2RadioButton1, answer2RadioButton2, answer2RadioButton3, answer2RadioButton4 }, 1))
                    correctAnswersCount++;
                checkQuestion2Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 3: Назначение readln
            question3GroupBox.Text = "3. Что делает readln?";
            answer3RadioButton1.Text = "Выводит текст";
            answer3RadioButton2.Text = "Считывает ввод"; // Правильный ответ
            answer3RadioButton3.Text = "Проверяет условие";
            answer3RadioButton4.Text = "Останавливает программу";

            checkQuestion3Button.Click += (s, e) => {
                if (CheckAnswer(3, new[] { answer3RadioButton1, answer3RadioButton2, answer3RadioButton3, answer3RadioButton4 }, 1))
                    correctAnswersCount++;
                checkQuestion3Button.Enabled = false;
                completedQuestions++;
                UpdateCompletionStatus();
            };

            // Вопрос 4: Объявление строковой переменной
            question4GroupBox.Text = "4. Как объявить строку?";
            answer4RadioButton1.Text = "var str: string;"; // Правильный ответ
            answer4RadioButton2.Text = "string str;";
            answer4RadioButton3.Text = "str: string;";
            answer4RadioButton4.Text = "var str = string;";

            checkQuestion4Button.Click += (s, e) => {
                if (CheckAnswer(4, new[] { answer4RadioButton1, answer4RadioButton2, answer4RadioButton3, answer4RadioButton4 }, 0))
                    correctAnswersCount++;
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
            // Задание 1: Работа с возрастом
            practiceTaskLabel.Text = "1. Напишите программу, которая:\n- Запрашивает возраст\n- Выводит \"Вам X лет\"\nПример:\nВведите возраст: 18\nВам 18 лет";

            checkPracticeButton.Click += (s, e) => {
                // Проверяем наличие ключевых элементов кода
                if (CheckPracticeAnswer(practiceCodeTextBox.Text, "readln(age)", "writeln('Вам ', age, ' лет')"))
                    correctAnswersCount++;

                // Блокируем редактор и кнопку после проверки
                practiceCodeTextBox.ReadOnly = true;
                checkPracticeButton.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };

            // Задание 2: Сумма двух чисел (ЗАМЕНА НА НОВОЕ ЗАДАНИЕ)
            task2Label.Text = "2. Напишите программу, которая:\n- Запрашивает два целых числа\n- Выводит их сумму\nПример:\nВведите первое число: 5\nВведите второе число: 3\nСумма: 8";

            checkTask2Button.Click += (s, e) => {
                if (CheckSumProgram(task2CodeBox.Text)) // Измененный метод проверки
                    correctAnswersCount++;
                task2CodeBox.ReadOnly = true;
                checkTask2Button.Enabled = false;
                completedPractice++;
                UpdateCompletionStatus();
            };
        
        }

        /// Проверяет практическое задание по ключевым словам
     
    
        private bool CheckPracticeAnswer(string userCode, params string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (!userCode.Contains(keyword))
                {
                    // Показываем пример правильного решения
                    MessageBox.Show("Неправильно! Пример:\nprogram Age;\nvar\n  age: integer;\nbegin\n  write('Введите возраст: ');\n  readln(age);\n  writeln('Вам ', age, ' лет');\nend.", "Результат");
                    return false;
                }
            }
            MessageBox.Show("Правильно!", "Результат");
            return true;
        }

        // Новый метод проверки для суммы
        private bool CheckSumProgram(string userCode)
        {
            bool hasVars = userCode.Contains("num1, num2, sum: integer");
            bool hasInput = userCode.Contains("readln(num1)") && userCode.Contains("readln(num2)");
            bool hasCalculation = userCode.Contains("num1 + num2");

            if (hasVars && hasInput && hasCalculation)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            MessageBox.Show("Неправильно! Пример:\nprogram SumTwoNumbers;\nvar\n  num1, num2, sum: integer;\nbegin\n  write('Введите первое число: ');\n  readln(num1);\n  write('Введите второе число: ');\n  readln(num2);\n  sum := num1 + num2;\n  writeln('Сумма: ', sum);\nend.", "Результат");
            return false;
        }

        /// Настраивает вкладку с дополнительными заданиями
        private void SetupAdvancedTab()
        {
            // Новое доп. задание: Удвоение числа
            advancedTaskLabel.Text = "Доп. задание: Напишите программу, которая:\n- Запрашивает целое число\n- Выводит его удвоенное значение\nПример:\nВведите число: 7\nУдвоенное число: 14";

            checkAdvancedButton.Click += (s, e) => {
                if (CheckDoubleProgram(advancedCodeTextBox.Text)) // Новый метод проверки
                    correctAnswersCount++;
                advancedCodeTextBox.ReadOnly = true;
                checkAdvancedButton.Enabled = false;
                completedAdvanced++;
                UpdateCompletionStatus();
            };
        }

        /// Проверяет программу удвоения числа
        private bool CheckDoubleProgram(string userCode)
        {
            bool hasVars = userCode.Contains("num, result: integer");
            bool hasInput = userCode.Contains("readln(num)");
            bool hasCalculation = userCode.Contains("num * 2");

            if (hasVars && hasInput && hasCalculation)
            {
                MessageBox.Show("Правильно!", "Результат");
                return true;
            }
            MessageBox.Show("Неправильно! Пример:\nprogram DoubleNumber;\nvar\n  num, result: integer;\nbegin\n  write('Введите число: ');\n  readln(num);\n  result := num * 2;\n  writeln('Удвоенное число: ', result);\nend.", "Результат");
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