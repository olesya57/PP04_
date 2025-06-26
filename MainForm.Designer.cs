namespace Learning___Program
{
    partial class MainForm
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
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.русскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.языкИнтерфейсаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сменитьТемуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pythonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pascalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.курсыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPythonStats = new System.Windows.Forms.Label();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.listViewPython = new System.Windows.Forms.ListView();
            this.tabPagePython = new System.Windows.Forms.TabPage();
            this.listViewPascal = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPascalStats = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tabPagePascal = new System.Windows.Forms.TabPage();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlCourses = new System.Windows.Forms.TabControl();
            this.btnResetProgress = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPagePython.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPagePascal.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControlCourses.SuspendLayout();
            this.SuspendLayout();
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.englishToolStripMenuItem.Text = "English";
            // 
            // русскийToolStripMenuItem
            // 
            this.русскийToolStripMenuItem.Name = "русскийToolStripMenuItem";
            this.русскийToolStripMenuItem.Size = new System.Drawing.Size(146, 26);
            this.русскийToolStripMenuItem.Text = "Русский";
            // 
            // языкИнтерфейсаToolStripMenuItem
            // 
            this.языкИнтерфейсаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.русскийToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.языкИнтерфейсаToolStripMenuItem.Name = "языкИнтерфейсаToolStripMenuItem";
            this.языкИнтерфейсаToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.языкИнтерфейсаToolStripMenuItem.Text = "Язык интерфейса";
            this.языкИнтерфейсаToolStripMenuItem.Click += new System.EventHandler(this.языкИнтерфейсаToolStripMenuItem_Click);
            // 
            // сменитьТемуToolStripMenuItem
            // 
            this.сменитьТемуToolStripMenuItem.Name = "сменитьТемуToolStripMenuItem";
            this.сменитьТемуToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.сменитьТемуToolStripMenuItem.Text = "Сменить тему";
            this.сменитьТемуToolStripMenuItem.Click += new System.EventHandler(this.сменитьТемуToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сменитьТемуToolStripMenuItem,
            this.языкИнтерфейсаToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(98, 26);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // pythonToolStripMenuItem
            // 
            this.pythonToolStripMenuItem.Name = "pythonToolStripMenuItem";
            this.pythonToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.pythonToolStripMenuItem.Text = "Python";
            this.pythonToolStripMenuItem.Click += new System.EventHandler(this.pythonToolStripMenuItem_Click);
            // 
            // pascalToolStripMenuItem
            // 
            this.pascalToolStripMenuItem.Name = "pascalToolStripMenuItem";
            this.pascalToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.pascalToolStripMenuItem.Text = "Pascal";
            this.pascalToolStripMenuItem.Click += new System.EventHandler(this.pascalToolStripMenuItem_Click);
            // 
            // курсыToolStripMenuItem
            // 
            this.курсыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pascalToolStripMenuItem,
            this.pythonToolStripMenuItem});
            this.курсыToolStripMenuItem.Name = "курсыToolStripMenuItem";
            this.курсыToolStripMenuItem.Size = new System.Drawing.Size(66, 26);
            this.курсыToolStripMenuItem.Text = "Курсы";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Thistle;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblPythonStats);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 376);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(953, 146);
            this.panel2.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "Статистика";
            // 
            // lblPythonStats
            // 
            this.lblPythonStats.AutoSize = true;
            this.lblPythonStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.lblPythonStats.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPythonStats.Location = new System.Drawing.Point(12, 71);
            this.lblPythonStats.Name = "lblPythonStats";
            this.lblPythonStats.Size = new System.Drawing.Size(164, 16);
            this.lblPythonStats.TabIndex = 32;
            this.lblPythonStats.Text = "\"Пройдено уроков: 2/15\"";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 26);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.курсыToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(973, 30);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // listViewPython
            // 
            this.listViewPython.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPython.GridLines = true;
            this.listViewPython.HideSelection = false;
            this.listViewPython.Location = new System.Drawing.Point(3, 3);
            this.listViewPython.Name = "listViewPython";
            this.listViewPython.Size = new System.Drawing.Size(953, 519);
            this.listViewPython.TabIndex = 3;
            this.listViewPython.UseCompatibleStateImageBehavior = false;
            // 
            // tabPagePython
            // 
            this.tabPagePython.Controls.Add(this.panel2);
            this.tabPagePython.Controls.Add(this.listViewPython);
            this.tabPagePython.Location = new System.Drawing.Point(4, 25);
            this.tabPagePython.Name = "tabPagePython";
            this.tabPagePython.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePython.Size = new System.Drawing.Size(959, 525);
            this.tabPagePython.TabIndex = 1;
            this.tabPagePython.Text = "Python";
            this.tabPagePython.UseVisualStyleBackColor = true;
            // 
            // listViewPascal
            // 
            this.listViewPascal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPascal.GridLines = true;
            this.listViewPascal.HideSelection = false;
            this.listViewPascal.Location = new System.Drawing.Point(3, 3);
            this.listViewPascal.Name = "listViewPascal";
            this.listViewPascal.Size = new System.Drawing.Size(953, 519);
            this.listViewPascal.TabIndex = 4;
            this.listViewPascal.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 26;
            this.label1.Text = "Статистика";
            // 
            // lblPascalStats
            // 
            this.lblPascalStats.AutoSize = true;
            this.lblPascalStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.lblPascalStats.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPascalStats.Location = new System.Drawing.Point(12, 71);
            this.lblPascalStats.Name = "lblPascalStats";
            this.lblPascalStats.Size = new System.Drawing.Size(164, 16);
            this.lblPascalStats.TabIndex = 28;
            this.lblPascalStats.Text = "\"Пройдено уроков: 2/15\"";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Thistle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPascalStats);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 376);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(953, 146);
            this.panel1.TabIndex = 30;
            // 
            // lblWelcome
            // 
            this.lblWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(3, 631);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(79, 34);
            this.lblWelcome.TabIndex = 29;
            this.lblWelcome.Text = "lblWelcome";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPagePascal
            // 
            this.tabPagePascal.Controls.Add(this.panel1);
            this.tabPagePascal.Controls.Add(this.listViewPascal);
            this.tabPagePascal.Location = new System.Drawing.Point(4, 25);
            this.tabPagePascal.Name = "tabPagePascal";
            this.tabPagePascal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePascal.Size = new System.Drawing.Size(959, 525);
            this.tabPagePascal.TabIndex = 0;
            this.tabPagePascal.Text = "Pascal";
            this.tabPagePascal.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRefresh.BackColor = System.Drawing.Color.LightYellow;
            this.btnRefresh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefresh.Location = new System.Drawing.Point(3, 595);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(209, 33);
            this.btnRefresh.TabIndex = 32;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnRefresh, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControlCourses, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblWelcome, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnResetProgress, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.699495F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.89141F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.573056F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.981464F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.854578F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(973, 702);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tabControlCourses
            // 
            this.tabControlCourses.Controls.Add(this.tabPagePascal);
            this.tabControlCourses.Controls.Add(this.tabPagePython);
            this.tabControlCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCourses.Location = new System.Drawing.Point(3, 35);
            this.tabControlCourses.Name = "tabControlCourses";
            this.tabControlCourses.SelectedIndex = 0;
            this.tabControlCourses.Size = new System.Drawing.Size(967, 554);
            this.tabControlCourses.TabIndex = 0;
            // 
            // btnResetProgress
            // 
            this.btnResetProgress.BackColor = System.Drawing.Color.LightYellow;
            this.btnResetProgress.Location = new System.Drawing.Point(3, 668);
            this.btnResetProgress.Name = "btnResetProgress";
            this.btnResetProgress.Size = new System.Drawing.Size(209, 31);
            this.btnResetProgress.TabIndex = 33;
            this.btnResetProgress.Text = "Снять прогресс";
            this.btnResetProgress.UseVisualStyleBackColor = false;
            this.btnResetProgress.Click += new System.EventHandler(this.btnResetProgress_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(973, 702);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ОБУЧАЮЩАЯ ПЛАТФОРМА";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPagePython.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPagePascal.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControlCourses.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem русскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem языкИнтерфейсаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сменитьТемуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pythonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pascalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem курсыToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPythonStats;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ListView listViewPython;
        private System.Windows.Forms.TabPage tabPagePython;
        private System.Windows.Forms.ListView listViewPascal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPascalStats;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPagePascal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControlCourses;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnResetProgress;
    }
}