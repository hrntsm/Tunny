
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
            this.optimizeRunButton = new System.Windows.Forms.Button();
            this.optimizeBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.optimizeStopButton = new System.Windows.Forms.Button();
            this.nTrialNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.nTrialText = new System.Windows.Forms.Label();
            this.loadIfExistsCheckBox = new System.Windows.Forms.CheckBox();
            this.optimizeProgressBar = new System.Windows.Forms.ProgressBar();
            this.samplerComboBox = new System.Windows.Forms.ComboBox();
            this.samplerTypeText = new System.Windows.Forms.Label();
            this.studyNameLabel = new System.Windows.Forms.Label();
            this.studyNameTextBox = new System.Windows.Forms.TextBox();
            this.optimizeTabControl = new System.Windows.Forms.TabControl();
            this.optimizeTabPage = new System.Windows.Forms.TabPage();
            this.visualizeTabPage = new System.Windows.Forms.TabPage();
            this.dashboardButton = new System.Windows.Forms.Button();
            this.visualizeButton = new System.Windows.Forms.Button();
            this.visualizeTypeLabel = new System.Windows.Forms.Label();
            this.visualizeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.outputAllTrialsButton = new System.Windows.Forms.Button();
            this.outputParatoSolutionButton = new System.Windows.Forms.Button();
            this.reflectToSliderButton = new System.Windows.Forms.Button();
            this.outputStopButton = new System.Windows.Forms.Button();
            this.outputProgressBar = new System.Windows.Forms.ProgressBar();
            this.outputModelNumberButton = new System.Windows.Forms.Button();
            this.outputModelNumTextBox = new System.Windows.Forms.TextBox();
            this.outputModelLabel = new System.Windows.Forms.Label();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.settingsToJson = new System.Windows.Forms.Button();
            this.settingsOpenAPIPage = new System.Windows.Forms.Button();
            this.settingsAPIComboBox = new System.Windows.Forms.ComboBox();
            this.settingsFolderOpen = new System.Windows.Forms.Button();
            this.settingLabel = new System.Windows.Forms.Label();
            this.settingsFromJson = new System.Windows.Forms.Button();
            this.fileTabPage = new System.Windows.Forms.TabPage();
            this.openResultFolderButton = new System.Windows.Forms.Button();
            this.clearResultButton = new System.Windows.Forms.Button();
            this.outputResultBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.optimizeTabControl.SuspendLayout();
            this.optimizeTabPage.SuspendLayout();
            this.visualizeTabPage.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.fileTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // optimizeRunButton
            // 
            this.optimizeRunButton.Location = new System.Drawing.Point(20, 252);
            this.optimizeRunButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeRunButton.Name = "optimizeRunButton";
            this.optimizeRunButton.Size = new System.Drawing.Size(180, 44);
            this.optimizeRunButton.TabIndex = 0;
            this.optimizeRunButton.Text = "RunOptimize";
            this.optimizeRunButton.UseVisualStyleBackColor = true;
            this.optimizeRunButton.Click += new System.EventHandler(this.OptimizeRunButton_Click);
            // 
            // optimizeStopButton
            // 
            this.optimizeStopButton.Enabled = false;
            this.optimizeStopButton.Location = new System.Drawing.Point(232, 252);
            this.optimizeStopButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeStopButton.Name = "optimizeStopButton";
            this.optimizeStopButton.Size = new System.Drawing.Size(130, 44);
            this.optimizeStopButton.TabIndex = 1;
            this.optimizeStopButton.Text = "Stop";
            this.optimizeStopButton.UseVisualStyleBackColor = true;
            this.optimizeStopButton.Click += new System.EventHandler(this.OptimizeStopButton_Click);
            // 
            // nTrialNumUpDown
            // 
            this.nTrialNumUpDown.Location = new System.Drawing.Point(196, 60);
            this.nTrialNumUpDown.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nTrialNumUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nTrialNumUpDown.Name = "nTrialNumUpDown";
            this.nTrialNumUpDown.Size = new System.Drawing.Size(165, 30);
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
            this.nTrialText.Location = new System.Drawing.Point(15, 63);
            this.nTrialText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nTrialText.Name = "nTrialText";
            this.nTrialText.Size = new System.Drawing.Size(142, 23);
            this.nTrialText.TabIndex = 3;
            this.nTrialText.Text = "Number of trial";
            // 
            // loadIfExistsCheckBox
            // 
            this.loadIfExistsCheckBox.AutoSize = true;
            this.loadIfExistsCheckBox.Checked = true;
            this.loadIfExistsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadIfExistsCheckBox.Location = new System.Drawing.Point(20, 135);
            this.loadIfExistsCheckBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.loadIfExistsCheckBox.Name = "loadIfExistsCheckBox";
            this.loadIfExistsCheckBox.Size = new System.Drawing.Size(237, 27);
            this.loadIfExistsCheckBox.TabIndex = 5;
            this.loadIfExistsCheckBox.Text = "Load if study file exists";
            this.loadIfExistsCheckBox.UseVisualStyleBackColor = true;
            // 
            // optimizeProgressBar
            // 
            this.optimizeProgressBar.Location = new System.Drawing.Point(20, 308);
            this.optimizeProgressBar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeProgressBar.Name = "optimizeProgressBar";
            this.optimizeProgressBar.Size = new System.Drawing.Size(340, 42);
            this.optimizeProgressBar.TabIndex = 6;
            // 
            // samplerComboBox
            // 
            this.samplerComboBox.FormattingEnabled = true;
            this.samplerComboBox.Items.AddRange(new object[] {
            "TPE",
            "NSGA-II",
            "CMA-ES",
            "Random",
            "Grid"});
            this.samplerComboBox.Location = new System.Drawing.Point(148, 12);
            this.samplerComboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.samplerComboBox.Name = "samplerComboBox";
            this.samplerComboBox.Size = new System.Drawing.Size(208, 31);
            this.samplerComboBox.TabIndex = 7;
            // 
            // samplerTypeText
            // 
            this.samplerTypeText.AutoSize = true;
            this.samplerTypeText.Location = new System.Drawing.Point(15, 16);
            this.samplerTypeText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.samplerTypeText.Name = "samplerTypeText";
            this.samplerTypeText.Size = new System.Drawing.Size(81, 23);
            this.samplerTypeText.TabIndex = 8;
            this.samplerTypeText.Text = "Sampler";
            // 
            // studyNameLabel
            // 
            this.studyNameLabel.AutoSize = true;
            this.studyNameLabel.Location = new System.Drawing.Point(15, 172);
            this.studyNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(116, 23);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Study Name";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(152, 166);
            this.studyNameTextBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.studyNameTextBox.Name = "studyNameTextBox";
            this.studyNameTextBox.Size = new System.Drawing.Size(205, 30);
            this.studyNameTextBox.TabIndex = 10;
            this.studyNameTextBox.Text = "study1";
            // 
            // optimizeTabControl
            // 
            this.optimizeTabControl.Controls.Add(this.optimizeTabPage);
            this.optimizeTabControl.Controls.Add(this.visualizeTabPage);
            this.optimizeTabControl.Controls.Add(this.outputTabPage);
            this.optimizeTabControl.Controls.Add(this.settingsTabPage);
            this.optimizeTabControl.Controls.Add(this.fileTabPage);
            this.optimizeTabControl.Location = new System.Drawing.Point(21, 22);
            this.optimizeTabControl.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeTabControl.Multiline = true;
            this.optimizeTabControl.Name = "optimizeTabControl";
            this.optimizeTabControl.SelectedIndex = 0;
            this.optimizeTabControl.Size = new System.Drawing.Size(387, 438);
            this.optimizeTabControl.TabIndex = 11;
            // 
            // optimizeTabPage
            // 
            this.optimizeTabPage.Controls.Add(this.studyNameTextBox);
            this.optimizeTabPage.Controls.Add(this.samplerComboBox);
            this.optimizeTabPage.Controls.Add(this.studyNameLabel);
            this.optimizeTabPage.Controls.Add(this.optimizeRunButton);
            this.optimizeTabPage.Controls.Add(this.samplerTypeText);
            this.optimizeTabPage.Controls.Add(this.optimizeStopButton);
            this.optimizeTabPage.Controls.Add(this.nTrialNumUpDown);
            this.optimizeTabPage.Controls.Add(this.optimizeProgressBar);
            this.optimizeTabPage.Controls.Add(this.nTrialText);
            this.optimizeTabPage.Controls.Add(this.loadIfExistsCheckBox);
            this.optimizeTabPage.Location = new System.Drawing.Point(4, 60);
            this.optimizeTabPage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeTabPage.Name = "optimizeTabPage";
            this.optimizeTabPage.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.optimizeTabPage.Size = new System.Drawing.Size(379, 374);
            this.optimizeTabPage.TabIndex = 0;
            this.optimizeTabPage.Text = "Optimize";
            this.optimizeTabPage.UseVisualStyleBackColor = true;
            // 
            // visualizeTabPage
            // 
            this.visualizeTabPage.Controls.Add(this.dashboardButton);
            this.visualizeTabPage.Controls.Add(this.visualizeButton);
            this.visualizeTabPage.Controls.Add(this.visualizeTypeLabel);
            this.visualizeTabPage.Controls.Add(this.visualizeTypeComboBox);
            this.visualizeTabPage.Location = new System.Drawing.Point(4, 60);
            this.visualizeTabPage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.visualizeTabPage.Name = "visualizeTabPage";
            this.visualizeTabPage.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.visualizeTabPage.Size = new System.Drawing.Size(379, 374);
            this.visualizeTabPage.TabIndex = 1;
            this.visualizeTabPage.Text = "Visualize";
            this.visualizeTabPage.UseVisualStyleBackColor = true;
            // 
            // dashboardButton
            // 
            this.dashboardButton.Location = new System.Drawing.Point(33, 36);
            this.dashboardButton.Margin = new System.Windows.Forms.Padding(4);
            this.dashboardButton.Name = "dashboardButton";
            this.dashboardButton.Size = new System.Drawing.Size(323, 39);
            this.dashboardButton.TabIndex = 11;
            this.dashboardButton.Text = "Open Optuna-Dashboard";
            this.dashboardButton.UseVisualStyleBackColor = true;
            this.dashboardButton.Click += new System.EventHandler(this.DashboardButton_Click);
            // 
            // visualizeButton
            // 
            this.visualizeButton.Location = new System.Drawing.Point(33, 203);
            this.visualizeButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.visualizeButton.Name = "visualizeButton";
            this.visualizeButton.Size = new System.Drawing.Size(323, 39);
            this.visualizeButton.TabIndex = 2;
            this.visualizeButton.Text = "Show selected type of plots";
            this.visualizeButton.UseVisualStyleBackColor = true;
            this.visualizeButton.Click += new System.EventHandler(this.SelectedTypePlotButton_Click);
            // 
            // visualizeTypeLabel
            // 
            this.visualizeTypeLabel.AutoSize = true;
            this.visualizeTypeLabel.Location = new System.Drawing.Point(4, 121);
            this.visualizeTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.visualizeTypeLabel.Name = "visualizeTypeLabel";
            this.visualizeTypeLabel.Size = new System.Drawing.Size(130, 23);
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
            this.visualizeTypeComboBox.Location = new System.Drawing.Point(33, 160);
            this.visualizeTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.visualizeTypeComboBox.Name = "visualizeTypeComboBox";
            this.visualizeTypeComboBox.Size = new System.Drawing.Size(323, 31);
            this.visualizeTypeComboBox.TabIndex = 0;
            // 
            // outputTabPage
            // 
            this.outputTabPage.Controls.Add(this.outputAllTrialsButton);
            this.outputTabPage.Controls.Add(this.outputParatoSolutionButton);
            this.outputTabPage.Controls.Add(this.reflectToSliderButton);
            this.outputTabPage.Controls.Add(this.outputStopButton);
            this.outputTabPage.Controls.Add(this.outputProgressBar);
            this.outputTabPage.Controls.Add(this.outputModelNumberButton);
            this.outputTabPage.Controls.Add(this.outputModelNumTextBox);
            this.outputTabPage.Controls.Add(this.outputModelLabel);
            this.outputTabPage.Location = new System.Drawing.Point(4, 60);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outputTabPage.Size = new System.Drawing.Size(379, 374);
            this.outputTabPage.TabIndex = 3;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // outputAllTrialsButton
            // 
            this.outputAllTrialsButton.Location = new System.Drawing.Point(35, 74);
            this.outputAllTrialsButton.Margin = new System.Windows.Forms.Padding(4);
            this.outputAllTrialsButton.Name = "outputAllTrialsButton";
            this.outputAllTrialsButton.Size = new System.Drawing.Size(297, 34);
            this.outputAllTrialsButton.TabIndex = 18;
            this.outputAllTrialsButton.Text = "All trials";
            this.outputAllTrialsButton.UseVisualStyleBackColor = true;
            this.outputAllTrialsButton.Click += new System.EventHandler(this.OutputAllTrialsButton_Click);
            // 
            // outputParatoSolutionButton
            // 
            this.outputParatoSolutionButton.Location = new System.Drawing.Point(35, 22);
            this.outputParatoSolutionButton.Margin = new System.Windows.Forms.Padding(4);
            this.outputParatoSolutionButton.Name = "outputParatoSolutionButton";
            this.outputParatoSolutionButton.Size = new System.Drawing.Size(297, 34);
            this.outputParatoSolutionButton.TabIndex = 17;
            this.outputParatoSolutionButton.Text = "Parato solutions";
            this.outputParatoSolutionButton.UseVisualStyleBackColor = true;
            this.outputParatoSolutionButton.Click += new System.EventHandler(this.OutputParatoSolutionButton_Click);
            // 
            // reflectToSliderButton
            // 
            this.reflectToSliderButton.Location = new System.Drawing.Point(35, 230);
            this.reflectToSliderButton.Margin = new System.Windows.Forms.Padding(4);
            this.reflectToSliderButton.Name = "reflectToSliderButton";
            this.reflectToSliderButton.Size = new System.Drawing.Size(325, 41);
            this.reflectToSliderButton.TabIndex = 16;
            this.reflectToSliderButton.Text = "Reflect the result on the sliders";
            this.reflectToSliderButton.UseVisualStyleBackColor = true;
            this.reflectToSliderButton.Click += new System.EventHandler(this.ReflectToSliderButton_Click);
            // 
            // outputStopButton
            // 
            this.outputStopButton.Enabled = false;
            this.outputStopButton.Location = new System.Drawing.Point(297, 320);
            this.outputStopButton.Margin = new System.Windows.Forms.Padding(4);
            this.outputStopButton.Name = "outputStopButton";
            this.outputStopButton.Size = new System.Drawing.Size(75, 40);
            this.outputStopButton.TabIndex = 15;
            this.outputStopButton.Text = "Stop";
            this.outputStopButton.UseVisualStyleBackColor = true;
            this.outputStopButton.Click += new System.EventHandler(this.OutputStopButton_Click);
            // 
            // outputProgressBar
            // 
            this.outputProgressBar.Location = new System.Drawing.Point(35, 320);
            this.outputProgressBar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.outputProgressBar.Name = "outputProgressBar";
            this.outputProgressBar.Size = new System.Drawing.Size(238, 40);
            this.outputProgressBar.TabIndex = 14;
            // 
            // outputModelNumberButton
            // 
            this.outputModelNumberButton.Location = new System.Drawing.Point(243, 179);
            this.outputModelNumberButton.Margin = new System.Windows.Forms.Padding(4);
            this.outputModelNumberButton.Name = "outputModelNumberButton";
            this.outputModelNumberButton.Size = new System.Drawing.Size(117, 30);
            this.outputModelNumberButton.TabIndex = 13;
            this.outputModelNumberButton.Text = "Output";
            this.outputModelNumberButton.UseVisualStyleBackColor = true;
            this.outputModelNumberButton.Click += new System.EventHandler(this.OutputModelNumberButton_Click);
            // 
            // outputModelNumTextBox
            // 
            this.outputModelNumTextBox.Location = new System.Drawing.Point(35, 179);
            this.outputModelNumTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.outputModelNumTextBox.Name = "outputModelNumTextBox";
            this.outputModelNumTextBox.Size = new System.Drawing.Size(200, 30);
            this.outputModelNumTextBox.TabIndex = 12;
            this.outputModelNumTextBox.Text = "0";
            // 
            // outputModelLabel
            // 
            this.outputModelLabel.AutoSize = true;
            this.outputModelLabel.Location = new System.Drawing.Point(38, 152);
            this.outputModelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outputModelLabel.Name = "outputModelLabel";
            this.outputModelLabel.Size = new System.Drawing.Size(175, 23);
            this.outputModelLabel.TabIndex = 11;
            this.outputModelLabel.Text = "Use model number";
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.settingsToJson);
            this.settingsTabPage.Controls.Add(this.settingsOpenAPIPage);
            this.settingsTabPage.Controls.Add(this.settingsAPIComboBox);
            this.settingsTabPage.Controls.Add(this.settingsFolderOpen);
            this.settingsTabPage.Controls.Add(this.settingLabel);
            this.settingsTabPage.Controls.Add(this.settingsFromJson);
            this.settingsTabPage.Location = new System.Drawing.Point(4, 60);
            this.settingsTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Size = new System.Drawing.Size(379, 374);
            this.settingsTabPage.TabIndex = 2;
            this.settingsTabPage.Text = "Settings";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsToJson
            // 
            this.settingsToJson.Location = new System.Drawing.Point(50, 204);
            this.settingsToJson.Margin = new System.Windows.Forms.Padding(4);
            this.settingsToJson.Name = "settingsToJson";
            this.settingsToJson.Size = new System.Drawing.Size(278, 34);
            this.settingsToJson.TabIndex = 5;
            this.settingsToJson.Text = "Save settings to json";
            this.settingsToJson.UseVisualStyleBackColor = true;
            this.settingsToJson.Click += new System.EventHandler(this.SettingsToJson_Click);
            // 
            // settingsOpenAPIPage
            // 
            this.settingsOpenAPIPage.Location = new System.Drawing.Point(183, 132);
            this.settingsOpenAPIPage.Margin = new System.Windows.Forms.Padding(4);
            this.settingsOpenAPIPage.Name = "settingsOpenAPIPage";
            this.settingsOpenAPIPage.Size = new System.Drawing.Size(165, 34);
            this.settingsOpenAPIPage.TabIndex = 4;
            this.settingsOpenAPIPage.Text = "Open API page";
            this.settingsOpenAPIPage.UseVisualStyleBackColor = true;
            this.settingsOpenAPIPage.Click += new System.EventHandler(this.SettingsOpenAPIPage_Click);
            // 
            // settingsAPIComboBox
            // 
            this.settingsAPIComboBox.FormattingEnabled = true;
            this.settingsAPIComboBox.Items.AddRange(new object[] {
            "TPE",
            "NSGA-II",
            "CMA-ES",
            "Random"});
            this.settingsAPIComboBox.Location = new System.Drawing.Point(26, 132);
            this.settingsAPIComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.settingsAPIComboBox.Name = "settingsAPIComboBox";
            this.settingsAPIComboBox.Size = new System.Drawing.Size(142, 31);
            this.settingsAPIComboBox.TabIndex = 3;
            // 
            // settingsFolderOpen
            // 
            this.settingsFolderOpen.Location = new System.Drawing.Point(50, 291);
            this.settingsFolderOpen.Margin = new System.Windows.Forms.Padding(4);
            this.settingsFolderOpen.Name = "settingsFolderOpen";
            this.settingsFolderOpen.Size = new System.Drawing.Size(278, 34);
            this.settingsFolderOpen.TabIndex = 2;
            this.settingsFolderOpen.Text = "Open Settings.json folder";
            this.settingsFolderOpen.UseVisualStyleBackColor = true;
            this.settingsFolderOpen.Click += new System.EventHandler(this.SettingsFolderOpen_Click);
            // 
            // settingLabel
            // 
            this.settingLabel.Location = new System.Drawing.Point(21, 18);
            this.settingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.settingLabel.Name = "settingLabel";
            this.settingLabel.Size = new System.Drawing.Size(327, 110);
            this.settingLabel.TabIndex = 1;
            this.settingLabel.Text = "Detailed optimization settings can be configured in the \"Settings.json\" file in t" +
    "he following folder.";
            // 
            // settingsFromJson
            // 
            this.settingsFromJson.Location = new System.Drawing.Point(50, 248);
            this.settingsFromJson.Margin = new System.Windows.Forms.Padding(4);
            this.settingsFromJson.Name = "settingsFromJson";
            this.settingsFromJson.Size = new System.Drawing.Size(278, 34);
            this.settingsFromJson.TabIndex = 0;
            this.settingsFromJson.Text = "Load settings from json";
            this.settingsFromJson.UseVisualStyleBackColor = true;
            this.settingsFromJson.Click += new System.EventHandler(this.SettingsFromJson_Click);
            // 
            // fileTabPage
            // 
            this.fileTabPage.Controls.Add(this.openResultFolderButton);
            this.fileTabPage.Controls.Add(this.clearResultButton);
            this.fileTabPage.Location = new System.Drawing.Point(4, 60);
            this.fileTabPage.Name = "fileTabPage";
            this.fileTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileTabPage.Size = new System.Drawing.Size(379, 374);
            this.fileTabPage.TabIndex = 4;
            this.fileTabPage.Text = "File";
            this.fileTabPage.UseVisualStyleBackColor = true;
            // 
            // openResultFolderButton
            // 
            this.openResultFolderButton.Location = new System.Drawing.Point(62, 27);
            this.openResultFolderButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.openResultFolderButton.Name = "openResultFolderButton";
            this.openResultFolderButton.Size = new System.Drawing.Size(264, 39);
            this.openResultFolderButton.TabIndex = 6;
            this.openResultFolderButton.Text = "Open result file folder";
            this.openResultFolderButton.UseVisualStyleBackColor = true;
            this.openResultFolderButton.Click += new System.EventHandler(this.OpenResultFolderButton_Click);
            // 
            // clearResultButton
            // 
            this.clearResultButton.Location = new System.Drawing.Point(62, 92);
            this.clearResultButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.clearResultButton.Name = "clearResultButton";
            this.clearResultButton.Size = new System.Drawing.Size(264, 42);
            this.clearResultButton.TabIndex = 5;
            this.clearResultButton.Text = "Clear result flie";
            this.clearResultButton.UseVisualStyleBackColor = true;
            this.clearResultButton.Click += new System.EventHandler(this.ClearResultButton_Click);
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(428, 475);
            this.Controls.Add(this.optimizeTabControl);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "OptimizationWindow";
            this.Text = "Tunny";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingXButton);
            this.Load += new System.EventHandler(this.OptimizationWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).EndInit();
            this.optimizeTabControl.ResumeLayout(false);
            this.optimizeTabPage.ResumeLayout(false);
            this.optimizeTabPage.PerformLayout();
            this.visualizeTabPage.ResumeLayout(false);
            this.visualizeTabPage.PerformLayout();
            this.outputTabPage.ResumeLayout(false);
            this.outputTabPage.PerformLayout();
            this.settingsTabPage.ResumeLayout(false);
            this.fileTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button optimizeRunButton;
        private System.ComponentModel.BackgroundWorker optimizeBackgroundWorker;
        private System.Windows.Forms.Button optimizeStopButton;
        private System.Windows.Forms.NumericUpDown nTrialNumUpDown;
        private System.Windows.Forms.CheckBox loadIfExistsCheckBox;
        private System.Windows.Forms.ProgressBar optimizeProgressBar;
        private System.Windows.Forms.ComboBox samplerComboBox;
        private System.Windows.Forms.Label samplerTypeText;
        private System.Windows.Forms.Label nTrialText;
        private System.Windows.Forms.Label studyNameLabel;
        private System.Windows.Forms.TextBox studyNameTextBox;
        private System.Windows.Forms.TabControl optimizeTabControl;
        private System.Windows.Forms.TabPage optimizeTabPage;
        private System.Windows.Forms.TabPage visualizeTabPage;
        private System.Windows.Forms.Button visualizeButton;
        private System.Windows.Forms.Label visualizeTypeLabel;
        private System.Windows.Forms.ComboBox visualizeTypeComboBox;
        private System.ComponentModel.BackgroundWorker outputResultBackgroundWorker;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.Button settingsOpenAPIPage;
        private System.Windows.Forms.ComboBox settingsAPIComboBox;
        private System.Windows.Forms.Button settingsFolderOpen;
        private System.Windows.Forms.Label settingLabel;
        private System.Windows.Forms.Button settingsFromJson;
        private System.Windows.Forms.Button settingsToJson;
        private System.Windows.Forms.Button dashboardButton;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.Button outputAllTrialsButton;
        private System.Windows.Forms.Button outputParatoSolutionButton;
        private System.Windows.Forms.Button reflectToSliderButton;
        private System.Windows.Forms.Button outputStopButton;
        private System.Windows.Forms.ProgressBar outputProgressBar;
        private System.Windows.Forms.Button outputModelNumberButton;
        private System.Windows.Forms.TextBox outputModelNumTextBox;
        private System.Windows.Forms.Label outputModelLabel;
        private System.Windows.Forms.TabPage fileTabPage;
        private System.Windows.Forms.Button openResultFolderButton;
        private System.Windows.Forms.Button clearResultButton;
    }
}

