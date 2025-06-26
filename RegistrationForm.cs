using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace Learning___Program
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            this.Load += RegistrationForm_Load;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Валидация данных
                if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtSecurityAnswer.Text))
                {
                    MessageBox.Show("Все поля обязательны для заполнения!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Валидация пароля
                if (!ValidatePassword(txtPassword.Text, out string passwordErrorMessage))
                {
                    MessageBox.Show($"Пароль не соответствует требованиям:\n{passwordErrorMessage}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Укажите корректный email!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка уникальности логина перед регистрацией
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlParameter[] checkParams = { new SqlParameter("@Username", txtLogin.Text.Trim()) };
                int userCount = (int)DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (userCount > 0)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Генерация хеша пароля
                var (hash, salt) = PasswordHasher.HashPassword(txtPassword.Text);

                // SQL-запрос
                string query = @"INSERT INTO Users 
                               (Username, PasswordHash, Salt, Email, Role, SecurityQuestion, SecurityAnswer)
                               VALUES
                               (@Username, @PasswordHash, @Salt, @Email, @Role, @SecurityQuestion, @SecurityAnswer)";

                SqlParameter[] parameters = {
                    new SqlParameter("@Username", txtLogin.Text.Trim()),
                    new SqlParameter("@PasswordHash", hash),
                    new SqlParameter("@Salt", salt),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@Role", cmbRole.SelectedItem.ToString()),
                    new SqlParameter("@SecurityQuestion", cmbSecurityQuestion.SelectedItem.ToString()),
                    new SqlParameter("@SecurityAnswer", txtSecurityAnswer.Text.Trim())
                };

                // Выполнение запроса
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Регистрация прошла успешно!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Возврат на форму входа
                    this.Hide();
                    new Form1().Show();
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации. Попробуйте снова.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Ошибка уникальности
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Ошибка базы данных: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Инициализация ComboBox
                cmbRole.BeginUpdate();
                cmbRole.Items.AddRange(new[] { "Ученик", "Учитель" });
                cmbRole.SelectedIndex = 0;
                cmbRole.EndUpdate();

                cmbSecurityQuestion.BeginUpdate();
                cmbSecurityQuestion.Items.AddRange(new[]
                {
                    "Ваша любимая цифра?",
                    "Девичья фамилия матери?",
                    "Ваше первое домашнее животное?",
                    "Ваш любимый учитель?"
                });
                cmbSecurityQuestion.SelectedIndex = 0;
                cmbSecurityQuestion.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form1().Show();
        }
    }
}