
namespace BayesOpt.UI
{
    partial class OptimizationWindow
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.RunOptimize = new System.Windows.Forms.Button();
            this.backgroundWorkerSolver = new System.ComponentModel.BackgroundWorker();
            this.Stop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RunOptimize
            // 
            this.RunOptimize.Location = new System.Drawing.Point(26, 43);
            this.RunOptimize.Name = "RunOptimize";
            this.RunOptimize.Size = new System.Drawing.Size(103, 23);
            this.RunOptimize.TabIndex = 0;
            this.RunOptimize.Text = "RunOptimize";
            this.RunOptimize.UseVisualStyleBackColor = true;
            this.RunOptimize.Click += new System.EventHandler(this.ButtonRunOptimize_Click);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(163, 43);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 1;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 102);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.RunOptimize);
            this.Name = "OptimizationWindow";
            this.Text = "Optimization Window";
            this.Load += new System.EventHandler(this.OptimizationWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RunOptimize;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSolver;
        private System.Windows.Forms.Button Stop;
    }
}

