namespace trashwebWinForm
{
    partial class frmInputId
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
            this.txt_Id = new System.Windows.Forms.TextBox();
            this.labelId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_Id
            // 
            this.txt_Id.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_Id.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_Id.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Id.Location = new System.Drawing.Point(137, 187);
            this.txt_Id.Name = "txt_Id";
            this.txt_Id.Size = new System.Drawing.Size(477, 33);
            this.txt_Id.TabIndex = 90;
            this.txt_Id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelId
            // 
            this.labelId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelId.AutoSize = true;
            this.labelId.Font = new System.Drawing.Font("Segoe UI Semibold", 18.25F, System.Drawing.FontStyle.Bold);
            this.labelId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.labelId.Location = new System.Drawing.Point(65, 184);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(40, 35);
            this.labelId.TabIndex = 97;
            this.labelId.Text = "ID";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 18.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(65, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 35);
            this.label1.TabIndex = 98;
            this.label1.Text = "MỜI BẠN NHẬP ID";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnSubmit.ForeColor = System.Drawing.Color.Transparent;
            this.btnSubmit.Location = new System.Drawing.Point(562, 425);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(126, 43);
            this.btnSubmit.TabIndex = 99;
            this.btnSubmit.Text = "Xác Nhận";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.button1.ForeColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(414, 425);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 43);
            this.button1.TabIndex = 100;
            this.button1.Text = "Quay lại từ đầu";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmInputId
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 480);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.txt_Id);
            this.Name = "frmInputId";
            this.Text = "frmInputId";
            this.Load += new System.EventHandler(this.frmInputId_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Id;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button button1;
    }
}