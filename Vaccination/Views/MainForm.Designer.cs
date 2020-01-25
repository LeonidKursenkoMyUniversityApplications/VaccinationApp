namespace Vaccination.Views
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxVaccFile = new System.Windows.Forms.TextBox();
            this.buttonVaccOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPopulation = new System.Windows.Forms.TextBox();
            this.buttonPopulationOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(532, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(266, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Далі";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Виберіть дані по захворюваності";
            // 
            // textBoxVaccFile
            // 
            this.textBoxVaccFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxVaccFile.Location = new System.Drawing.Point(13, 43);
            this.textBoxVaccFile.Name = "textBoxVaccFile";
            this.textBoxVaccFile.Size = new System.Drawing.Size(665, 26);
            this.textBoxVaccFile.TabIndex = 2;
            // 
            // buttonVaccOpen
            // 
            this.buttonVaccOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonVaccOpen.Location = new System.Drawing.Point(684, 43);
            this.buttonVaccOpen.Name = "buttonVaccOpen";
            this.buttonVaccOpen.Size = new System.Drawing.Size(114, 32);
            this.buttonVaccOpen.TabIndex = 3;
            this.buttonVaccOpen.Text = "Вибрати";
            this.buttonVaccOpen.UseVisualStyleBackColor = true;
            this.buttonVaccOpen.Click += new System.EventHandler(this.ButtonVaccOpen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(390, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Виберіть дані по віковій структурі населення";
            // 
            // textBoxPopulation
            // 
            this.textBoxPopulation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPopulation.Location = new System.Drawing.Point(13, 116);
            this.textBoxPopulation.Name = "textBoxPopulation";
            this.textBoxPopulation.Size = new System.Drawing.Size(665, 26);
            this.textBoxPopulation.TabIndex = 2;
            // 
            // buttonPopulationOpen
            // 
            this.buttonPopulationOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPopulationOpen.Location = new System.Drawing.Point(684, 116);
            this.buttonPopulationOpen.Name = "buttonPopulationOpen";
            this.buttonPopulationOpen.Size = new System.Drawing.Size(114, 32);
            this.buttonPopulationOpen.TabIndex = 3;
            this.buttonPopulationOpen.Text = "Вибрати";
            this.buttonPopulationOpen.UseVisualStyleBackColor = true;
            this.buttonPopulationOpen.Click += new System.EventHandler(this.ButtonPopulationOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(812, 226);
            this.Controls.Add(this.buttonPopulationOpen);
            this.Controls.Add(this.buttonVaccOpen);
            this.Controls.Add(this.textBoxPopulation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxVaccFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Ротавірусна інфекція";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxVaccFile;
        private System.Windows.Forms.Button buttonVaccOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPopulation;
        private System.Windows.Forms.Button buttonPopulationOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

