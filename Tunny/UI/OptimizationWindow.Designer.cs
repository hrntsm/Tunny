
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizationWindow));
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
            this.RestoreButton = new System.Windows.Forms.Button();
            this.restoreModelNumTextBox = new System.Windows.Forms.TextBox();
            this.restoreModelLabel = new System.Windows.Forms.Label();
            this.openResultFolderButton = new System.Windows.Forms.Button();
            this.clearResultButton = new System.Windows.Forms.Button();
            this.VisualizeButton = new System.Windows.Forms.Button();
            this.visualizeTypeLabel = new System.Windows.Forms.Label();
            this.visualizeTypeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.optimizeTabControl.SuspendLayout();
            this.optimizeTabPage.SuspendLayout();
            this.resultTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // runOptimizeButton
            // 
            this.runOptimizeButton.Location = new System.Drawing.Point(13, 168);
            this.runOptimizeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.runOptimizeButton.Name = "runOptimizeButton";
            this.runOptimizeButton.Size = new System.Drawing.Size(120, 29);
            this.runOptimizeButton.TabIndex = 0;
            this.runOptimizeButton.Text = "RunOptimize";
            this.runOptimizeButton.UseVisualStyleBackColor = true;
            this.runOptimizeButton.Click += new System.EventHandler(this.ButtonRunOptimize_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(155, 168);
            this.stopButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(87, 29);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // nTrialNumUpDown
            // 
            this.nTrialNumUpDown.Location = new System.Drawing.Point(131, 40);
            this.nTrialNumUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nTrialNumUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nTrialNumUpDown.Name = "nTrialNumUpDown";
            this.nTrialNumUpDown.Size = new System.Drawing.Size(110, 23);
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
            this.nTrialText.Location = new System.Drawing.Point(10, 42);
            this.nTrialText.Name = "nTrialText";
            this.nTrialText.Size = new System.Drawing.Size(96, 15);
            this.nTrialText.TabIndex = 3;
            this.nTrialText.Text = "Number of trial";
            // 
            // loadIfExistsCheckBox
            // 
            this.loadIfExistsCheckBox.AutoSize = true;
            this.loadIfExistsCheckBox.Checked = true;
            this.loadIfExistsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadIfExistsCheckBox.Location = new System.Drawing.Point(13, 90);
            this.loadIfExistsCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.loadIfExistsCheckBox.Name = "loadIfExistsCheckBox";
            this.loadIfExistsCheckBox.Size = new System.Drawing.Size(160, 19);
            this.loadIfExistsCheckBox.TabIndex = 5;
            this.loadIfExistsCheckBox.Text = "Load if study file exists";
            this.loadIfExistsCheckBox.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 204);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(227, 29);
            this.progressBar.TabIndex = 6;
            // 
            // samplerComboBox
            // 
            this.samplerComboBox.FormattingEnabled = true;
            this.samplerComboBox.Items.AddRange(new object[] {
            "TPE",
            "NSGA-II",
            "CMA-ES",
            "Random"});
            this.samplerComboBox.Location = new System.Drawing.Point(99, 8);
            this.samplerComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.samplerComboBox.Name = "samplerComboBox";
            this.samplerComboBox.Size = new System.Drawing.Size(140, 23);
            this.samplerComboBox.TabIndex = 7;
            // 
            // SamplerTypeText
            // 
            this.SamplerTypeText.AutoSize = true;
            this.SamplerTypeText.Location = new System.Drawing.Point(10, 11);
            this.SamplerTypeText.Name = "SamplerTypeText";
            this.SamplerTypeText.Size = new System.Drawing.Size(56, 15);
            this.SamplerTypeText.TabIndex = 8;
            this.SamplerTypeText.Text = "Sampler";
            // 
            // studyNameLabel
            // 
            this.studyNameLabel.AutoSize = true;
            this.studyNameLabel.Location = new System.Drawing.Point(10, 115);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(80, 15);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Study Name";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(101, 111);
            this.studyNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.studyNameTextBox.Name = "studyNameTextBox";
            this.studyNameTextBox.Size = new System.Drawing.Size(138, 23);
            this.studyNameTextBox.TabIndex = 10;
            this.studyNameTextBox.Text = "study1";
            // 
            // optimizeTabControl
            // 
            this.optimizeTabControl.Controls.Add(this.optimizeTabPage);
            this.optimizeTabControl.Controls.Add(this.resultTabPage);
            this.optimizeTabControl.Location = new System.Drawing.Point(14, 15);
            this.optimizeTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabControl.Name = "optimizeTabControl";
            this.optimizeTabControl.SelectedIndex = 0;
            this.optimizeTabControl.Size = new System.Drawing.Size(258, 270);
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
            this.optimizeTabPage.Location = new System.Drawing.Point(4, 24);
            this.optimizeTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabPage.Name = "optimizeTabPage";
            this.optimizeTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabPage.Size = new System.Drawing.Size(250, 242);
            this.optimizeTabPage.TabIndex = 0;
            this.optimizeTabPage.Text = "Optimize";
            this.optimizeTabPage.UseVisualStyleBackColor = true;
            // 
            // resultTabPage
            // 
            this.resultTabPage.Controls.Add(this.RestoreButton);
            this.resultTabPage.Controls.Add(this.restoreModelNumTextBox);
            this.resultTabPage.Controls.Add(this.restoreModelLabel);
            this.resultTabPage.Controls.Add(this.openResultFolderButton);
            this.resultTabPage.Controls.Add(this.clearResultButton);
            this.resultTabPage.Controls.Add(this.VisualizeButton);
            this.resultTabPage.Controls.Add(this.visualizeTypeLabel);
            this.resultTabPage.Controls.Add(this.visualizeTypeComboBox);
            this.resultTabPage.Location = new System.Drawing.Point(4, 24);
            this.resultTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resultTabPage.Name = "resultTabPage";
            this.resultTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resultTabPage.Size = new System.Drawing.Size(250, 242);
            this.resultTabPage.TabIndex = 1;
            this.resultTabPage.Text = "Result";
            this.resultTabPage.UseVisualStyleBackColor = true;
            // 
            // RestoreButton
            // 
            this.RestoreButton.Location = new System.Drawing.Point(172, 173);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(75, 23);
            this.RestoreButton.TabIndex = 7;
            this.RestoreButton.Text = "Restore";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // restoreModelNumTextBox
            // 
            this.restoreModelNumTextBox.Location = new System.Drawing.Point(6, 174);
            this.restoreModelNumTextBox.Name = "restoreModelNumTextBox";
            this.restoreModelNumTextBox.Size = new System.Drawing.Size(158, 23);
            this.restoreModelNumTextBox.TabIndex = 6;
            this.restoreModelNumTextBox.Text = "-1";
            // 
            // restoreModelLabel
            // 
            this.restoreModelLabel.AutoSize = true;
            this.restoreModelLabel.Location = new System.Drawing.Point(7, 156);
            this.restoreModelLabel.Name = "restoreModelLabel";
            this.restoreModelLabel.Size = new System.Drawing.Size(162, 15);
            this.restoreModelLabel.TabIndex = 5;
            this.restoreModelLabel.Text = "Set restore model number";
            // 
            // openResultFolderButton
            // 
            this.openResultFolderButton.Location = new System.Drawing.Point(31, 61);
            this.openResultFolderButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.openResultFolderButton.Name = "openResultFolderButton";
            this.openResultFolderButton.Size = new System.Drawing.Size(176, 29);
            this.openResultFolderButton.TabIndex = 4;
            this.openResultFolderButton.Text = "Open result file folder";
            this.openResultFolderButton.UseVisualStyleBackColor = true;
            this.openResultFolderButton.Click += new System.EventHandler(this.OpenResultFolderButton_Click);
            // 
            // clearResultButton
            // 
            this.clearResultButton.Location = new System.Drawing.Point(31, 97);
            this.clearResultButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clearResultButton.Name = "clearResultButton";
            this.clearResultButton.Size = new System.Drawing.Size(176, 29);
            this.clearResultButton.TabIndex = 3;
            this.clearResultButton.Text = "Clear result flie";
            this.clearResultButton.UseVisualStyleBackColor = true;
            this.clearResultButton.Click += new System.EventHandler(this.ClearResultButton_Click);
            // 
            // VisualizeButton
            // 
            this.VisualizeButton.Location = new System.Drawing.Point(187, 30);
            this.VisualizeButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VisualizeButton.Name = "VisualizeButton";
            this.VisualizeButton.Size = new System.Drawing.Size(57, 23);
            this.VisualizeButton.TabIndex = 2;
            this.VisualizeButton.Text = "Show";
            this.VisualizeButton.UseVisualStyleBackColor = true;
            this.VisualizeButton.Click += new System.EventHandler(this.VisualizeButton_Click);
            // 
            // visualizeTypeLabel
            // 
            this.visualizeTypeLabel.AutoSize = true;
            this.visualizeTypeLabel.Location = new System.Drawing.Point(3, 4);
            this.visualizeTypeLabel.Name = "visualizeTypeLabel";
            this.visualizeTypeLabel.Size = new System.Drawing.Size(87, 15);
            this.visualizeTypeLabel.TabIndex = 1;
            this.visualizeTypeLabel.Text = "Visualize type";
            // 
            // visualizeTypeComboBox
            // 
            this.visualizeTypeComboBox.FormattingEnabled = true;
            this.visualizeTypeComboBox.Items.AddRange(new object[] {
            "contour",
            "EDF",
            "intermediate values",
            "optimization history",
            "parallel coordinate",
            "param importances",
            "pareto front",
            "slice"});
            this.visualizeTypeComboBox.Location = new System.Drawing.Point(6, 30);
            this.visualizeTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visualizeTypeComboBox.Name = "visualizeTypeComboBox";
            this.visualizeTypeComboBox.Size = new System.Drawing.Size(175, 23);
            this.visualizeTypeComboBox.TabIndex = 0;
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 298);
            this.Controls.Add(this.optimizeTabControl);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OptimizationWindow";
            this.Text = "Tunny";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingXButton);
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
        private System.Windows.Forms.Button clearResultButton;
        private System.Windows.Forms.Button openResultFolderButton;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.TextBox restoreModelNumTextBox;
        private System.Windows.Forms.Label restoreModelLabel;
    }
}

