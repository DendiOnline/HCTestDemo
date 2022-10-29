namespace HCTestDemo
{
    partial class LinearReg
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartLabTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Slope = new System.Windows.Forms.TextBox();
            this.Intercept = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartLabTrend)).BeginInit();
            this.SuspendLayout();
            // 
            // chartLabTrend
            // 
            chartArea4.Name = "ChartArea1";
            this.chartLabTrend.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartLabTrend.Legends.Add(legend4);
            this.chartLabTrend.Location = new System.Drawing.Point(12, 12);
            this.chartLabTrend.Name = "chartLabTrend";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartLabTrend.Series.Add(series4);
            this.chartLabTrend.Size = new System.Drawing.Size(613, 274);
            this.chartLabTrend.TabIndex = 0;
            // 
            // Slope
            // 
            this.Slope.Location = new System.Drawing.Point(73, 307);
            this.Slope.Name = "Slope";
            this.Slope.Size = new System.Drawing.Size(100, 21);
            this.Slope.TabIndex = 36;
            // 
            // Intercept
            // 
            this.Intercept.Location = new System.Drawing.Point(286, 307);
            this.Intercept.Name = "Intercept";
            this.Intercept.Size = new System.Drawing.Size(100, 21);
            this.Intercept.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 38;
            this.label1.Text = "K值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 307);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "B值";
            // 
            // LinearReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 339);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Intercept);
            this.Controls.Add(this.Slope);
            this.Controls.Add(this.chartLabTrend);
            this.Name = "LinearReg";
            this.Text = "LinearReg";
            this.Load += new System.EventHandler(this.LinearReg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartLabTrend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartLabTrend;
        private System.Windows.Forms.TextBox Slope;
        private System.Windows.Forms.TextBox Intercept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}