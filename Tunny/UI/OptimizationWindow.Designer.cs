
namespace Tunny.UI
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
            this.samplerComboBox = new System.Windows.Forms.ComboBox();
            this.SamplerTypeText = new System.Windows.Forms.Label();
            this.studyNameLabel = new System.Windows.Forms.Label();
            this.studyNameTextBox = new System.Windows.Forms.TextBox();
            this.optimizeTabControl = new System.Windows.Forms.TabControl();
            this.optimizeTabPage = new System.Windows.Forms.TabPage();
            this.resultTabPage = new System.Windows.Forms.TabPage();
            this.visualizeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.visualizeTypeLabel = new System.Windows.Forms.Label();
            this.VisualizeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.optimizeTabControl.SuspendLayout();
            this.optimizeTabPage.SuspendLayout();
            this.resultTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // runOptimizeButton
            // 
            this.runOptimizeButton.Location = new System.Drawing.Point(11, 134);
            this.runOptimizeButton.Name = "runOptimizeButton";
            this.runOptimizeButton.Size = new System.Drawing.Size(103, 23);
            this.runOptimizeButton.TabIndex = 0;
            this.runOptimizeButton.Text = "RunOptimize";
            this.runOptimizeButton.UseVisualStyleBackColor = true;
            this.runOptimizeButton.Click += new System.EventHandler(this.ButtonRunOptimize_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(133, 134);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // nTrialNumUpDown
            // 
            this.nTrialNumUpDown.Location = new System.Drawing.Point(112, 32);
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
            // 
            // nTrialText
            // 
            this.nTrialText.AutoSize = true;
            this.nTrialText.Location = new System.Drawing.Point(9, 34);
            this.nTrialText.Name = "nTrialText";
            this.nTrialText.Size = new System.Drawing.Size(82, 12);
            this.nTrialText.TabIndex = 3;
            this.nTrialText.Text = "Number of trial";
            // 
            // loadIfExistsCheckBox
            // 
            this.loadIfExistsCheckBox.AutoSize = true;
            this.loadIfExistsCheckBox.Checked = true;
            this.loadIfExistsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadIfExistsCheckBox.Location = new System.Drawing.Point(11, 72);
            this.loadIfExistsCheckBox.Name = "loadIfExistsCheckBox";
            this.loadIfExistsCheckBox.Size = new System.Drawing.Size(146, 16);
            this.loadIfExistsCheckBox.TabIndex = 5;
            this.loadIfExistsCheckBox.Text = "Load if study file exists";
            this.loadIfExistsCheckBox.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(11, 163);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(195, 23);
            this.progressBar.TabIndex = 6;
            // 
            // samplerComboBox
            // 
            this.samplerComboBox.FormattingEnabled = true;
            this.samplerComboBox.Items.AddRange(new object[] {
            "TPE",
            "NSGAII",
            "MOTPE"});
            this.samplerComboBox.Location = new System.Drawing.Point(85, 6);
            this.samplerComboBox.Name = "samplerComboBox";
            this.samplerComboBox.Size = new System.Drawing.Size(121, 20);
            this.samplerComboBox.TabIndex = 7;
            // 
            // SamplerTypeText
            // 
            this.SamplerTypeText.AutoSize = true;
            this.SamplerTypeText.Location = new System.Drawing.Point(11, 9);
            this.SamplerTypeText.Name = "SamplerTypeText";
            this.SamplerTypeText.Size = new System.Drawing.Size(46, 12);
            this.SamplerTypeText.TabIndex = 8;
            this.SamplerTypeText.Text = "Sampler";
            // 
            // studyNameLabel
            // 
            this.studyNameLabel.AutoSize = true;
            this.studyNameLabel.Location = new System.Drawing.Point(9, 92);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(67, 12);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Study Name";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(87, 89);
            this.studyNameTextBox.Name = "studyNameTextBox";
            this.studyNameTextBox.Size = new System.Drawing.Size(119, 19);
            this.studyNameTextBox.TabIndex = 10;
            this.studyNameTextBox.Text = "study1";
            // 
            // optimizeTabControl
            // 
            this.optimizeTabControl.Controls.Add(this.optimizeTabPage);
            this.optimizeTabControl.Controls.Add(this.resultTabPage);
            this.optimizeTabControl.Location = new System.Drawing.Point(12, 12);
            this.optimizeTabControl.Name = "optimizeTabControl";
            this.optimizeTabControl.SelectedIndex = 0;
            this.optimizeTabControl.Size = new System.Drawing.Size(221, 223);
            this.optimizeTabControl.TabIndex = 11;
            // 
            // optimizeTabPage
            // 
            this.optimizeTabPage.Controls.Add(this.studyNameTextBox);
            this.optimizeTabPage.Controls.Add(this.samplerComboBox);
            this.optimizeTabPage.Controls.Add(this.studyNameLabel);
            this.optimizeTabPage.Controls.Add(this.runOptimizeButton);
            this.optimizeTabPage.Controls.Add(this.SamplerTypeText);
            this.optimizeTabPage.Controls.Add(this.stopButton);
            this.optimizeTabPage.Controls.Add(this.nTrialNumUpDown);
            this.optimizeTabPage.Controls.Add(this.progressBar);
            this.optimizeTabPage.Controls.Add(this.nTrialText);
            this.optimizeTabPage.Controls.Add(this.loadIfExistsCheckBox);
            this.optimizeTabPage.Location = new System.Drawing.Point(4, 22);
            this.optimizeTabPage.Name = "optimizeTabPage";
            this.optimizeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.optimizeTabPage.Size = new System.Drawing.Size(213, 197);
            this.optimizeTabPage.TabIndex = 0;
            this.optimizeTabPage.Text = "Optimize";
            this.optimizeTabPage.UseVisualStyleBackColor = true;
            // 
            // resultTabPage
            // 
            this.resultTabPage.Controls.Add(this.VisualizeButton);
            this.resultTabPage.Controls.Add(this.visualizeTypeLabel);
            this.resultTabPage.Controls.Add(this.visualizeTypeComboBox);
            this.resultTabPage.Location = new System.Drawing.Point(4, 22);
            this.resultTabPage.Name = "resultTabPage";
            this.resultTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.resultTabPage.Size = new System.Drawing.Size(213, 197);
            this.resultTabPage.TabIndex = 1;
            this.resultTabPage.Text = "Result";
            this.resultTabPage.UseVisualStyleBackColor = true;
            // 
            // visualizeTypeComboBox
            // 
            this.visualizeTypeComboBox.FormattingEnabled = true;
            this.visualizeTypeComboBox.Items.AddRange(new object[] {
            "contour",
            "edf",
            "intermediate values",
            "optimization history",
            "parallel coordinate",
            "param importances",
            "pareto front",
            "slice"});
            this.visualizeTypeComboBox.Location = new System.Drawing.Point(38, 28);
            this.visualizeTypeComboBox.Name = "visualizeTypeComboBox";
            this.visualizeTypeComboBox.Size = new System.Drawing.Size(150, 20);
            this.visualizeTypeComboBox.TabIndex = 0;
            // 
            // visualizeTypeLabel
            // 
            this.visualizeTypeLabel.AutoSize = true;
            this.visualizeTypeLabel.Location = new System.Drawing.Point(3, 9);
            this.visualizeTypeLabel.Name = "visualizeTypeLabel";
            this.visualizeTypeLabel.Size = new System.Drawing.Size(77, 12);
            this.visualizeTypeLabel.TabIndex = 1;
            this.visualizeTypeLabel.Text = "Visualize type";
            // 
            // VisualizeButton
            // 
            this.VisualizeButton.Location = new System.Drawing.Point(6, 54);
            this.VisualizeButton.Name = "VisualizeButton";
            this.VisualizeButton.Size = new System.Drawing.Size(75, 23);
            this.VisualizeButton.TabIndex = 2;
            this.VisualizeButton.Text = "Show";
            this.VisualizeButton.UseVisualStyleBackColor = true;
            this.VisualizeButton.Click += new System.EventHandler(this.VisualizeButton_Click);
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 245);
            this.Controls.Add(this.optimizeTabControl);
            this.Name = "OptimizationWindow";
            this.Text = "Opt";
            this.Load += new System.EventHandler(this.OptimizationWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).EndInit();
            this.optimizeTabControl.ResumeLayout(false);
            this.optimizeTabPage.ResumeLayout(false);
            this.optimizeTabPage.PerformLayout();
            this.resultTabPage.ResumeLayout(false);
            this.resultTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button runOptimizeButton;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSolver;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown nTrialNumUpDown;
        private System.Windows.Forms.CheckBox loadIfExistsCheckBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox samplerComboBox;
        private System.Windows.Forms.Label SamplerTypeText;
        private System.Windows.Forms.Label nTrialText;
        private System.Windows.Forms.Label studyNameLabel;
        private System.Windows.Forms.TextBox studyNameTextBox;
        private System.Windows.Forms.TabControl optimizeTabControl;
        private System.Windows.Forms.TabPage optimizeTabPage;
        private System.Windows.Forms.TabPage resultTabPage;
        private System.Windows.Forms.Button VisualizeButton;
        private System.Windows.Forms.Label visualizeTypeLabel;
        private System.Windows.Forms.ComboBox visualizeTypeComboBox;
    }
}

