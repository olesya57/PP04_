namespace Learning___Program
{
    partial class PasswordRecoveryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBack = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSecurityQuestion = new System.Windows.Forms.Label();
            this.txtSecurityAnswer = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.btnCheckUser = new System.Windows.Forms.Button();
            this.btnConfirmAnswer = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.LightYellow;
            this.btnBack.Location = new System.Drawing.Point(396, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(69, 26);
            this.btnBack.TabIndex = 100;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BackColor = System.Drawing.Color.LightYellow;
            this.btnChangePassword.Location = new System.Drawing.Point(158, 430);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(175, 39);
            this.btnChangePassword.TabIndex = 99;
            this.btnChangePassword.Text = "Изменить пароль";
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(190, 371);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(203, 22);
            this.txtConfirmPassword.TabIndex = 98;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(19, 374);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(132, 16);
            this.lblConfirmPassword.TabIndex = 97;
            this.lblConfirmPassword.Text = "Повторите пароль:";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(190, 335);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(203, 22);
            this.txtNewPassword.TabIndex = 96;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(19, 341);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(103, 16);
            this.lblPassword.TabIndex = 95;
            this.lblPassword.Text = "Новый пароль:";
            // 
            // lblSecurityQuestion
            // 
            this.lblSecurityQuestion.AutoSize = true;
            this.lblSecurityQuestion.Location = new System.Drawing.Point(19, 154);
            this.lblSecurityQuestion.Name = "lblSecurityQuestion";
            this.lblSecurityQuestion.Size = new System.Drawing.Size(144, 16);
            this.lblSecurityQuestion.TabIndex = 94;
            this.lblSecurityQuestion.Text = "Контрольный вопрос";
            // 
            // txtSecurityAnswer
            // 
            this.txtSecurityAnswer.Location = new System.Drawing.Point(22, 233);
            this.txtSecurityAnswer.Name = "txtSecurityAnswer";
            this.txtSecurityAnswer.Size = new System.Drawing.Size(203, 22);
            this.txtSecurityAnswer.TabIndex = 93;
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(19, 210);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(50, 16);
            this.lbl.TabIndex = 92;
            this.lbl.Text = "Ответ:";
            // 
            // btnCheckUser
            // 
            this.btnCheckUser.BackColor = System.Drawing.Color.LightYellow;
            this.btnCheckUser.Location = new System.Drawing.Point(100, 85);
            this.btnCheckUser.Name = "btnCheckUser";
            this.btnCheckUser.Size = new System.Drawing.Size(257, 33);
            this.btnCheckUser.TabIndex = 91;
            this.btnCheckUser.Text = "Проверить пользователя";
            this.btnCheckUser.UseVisualStyleBackColor = false;
            this.btnCheckUser.Click += new System.EventHandler(this.btnCheckUser_Click);
            // 
            // btnConfirmAnswer
            // 
            this.btnConfirmAnswer.BackColor = System.Drawing.Color.LightYellow;
            this.btnConfirmAnswer.Location = new System.Drawing.Point(22, 274);
            this.btnConfirmAnswer.Name = "btnConfirmAnswer";
            this.btnConfirmAnswer.Size = new System.Drawing.Size(175, 39);
            this.btnConfirmAnswer.TabIndex = 90;
            this.btnConfirmAnswer.Text = "Подтвердить ответна";
            this.btnConfirmAnswer.UseVisualStyleBackColor = false;
            this.btnConfirmAnswer.Click += new System.EventHandler(this.btnConfirmAnswer_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(190, 37);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(203, 22);
            this.txtUsername.TabIndex = 89;
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(19, 40);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(107, 16);
            this.lblLogin.TabIndex = 88;
            this.lblLogin.Text = "Введите логин:";
            // 
            // PasswordRecoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(481, 491);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblSecurityQuestion);
            this.Controls.Add(this.txtSecurityAnswer);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.btnCheckUser);
            this.Controls.Add(this.btnConfirmAnswer);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PasswordRecoveryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ВОССТАНОВЛЕНИЕ ПАРОЛЯ ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblSecurityQuestion;
        private System.Windows.Forms.TextBox txtSecurityAnswer;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Button btnCheckUser;
        private System.Windows.Forms.Button btnConfirmAnswer;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblLogin;
    }
}