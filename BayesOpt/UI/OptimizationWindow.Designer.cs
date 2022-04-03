
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
            this.runOptimizeButton = new System.Windows.Forms.Button();
            this.backgroundWorkerSolver = new System.ComponentModel.BackgroundWorker();
            this.stopButton = new System.Windows.Forms.Button();
            this.nTrialNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.nTrialText = new System.Windows.Forms.Label();
            this.loadIfExistsCheckBox = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SamplerComboBox = new System.Windows.Forms.ComboBox();
            this.SamplerTypeText = new System.Windows.Forms.Label();
            this.studyNameLabel = new System.Windows.Forms.Label();
            this.studyNameTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // runOptimizeButton
            // 
            this.runOptimizeButton.Location = new System.Drawing.Point(12, 140);
            this.runOptimizeButton.Name = "runOptimizeButton";
            this.runOptimizeButton.Size = new System.Drawing.Size(103, 23);
            this.runOptimizeButton.TabIndex = 0;
            this.runOptimizeButton.Text = "RunOptimize";
            this.runOptimizeButton.UseVisualStyleBackColor = true;
            this.runOptimizeButton.Click += new System.EventHandler(this.ButtonRunOptimize_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(134, 140);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // nTrialNumUpDown
            // 
            this.nTrialNumUpDown.Location = new System.Drawing.Point(113, 38);
            this.nTrialNumUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nTrialNumUpDown.Name = "nTrialNumUpDown";
            this.nTrialNumUpDown.Size = new System.Drawing.Size(94, 19);
            this.nTrialNumUpDown.TabIndex = 2;
            this.nTrialNumUpDown.ThousandsSeparator = true;
            this.nTrialNumUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nTrialNumUpDown.ValueChanged += new System.EventHandler(this.nTrialNumUpDown_ValueChanged);
            // 
            // nTrialText
            // 
            this.nTrialText.AutoSize = true;
            this.nTrialText.Location = new System.Drawing.Point(10, 40);
            this.nTrialText.Name = "nTrialText";
            this.nTrialText.Size = new System.Drawing.Size(82, 12);
            this.nTrialText.TabIndex = 3;
            this.nTrialText.Text = "Number of trial";
            this.nTrialText.Click += new System.EventHandler(this.label1_Click);
            // 
            // loadIfExistsCheckBox
            // 
            this.loadIfExistsCheckBox.AutoSize = true;
            this.loadIfExistsCheckBox.Location = new System.Drawing.Point(12, 78);
            this.loadIfExistsCheckBox.Name = "loadIfExistsCheckBox";
            this.loadIfExistsCheckBox.Size = new System.Drawing.Size(146, 16);
            this.loadIfExistsCheckBox.TabIndex = 5;
            this.loadIfExistsCheckBox.Text = "Load if study file exists";
            this.loadIfExistsCheckBox.UseVisualStyleBackColor = true;
            this.loadIfExistsCheckBox.CheckedChanged += new System.EventHandler(this.LoadIfExists_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 169);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(195, 23);
            this.progressBar.TabIndex = 6;
            // 
            // SamplerComboBox
            // 
            this.SamplerComboBox.FormattingEnabled = true;
            this.SamplerComboBox.Items.AddRange(new object[] {
            "TPE",
            "NSGAII",
            "MOTPE"});
            this.SamplerComboBox.Location = new System.Drawing.Point(88, 12);
            this.SamplerComboBox.Name = "SamplerComboBox";
            this.SamplerComboBox.Size = new System.Drawing.Size(121, 20);
            this.SamplerComboBox.TabIndex = 7;
            // 
            // SamplerTypeText
            // 
            this.SamplerTypeText.AutoSize = true;
            this.SamplerTypeText.Location = new System.Drawing.Point(12, 15);
            this.SamplerTypeText.Name = "SamplerTypeText";
            this.SamplerTypeText.Size = new System.Drawing.Size(46, 12);
            this.SamplerTypeText.TabIndex = 8;
            this.SamplerTypeText.Text = "Sampler";
            // 
            // studyNameLabel
            // 
            this.studyNameLabel.AutoSize = true;
            this.studyNameLabel.Location = new System.Drawing.Point(10, 98);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(67, 12);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Study Name";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(88, 95);
            this.studyNameTextBox.Name = "studyNameTextBox";
            this.studyNameTextBox.Size = new System.Drawing.Size(119, 19);
            this.studyNameTextBox.TabIndex = 10;
            this.studyNameTextBox.Text = "study1";
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 204);
            this.Controls.Add(this.studyNameTextBox);
            this.Controls.Add(this.studyNameLabel);
            this.Controls.Add(this.SamplerTypeText);
            this.Controls.Add(this.SamplerComboBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.loadIfExistsCheckBox);
            this.Controls.Add(this.nTrialText);
            this.Controls.Add(this.nTrialNumUpDown);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.runOptimizeButton);
            this.Name = "OptimizationWindow";
            this.Text = "Opt";
            this.Load += new System.EventHandler(this.OptimizationWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runOptimizeButton;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSolver;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown nTrialNumUpDown;
        private System.Windows.Forms.CheckBox loadIfExistsCheckBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox SamplerComboBox;
        private System.Windows.Forms.Label SamplerTypeText;
        private System.Windows.Forms.Label nTrialText;
        private System.Windows.Forms.Label studyNameLabel;
        private System.Windows.Forms.TextBox studyNameTextBox;
    }
}

