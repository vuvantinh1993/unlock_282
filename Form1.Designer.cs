﻿
namespace unlock_282
{
    partial class Form1
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
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._2fa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.proxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Loaddata = new System.Windows.Forms.Button();
            this.nbrLuong = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStop = new System.Windows.Forms.CheckBox();
            this.ClearAll = new System.Windows.Forms.Button();
            this.opennow = new System.Windows.Forms.Button();
            this.CbStopAll = new System.Windows.Forms.CheckBox();
            this.tbkey = new System.Windows.Forms.TextBox();
            this.cbbDichVu = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.tbapiproxy = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.AllowUserToAddRows = false;
            this.dgvAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stt,
            this.uid,
            this.pass,
            this._2fa,
            this.proxy,
            this.note,
            this.status});
            this.dgvAccounts.Location = new System.Drawing.Point(13, 61);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.Size = new System.Drawing.Size(826, 477);
            this.dgvAccounts.TabIndex = 0;
            this.dgvAccounts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_ColumnHeaderMouseClick);
            this.dgvAccounts.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvAccounts_RowPrePaint);
            this.dgvAccounts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvAccounts_MouseClick);
            // 
            // stt
            // 
            this.stt.DataPropertyName = "stt";
            this.stt.HeaderText = "stt";
            this.stt.Name = "stt";
            this.stt.Width = 40;
            // 
            // uid
            // 
            this.uid.DataPropertyName = "uid";
            this.uid.HeaderText = "uid";
            this.uid.Name = "uid";
            // 
            // pass
            // 
            this.pass.DataPropertyName = "pass";
            this.pass.HeaderText = "pass";
            this.pass.Name = "pass";
            // 
            // _2fa
            // 
            this._2fa.DataPropertyName = "_2fa";
            this._2fa.HeaderText = "_2fa";
            this._2fa.Name = "_2fa";
            // 
            // proxy
            // 
            this.proxy.DataPropertyName = "proxy";
            this.proxy.HeaderText = "proxy";
            this.proxy.Name = "proxy";
            // 
            // note
            // 
            this.note.DataPropertyName = "note";
            this.note.HeaderText = "note";
            this.note.Name = "note";
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "status";
            this.status.Name = "status";
            // 
            // Loaddata
            // 
            this.Loaddata.Location = new System.Drawing.Point(751, 29);
            this.Loaddata.Name = "Loaddata";
            this.Loaddata.Size = new System.Drawing.Size(75, 23);
            this.Loaddata.TabIndex = 2;
            this.Loaddata.Text = "Thêm nhanh";
            this.Loaddata.UseVisualStyleBackColor = true;
            this.Loaddata.Click += new System.EventHandler(this.Loaddata_Click);
            // 
            // nbrLuong
            // 
            this.nbrLuong.Location = new System.Drawing.Point(65, 10);
            this.nbrLuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbrLuong.Name = "nbrLuong";
            this.nbrLuong.Size = new System.Drawing.Size(55, 20);
            this.nbrLuong.TabIndex = 3;
            this.nbrLuong.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Luồng";
            // 
            // cbStop
            // 
            this.cbStop.AutoSize = true;
            this.cbStop.Location = new System.Drawing.Point(126, 8);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(48, 17);
            this.cbStop.TabIndex = 5;
            this.cbStop.Text = "Stop";
            this.cbStop.UseVisualStyleBackColor = true;
            // 
            // ClearAll
            // 
            this.ClearAll.Location = new System.Drawing.Point(670, 32);
            this.ClearAll.Name = "ClearAll";
            this.ClearAll.Size = new System.Drawing.Size(75, 23);
            this.ClearAll.TabIndex = 6;
            this.ClearAll.Text = "Xoá hết";
            this.ClearAll.UseVisualStyleBackColor = true;
            this.ClearAll.Click += new System.EventHandler(this.ClearAll_Click);
            // 
            // opennow
            // 
            this.opennow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.opennow.ForeColor = System.Drawing.Color.Blue;
            this.opennow.Location = new System.Drawing.Point(322, 15);
            this.opennow.Name = "opennow";
            this.opennow.Size = new System.Drawing.Size(143, 37);
            this.opennow.TabIndex = 7;
            this.opennow.Text = "Giải 282 www";
            this.opennow.UseVisualStyleBackColor = false;
            this.opennow.Click += new System.EventHandler(this.opennow_Click);
            // 
            // CbStopAll
            // 
            this.CbStopAll.AutoSize = true;
            this.CbStopAll.Location = new System.Drawing.Point(180, 8);
            this.CbStopAll.Name = "CbStopAll";
            this.CbStopAll.Size = new System.Drawing.Size(71, 17);
            this.CbStopAll.TabIndex = 11;
            this.CbStopAll.Text = "dừng hẳn";
            this.CbStopAll.UseVisualStyleBackColor = true;
            // 
            // tbkey
            // 
            this.tbkey.Location = new System.Drawing.Point(211, 32);
            this.tbkey.Name = "tbkey";
            this.tbkey.Size = new System.Drawing.Size(105, 20);
            this.tbkey.TabIndex = 12;
            this.tbkey.Text = "f7f51ab4f9556ae39d68a4f1d3eb3869";
            // 
            // cbbDichVu
            // 
            this.cbbDichVu.FormattingEnabled = true;
            this.cbbDichVu.Items.AddRange(new object[] {
            "CTSC",
            "OTPSIM",
            "CODETEXTNOW"});
            this.cbbDichVu.Location = new System.Drawing.Point(127, 32);
            this.cbbDichVu.Name = "cbbDichVu";
            this.cbbDichVu.Size = new System.Drawing.Size(78, 21);
            this.cbbDichVu.TabIndex = 14;
            this.cbbDichVu.Text = "CTSC";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Số lượng";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(65, 33);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(55, 20);
            this.numericUpDown1.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(670, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Tắt Chrome";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbapiproxy
            // 
            this.tbapiproxy.Location = new System.Drawing.Point(845, 61);
            this.tbapiproxy.Multiline = true;
            this.tbapiproxy.Name = "tbapiproxy";
            this.tbapiproxy.Size = new System.Drawing.Size(165, 301);
            this.tbapiproxy.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 540);
            this.Controls.Add(this.tbapiproxy);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.cbbDichVu);
            this.Controls.Add(this.tbkey);
            this.Controls.Add(this.CbStopAll);
            this.Controls.Add(this.opennow);
            this.Controls.Add(this.ClearAll);
            this.Controls.Add(this.cbStop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nbrLuong);
            this.Controls.Add(this.Loaddata);
            this.Controls.Add(this.dgvAccounts);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.DataGridViewTextBoxColumn stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn uid;
        private System.Windows.Forms.DataGridViewTextBoxColumn pass;
        private System.Windows.Forms.DataGridViewTextBoxColumn _2fa;
        private System.Windows.Forms.DataGridViewTextBoxColumn proxy;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.Button Loaddata;
        private System.Windows.Forms.NumericUpDown nbrLuong;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbStop;
        private System.Windows.Forms.Button ClearAll;
        private System.Windows.Forms.Button opennow;
        private System.Windows.Forms.CheckBox CbStopAll;
        private System.Windows.Forms.TextBox tbkey;
        private System.Windows.Forms.ComboBox cbbDichVu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbapiproxy;
    }
}

