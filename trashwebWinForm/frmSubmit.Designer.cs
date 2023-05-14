namespace trashwebWinForm
{
    partial class frmSubmit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPrev = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRecycle = new System.Windows.Forms.Button();
            this.btnDangerous = new System.Windows.Forms.Button();
            this.btnOther = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNumTrash = new System.Windows.Forms.Label();
            this.btnOld = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 480);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(123, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(454, 383);
            this.panel4.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(454, 383);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnPrev);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(123, 383);
            this.panel5.TabIndex = 4;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(110)))), ((int)(((byte)(1)))));
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnPrev.ForeColor = System.Drawing.Color.Transparent;
            this.btnPrev.Location = new System.Drawing.Point(0, 0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(123, 383);
            this.btnPrev.TabIndex = 90;
            this.btnPrev.Text = "Quay lại";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnNext);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(577, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(123, 383);
            this.panel3.TabIndex = 2;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(110)))), ((int)(((byte)(1)))));
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnNext.ForeColor = System.Drawing.Color.Transparent;
            this.btnNext.Location = new System.Drawing.Point(0, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(123, 383);
            this.btnNext.TabIndex = 90;
            this.btnNext.Text = "Tiếp tục";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Controls.Add(this.btnOld);
            this.panel2.Controls.Add(this.lblNumTrash);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnOther);
            this.panel2.Controls.Add(this.btnDangerous);
            this.panel2.Controls.Add(this.btnRecycle);
            this.panel2.Controls.Add(this.btnLeft);
            this.panel2.Controls.Add(this.btnDone);
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 383);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 97);
            this.panel2.TabIndex = 1;
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnLeft.ForeColor = System.Drawing.Color.Transparent;
            this.btnLeft.Location = new System.Drawing.Point(435, 42);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(126, 43);
            this.btnLeft.TabIndex = 88;
            this.btnLeft.Text = "Bỏ Qua";
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnDone.FlatAppearance.BorderSize = 0;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnDone.ForeColor = System.Drawing.Color.Transparent;
            this.btnDone.Location = new System.Drawing.Point(567, 42);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(126, 43);
            this.btnDone.TabIndex = 86;
            this.btnDone.Text = "Xác Nhận";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F);
            this.btnEdit.ForeColor = System.Drawing.Color.Transparent;
            this.btnEdit.Location = new System.Drawing.Point(303, 42);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(126, 43);
            this.btnEdit.TabIndex = 98;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRecycle
            // 
            this.btnRecycle.Enabled = false;
            this.btnRecycle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecycle.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.btnRecycle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnRecycle.Location = new System.Drawing.Point(12, 41);
            this.btnRecycle.Name = "btnRecycle";
            this.btnRecycle.Size = new System.Drawing.Size(79, 43);
            this.btnRecycle.TabIndex = 99;
            this.btnRecycle.Text = "Recycle";
            this.btnRecycle.UseVisualStyleBackColor = true;
            this.btnRecycle.Click += new System.EventHandler(this.btnRecycle_Click);
            // 
            // btnDangerous
            // 
            this.btnDangerous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangerous.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.btnDangerous.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnDangerous.Location = new System.Drawing.Point(97, 41);
            this.btnDangerous.Name = "btnDangerous";
            this.btnDangerous.Size = new System.Drawing.Size(108, 43);
            this.btnDangerous.TabIndex = 100;
            this.btnDangerous.Text = "Dangerous";
            this.btnDangerous.UseVisualStyleBackColor = true;
            this.btnDangerous.Visible = false;
            this.btnDangerous.Click += new System.EventHandler(this.btnDangerous_Click);
            // 
            // btnOther
            // 
            this.btnOther.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOther.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.btnOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnOther.Location = new System.Drawing.Point(211, 41);
            this.btnOther.Name = "btnOther";
            this.btnOther.Size = new System.Drawing.Size(79, 43);
            this.btnOther.TabIndex = 101;
            this.btnOther.Text = "Other";
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Visible = false;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(310, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 17);
            this.label1.TabIndex = 102;
            this.label1.Text = "Tổng số rác đã vứt: ";
            // 
            // lblNumTrash
            // 
            this.lblNumTrash.AutoSize = true;
            this.lblNumTrash.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25F, System.Drawing.FontStyle.Bold);
            this.lblNumTrash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.lblNumTrash.Location = new System.Drawing.Point(445, 13);
            this.lblNumTrash.Name = "lblNumTrash";
            this.lblNumTrash.Size = new System.Drawing.Size(19, 17);
            this.lblNumTrash.TabIndex = 103;
            this.lblNumTrash.Text = " 0";
            // 
            // btnOld
            // 
            this.btnOld.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnOld.FlatAppearance.BorderSize = 0;
            this.btnOld.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOld.ForeColor = System.Drawing.Color.Transparent;
            this.btnOld.Location = new System.Drawing.Point(502, 9);
            this.btnOld.Name = "btnOld";
            this.btnOld.Size = new System.Drawing.Size(75, 23);
            this.btnOld.TabIndex = 104;
            this.btnOld.Text = "Load Old";
            this.btnOld.UseVisualStyleBackColor = false;
            this.btnOld.Click += new System.EventHandler(this.btnOld_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(88)))), ((int)(((byte)(67)))));
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.Transparent;
            this.btnNew.Location = new System.Drawing.Point(583, 9);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 105;
            this.btnNew.Text = "Load New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // frmSubmit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 480);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(716, 519);
            this.MinimumSize = new System.Drawing.Size(716, 519);
            this.Name = "frmSubmit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Submit";
            this.Load += new System.EventHandler(this.frmSubmit_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRecycle;
        private System.Windows.Forms.Button btnOther;
        private System.Windows.Forms.Button btnDangerous;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNumTrash;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnOld;
    }
}