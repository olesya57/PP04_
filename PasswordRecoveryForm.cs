using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learning___Program
{
    public partial class PasswordRecoveryForm : Form
    {
        // Поля для хранения контрольного вопроса и имени пользователя
        private string securityQuestion = "";
        private string username = "";

        public PasswordRecoveryForm()
        {
            InitializeComponent();
            SetupForm(); // Настраиваем начальное состояние формы
        }

        // Метод для валидации пароля
        private bool ValidatePassword(string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Проверка на пустоту
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Пароль не может быть пустым!";
                return false;
            }

            // Проверка минимальной длины
            if (password.Length < 3)
            {
                errorMessage = "Пароль должен содержать минимум 3 символа!";
                return false;
            }

            // Проверка на наличие цифр
            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Пароль должен содержать хотя бы одну цифру!";
                return false;
            }

            // Проверка на наличие букв
            if (!password.Any(char.IsLetter))
            {
                errorMessage = "Пароль должен содержать хотя бы одну букву!";
                return false;
            }

            // Проверка на запрещённые символы
            var regex = new Regex(@"^[a-zA-Z0-9.!?_]+$");
            if (!regex.IsMatch(password))
            {
                errorMessage = "Пароль содержит недопустимые символы! (Разрешены только буквы английского алфавита, цифры, а также символы: . ! ? _)";
                return false;
            }



            return true;
        }

        // Обработчик кнопки проверки пользователя
        private void btnCheckUser_Click(object sender, EventArgs e)
        {
            username = txtUsername.Text.Trim(); // Получаем введенный логин

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Запрос для получения контрольного вопроса пользователя
                string query = "SELECT SecurityQuestion FROM Users WHERE Username = @Username";
                SqlParameter[] parameters = { new SqlParameter("@Username", username) };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                // Проверяем, найден ли пользователь
                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Пользователь с таким логином не найден!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Сохраняем контрольный вопрос и показываем его
                securityQuestion = result.Rows[0]["SecurityQuestion"].ToString();
                lblSecurityQuestion.Text = $"Контрольный вопрос: {securityQuestion}";

                // Показываем элементы для ввода ответа
                lblSecurityQuestion.Visible = true;
                txtSecurityAnswer.Visible = true;
                btnConfirmAnswer.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке пользователя: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик кнопки подтверждения ответа
        private void btnConfirmAnswer_Click(object sender, EventArgs e)
        {
            string answer = txtSecurityAnswer.Text.Trim(); // Получаем ответ

            if (string.IsNullOrEmpty(answer))
            {
                MessageBox.Show("Введите ответ на контрольный вопрос!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Проверяем правильность ответа на контрольный вопрос
                string query = "SELECT 1 FROM Users WHERE Username = @Username AND SecurityAnswer = @SecurityAnswer";
                SqlParameter[] parameters = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@SecurityAnswer", answer)
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный ответ на контрольный вопрос!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Если ответ верный, показываем элементы для смены пароля
                txtNewPassword.Visible = true;
                lblConfirmPassword.Visible = true;
                txtConfirmPassword.Visible = true;
                btnChangePassword.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке ответа: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик кнопки изменения пароля
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Проверяем, что поля заполнены
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Введите новый пароль и подтверждение!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Валидация пароля
            if (!ValidatePassword(newPassword, out string errorMessage))
            {
                MessageBox.Show($"Пароль не соответствует требованиям:\n{errorMessage}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем совпадение паролей
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Генерируем новый хеш и соль для пароля
                var (hash, salt) = PasswordHasher.HashPassword(newPassword);

                // Обновляем пароль в базе данных
                string query = @"UPDATE Users 
                               SET PasswordHash = @PasswordHash, 
                                   Salt = @Salt 
                               WHERE Username = @Username";

                SqlParameter[] parameters = {
                    new SqlParameter("@PasswordHash", hash),
                    new SqlParameter("@Salt", salt),
                    new SqlParameter("@Username", username)
                };

                // Выполняем запрос и проверяем результат
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Пароль успешно изменен!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Возвращаемся к форме входа
                    Form1 form = new Form1();
                    this.Hide();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Не удалось изменить пароль!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении пароля: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик кнопки "Назад"
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Возвращаемся к форме входа
            Form1 Form1 = new Form1();
            Form1.Show();
            this.Hide();
        }

        // Метод начальной настройки формы
        private void SetupForm()
        {
            // Скрываем элементы, которые понадобятся только после проверки пользователя
            lblSecurityQuestion.Visible = false;
            txtSecurityAnswer.Visible = false;
            btnConfirmAnswer.Visible = false;
            txtNewPassword.Visible = false;
            lblConfirmPassword.Visible = false;
            txtConfirmPassword.Visible = false;
            btnChangePassword.Visible = false;
        }
    }
}