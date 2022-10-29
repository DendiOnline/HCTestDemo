namespace HCTestDemo
{
    partial class ParameModifica
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMsgList = new Sunny.UI.UIDataGridView();
            this.label78 = new System.Windows.Forms.Label();
            this.uiRoundProcess1 = new Sunny.UI.UIRoundProcess();
            this.excelPath = new System.Windows.Forms.TextBox();
            this.label79 = new System.Windows.Forms.Label();
            this.bt_WriteMsg = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.btnReadMsg = new System.Windows.Forms.Button();
            this.btClearMsg = new System.Windows.Forms.Button();
            this.chooseFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsgList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMsgList
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvMsgList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMsgList.BackgroundColor = System.Drawing.Color.White;
            this.dgvMsgList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMsgList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMsgList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMsgList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMsgList.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvMsgList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvMsgList.EnableHeadersVisualStyles = false;
            this.dgvMsgList.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dgvMsgList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvMsgList.Location = new System.Drawing.Point(0, 0);
            this.dgvMsgList.Margin = new System.Windows.Forms.Padding(0);
            this.dgvMsgList.Name = "dgvMsgList";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMsgList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvMsgList.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvMsgList.RowTemplate.Height = 23;
            this.dgvMsgList.SelectedIndex = -1;
            this.dgvMsgList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMsgList.ShowGridLine = true;
            this.dgvMsgList.Size = new System.Drawing.Size(848, 414);
            this.dgvMsgList.Style = Sunny.UI.UIStyle.Custom;
            this.dgvMsgList.TabIndex = 49;
            this.dgvMsgList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMsgList_CellContentClick);
            this.dgvMsgList.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMsgList_CellContentDoubleClick);
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(642, 449);
            this.label78.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(65, 12);
            this.label78.TabIndex = 90;
            this.label78.Text = "读写进度：";
            // 
            // uiRoundProcess1
            // 
            this.uiRoundProcess1.BackColor = System.Drawing.Color.Transparent;
            this.uiRoundProcess1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRoundProcess1.Location = new System.Drawing.Point(660, 449);
            this.uiRoundProcess1.Margin = new System.Windows.Forms.Padding(2);
            this.uiRoundProcess1.Maximum = 48;
            this.uiRoundProcess1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiRoundProcess1.Name = "uiRoundProcess1";
            this.uiRoundProcess1.ShowProcess = true;
            this.uiRoundProcess1.Size = new System.Drawing.Size(120, 120);
            this.uiRoundProcess1.Style = Sunny.UI.UIStyle.Custom;
            this.uiRoundProcess1.TabIndex = 89;
            this.uiRoundProcess1.Text = "0.0%";
            // 
            // excelPath
            // 
            this.excelPath.Location = new System.Drawing.Point(13, 449);
            this.excelPath.Margin = new System.Windows.Forms.Padding(2);
            this.excelPath.Name = "excelPath";
            this.excelPath.Size = new System.Drawing.Size(560, 21);
            this.excelPath.TabIndex = 88;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(11, 435);
            this.label79.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(29, 12);
            this.label79.TabIndex = 87;
            this.label79.Text = "地址";
            // 
            // bt_WriteMsg
            // 
            this.bt_WriteMsg.Location = new System.Drawing.Point(126, 551);
            this.bt_WriteMsg.Margin = new System.Windows.Forms.Padding(2);
            this.bt_WriteMsg.Name = "bt_WriteMsg";
            this.bt_WriteMsg.Size = new System.Drawing.Size(74, 23);
            this.bt_WriteMsg.TabIndex = 86;
            this.bt_WriteMsg.Text = "写入";
            this.bt_WriteMsg.UseVisualStyleBackColor = true;
            this.bt_WriteMsg.Click += new System.EventHandler(this.bt_WriteMsg_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Location = new System.Drawing.Point(13, 551);
            this.SelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(74, 23);
            this.SelectAll.TabIndex = 85;
            this.SelectAll.Text = "全不选";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // btnReadMsg
            // 
            this.btnReadMsg.Location = new System.Drawing.Point(126, 496);
            this.btnReadMsg.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadMsg.Name = "btnReadMsg";
            this.btnReadMsg.Size = new System.Drawing.Size(74, 23);
            this.btnReadMsg.TabIndex = 84;
            this.btnReadMsg.Text = "读取";
            this.btnReadMsg.UseVisualStyleBackColor = true;
            this.btnReadMsg.Click += new System.EventHandler(this.btnReadMsg_Click);
            // 
            // btClearMsg
            // 
            this.btClearMsg.Location = new System.Drawing.Point(229, 496);
            this.btClearMsg.Margin = new System.Windows.Forms.Padding(2);
            this.btClearMsg.Name = "btClearMsg";
            this.btClearMsg.Size = new System.Drawing.Size(74, 23);
            this.btClearMsg.TabIndex = 83;
            this.btClearMsg.Text = "清空";
            this.btClearMsg.UseVisualStyleBackColor = true;
            this.btClearMsg.Click += new System.EventHandler(this.btClearMsg_Click);
            // 
            // chooseFile
            // 
            this.chooseFile.Location = new System.Drawing.Point(13, 496);
            this.chooseFile.Margin = new System.Windows.Forms.Padding(2);
            this.chooseFile.Name = "chooseFile";
            this.chooseFile.Size = new System.Drawing.Size(74, 23);
            this.chooseFile.TabIndex = 82;
            this.chooseFile.Text = "选择";
            this.chooseFile.UseVisualStyleBackColor = true;
            this.chooseFile.Click += new System.EventHandler(this.chooseFile_Click);
            // 
            // ParameModifica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.label78);
            this.Controls.Add(this.uiRoundProcess1);
            this.Controls.Add(this.excelPath);
            this.Controls.Add(this.label79);
            this.Controls.Add(this.bt_WriteMsg);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.btnReadMsg);
            this.Controls.Add(this.btClearMsg);
            this.Controls.Add(this.chooseFile);
            this.Controls.Add(this.dgvMsgList);
            this.Name = "ParameModifica";
            this.Size = new System.Drawing.Size(848, 613);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMsgList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UIDataGridView dgvMsgList;
        private System.Windows.Forms.Label label78;
        private Sunny.UI.UIRoundProcess uiRoundProcess1;
        private System.Windows.Forms.TextBox excelPath;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Button bt_WriteMsg;
        private System.Windows.Forms.Button SelectAll;
        private System.Windows.Forms.Button btnReadMsg;
        private System.Windows.Forms.Button btClearMsg;
        private System.Windows.Forms.Button chooseFile;
    }
}
