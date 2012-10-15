namespace Flexible_computing.Forms
{
    partial class PropertiesForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bSave = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.pBSave = new System.Windows.Forms.ProgressBar();
            this.lPropertiesSaved = new System.Windows.Forms.Label();
            this.cBClearLogs = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cBDebug = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cBInputLimit = new System.Windows.Forms.CheckBox();
            this.tBInputLimit = new System.Windows.Forms.TextBox();
            this.lInputLimitLabel = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.tBResultsFileSave = new System.Windows.Forms.TextBox();
            this.bOpenDialog = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(14, 14);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(758, 441);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(750, 413);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основные";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(750, 413);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Вычисления";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(591, 457);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(87, 27);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(685, 457);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(87, 27);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // pBSave
            // 
            this.pBSave.Location = new System.Drawing.Point(14, 472);
            this.pBSave.Name = "pBSave";
            this.pBSave.Size = new System.Drawing.Size(117, 12);
            this.pBSave.TabIndex = 3;
            // 
            // lPropertiesSaved
            // 
            this.lPropertiesSaved.BackColor = System.Drawing.Color.Honeydew;
            this.lPropertiesSaved.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lPropertiesSaved.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lPropertiesSaved.Image = global::Flexible_computing.Properties.Resources.success;
            this.lPropertiesSaved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lPropertiesSaved.Location = new System.Drawing.Point(137, 457);
            this.lPropertiesSaved.Name = "lPropertiesSaved";
            this.lPropertiesSaved.Size = new System.Drawing.Size(169, 27);
            this.lPropertiesSaved.TabIndex = 4;
            this.lPropertiesSaved.Text = "Настройки Сохранены   ";
            this.lPropertiesSaved.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cBClearLogs
            // 
            this.cBClearLogs.Location = new System.Drawing.Point(28, 22);
            this.cBClearLogs.Name = "cBClearLogs";
            this.cBClearLogs.Size = new System.Drawing.Size(121, 20);
            this.cBClearLogs.TabIndex = 0;
            this.cBClearLogs.Text = "Очищать Логи";
            this.toolTip1.SetToolTip(this.cBClearLogs, "Очищать логи при перерасчете");
            this.cBClearLogs.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.cBDebug);
            this.groupBox1.Controls.Add(this.cBClearLogs);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(725, 123);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Отображение";
            // 
            // cBDebug
            // 
            this.cBDebug.AutoSize = true;
            this.cBDebug.Location = new System.Drawing.Point(28, 48);
            this.cBDebug.Name = "cBDebug";
            this.cBDebug.Size = new System.Drawing.Size(71, 19);
            this.cBDebug.TabIndex = 1;
            this.cBDebug.Text = "Отладка";
            this.toolTip1.SetToolTip(this.cBDebug, "Отображать отладочные элементы управления");
            this.cBDebug.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lInputLimitLabel);
            this.groupBox2.Controls.Add(this.tBInputLimit);
            this.groupBox2.Controls.Add(this.cBInputLimit);
            this.groupBox2.Location = new System.Drawing.Point(16, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(717, 157);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Входные данные";
            // 
            // cBInputLimit
            // 
            this.cBInputLimit.AutoSize = true;
            this.cBInputLimit.Location = new System.Drawing.Point(25, 22);
            this.cBInputLimit.Name = "cBInputLimit";
            this.cBInputLimit.Size = new System.Drawing.Size(170, 19);
            this.cBInputLimit.TabIndex = 0;
            this.cBInputLimit.Text = "Ограничить входно число";
            this.cBInputLimit.UseVisualStyleBackColor = true;
            // 
            // tBInputLimit
            // 
            this.tBInputLimit.Enabled = false;
            this.tBInputLimit.Location = new System.Drawing.Point(49, 47);
            this.tBInputLimit.MaxLength = 15;
            this.tBInputLimit.Name = "tBInputLimit";
            this.tBInputLimit.Size = new System.Drawing.Size(100, 23);
            this.tBInputLimit.TabIndex = 1;
            // 
            // lInputLimitLabel
            // 
            this.lInputLimitLabel.AutoSize = true;
            this.lInputLimitLabel.Enabled = false;
            this.lInputLimitLabel.Location = new System.Drawing.Point(157, 50);
            this.lInputLimitLabel.Name = "lInputLimitLabel";
            this.lInputLimitLabel.Size = new System.Drawing.Size(130, 15);
            this.lInputLimitLabel.TabIndex = 2;
            this.lInputLimitLabel.Text = "Количество символов";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(28, 73);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(82, 19);
            this.checkBox4.TabIndex = 2;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bOpenDialog);
            this.groupBox3.Controls.Add(this.tBResultsFileSave);
            this.groupBox3.Controls.Add(this.checkBox5);
            this.groupBox3.Location = new System.Drawing.Point(16, 210);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(717, 185);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Выходные данные";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(25, 22);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(189, 19);
            this.checkBox5.TabIndex = 0;
            this.checkBox5.Text = "Сохранять результаты в файл";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // tBResultsFileSave
            // 
            this.tBResultsFileSave.Enabled = false;
            this.tBResultsFileSave.Location = new System.Drawing.Point(49, 47);
            this.tBResultsFileSave.Name = "tBResultsFileSave";
            this.tBResultsFileSave.Size = new System.Drawing.Size(238, 23);
            this.tBResultsFileSave.TabIndex = 1;
            // 
            // bOpenDialog
            // 
            this.bOpenDialog.Enabled = false;
            this.bOpenDialog.Location = new System.Drawing.Point(293, 47);
            this.bOpenDialog.Name = "bOpenDialog";
            this.bOpenDialog.Size = new System.Drawing.Size(85, 23);
            this.bOpenDialog.TabIndex = 2;
            this.bOpenDialog.Text = "Добавить ...";
            this.bOpenDialog.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // PropertiesForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 491);
            this.Controls.Add(this.lPropertiesSaved);
            this.Controls.Add(this.pBSave);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Свойства";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PropertiesForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ProgressBar pBSave;
        private System.Windows.Forms.Label lPropertiesSaved;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cBClearLogs;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cBDebug;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lInputLimitLabel;
        private System.Windows.Forms.TextBox tBInputLimit;
        private System.Windows.Forms.CheckBox cBInputLimit;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bOpenDialog;
        private System.Windows.Forms.TextBox tBResultsFileSave;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}