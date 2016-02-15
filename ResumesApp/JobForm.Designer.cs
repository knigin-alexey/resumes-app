namespace ResumesApp
{
    partial class JobForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tBxJobName = new System.Windows.Forms.TextBox();
            this.tBxDismissReason = new System.Windows.Forms.TextBox();
            this.dTPReceiptDate = new System.Windows.Forms.DateTimePicker();
            this.dTPDissmissDate = new System.Windows.Forms.DateTimePicker();
            this.bntConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Место работы";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Дата поступления";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Дата увольнения";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Причина увольнения";
            // 
            // tBxJobName
            // 
            this.tBxJobName.Location = new System.Drawing.Point(137, 6);
            this.tBxJobName.Name = "tBxJobName";
            this.tBxJobName.Size = new System.Drawing.Size(200, 20);
            this.tBxJobName.TabIndex = 4;
            // 
            // tBxDismissReason
            // 
            this.tBxDismissReason.Location = new System.Drawing.Point(137, 100);
            this.tBxDismissReason.Name = "tBxDismissReason";
            this.tBxDismissReason.Size = new System.Drawing.Size(200, 20);
            this.tBxDismissReason.TabIndex = 5;
            // 
            // dTPReceiptDate
            // 
            this.dTPReceiptDate.Location = new System.Drawing.Point(137, 32);
            this.dTPReceiptDate.Name = "dTPReceiptDate";
            this.dTPReceiptDate.Size = new System.Drawing.Size(200, 20);
            this.dTPReceiptDate.TabIndex = 6;
            // 
            // dTPDissmissDate
            // 
            this.dTPDissmissDate.Location = new System.Drawing.Point(137, 58);
            this.dTPDissmissDate.Name = "dTPDissmissDate";
            this.dTPDissmissDate.Size = new System.Drawing.Size(200, 20);
            this.dTPDissmissDate.TabIndex = 7;
            // 
            // bntConfirm
            // 
            this.bntConfirm.Location = new System.Drawing.Point(182, 126);
            this.bntConfirm.Name = "bntConfirm";
            this.bntConfirm.Size = new System.Drawing.Size(75, 23);
            this.bntConfirm.TabIndex = 8;
            this.bntConfirm.Text = "OK";
            this.bntConfirm.UseVisualStyleBackColor = true;
            this.bntConfirm.Click += new System.EventHandler(this.bntConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(263, 126);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(137, 79);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(134, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "По настоящее время";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // JobForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 158);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntConfirm);
            this.Controls.Add(this.dTPDissmissDate);
            this.Controls.Add(this.dTPReceiptDate);
            this.Controls.Add(this.tBxDismissReason);
            this.Controls.Add(this.tBxJobName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "JobForm";
            this.Text = "JobForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBxJobName;
        private System.Windows.Forms.TextBox tBxDismissReason;
        private System.Windows.Forms.DateTimePicker dTPReceiptDate;
        private System.Windows.Forms.DateTimePicker dTPDissmissDate;
        private System.Windows.Forms.Button bntConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}