namespace Vaccination.Views
{
    partial class ForecastDateForm
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
            this.dateTimePickerFirst = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonOptimistic = new System.Windows.Forms.RadioButton();
            this.radioButtonRealistic = new System.Windows.Forms.RadioButton();
            this.radioButtonPessimistic = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxBirth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonDynamicBirth = new System.Windows.Forms.RadioButton();
            this.radioButtonConstBirth = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxDeath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonDeathDynamic = new System.Windows.Forms.RadioButton();
            this.radioButtonDeathConst = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePickerFirst
            // 
            this.dateTimePickerFirst.CustomFormat = "MM.yyyy";
            this.dateTimePickerFirst.Enabled = false;
            this.dateTimePickerFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerFirst.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFirst.Location = new System.Drawing.Point(56, 39);
            this.dateTimePickerFirst.Name = "dateTimePickerFirst";
            this.dateTimePickerFirst.Size = new System.Drawing.Size(113, 28);
            this.dateTimePickerFirst.TabIndex = 2;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "MM.yyyy";
            this.dateTimePickerEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(56, 90);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(113, 28);
            this.dateTimePickerEnd.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(10, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Від";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(10, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "До";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCancel.Location = new System.Drawing.Point(12, 379);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(269, 40);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Скасувати";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOk.Location = new System.Drawing.Point(287, 379);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(290, 40);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "Далі";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonOptimistic);
            this.groupBox1.Controls.Add(this.radioButtonRealistic);
            this.groupBox1.Controls.Add(this.radioButtonPessimistic);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(287, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 155);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Виберіть сценарій прогнозу";
            // 
            // radioButtonOptimistic
            // 
            this.radioButtonOptimistic.AutoSize = true;
            this.radioButtonOptimistic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonOptimistic.Location = new System.Drawing.Point(7, 111);
            this.radioButtonOptimistic.Name = "radioButtonOptimistic";
            this.radioButtonOptimistic.Size = new System.Drawing.Size(180, 29);
            this.radioButtonOptimistic.TabIndex = 2;
            this.radioButtonOptimistic.TabStop = true;
            this.radioButtonOptimistic.Text = "Оптимістичний";
            this.radioButtonOptimistic.UseVisualStyleBackColor = true;
            // 
            // radioButtonRealistic
            // 
            this.radioButtonRealistic.AutoSize = true;
            this.radioButtonRealistic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonRealistic.Location = new System.Drawing.Point(7, 79);
            this.radioButtonRealistic.Name = "radioButtonRealistic";
            this.radioButtonRealistic.Size = new System.Drawing.Size(159, 29);
            this.radioButtonRealistic.TabIndex = 1;
            this.radioButtonRealistic.TabStop = true;
            this.radioButtonRealistic.Text = "Реалістичний";
            this.radioButtonRealistic.UseVisualStyleBackColor = true;
            // 
            // radioButtonPessimistic
            // 
            this.radioButtonPessimistic.AutoSize = true;
            this.radioButtonPessimistic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonPessimistic.Location = new System.Drawing.Point(7, 39);
            this.radioButtonPessimistic.Name = "radioButtonPessimistic";
            this.radioButtonPessimistic.Size = new System.Drawing.Size(177, 29);
            this.radioButtonPessimistic.TabIndex = 0;
            this.radioButtonPessimistic.TabStop = true;
            this.radioButtonPessimistic.Text = "Песимістичний";
            this.radioButtonPessimistic.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePickerFirst);
            this.groupBox2.Controls.Add(this.dateTimePickerEnd);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(12, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 155);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Виберіть період прогнозу";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxBirth);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.radioButtonDynamicBirth);
            this.groupBox3.Controls.Add(this.radioButtonConstBirth);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(287, 179);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 184);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Виберіть тип народжуваності";
            // 
            // textBoxBirth
            // 
            this.textBoxBirth.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxBirth.Location = new System.Drawing.Point(184, 125);
            this.textBoxBirth.Name = "textBoxBirth";
            this.textBoxBirth.Size = new System.Drawing.Size(100, 28);
            this.textBoxBirth.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Народжуваність";
            // 
            // radioButtonDynamicBirth
            // 
            this.radioButtonDynamicBirth.AutoSize = true;
            this.radioButtonDynamicBirth.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonDynamicBirth.Location = new System.Drawing.Point(6, 84);
            this.radioButtonDynamicBirth.Name = "radioButtonDynamicBirth";
            this.radioButtonDynamicBirth.Size = new System.Drawing.Size(125, 28);
            this.radioButtonDynamicBirth.TabIndex = 0;
            this.radioButtonDynamicBirth.TabStop = true;
            this.radioButtonDynamicBirth.Text = "Динамічна";
            this.radioButtonDynamicBirth.UseVisualStyleBackColor = true;
            this.radioButtonDynamicBirth.CheckedChanged += new System.EventHandler(this.RadioButtonDynamicBirth_CheckedChanged);
            // 
            // radioButtonConstBirth
            // 
            this.radioButtonConstBirth.AutoSize = true;
            this.radioButtonConstBirth.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonConstBirth.Location = new System.Drawing.Point(7, 41);
            this.radioButtonConstBirth.Name = "radioButtonConstBirth";
            this.radioButtonConstBirth.Size = new System.Drawing.Size(111, 28);
            this.radioButtonConstBirth.TabIndex = 0;
            this.radioButtonConstBirth.TabStop = true;
            this.radioButtonConstBirth.Text = "Постійна";
            this.radioButtonConstBirth.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxDeath);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.radioButtonDeathDynamic);
            this.groupBox4.Controls.Add(this.radioButtonDeathConst);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(12, 179);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(269, 184);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Виберіть тип смертності";
            // 
            // textBoxDeath
            // 
            this.textBoxDeath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDeath.Location = new System.Drawing.Point(160, 125);
            this.textBoxDeath.Name = "textBoxDeath";
            this.textBoxDeath.Size = new System.Drawing.Size(103, 28);
            this.textBoxDeath.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(7, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Смертність";
            // 
            // radioButtonDeathDynamic
            // 
            this.radioButtonDeathDynamic.AutoSize = true;
            this.radioButtonDeathDynamic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonDeathDynamic.Location = new System.Drawing.Point(6, 84);
            this.radioButtonDeathDynamic.Name = "radioButtonDeathDynamic";
            this.radioButtonDeathDynamic.Size = new System.Drawing.Size(125, 28);
            this.radioButtonDeathDynamic.TabIndex = 0;
            this.radioButtonDeathDynamic.TabStop = true;
            this.radioButtonDeathDynamic.Text = "Динамічна";
            this.radioButtonDeathDynamic.UseVisualStyleBackColor = true;
            this.radioButtonDeathDynamic.CheckedChanged += new System.EventHandler(this.RadioButtonDeathDynamic_CheckedChanged);
            // 
            // radioButtonDeathConst
            // 
            this.radioButtonDeathConst.AutoSize = true;
            this.radioButtonDeathConst.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonDeathConst.Location = new System.Drawing.Point(7, 41);
            this.radioButtonDeathConst.Name = "radioButtonDeathConst";
            this.radioButtonDeathConst.Size = new System.Drawing.Size(111, 28);
            this.radioButtonDeathConst.TabIndex = 0;
            this.radioButtonDeathConst.TabStop = true;
            this.radioButtonDeathConst.Text = "Постійна";
            this.radioButtonDeathConst.UseVisualStyleBackColor = true;
            // 
            // ForecastDateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(586, 431);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForecastDateForm";
            this.ShowIcon = false;
            this.Text = "Вибір прогнозу";
            this.Load += new System.EventHandler(this.ForecastDateForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePickerFirst;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonOptimistic;
        private System.Windows.Forms.RadioButton radioButtonRealistic;
        private System.Windows.Forms.RadioButton radioButtonPessimistic;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxBirth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonDynamicBirth;
        private System.Windows.Forms.RadioButton radioButtonConstBirth;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxDeath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonDeathDynamic;
        private System.Windows.Forms.RadioButton radioButtonDeathConst;
    }
}