
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizationWindow));
            this.optimizeRunButton = new System.Windows.Forms.Button();
            this.optimizeBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.optimizeStopButton = new System.Windows.Forms.Button();
            this.nTrialNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.nTrialText = new System.Windows.Forms.Label();
            this.continueStudyCheckBox = new System.Windows.Forms.CheckBox();
            this.optimizeProgressBar = new System.Windows.Forms.ProgressBar();
            this.samplerComboBox = new System.Windows.Forms.ComboBox();
            this.samplerTypeText = new System.Windows.Forms.Label();
            this.studyNameLabel = new System.Windows.Forms.Label();
            this.studyNameTextBox = new System.Windows.Forms.TextBox();
            this.optimizeTabControl = new System.Windows.Forms.TabControl();
            this.optimizeTabPage = new System.Windows.Forms.TabPage();
            this.optimizeBestValueLabel = new System.Windows.Forms.Label();
            this.optimizeTrialNumLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.existedStudyNameLabel = new System.Windows.Forms.Label();
            this.existingStudyComboBox = new System.Windows.Forms.ComboBox();
            this.copyStudyCheckBox = new System.Windows.Forms.CheckBox();
            this.timeoutNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.Timeout = new System.Windows.Forms.Label();
            this.visualizeTabPage = new System.Windows.Forms.TabPage();
            this.visualizeIncludeDominatedCheckBox = new System.Windows.Forms.CheckBox();
            this.visualizeTypeLabel = new System.Windows.Forms.Label();
            this.visualizeTargetStudyNameLabel = new System.Windows.Forms.Label();
            this.visualizeObjectiveListBox = new System.Windows.Forms.ListBox();
            this.visualizeTargetStudyComboBox = new System.Windows.Forms.ComboBox();
            this.visualizeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.visualizeVariableListBox = new System.Windows.Forms.ListBox();
            this.visualizeTargetVariableLabel = new System.Windows.Forms.Label();
            this.visualizeClusterNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.visualizeTargetObjectiveLabel = new System.Windows.Forms.Label();
            this.dashboardButton = new System.Windows.Forms.Button();
            this.visualizeNumClusterLabel = new System.Windows.Forms.Label();
            this.visualizeShowPlotButton = new System.Windows.Forms.Button();
            this.visualizeSavePlotButton = new System.Windows.Forms.Button();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.outputUseModelNumberGroupBox = new System.Windows.Forms.GroupBox();
            this.outputModelNumTextBox = new System.Windows.Forms.TextBox();
            this.outputModelNumberButton = new System.Windows.Forms.Button();
            this.reflectToSliderButton = new System.Windows.Forms.Button();
            this.outputTargetStudyComboBox = new System.Windows.Forms.ComboBox();
            this.outputTargetStudyLabel = new System.Windows.Forms.Label();
            this.outputAllTrialsButton = new System.Windows.Forms.Button();
            this.outputParatoSolutionButton = new System.Windows.Forms.Button();
            this.outputStopButton = new System.Windows.Forms.Button();
            this.outputProgressBar = new System.Windows.Forms.ProgressBar();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.TPE = new System.Windows.Forms.TabPage();
            this.tpeDefaultButton = new System.Windows.Forms.Button();
            this.tpeNEICandidatesLabel = new System.Windows.Forms.Label();
            this.tpeNStartupTrialsLabel = new System.Windows.Forms.Label();
            this.tpePriorWeightLabel = new System.Windows.Forms.Label();
            this.tpeEINumUpDown = new System.Windows.Forms.NumericUpDown();
            this.tpeStartupNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.tpePriorNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.tpeConstantLiarCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeWarnIndependentSamplingCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeGroupCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeMultivariateCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeConsiderEndpointsCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeConsiderMagicClipCheckBox = new System.Windows.Forms.CheckBox();
            this.tpeConsiderPriorCheckBox = new System.Windows.Forms.CheckBox();
            this.GP = new System.Windows.Forms.TabPage();
            this.boTorchDefaultButton = new System.Windows.Forms.Button();
            this.boTorchNStartupTrialsLabel = new System.Windows.Forms.Label();
            this.boTorchStartupNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.NSGAII = new System.Windows.Forms.TabPage();
            this.nsgaCrossoverComboBox = new System.Windows.Forms.ComboBox();
            this.nsgaCrossoverCheckBox = new System.Windows.Forms.CheckBox();
            this.nsgaDefaultButton = new System.Windows.Forms.Button();
            this.nsgaMutationProbCheckBox = new System.Windows.Forms.CheckBox();
            this.nsgaPopulationSizeLabel = new System.Windows.Forms.Label();
            this.nsgaPopulationSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaSwappingProbLabel = new System.Windows.Forms.Label();
            this.nsgaSwappingProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaCrossoverProbLabel = new System.Windows.Forms.Label();
            this.nsgaCrossoverProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaMutationProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.CMAES = new System.Windows.Forms.TabPage();
            this.cmaEsWarmStartComboBox = new System.Windows.Forms.ComboBox();
            this.cmaEsWarmStartCmaEsCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsPopulationSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmaEsPopulationSizeLabel = new System.Windows.Forms.Label();
            this.cmaEsDefaultButton = new System.Windows.Forms.Button();
            this.cmaEsRestartCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsUseSaparableCmaCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsNStartupTrialsLabel = new System.Windows.Forms.Label();
            this.cmaEsStartupNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmaEsConsiderPruneTrialsCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsWarnIndependentSamplingCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsIncPopsizeLabel = new System.Windows.Forms.Label();
            this.cmaEsIncPopSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmaEsSigmaCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaEsSigmaNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.QMC = new System.Windows.Forms.TabPage();
            this.qmcDefaultButton = new System.Windows.Forms.Button();
            this.qmcTypeComboBox = new System.Windows.Forms.ComboBox();
            this.qmcTypeLabel = new System.Windows.Forms.Label();
            this.qmcWarnAsyncSeedingCheckBox = new System.Windows.Forms.CheckBox();
            this.qmcScrambleCheckBox = new System.Windows.Forms.CheckBox();
            this.qmcWarnIndependentSamplingCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.miscDefaultButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.runGarbageCollectionLabel = new System.Windows.Forms.Label();
            this.fileTabPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.setResultFilePathButton = new System.Windows.Forms.Button();
            this.clearResultButton = new System.Windows.Forms.Button();
            this.openResultFolderButton = new System.Windows.Forms.Button();
            this.licenseGroupBox = new System.Windows.Forms.GroupBox();
            this.showThirdPartyLicenseButton = new System.Windows.Forms.Button();
            this.showTunnyLicenseButton = new System.Windows.Forms.Button();
            this.outputResultBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.optimizeTabControl.SuspendLayout();
            this.optimizeTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumUpDown)).BeginInit();
            this.visualizeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizeClusterNumUpDown)).BeginInit();
            this.outputTabPage.SuspendLayout();
            this.outputUseModelNumberGroupBox.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.TPE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpeEINumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpeStartupNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpePriorNumUpDown)).BeginInit();
            this.GP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boTorchStartupNumUpDown)).BeginInit();
            this.NSGAII.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaPopulationSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaSwappingProbUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaCrossoverProbUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaMutationProbUpDown)).BeginInit();
            this.CMAES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsPopulationSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsStartupNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsIncPopSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsSigmaNumUpDown)).BeginInit();
            this.QMC.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.fileTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.licenseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // optimizeRunButton
            // 
            this.optimizeRunButton.Location = new System.Drawing.Point(32, 208);
            this.optimizeRunButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeRunButton.Name = "optimizeRunButton";
            this.optimizeRunButton.Size = new System.Drawing.Size(102, 23);
            this.optimizeRunButton.TabIndex = 0;
            this.optimizeRunButton.Text = "RunOptimize";
            this.optimizeRunButton.UseVisualStyleBackColor = true;
            this.optimizeRunButton.Click += new System.EventHandler(this.OptimizeRunButton_Click);
            // 
            // optimizeStopButton
            // 
            this.optimizeStopButton.Enabled = false;
            this.optimizeStopButton.Location = new System.Drawing.Point(164, 208);
            this.optimizeStopButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeStopButton.Name = "optimizeStopButton";
            this.optimizeStopButton.Size = new System.Drawing.Size(84, 23);
            this.optimizeStopButton.TabIndex = 1;
            this.optimizeStopButton.Text = "Stop";
            this.optimizeStopButton.UseVisualStyleBackColor = true;
            this.optimizeStopButton.Click += new System.EventHandler(this.OptimizeStopButton_Click);
            // 
            // nTrialNumUpDown
            // 
            this.nTrialNumUpDown.Location = new System.Drawing.Point(164, 35);
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
            this.nTrialText.Location = new System.Drawing.Point(10, 37);
            this.nTrialText.Name = "nTrialText";
            this.nTrialText.Size = new System.Drawing.Size(96, 15);
            this.nTrialText.TabIndex = 3;
            this.nTrialText.Text = "Number of trial";
            // 
            // continueStudyCheckBox
            // 
            this.continueStudyCheckBox.AutoSize = true;
            this.continueStudyCheckBox.Location = new System.Drawing.Point(8, 46);
            this.continueStudyCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.continueStudyCheckBox.Name = "continueStudyCheckBox";
            this.continueStudyCheckBox.Size = new System.Drawing.Size(77, 19);
            this.continueStudyCheckBox.TabIndex = 5;
            this.continueStudyCheckBox.Text = "Continue";
            this.continueStudyCheckBox.UseVisualStyleBackColor = true;
            this.continueStudyCheckBox.CheckedChanged += new System.EventHandler(this.ContinueStudyCheckBox_CheckedChanged);
            // 
            // optimizeProgressBar
            // 
            this.optimizeProgressBar.Location = new System.Drawing.Point(13, 239);
            this.optimizeProgressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeProgressBar.Name = "optimizeProgressBar";
            this.optimizeProgressBar.Size = new System.Drawing.Size(259, 23);
            this.optimizeProgressBar.TabIndex = 6;
            // 
            // samplerComboBox
            // 
            this.samplerComboBox.FormattingEnabled = true;
            this.samplerComboBox.Items.AddRange(new object[] {
            "BayesianOptimization (TPE)",
            "BayesianOptimization (GP)",
            "GeneticAlgorithm (NSGA-II)",
            "EvolutionStrategy (CMA-ES)",
            "Quasi-MonteCarlo",
            "Random",
            "Grid"});
            this.samplerComboBox.Location = new System.Drawing.Point(69, 9);
            this.samplerComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.samplerComboBox.Name = "samplerComboBox";
            this.samplerComboBox.Size = new System.Drawing.Size(205, 23);
            this.samplerComboBox.TabIndex = 7;
            // 
            // samplerTypeText
            // 
            this.samplerTypeText.AutoSize = true;
            this.samplerTypeText.Location = new System.Drawing.Point(10, 11);
            this.samplerTypeText.Name = "samplerTypeText";
            this.samplerTypeText.Size = new System.Drawing.Size(56, 15);
            this.samplerTypeText.TabIndex = 8;
            this.samplerTypeText.Text = "Sampler";
            // 
            // studyNameLabel
            // 
            this.studyNameLabel.AutoSize = true;
            this.studyNameLabel.Location = new System.Drawing.Point(5, 22);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(114, 15);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Create New Study";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(135, 20);
            this.studyNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.studyNameTextBox.Name = "studyNameTextBox";
            this.studyNameTextBox.Size = new System.Drawing.Size(121, 23);
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
            this.optimizeTabControl.Location = new System.Drawing.Point(0, -1);
            this.optimizeTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabControl.Name = "optimizeTabControl";
            this.optimizeTabControl.SelectedIndex = 0;
            this.optimizeTabControl.Size = new System.Drawing.Size(285, 317);
            this.optimizeTabControl.TabIndex = 11;
            // 
            // optimizeTabPage
            // 
            this.optimizeTabPage.Controls.Add(this.optimizeBestValueLabel);
            this.optimizeTabPage.Controls.Add(this.optimizeTrialNumLabel);
            this.optimizeTabPage.Controls.Add(this.groupBox2);
            this.optimizeTabPage.Controls.Add(this.timeoutNumUpDown);
            this.optimizeTabPage.Controls.Add(this.Timeout);
            this.optimizeTabPage.Controls.Add(this.samplerComboBox);
            this.optimizeTabPage.Controls.Add(this.optimizeRunButton);
            this.optimizeTabPage.Controls.Add(this.samplerTypeText);
            this.optimizeTabPage.Controls.Add(this.optimizeStopButton);
            this.optimizeTabPage.Controls.Add(this.nTrialNumUpDown);
            this.optimizeTabPage.Controls.Add(this.optimizeProgressBar);
            this.optimizeTabPage.Controls.Add(this.nTrialText);
            this.optimizeTabPage.Location = new System.Drawing.Point(4, 24);
            this.optimizeTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabPage.Name = "optimizeTabPage";
            this.optimizeTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optimizeTabPage.Size = new System.Drawing.Size(277, 289);
            this.optimizeTabPage.TabIndex = 0;
            this.optimizeTabPage.Text = "Optimize";
            this.optimizeTabPage.UseVisualStyleBackColor = true;
            // 
            // optimizeBestValueLabel
            // 
            this.optimizeBestValueLabel.AutoSize = true;
            this.optimizeBestValueLabel.Location = new System.Drawing.Point(102, 265);
            this.optimizeBestValueLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.optimizeBestValueLabel.Name = "optimizeBestValueLabel";
            this.optimizeBestValueLabel.Size = new System.Drawing.Size(87, 15);
            this.optimizeBestValueLabel.TabIndex = 15;
            this.optimizeBestValueLabel.Text = "BestValue: # ";
            // 
            // optimizeTrialNumLabel
            // 
            this.optimizeTrialNumLabel.AutoSize = true;
            this.optimizeTrialNumLabel.Location = new System.Drawing.Point(29, 265);
            this.optimizeTrialNumLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.optimizeTrialNumLabel.Name = "optimizeTrialNumLabel";
            this.optimizeTrialNumLabel.Size = new System.Drawing.Size(55, 15);
            this.optimizeTrialNumLabel.TabIndex = 14;
            this.optimizeTrialNumLabel.Text = "Trial: # ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.existedStudyNameLabel);
            this.groupBox2.Controls.Add(this.existingStudyComboBox);
            this.groupBox2.Controls.Add(this.copyStudyCheckBox);
            this.groupBox2.Controls.Add(this.studyNameLabel);
            this.groupBox2.Controls.Add(this.continueStudyCheckBox);
            this.groupBox2.Controls.Add(this.studyNameTextBox);
            this.groupBox2.Location = new System.Drawing.Point(13, 100);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(260, 102);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Study Name";
            // 
            // existedStudyNameLabel
            // 
            this.existedStudyNameLabel.AutoSize = true;
            this.existedStudyNameLabel.Location = new System.Drawing.Point(5, 75);
            this.existedStudyNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.existedStudyNameLabel.Name = "existedStudyNameLabel";
            this.existedStudyNameLabel.Size = new System.Drawing.Size(90, 15);
            this.existedStudyNameLabel.TabIndex = 14;
            this.existedStudyNameLabel.Text = "Existing Study";
            // 
            // existingStudyComboBox
            // 
            this.existingStudyComboBox.FormattingEnabled = true;
            this.existingStudyComboBox.Location = new System.Drawing.Point(135, 70);
            this.existingStudyComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.existingStudyComboBox.Name = "existingStudyComboBox";
            this.existingStudyComboBox.Size = new System.Drawing.Size(121, 23);
            this.existingStudyComboBox.TabIndex = 13;
            // 
            // copyStudyCheckBox
            // 
            this.copyStudyCheckBox.AutoSize = true;
            this.copyStudyCheckBox.Location = new System.Drawing.Point(103, 46);
            this.copyStudyCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.copyStudyCheckBox.Name = "copyStudyCheckBox";
            this.copyStudyCheckBox.Size = new System.Drawing.Size(55, 19);
            this.copyStudyCheckBox.TabIndex = 12;
            this.copyStudyCheckBox.Text = "Copy";
            this.copyStudyCheckBox.UseVisualStyleBackColor = true;
            this.copyStudyCheckBox.CheckedChanged += new System.EventHandler(this.CopyStudyCheckBox_CheckedChanged);
            // 
            // timeoutNumUpDown
            // 
            this.timeoutNumUpDown.Location = new System.Drawing.Point(164, 63);
            this.timeoutNumUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeoutNumUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.timeoutNumUpDown.Name = "timeoutNumUpDown";
            this.timeoutNumUpDown.Size = new System.Drawing.Size(109, 23);
            this.timeoutNumUpDown.TabIndex = 12;
            this.timeoutNumUpDown.ThousandsSeparator = true;
            this.toolTip1.SetToolTip(this.timeoutNumUpDown, "After this time has elapsed, optimization stops.\r\nIf 0 is entered, no stop by tim" +
        "e is performed.");
            // 
            // Timeout
            // 
            this.Timeout.AutoSize = true;
            this.Timeout.Location = new System.Drawing.Point(10, 63);
            this.Timeout.Name = "Timeout";
            this.Timeout.Size = new System.Drawing.Size(89, 15);
            this.Timeout.TabIndex = 11;
            this.Timeout.Text = "Timeout (sec)";
            // 
            // visualizeTabPage
            // 
            this.visualizeTabPage.Controls.Add(this.visualizeIncludeDominatedCheckBox);
            this.visualizeTabPage.Controls.Add(this.visualizeTypeLabel);
            this.visualizeTabPage.Controls.Add(this.visualizeTargetStudyNameLabel);
            this.visualizeTabPage.Controls.Add(this.visualizeObjectiveListBox);
            this.visualizeTabPage.Controls.Add(this.visualizeTargetStudyComboBox);
            this.visualizeTabPage.Controls.Add(this.visualizeTypeComboBox);
            this.visualizeTabPage.Controls.Add(this.visualizeVariableListBox);
            this.visualizeTabPage.Controls.Add(this.visualizeTargetVariableLabel);
            this.visualizeTabPage.Controls.Add(this.visualizeClusterNumUpDown);
            this.visualizeTabPage.Controls.Add(this.visualizeTargetObjectiveLabel);
            this.visualizeTabPage.Controls.Add(this.dashboardButton);
            this.visualizeTabPage.Controls.Add(this.visualizeNumClusterLabel);
            this.visualizeTabPage.Controls.Add(this.visualizeShowPlotButton);
            this.visualizeTabPage.Controls.Add(this.visualizeSavePlotButton);
            this.visualizeTabPage.Location = new System.Drawing.Point(4, 24);
            this.visualizeTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visualizeTabPage.Name = "visualizeTabPage";
            this.visualizeTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visualizeTabPage.Size = new System.Drawing.Size(277, 289);
            this.visualizeTabPage.TabIndex = 1;
            this.visualizeTabPage.Text = "Visualize";
            this.visualizeTabPage.UseVisualStyleBackColor = true;
            // 
            // visualizeIncludeDominatedCheckBox
            // 
            this.visualizeIncludeDominatedCheckBox.AutoSize = true;
            this.visualizeIncludeDominatedCheckBox.Checked = true;
            this.visualizeIncludeDominatedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.visualizeIncludeDominatedCheckBox.Enabled = false;
            this.visualizeIncludeDominatedCheckBox.Location = new System.Drawing.Point(17, 269);
            this.visualizeIncludeDominatedCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeIncludeDominatedCheckBox.Name = "visualizeIncludeDominatedCheckBox";
            this.visualizeIncludeDominatedCheckBox.Size = new System.Drawing.Size(255, 19);
            this.visualizeIncludeDominatedCheckBox.TabIndex = 27;
            this.visualizeIncludeDominatedCheckBox.Text = "Include dominated trials in pareto front";
            this.visualizeIncludeDominatedCheckBox.UseVisualStyleBackColor = true;
            // 
            // visualizeTypeLabel
            // 
            this.visualizeTypeLabel.AutoSize = true;
            this.visualizeTypeLabel.Location = new System.Drawing.Point(15, 73);
            this.visualizeTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.visualizeTypeLabel.Name = "visualizeTypeLabel";
            this.visualizeTypeLabel.Size = new System.Drawing.Size(89, 15);
            this.visualizeTypeLabel.TabIndex = 22;
            this.visualizeTypeLabel.Text = "Visualize Type";
            // 
            // visualizeTargetStudyNameLabel
            // 
            this.visualizeTargetStudyNameLabel.AutoSize = true;
            this.visualizeTargetStudyNameLabel.Location = new System.Drawing.Point(15, 41);
            this.visualizeTargetStudyNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.visualizeTargetStudyNameLabel.Name = "visualizeTargetStudyNameLabel";
            this.visualizeTargetStudyNameLabel.Size = new System.Drawing.Size(83, 15);
            this.visualizeTargetStudyNameLabel.TabIndex = 21;
            this.visualizeTargetStudyNameLabel.Text = "Target Study";
            // 
            // visualizeObjectiveListBox
            // 
            this.visualizeObjectiveListBox.FormattingEnabled = true;
            this.visualizeObjectiveListBox.ItemHeight = 15;
            this.visualizeObjectiveListBox.Location = new System.Drawing.Point(148, 133);
            this.visualizeObjectiveListBox.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeObjectiveListBox.Name = "visualizeObjectiveListBox";
            this.visualizeObjectiveListBox.ScrollAlwaysVisible = true;
            this.visualizeObjectiveListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.visualizeObjectiveListBox.Size = new System.Drawing.Size(128, 64);
            this.visualizeObjectiveListBox.TabIndex = 26;
            // 
            // visualizeTargetStudyComboBox
            // 
            this.visualizeTargetStudyComboBox.FormattingEnabled = true;
            this.visualizeTargetStudyComboBox.Location = new System.Drawing.Point(123, 39);
            this.visualizeTargetStudyComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeTargetStudyComboBox.Name = "visualizeTargetStudyComboBox";
            this.visualizeTargetStudyComboBox.Size = new System.Drawing.Size(145, 23);
            this.visualizeTargetStudyComboBox.TabIndex = 20;
            this.visualizeTargetStudyComboBox.SelectedIndexChanged += new System.EventHandler(this.VisualizeTargetStudy_Changed);
            // 
            // visualizeTypeComboBox
            // 
            this.visualizeTypeComboBox.FormattingEnabled = true;
            this.visualizeTypeComboBox.Items.AddRange(new object[] {
            "contour",
            "EDF",
            "optimization history",
            "parallel coordinate",
            "param importances",
            "pareto front",
            "slice",
            "hypervolume",
            "clustering"});
            this.visualizeTypeComboBox.Location = new System.Drawing.Point(123, 73);
            this.visualizeTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visualizeTypeComboBox.Name = "visualizeTypeComboBox";
            this.visualizeTypeComboBox.Size = new System.Drawing.Size(145, 23);
            this.visualizeTypeComboBox.TabIndex = 0;
            this.visualizeTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.VisualizeType_Changed);
            // 
            // visualizeVariableListBox
            // 
            this.visualizeVariableListBox.FormattingEnabled = true;
            this.visualizeVariableListBox.ItemHeight = 15;
            this.visualizeVariableListBox.Location = new System.Drawing.Point(17, 133);
            this.visualizeVariableListBox.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeVariableListBox.Name = "visualizeVariableListBox";
            this.visualizeVariableListBox.ScrollAlwaysVisible = true;
            this.visualizeVariableListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.visualizeVariableListBox.Size = new System.Drawing.Size(128, 64);
            this.visualizeVariableListBox.TabIndex = 25;
            // 
            // visualizeTargetVariableLabel
            // 
            this.visualizeTargetVariableLabel.AutoSize = true;
            this.visualizeTargetVariableLabel.Location = new System.Drawing.Point(145, 108);
            this.visualizeTargetVariableLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.visualizeTargetVariableLabel.Name = "visualizeTargetVariableLabel";
            this.visualizeTargetVariableLabel.Size = new System.Drawing.Size(95, 15);
            this.visualizeTargetVariableLabel.TabIndex = 24;
            this.visualizeTargetVariableLabel.Text = "Target Variable";
            // 
            // visualizeClusterNumUpDown
            // 
            this.visualizeClusterNumUpDown.Enabled = false;
            this.visualizeClusterNumUpDown.Location = new System.Drawing.Point(205, 239);
            this.visualizeClusterNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeClusterNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.visualizeClusterNumUpDown.Name = "visualizeClusterNumUpDown";
            this.visualizeClusterNumUpDown.Size = new System.Drawing.Size(62, 23);
            this.visualizeClusterNumUpDown.TabIndex = 13;
            this.visualizeClusterNumUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // visualizeTargetObjectiveLabel
            // 
            this.visualizeTargetObjectiveLabel.AutoSize = true;
            this.visualizeTargetObjectiveLabel.Location = new System.Drawing.Point(15, 108);
            this.visualizeTargetObjectiveLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.visualizeTargetObjectiveLabel.Name = "visualizeTargetObjectiveLabel";
            this.visualizeTargetObjectiveLabel.Size = new System.Drawing.Size(104, 15);
            this.visualizeTargetObjectiveLabel.TabIndex = 23;
            this.visualizeTargetObjectiveLabel.Text = "Target Objective";
            // 
            // dashboardButton
            // 
            this.dashboardButton.Location = new System.Drawing.Point(54, 7);
            this.dashboardButton.Name = "dashboardButton";
            this.dashboardButton.Size = new System.Drawing.Size(178, 22);
            this.dashboardButton.TabIndex = 11;
            this.dashboardButton.Text = "Open Optuna-Dashboard";
            this.dashboardButton.UseVisualStyleBackColor = true;
            this.dashboardButton.Click += new System.EventHandler(this.DashboardButton_Click);
            // 
            // visualizeNumClusterLabel
            // 
            this.visualizeNumClusterLabel.AutoSize = true;
            this.visualizeNumClusterLabel.Location = new System.Drawing.Point(15, 244);
            this.visualizeNumClusterLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.visualizeNumClusterLabel.Name = "visualizeNumClusterLabel";
            this.visualizeNumClusterLabel.Size = new System.Drawing.Size(112, 15);
            this.visualizeNumClusterLabel.TabIndex = 14;
            this.visualizeNumClusterLabel.Text = "Number of cluster";
            // 
            // visualizeShowPlotButton
            // 
            this.visualizeShowPlotButton.Location = new System.Drawing.Point(24, 203);
            this.visualizeShowPlotButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.visualizeShowPlotButton.Name = "visualizeShowPlotButton";
            this.visualizeShowPlotButton.Size = new System.Drawing.Size(110, 23);
            this.visualizeShowPlotButton.TabIndex = 2;
            this.visualizeShowPlotButton.Text = "Show";
            this.visualizeShowPlotButton.UseVisualStyleBackColor = true;
            this.visualizeShowPlotButton.Click += new System.EventHandler(this.VisualizeShowPlotButton_Click);
            // 
            // visualizeSavePlotButton
            // 
            this.visualizeSavePlotButton.Location = new System.Drawing.Point(157, 203);
            this.visualizeSavePlotButton.Margin = new System.Windows.Forms.Padding(2);
            this.visualizeSavePlotButton.Name = "visualizeSavePlotButton";
            this.visualizeSavePlotButton.Size = new System.Drawing.Size(110, 23);
            this.visualizeSavePlotButton.TabIndex = 3;
            this.visualizeSavePlotButton.Text = "Save";
            this.visualizeSavePlotButton.UseVisualStyleBackColor = true;
            this.visualizeSavePlotButton.Click += new System.EventHandler(this.VisualizeSavePlotButton_Click);
            // 
            // outputTabPage
            // 
            this.outputTabPage.Controls.Add(this.outputUseModelNumberGroupBox);
            this.outputTabPage.Controls.Add(this.outputTargetStudyComboBox);
            this.outputTabPage.Controls.Add(this.outputTargetStudyLabel);
            this.outputTabPage.Controls.Add(this.outputAllTrialsButton);
            this.outputTabPage.Controls.Add(this.outputParatoSolutionButton);
            this.outputTabPage.Controls.Add(this.outputStopButton);
            this.outputTabPage.Controls.Add(this.outputProgressBar);
            this.outputTabPage.Location = new System.Drawing.Point(4, 24);
            this.outputTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.outputTabPage.Size = new System.Drawing.Size(277, 289);
            this.outputTabPage.TabIndex = 3;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // outputUseModelNumberGroupBox
            // 
            this.outputUseModelNumberGroupBox.Controls.Add(this.outputModelNumTextBox);
            this.outputUseModelNumberGroupBox.Controls.Add(this.outputModelNumberButton);
            this.outputUseModelNumberGroupBox.Controls.Add(this.reflectToSliderButton);
            this.outputUseModelNumberGroupBox.Location = new System.Drawing.Point(5, 97);
            this.outputUseModelNumberGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.outputUseModelNumberGroupBox.Name = "outputUseModelNumberGroupBox";
            this.outputUseModelNumberGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.outputUseModelNumberGroupBox.Size = new System.Drawing.Size(262, 135);
            this.outputUseModelNumberGroupBox.TabIndex = 21;
            this.outputUseModelNumberGroupBox.TabStop = false;
            this.outputUseModelNumberGroupBox.Text = "Use Model Number";
            // 
            // outputModelNumTextBox
            // 
            this.outputModelNumTextBox.Location = new System.Drawing.Point(14, 30);
            this.outputModelNumTextBox.Name = "outputModelNumTextBox";
            this.outputModelNumTextBox.Size = new System.Drawing.Size(241, 23);
            this.outputModelNumTextBox.TabIndex = 12;
            this.outputModelNumTextBox.Text = "0";
            // 
            // outputModelNumberButton
            // 
            this.outputModelNumberButton.Location = new System.Drawing.Point(137, 63);
            this.outputModelNumberButton.Name = "outputModelNumberButton";
            this.outputModelNumberButton.Size = new System.Drawing.Size(117, 55);
            this.outputModelNumberButton.TabIndex = 13;
            this.outputModelNumberButton.Text = "Output the above number models";
            this.outputModelNumberButton.UseVisualStyleBackColor = true;
            this.outputModelNumberButton.Click += new System.EventHandler(this.OutputModelNumberButton_Click);
            // 
            // reflectToSliderButton
            // 
            this.reflectToSliderButton.Location = new System.Drawing.Point(14, 63);
            this.reflectToSliderButton.Name = "reflectToSliderButton";
            this.reflectToSliderButton.Size = new System.Drawing.Size(117, 55);
            this.reflectToSliderButton.TabIndex = 16;
            this.reflectToSliderButton.Text = "Reflect the result on the sliders";
            this.reflectToSliderButton.UseVisualStyleBackColor = true;
            this.reflectToSliderButton.Click += new System.EventHandler(this.ReflectToSliderButton_Click);
            // 
            // outputTargetStudyComboBox
            // 
            this.outputTargetStudyComboBox.FormattingEnabled = true;
            this.outputTargetStudyComboBox.Location = new System.Drawing.Point(113, 16);
            this.outputTargetStudyComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.outputTargetStudyComboBox.Name = "outputTargetStudyComboBox";
            this.outputTargetStudyComboBox.Size = new System.Drawing.Size(148, 23);
            this.outputTargetStudyComboBox.TabIndex = 20;
            // 
            // outputTargetStudyLabel
            // 
            this.outputTargetStudyLabel.AutoSize = true;
            this.outputTargetStudyLabel.Location = new System.Drawing.Point(23, 18);
            this.outputTargetStudyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.outputTargetStudyLabel.Name = "outputTargetStudyLabel";
            this.outputTargetStudyLabel.Size = new System.Drawing.Size(83, 15);
            this.outputTargetStudyLabel.TabIndex = 19;
            this.outputTargetStudyLabel.Text = "Target Study";
            // 
            // outputAllTrialsButton
            // 
            this.outputAllTrialsButton.Location = new System.Drawing.Point(143, 56);
            this.outputAllTrialsButton.Name = "outputAllTrialsButton";
            this.outputAllTrialsButton.Size = new System.Drawing.Size(117, 23);
            this.outputAllTrialsButton.TabIndex = 18;
            this.outputAllTrialsButton.Text = "All trials";
            this.outputAllTrialsButton.UseVisualStyleBackColor = true;
            this.outputAllTrialsButton.Click += new System.EventHandler(this.OutputAllTrialsButton_Click);
            // 
            // outputParatoSolutionButton
            // 
            this.outputParatoSolutionButton.Location = new System.Drawing.Point(19, 56);
            this.outputParatoSolutionButton.Name = "outputParatoSolutionButton";
            this.outputParatoSolutionButton.Size = new System.Drawing.Size(117, 23);
            this.outputParatoSolutionButton.TabIndex = 17;
            this.outputParatoSolutionButton.Text = "Pareto solutions";
            this.outputParatoSolutionButton.UseVisualStyleBackColor = true;
            this.outputParatoSolutionButton.Click += new System.EventHandler(this.OutputParatoSolutionButton_Click);
            // 
            // outputStopButton
            // 
            this.outputStopButton.Enabled = false;
            this.outputStopButton.Location = new System.Drawing.Point(217, 248);
            this.outputStopButton.Name = "outputStopButton";
            this.outputStopButton.Size = new System.Drawing.Size(50, 23);
            this.outputStopButton.TabIndex = 15;
            this.outputStopButton.Text = "Stop";
            this.outputStopButton.UseVisualStyleBackColor = true;
            this.outputStopButton.Click += new System.EventHandler(this.OutputStopButton_Click);
            // 
            // outputProgressBar
            // 
            this.outputProgressBar.Location = new System.Drawing.Point(19, 248);
            this.outputProgressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.outputProgressBar.Name = "outputProgressBar";
            this.outputProgressBar.Size = new System.Drawing.Size(182, 23);
            this.outputProgressBar.TabIndex = 14;
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.settingsTabControl);
            this.settingsTabPage.Location = new System.Drawing.Point(4, 24);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Size = new System.Drawing.Size(277, 289);
            this.settingsTabPage.TabIndex = 2;
            this.settingsTabPage.Text = "Settings";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.TPE);
            this.settingsTabControl.Controls.Add(this.GP);
            this.settingsTabControl.Controls.Add(this.NSGAII);
            this.settingsTabControl.Controls.Add(this.CMAES);
            this.settingsTabControl.Controls.Add(this.QMC);
            this.settingsTabControl.Controls.Add(this.tabPage1);
            this.settingsTabControl.Location = new System.Drawing.Point(2, 2);
            this.settingsTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(276, 286);
            this.settingsTabControl.TabIndex = 0;
            // 
            // TPE
            // 
            this.TPE.Controls.Add(this.tpeDefaultButton);
            this.TPE.Controls.Add(this.tpeNEICandidatesLabel);
            this.TPE.Controls.Add(this.tpeNStartupTrialsLabel);
            this.TPE.Controls.Add(this.tpePriorWeightLabel);
            this.TPE.Controls.Add(this.tpeEINumUpDown);
            this.TPE.Controls.Add(this.tpeStartupNumUpDown);
            this.TPE.Controls.Add(this.tpePriorNumUpDown);
            this.TPE.Controls.Add(this.tpeConstantLiarCheckBox);
            this.TPE.Controls.Add(this.tpeWarnIndependentSamplingCheckBox);
            this.TPE.Controls.Add(this.tpeGroupCheckBox);
            this.TPE.Controls.Add(this.tpeMultivariateCheckBox);
            this.TPE.Controls.Add(this.tpeConsiderEndpointsCheckBox);
            this.TPE.Controls.Add(this.tpeConsiderMagicClipCheckBox);
            this.TPE.Controls.Add(this.tpeConsiderPriorCheckBox);
            this.TPE.Location = new System.Drawing.Point(4, 24);
            this.TPE.Margin = new System.Windows.Forms.Padding(2);
            this.TPE.Name = "TPE";
            this.TPE.Padding = new System.Windows.Forms.Padding(2);
            this.TPE.Size = new System.Drawing.Size(268, 258);
            this.TPE.TabIndex = 0;
            this.TPE.Text = "TPE";
            this.TPE.UseVisualStyleBackColor = true;
            // 
            // tpeDefaultButton
            // 
            this.tpeDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.tpeDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.tpeDefaultButton.Name = "tpeDefaultButton";
            this.tpeDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.tpeDefaultButton.TabIndex = 13;
            this.tpeDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.tpeDefaultButton, "Set to Optuna\'s default value.");
            this.tpeDefaultButton.UseVisualStyleBackColor = true;
            this.tpeDefaultButton.Click += new System.EventHandler(this.TpeDefaultButton_Click);
            // 
            // tpeNEICandidatesLabel
            // 
            this.tpeNEICandidatesLabel.AutoSize = true;
            this.tpeNEICandidatesLabel.Location = new System.Drawing.Point(3, 29);
            this.tpeNEICandidatesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tpeNEICandidatesLabel.Name = "tpeNEICandidatesLabel";
            this.tpeNEICandidatesLabel.Size = new System.Drawing.Size(151, 15);
            this.tpeNEICandidatesLabel.TabIndex = 12;
            this.tpeNEICandidatesLabel.Text = "Number of EI candidates";
            this.toolTip1.SetToolTip(this.tpeNEICandidatesLabel, "Number of candidate samples used to calculate\r\nthe expected improvement.");
            // 
            // tpeNStartupTrialsLabel
            // 
            this.tpeNStartupTrialsLabel.AutoSize = true;
            this.tpeNStartupTrialsLabel.Location = new System.Drawing.Point(3, 5);
            this.tpeNStartupTrialsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tpeNStartupTrialsLabel.Name = "tpeNStartupTrialsLabel";
            this.tpeNStartupTrialsLabel.Size = new System.Drawing.Size(148, 15);
            this.tpeNStartupTrialsLabel.TabIndex = 11;
            this.tpeNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.tpeNStartupTrialsLabel, "The random sampling is used instead of the TPE algorithm\r\nuntil the given number " +
        "of trials finish in the same study.");
            // 
            // tpePriorWeightLabel
            // 
            this.tpePriorWeightLabel.AutoSize = true;
            this.tpePriorWeightLabel.Location = new System.Drawing.Point(3, 53);
            this.tpePriorWeightLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tpePriorWeightLabel.Name = "tpePriorWeightLabel";
            this.tpePriorWeightLabel.Size = new System.Drawing.Size(78, 15);
            this.tpePriorWeightLabel.TabIndex = 10;
            this.tpePriorWeightLabel.Text = "Prior Weight";
            this.toolTip1.SetToolTip(this.tpePriorWeightLabel, "The weight of the prior.");
            // 
            // tpeEINumUpDown
            // 
            this.tpeEINumUpDown.Location = new System.Drawing.Point(204, 28);
            this.tpeEINumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.tpeEINumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tpeEINumUpDown.Name = "tpeEINumUpDown";
            this.tpeEINumUpDown.Size = new System.Drawing.Size(63, 23);
            this.tpeEINumUpDown.TabIndex = 9;
            this.tpeEINumUpDown.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // tpeStartupNumUpDown
            // 
            this.tpeStartupNumUpDown.Location = new System.Drawing.Point(204, 4);
            this.tpeStartupNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.tpeStartupNumUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tpeStartupNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tpeStartupNumUpDown.Name = "tpeStartupNumUpDown";
            this.tpeStartupNumUpDown.Size = new System.Drawing.Size(63, 23);
            this.tpeStartupNumUpDown.TabIndex = 8;
            this.tpeStartupNumUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tpePriorNumUpDown
            // 
            this.tpePriorNumUpDown.DecimalPlaces = 1;
            this.tpePriorNumUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tpePriorNumUpDown.Location = new System.Drawing.Point(204, 52);
            this.tpePriorNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.tpePriorNumUpDown.Name = "tpePriorNumUpDown";
            this.tpePriorNumUpDown.Size = new System.Drawing.Size(63, 23);
            this.tpePriorNumUpDown.TabIndex = 7;
            this.tpePriorNumUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // tpeConstantLiarCheckBox
            // 
            this.tpeConstantLiarCheckBox.AutoSize = true;
            this.tpeConstantLiarCheckBox.Location = new System.Drawing.Point(161, 135);
            this.tpeConstantLiarCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeConstantLiarCheckBox.Name = "tpeConstantLiarCheckBox";
            this.tpeConstantLiarCheckBox.Size = new System.Drawing.Size(104, 19);
            this.tpeConstantLiarCheckBox.TabIndex = 6;
            this.tpeConstantLiarCheckBox.Text = "Constant Liar";
            this.toolTip1.SetToolTip(this.tpeConstantLiarCheckBox, "If True, \r\npenalize running trials to avoid suggesting parameter configurations n" +
        "earby.");
            this.tpeConstantLiarCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeWarnIndependentSamplingCheckBox
            // 
            this.tpeWarnIndependentSamplingCheckBox.AutoSize = true;
            this.tpeWarnIndependentSamplingCheckBox.Checked = true;
            this.tpeWarnIndependentSamplingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeWarnIndependentSamplingCheckBox.Location = new System.Drawing.Point(3, 159);
            this.tpeWarnIndependentSamplingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeWarnIndependentSamplingCheckBox.Name = "tpeWarnIndependentSamplingCheckBox";
            this.tpeWarnIndependentSamplingCheckBox.Size = new System.Drawing.Size(191, 19);
            this.tpeWarnIndependentSamplingCheckBox.TabIndex = 5;
            this.tpeWarnIndependentSamplingCheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.tpeWarnIndependentSamplingCheckBox, "If this is True and multivariate=True, \r\na warning message is emitted\r\nwhen the v" +
        "alue of a parameter is sampled by using an independent sampler. \r\nIf multivariat" +
        "e=False, this flag has no effect.");
            this.tpeWarnIndependentSamplingCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeGroupCheckBox
            // 
            this.tpeGroupCheckBox.AutoSize = true;
            this.tpeGroupCheckBox.Location = new System.Drawing.Point(161, 113);
            this.tpeGroupCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeGroupCheckBox.Name = "tpeGroupCheckBox";
            this.tpeGroupCheckBox.Size = new System.Drawing.Size(61, 19);
            this.tpeGroupCheckBox.TabIndex = 4;
            this.tpeGroupCheckBox.Text = "Group";
            this.toolTip1.SetToolTip(this.tpeGroupCheckBox, "If this and multivariate are True,\r\nthe multivariate TPE with the group decompose" +
        "d search space\r\nis used when suggesting parameters. ");
            this.tpeGroupCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeMultivariateCheckBox
            // 
            this.tpeMultivariateCheckBox.AutoSize = true;
            this.tpeMultivariateCheckBox.Checked = true;
            this.tpeMultivariateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeMultivariateCheckBox.Location = new System.Drawing.Point(161, 91);
            this.tpeMultivariateCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeMultivariateCheckBox.Name = "tpeMultivariateCheckBox";
            this.tpeMultivariateCheckBox.Size = new System.Drawing.Size(95, 19);
            this.tpeMultivariateCheckBox.TabIndex = 3;
            this.tpeMultivariateCheckBox.Text = "Multivariate";
            this.toolTip1.SetToolTip(this.tpeMultivariateCheckBox, "If this is True, \r\nthe multivariate TPE is used when suggesting parameters. \r\nThe" +
        " multivariate TPE is reported to outperform the independent TPE.");
            this.tpeMultivariateCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeConsiderEndpointsCheckBox
            // 
            this.tpeConsiderEndpointsCheckBox.AutoSize = true;
            this.tpeConsiderEndpointsCheckBox.Location = new System.Drawing.Point(3, 113);
            this.tpeConsiderEndpointsCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeConsiderEndpointsCheckBox.Name = "tpeConsiderEndpointsCheckBox";
            this.tpeConsiderEndpointsCheckBox.Size = new System.Drawing.Size(136, 19);
            this.tpeConsiderEndpointsCheckBox.TabIndex = 2;
            this.tpeConsiderEndpointsCheckBox.Text = "Consider Endpoints";
            this.toolTip1.SetToolTip(this.tpeConsiderEndpointsCheckBox, "Take endpoints of domains into account\r\nwhen calculating variances of Gaussians i" +
        "n Parzen estimator.");
            this.tpeConsiderEndpointsCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeConsiderMagicClipCheckBox
            // 
            this.tpeConsiderMagicClipCheckBox.AutoSize = true;
            this.tpeConsiderMagicClipCheckBox.Checked = true;
            this.tpeConsiderMagicClipCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeConsiderMagicClipCheckBox.Location = new System.Drawing.Point(3, 135);
            this.tpeConsiderMagicClipCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeConsiderMagicClipCheckBox.Name = "tpeConsiderMagicClipCheckBox";
            this.tpeConsiderMagicClipCheckBox.Size = new System.Drawing.Size(138, 19);
            this.tpeConsiderMagicClipCheckBox.TabIndex = 1;
            this.tpeConsiderMagicClipCheckBox.Text = "Consider Magic Clip";
            this.toolTip1.SetToolTip(this.tpeConsiderMagicClipCheckBox, "Enable a heuristic to limit the smallest variances of Gaussians used in the Parze" +
        "n estimator.");
            this.tpeConsiderMagicClipCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeConsiderPriorCheckBox
            // 
            this.tpeConsiderPriorCheckBox.AutoSize = true;
            this.tpeConsiderPriorCheckBox.Checked = true;
            this.tpeConsiderPriorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeConsiderPriorCheckBox.Location = new System.Drawing.Point(3, 91);
            this.tpeConsiderPriorCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.tpeConsiderPriorCheckBox.Name = "tpeConsiderPriorCheckBox";
            this.tpeConsiderPriorCheckBox.Size = new System.Drawing.Size(107, 19);
            this.tpeConsiderPriorCheckBox.TabIndex = 0;
            this.tpeConsiderPriorCheckBox.Text = "Consider Prior";
            this.toolTip1.SetToolTip(this.tpeConsiderPriorCheckBox, "Enhance the stability of Parzen estimator by imposing a Gaussian prior when True." +
        "");
            this.tpeConsiderPriorCheckBox.UseVisualStyleBackColor = true;
            // 
            // GP
            // 
            this.GP.Controls.Add(this.boTorchDefaultButton);
            this.GP.Controls.Add(this.boTorchNStartupTrialsLabel);
            this.GP.Controls.Add(this.boTorchStartupNumUpDown);
            this.GP.Location = new System.Drawing.Point(4, 24);
            this.GP.Margin = new System.Windows.Forms.Padding(2);
            this.GP.Name = "GP";
            this.GP.Padding = new System.Windows.Forms.Padding(2);
            this.GP.Size = new System.Drawing.Size(268, 258);
            this.GP.TabIndex = 1;
            this.GP.Text = "GP";
            this.GP.UseVisualStyleBackColor = true;
            // 
            // boTorchDefaultButton
            // 
            this.boTorchDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.boTorchDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.boTorchDefaultButton.Name = "boTorchDefaultButton";
            this.boTorchDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.boTorchDefaultButton.TabIndex = 14;
            this.boTorchDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.boTorchDefaultButton, "Set to Optuna\'s default value.");
            this.boTorchDefaultButton.UseVisualStyleBackColor = true;
            this.boTorchDefaultButton.Click += new System.EventHandler(this.BoTorchDefaultButton_Click);
            // 
            // boTorchNStartupTrialsLabel
            // 
            this.boTorchNStartupTrialsLabel.AutoSize = true;
            this.boTorchNStartupTrialsLabel.Location = new System.Drawing.Point(4, 5);
            this.boTorchNStartupTrialsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.boTorchNStartupTrialsLabel.Name = "boTorchNStartupTrialsLabel";
            this.boTorchNStartupTrialsLabel.Size = new System.Drawing.Size(148, 15);
            this.boTorchNStartupTrialsLabel.TabIndex = 13;
            this.boTorchNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.boTorchNStartupTrialsLabel, "Number of initial trials, that is the number of trials to resort to independent s" +
        "ampling.");
            // 
            // boTorchStartupNumUpDown
            // 
            this.boTorchStartupNumUpDown.Location = new System.Drawing.Point(204, 4);
            this.boTorchStartupNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.boTorchStartupNumUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.boTorchStartupNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.boTorchStartupNumUpDown.Name = "boTorchStartupNumUpDown";
            this.boTorchStartupNumUpDown.Size = new System.Drawing.Size(63, 23);
            this.boTorchStartupNumUpDown.TabIndex = 12;
            this.boTorchStartupNumUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NSGAII
            // 
            this.NSGAII.Controls.Add(this.nsgaCrossoverComboBox);
            this.NSGAII.Controls.Add(this.nsgaCrossoverCheckBox);
            this.NSGAII.Controls.Add(this.nsgaDefaultButton);
            this.NSGAII.Controls.Add(this.nsgaMutationProbCheckBox);
            this.NSGAII.Controls.Add(this.nsgaPopulationSizeLabel);
            this.NSGAII.Controls.Add(this.nsgaPopulationSizeUpDown);
            this.NSGAII.Controls.Add(this.nsgaSwappingProbLabel);
            this.NSGAII.Controls.Add(this.nsgaSwappingProbUpDown);
            this.NSGAII.Controls.Add(this.nsgaCrossoverProbLabel);
            this.NSGAII.Controls.Add(this.nsgaCrossoverProbUpDown);
            this.NSGAII.Controls.Add(this.nsgaMutationProbUpDown);
            this.NSGAII.Location = new System.Drawing.Point(4, 24);
            this.NSGAII.Margin = new System.Windows.Forms.Padding(2);
            this.NSGAII.Name = "NSGAII";
            this.NSGAII.Size = new System.Drawing.Size(268, 258);
            this.NSGAII.TabIndex = 3;
            this.NSGAII.Text = "NSGAII";
            this.NSGAII.UseVisualStyleBackColor = true;
            // 
            // nsgaCrossoverComboBox
            // 
            this.nsgaCrossoverComboBox.FormattingEnabled = true;
            this.nsgaCrossoverComboBox.Items.AddRange(new object[] {
            "Uniform",
            "BLXAlpha",
            "SPX",
            "SBX",
            "VSBX",
            "UNDX"});
            this.nsgaCrossoverComboBox.Location = new System.Drawing.Point(177, 103);
            this.nsgaCrossoverComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nsgaCrossoverComboBox.Name = "nsgaCrossoverComboBox";
            this.nsgaCrossoverComboBox.Size = new System.Drawing.Size(91, 23);
            this.nsgaCrossoverComboBox.TabIndex = 33;
            this.nsgaCrossoverComboBox.Text = "Uniform";
            // 
            // nsgaCrossoverCheckBox
            // 
            this.nsgaCrossoverCheckBox.AutoSize = true;
            this.nsgaCrossoverCheckBox.Location = new System.Drawing.Point(9, 103);
            this.nsgaCrossoverCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaCrossoverCheckBox.Name = "nsgaCrossoverCheckBox";
            this.nsgaCrossoverCheckBox.Size = new System.Drawing.Size(84, 19);
            this.nsgaCrossoverCheckBox.TabIndex = 24;
            this.nsgaCrossoverCheckBox.Text = "Crossover";
            this.toolTip1.SetToolTip(this.nsgaCrossoverCheckBox, "Crossover to be applied when creating child individuals. ");
            this.nsgaCrossoverCheckBox.UseVisualStyleBackColor = true;
            this.nsgaCrossoverCheckBox.CheckedChanged += new System.EventHandler(this.NsgaCrossoverCheckBox_CheckedChanged);
            // 
            // nsgaDefaultButton
            // 
            this.nsgaDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.nsgaDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaDefaultButton.Name = "nsgaDefaultButton";
            this.nsgaDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.nsgaDefaultButton.TabIndex = 23;
            this.nsgaDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.nsgaDefaultButton, "Set to Optuna\'s default value.");
            this.nsgaDefaultButton.UseVisualStyleBackColor = true;
            this.nsgaDefaultButton.Click += new System.EventHandler(this.NsgaDefaultButton_Click);
            // 
            // nsgaMutationProbCheckBox
            // 
            this.nsgaMutationProbCheckBox.AutoSize = true;
            this.nsgaMutationProbCheckBox.Location = new System.Drawing.Point(9, 5);
            this.nsgaMutationProbCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaMutationProbCheckBox.Name = "nsgaMutationProbCheckBox";
            this.nsgaMutationProbCheckBox.Size = new System.Drawing.Size(142, 19);
            this.nsgaMutationProbCheckBox.TabIndex = 22;
            this.nsgaMutationProbCheckBox.Text = "Mutation Probability";
            this.toolTip1.SetToolTip(this.nsgaMutationProbCheckBox, "If False, \r\nthe solver automatically calculates mutation probability.");
            this.nsgaMutationProbCheckBox.UseVisualStyleBackColor = true;
            this.nsgaMutationProbCheckBox.CheckedChanged += new System.EventHandler(this.NsgaMutationProbCheckedChanged);
            // 
            // nsgaPopulationSizeLabel
            // 
            this.nsgaPopulationSizeLabel.AutoSize = true;
            this.nsgaPopulationSizeLabel.Location = new System.Drawing.Point(6, 78);
            this.nsgaPopulationSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nsgaPopulationSizeLabel.Name = "nsgaPopulationSizeLabel";
            this.nsgaPopulationSizeLabel.Size = new System.Drawing.Size(95, 15);
            this.nsgaPopulationSizeLabel.TabIndex = 21;
            this.nsgaPopulationSizeLabel.Text = "Population Size";
            this.toolTip1.SetToolTip(this.nsgaPopulationSizeLabel, "Number of individuals (trials) in a generation.");
            // 
            // nsgaPopulationSizeUpDown
            // 
            this.nsgaPopulationSizeUpDown.Location = new System.Drawing.Point(206, 77);
            this.nsgaPopulationSizeUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaPopulationSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nsgaPopulationSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaPopulationSizeUpDown.Name = "nsgaPopulationSizeUpDown";
            this.nsgaPopulationSizeUpDown.Size = new System.Drawing.Size(63, 23);
            this.nsgaPopulationSizeUpDown.TabIndex = 20;
            this.nsgaPopulationSizeUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nsgaSwappingProbLabel
            // 
            this.nsgaSwappingProbLabel.AutoSize = true;
            this.nsgaSwappingProbLabel.Location = new System.Drawing.Point(6, 54);
            this.nsgaSwappingProbLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nsgaSwappingProbLabel.Name = "nsgaSwappingProbLabel";
            this.nsgaSwappingProbLabel.Size = new System.Drawing.Size(128, 15);
            this.nsgaSwappingProbLabel.TabIndex = 19;
            this.nsgaSwappingProbLabel.Text = "Swapping Probability";
            this.toolTip1.SetToolTip(this.nsgaSwappingProbLabel, "Probability of swapping each parameter of the parents during crossover.");
            // 
            // nsgaSwappingProbUpDown
            // 
            this.nsgaSwappingProbUpDown.DecimalPlaces = 2;
            this.nsgaSwappingProbUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nsgaSwappingProbUpDown.Location = new System.Drawing.Point(206, 53);
            this.nsgaSwappingProbUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaSwappingProbUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaSwappingProbUpDown.Name = "nsgaSwappingProbUpDown";
            this.nsgaSwappingProbUpDown.Size = new System.Drawing.Size(63, 23);
            this.nsgaSwappingProbUpDown.TabIndex = 18;
            this.nsgaSwappingProbUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // nsgaCrossoverProbLabel
            // 
            this.nsgaCrossoverProbLabel.AutoSize = true;
            this.nsgaCrossoverProbLabel.Location = new System.Drawing.Point(6, 30);
            this.nsgaCrossoverProbLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nsgaCrossoverProbLabel.Name = "nsgaCrossoverProbLabel";
            this.nsgaCrossoverProbLabel.Size = new System.Drawing.Size(130, 15);
            this.nsgaCrossoverProbLabel.TabIndex = 17;
            this.nsgaCrossoverProbLabel.Text = "Crossover Probability";
            this.toolTip1.SetToolTip(this.nsgaCrossoverProbLabel, "Probability that a crossover (parameters swapping between parents)\r\nwill occur wh" +
        "en creating a new individual.");
            // 
            // nsgaCrossoverProbUpDown
            // 
            this.nsgaCrossoverProbUpDown.DecimalPlaces = 2;
            this.nsgaCrossoverProbUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nsgaCrossoverProbUpDown.Location = new System.Drawing.Point(206, 29);
            this.nsgaCrossoverProbUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaCrossoverProbUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaCrossoverProbUpDown.Name = "nsgaCrossoverProbUpDown";
            this.nsgaCrossoverProbUpDown.Size = new System.Drawing.Size(63, 23);
            this.nsgaCrossoverProbUpDown.TabIndex = 16;
            this.nsgaCrossoverProbUpDown.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // nsgaMutationProbUpDown
            // 
            this.nsgaMutationProbUpDown.DecimalPlaces = 2;
            this.nsgaMutationProbUpDown.Enabled = false;
            this.nsgaMutationProbUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nsgaMutationProbUpDown.Location = new System.Drawing.Point(206, 5);
            this.nsgaMutationProbUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.nsgaMutationProbUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaMutationProbUpDown.Name = "nsgaMutationProbUpDown";
            this.nsgaMutationProbUpDown.Size = new System.Drawing.Size(63, 23);
            this.nsgaMutationProbUpDown.TabIndex = 14;
            // 
            // CMAES
            // 
            this.CMAES.Controls.Add(this.cmaEsWarmStartComboBox);
            this.CMAES.Controls.Add(this.cmaEsWarmStartCmaEsCheckBox);
            this.CMAES.Controls.Add(this.cmaEsPopulationSizeUpDown);
            this.CMAES.Controls.Add(this.cmaEsPopulationSizeLabel);
            this.CMAES.Controls.Add(this.cmaEsDefaultButton);
            this.CMAES.Controls.Add(this.cmaEsRestartCheckBox);
            this.CMAES.Controls.Add(this.cmaEsUseSaparableCmaCheckBox);
            this.CMAES.Controls.Add(this.cmaEsNStartupTrialsLabel);
            this.CMAES.Controls.Add(this.cmaEsStartupNumUpDown);
            this.CMAES.Controls.Add(this.cmaEsConsiderPruneTrialsCheckBox);
            this.CMAES.Controls.Add(this.cmaEsWarnIndependentSamplingCheckBox);
            this.CMAES.Controls.Add(this.cmaEsIncPopsizeLabel);
            this.CMAES.Controls.Add(this.cmaEsIncPopSizeUpDown);
            this.CMAES.Controls.Add(this.cmaEsSigmaCheckBox);
            this.CMAES.Controls.Add(this.cmaEsSigmaNumUpDown);
            this.CMAES.Location = new System.Drawing.Point(4, 24);
            this.CMAES.Margin = new System.Windows.Forms.Padding(2);
            this.CMAES.Name = "CMAES";
            this.CMAES.Size = new System.Drawing.Size(268, 258);
            this.CMAES.TabIndex = 2;
            this.CMAES.Text = "CMA-ES";
            this.CMAES.UseVisualStyleBackColor = true;
            // 
            // cmaEsWarmStartComboBox
            // 
            this.cmaEsWarmStartComboBox.Enabled = false;
            this.cmaEsWarmStartComboBox.FormattingEnabled = true;
            this.cmaEsWarmStartComboBox.Location = new System.Drawing.Point(146, 206);
            this.cmaEsWarmStartComboBox.Name = "cmaEsWarmStartComboBox";
            this.cmaEsWarmStartComboBox.Size = new System.Drawing.Size(121, 23);
            this.cmaEsWarmStartComboBox.TabIndex = 37;
            // 
            // cmaEsWarmStartCmaEsCheckBox
            // 
            this.cmaEsWarmStartCmaEsCheckBox.AutoSize = true;
            this.cmaEsWarmStartCmaEsCheckBox.Location = new System.Drawing.Point(7, 190);
            this.cmaEsWarmStartCmaEsCheckBox.Name = "cmaEsWarmStartCmaEsCheckBox";
            this.cmaEsWarmStartCmaEsCheckBox.Size = new System.Drawing.Size(145, 19);
            this.cmaEsWarmStartCmaEsCheckBox.TabIndex = 36;
            this.cmaEsWarmStartCmaEsCheckBox.Text = "Warm Start CMA-ES";
            this.cmaEsWarmStartCmaEsCheckBox.UseVisualStyleBackColor = true;
            this.cmaEsWarmStartCmaEsCheckBox.CheckedChanged += new System.EventHandler(this.CmaEsWarmStartCmaEsCheckBox_CheckedChanged);
            // 
            // cmaEsPopulationSizeUpDown
            // 
            this.cmaEsPopulationSizeUpDown.Enabled = false;
            this.cmaEsPopulationSizeUpDown.Location = new System.Drawing.Point(206, 138);
            this.cmaEsPopulationSizeUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsPopulationSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmaEsPopulationSizeUpDown.Name = "cmaEsPopulationSizeUpDown";
            this.cmaEsPopulationSizeUpDown.Size = new System.Drawing.Size(63, 23);
            this.cmaEsPopulationSizeUpDown.TabIndex = 35;
            this.cmaEsPopulationSizeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cmaEsPopulationSizeLabel
            // 
            this.cmaEsPopulationSizeLabel.AutoSize = true;
            this.cmaEsPopulationSizeLabel.Location = new System.Drawing.Point(5, 139);
            this.cmaEsPopulationSizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cmaEsPopulationSizeLabel.Name = "cmaEsPopulationSizeLabel";
            this.cmaEsPopulationSizeLabel.Size = new System.Drawing.Size(95, 15);
            this.cmaEsPopulationSizeLabel.TabIndex = 34;
            this.cmaEsPopulationSizeLabel.Text = "Population Size";
            this.toolTip1.SetToolTip(this.cmaEsPopulationSizeLabel, "A population size of CMA-ES. \r\nWhen set restart_strategy is checked,\r\nthis is use" +
        "d as the initial population size.");
            // 
            // cmaEsDefaultButton
            // 
            this.cmaEsDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.cmaEsDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsDefaultButton.Name = "cmaEsDefaultButton";
            this.cmaEsDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.cmaEsDefaultButton.TabIndex = 33;
            this.cmaEsDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.cmaEsDefaultButton, "Set to Optuna\'s default value.");
            this.cmaEsDefaultButton.UseVisualStyleBackColor = true;
            this.cmaEsDefaultButton.Click += new System.EventHandler(this.CmaEsDefaultButton_Click);
            // 
            // cmaEsRestartCheckBox
            // 
            this.cmaEsRestartCheckBox.AutoSize = true;
            this.cmaEsRestartCheckBox.Location = new System.Drawing.Point(7, 116);
            this.cmaEsRestartCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsRestartCheckBox.Name = "cmaEsRestartCheckBox";
            this.cmaEsRestartCheckBox.Size = new System.Drawing.Size(120, 19);
            this.cmaEsRestartCheckBox.TabIndex = 32;
            this.cmaEsRestartCheckBox.Text = "RestartStrategy";
            this.toolTip1.SetToolTip(this.cmaEsRestartCheckBox, "If given False, \r\nCMA-ES will not restart.\r\nStrategy for restarting CMA-ES optimi" +
        "zation when converges to a local minimum. ");
            this.cmaEsRestartCheckBox.UseVisualStyleBackColor = true;
            this.cmaEsRestartCheckBox.CheckedChanged += new System.EventHandler(this.CmaEsRestartStrategyCheckedChanged);
            // 
            // cmaEsUseSaparableCmaCheckBox
            // 
            this.cmaEsUseSaparableCmaCheckBox.AutoSize = true;
            this.cmaEsUseSaparableCmaCheckBox.Location = new System.Drawing.Point(7, 93);
            this.cmaEsUseSaparableCmaCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsUseSaparableCmaCheckBox.Name = "cmaEsUseSaparableCmaCheckBox";
            this.cmaEsUseSaparableCmaCheckBox.Size = new System.Drawing.Size(140, 19);
            this.cmaEsUseSaparableCmaCheckBox.TabIndex = 31;
            this.cmaEsUseSaparableCmaCheckBox.Text = "Use Separable CMA";
            this.toolTip1.SetToolTip(this.cmaEsUseSaparableCmaCheckBox, resources.GetString("cmaEsUseSaparableCmaCheckBox.ToolTip"));
            this.cmaEsUseSaparableCmaCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaEsNStartupTrialsLabel
            // 
            this.cmaEsNStartupTrialsLabel.AutoSize = true;
            this.cmaEsNStartupTrialsLabel.Location = new System.Drawing.Point(5, 3);
            this.cmaEsNStartupTrialsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cmaEsNStartupTrialsLabel.Name = "cmaEsNStartupTrialsLabel";
            this.cmaEsNStartupTrialsLabel.Size = new System.Drawing.Size(148, 15);
            this.cmaEsNStartupTrialsLabel.TabIndex = 30;
            this.cmaEsNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.cmaEsNStartupTrialsLabel, "The independent sampling is used instead of the CMA-ES algorithm\r\nuntil the given" +
        " number of trials finish in the same study.");
            // 
            // cmaEsStartupNumUpDown
            // 
            this.cmaEsStartupNumUpDown.Location = new System.Drawing.Point(206, 3);
            this.cmaEsStartupNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsStartupNumUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.cmaEsStartupNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmaEsStartupNumUpDown.Name = "cmaEsStartupNumUpDown";
            this.cmaEsStartupNumUpDown.Size = new System.Drawing.Size(63, 23);
            this.cmaEsStartupNumUpDown.TabIndex = 29;
            this.cmaEsStartupNumUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmaEsConsiderPruneTrialsCheckBox
            // 
            this.cmaEsConsiderPruneTrialsCheckBox.AutoSize = true;
            this.cmaEsConsiderPruneTrialsCheckBox.Location = new System.Drawing.Point(7, 71);
            this.cmaEsConsiderPruneTrialsCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsConsiderPruneTrialsCheckBox.Name = "cmaEsConsiderPruneTrialsCheckBox";
            this.cmaEsConsiderPruneTrialsCheckBox.Size = new System.Drawing.Size(155, 19);
            this.cmaEsConsiderPruneTrialsCheckBox.TabIndex = 28;
            this.cmaEsConsiderPruneTrialsCheckBox.Text = "Consider Pruned Trials";
            this.toolTip1.SetToolTip(this.cmaEsConsiderPruneTrialsCheckBox, "If this is True, \r\nthe PRUNED trials are considered for sampling.");
            this.cmaEsConsiderPruneTrialsCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaEsWarnIndependentSamplingCheckBox
            // 
            this.cmaEsWarnIndependentSamplingCheckBox.AutoSize = true;
            this.cmaEsWarnIndependentSamplingCheckBox.Checked = true;
            this.cmaEsWarnIndependentSamplingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmaEsWarnIndependentSamplingCheckBox.Location = new System.Drawing.Point(7, 49);
            this.cmaEsWarnIndependentSamplingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsWarnIndependentSamplingCheckBox.Name = "cmaEsWarnIndependentSamplingCheckBox";
            this.cmaEsWarnIndependentSamplingCheckBox.Size = new System.Drawing.Size(191, 19);
            this.cmaEsWarnIndependentSamplingCheckBox.TabIndex = 27;
            this.cmaEsWarnIndependentSamplingCheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.cmaEsWarnIndependentSamplingCheckBox, "If this is True, \r\na warning message is emitted when the value of a parameter is " +
        "sampled by using an independent sampler.");
            this.cmaEsWarnIndependentSamplingCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaEsIncPopsizeLabel
            // 
            this.cmaEsIncPopsizeLabel.AutoSize = true;
            this.cmaEsIncPopsizeLabel.Location = new System.Drawing.Point(5, 163);
            this.cmaEsIncPopsizeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cmaEsIncPopsizeLabel.Name = "cmaEsIncPopsizeLabel";
            this.cmaEsIncPopsizeLabel.Size = new System.Drawing.Size(159, 15);
            this.cmaEsIncPopsizeLabel.TabIndex = 26;
            this.cmaEsIncPopsizeLabel.Text = "Increasing Population Size";
            this.toolTip1.SetToolTip(this.cmaEsIncPopsizeLabel, "Multiplier for increasing population size before each restart.");
            // 
            // cmaEsIncPopSizeUpDown
            // 
            this.cmaEsIncPopSizeUpDown.Enabled = false;
            this.cmaEsIncPopSizeUpDown.Location = new System.Drawing.Point(206, 162);
            this.cmaEsIncPopSizeUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsIncPopSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmaEsIncPopSizeUpDown.Name = "cmaEsIncPopSizeUpDown";
            this.cmaEsIncPopSizeUpDown.Size = new System.Drawing.Size(63, 23);
            this.cmaEsIncPopSizeUpDown.TabIndex = 25;
            this.cmaEsIncPopSizeUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cmaEsSigmaCheckBox
            // 
            this.cmaEsSigmaCheckBox.AutoSize = true;
            this.cmaEsSigmaCheckBox.Location = new System.Drawing.Point(7, 26);
            this.cmaEsSigmaCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsSigmaCheckBox.Name = "cmaEsSigmaCheckBox";
            this.cmaEsSigmaCheckBox.Size = new System.Drawing.Size(70, 19);
            this.cmaEsSigmaCheckBox.TabIndex = 24;
            this.cmaEsSigmaCheckBox.Text = "Sigma0";
            this.toolTip1.SetToolTip(this.cmaEsSigmaCheckBox, "Initial standard deviation of CMA-ES. By default, sigma0 is set to min_range / 6," +
        "\r\nwhere min_range denotes the minimum range of the distributions in the search s" +
        "pace.");
            this.cmaEsSigmaCheckBox.UseVisualStyleBackColor = true;
            this.cmaEsSigmaCheckBox.CheckedChanged += new System.EventHandler(this.CmaEsSigmaCheckedChanged);
            // 
            // cmaEsSigmaNumUpDown
            // 
            this.cmaEsSigmaNumUpDown.DecimalPlaces = 2;
            this.cmaEsSigmaNumUpDown.Enabled = false;
            this.cmaEsSigmaNumUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.cmaEsSigmaNumUpDown.Location = new System.Drawing.Point(206, 25);
            this.cmaEsSigmaNumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.cmaEsSigmaNumUpDown.Name = "cmaEsSigmaNumUpDown";
            this.cmaEsSigmaNumUpDown.Size = new System.Drawing.Size(63, 23);
            this.cmaEsSigmaNumUpDown.TabIndex = 23;
            // 
            // QMC
            // 
            this.QMC.Controls.Add(this.qmcDefaultButton);
            this.QMC.Controls.Add(this.qmcTypeComboBox);
            this.QMC.Controls.Add(this.qmcTypeLabel);
            this.QMC.Controls.Add(this.qmcWarnAsyncSeedingCheckBox);
            this.QMC.Controls.Add(this.qmcScrambleCheckBox);
            this.QMC.Controls.Add(this.qmcWarnIndependentSamplingCheckBox);
            this.QMC.Location = new System.Drawing.Point(4, 24);
            this.QMC.Margin = new System.Windows.Forms.Padding(2);
            this.QMC.Name = "QMC";
            this.QMC.Size = new System.Drawing.Size(268, 258);
            this.QMC.TabIndex = 4;
            this.QMC.Text = "QMC";
            this.QMC.UseVisualStyleBackColor = true;
            // 
            // qmcDefaultButton
            // 
            this.qmcDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.qmcDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.qmcDefaultButton.Name = "qmcDefaultButton";
            this.qmcDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.qmcDefaultButton.TabIndex = 33;
            this.qmcDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.qmcDefaultButton, "Set to Optuna\'s default value.");
            this.qmcDefaultButton.UseVisualStyleBackColor = true;
            this.qmcDefaultButton.Click += new System.EventHandler(this.QmcDefaultButton_Click);
            // 
            // qmcTypeComboBox
            // 
            this.qmcTypeComboBox.FormattingEnabled = true;
            this.qmcTypeComboBox.Items.AddRange(new object[] {
            "sobol",
            "halton"});
            this.qmcTypeComboBox.Location = new System.Drawing.Point(178, 7);
            this.qmcTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.qmcTypeComboBox.Name = "qmcTypeComboBox";
            this.qmcTypeComboBox.Size = new System.Drawing.Size(91, 23);
            this.qmcTypeComboBox.TabIndex = 32;
            this.qmcTypeComboBox.Text = "sobol";
            // 
            // qmcTypeLabel
            // 
            this.qmcTypeLabel.AutoSize = true;
            this.qmcTypeLabel.Location = new System.Drawing.Point(2, 9);
            this.qmcTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.qmcTypeLabel.Name = "qmcTypeLabel";
            this.qmcTypeLabel.Size = new System.Drawing.Size(66, 15);
            this.qmcTypeLabel.TabIndex = 31;
            this.qmcTypeLabel.Text = "QMC Type";
            this.toolTip1.SetToolTip(this.qmcTypeLabel, "The type of QMC sequence to be sampled.\r\n");
            // 
            // qmcWarnAsyncSeedingCheckBox
            // 
            this.qmcWarnAsyncSeedingCheckBox.AutoSize = true;
            this.qmcWarnAsyncSeedingCheckBox.Checked = true;
            this.qmcWarnAsyncSeedingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qmcWarnAsyncSeedingCheckBox.Location = new System.Drawing.Point(5, 81);
            this.qmcWarnAsyncSeedingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.qmcWarnAsyncSeedingCheckBox.Name = "qmcWarnAsyncSeedingCheckBox";
            this.qmcWarnAsyncSeedingCheckBox.Size = new System.Drawing.Size(190, 19);
            this.qmcWarnAsyncSeedingCheckBox.TabIndex = 30;
            this.qmcWarnAsyncSeedingCheckBox.Text = "Warn Asynchronous Seeding";
            this.toolTip1.SetToolTip(this.qmcWarnAsyncSeedingCheckBox, "If this is True, \r\na warning message is emitted \r\nwhen the scrambling (randomizat" +
        "ion) is applied to the QMC sequence\r\nand the random seed of the sampler is not s" +
        "et manually.");
            this.qmcWarnAsyncSeedingCheckBox.UseVisualStyleBackColor = true;
            // 
            // qmcScrambleCheckBox
            // 
            this.qmcScrambleCheckBox.AutoSize = true;
            this.qmcScrambleCheckBox.Location = new System.Drawing.Point(5, 37);
            this.qmcScrambleCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.qmcScrambleCheckBox.Name = "qmcScrambleCheckBox";
            this.qmcScrambleCheckBox.Size = new System.Drawing.Size(81, 19);
            this.qmcScrambleCheckBox.TabIndex = 29;
            this.qmcScrambleCheckBox.Text = "Scramble";
            this.toolTip1.SetToolTip(this.qmcScrambleCheckBox, "If this option is True, \r\nscrambling (randomization) is applied to the QMC sequen" +
        "ces.");
            this.qmcScrambleCheckBox.UseVisualStyleBackColor = true;
            // 
            // qmcWarnIndependentSamplingCheckBox
            // 
            this.qmcWarnIndependentSamplingCheckBox.AutoSize = true;
            this.qmcWarnIndependentSamplingCheckBox.Checked = true;
            this.qmcWarnIndependentSamplingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qmcWarnIndependentSamplingCheckBox.Location = new System.Drawing.Point(5, 59);
            this.qmcWarnIndependentSamplingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.qmcWarnIndependentSamplingCheckBox.Name = "qmcWarnIndependentSamplingCheckBox";
            this.qmcWarnIndependentSamplingCheckBox.Size = new System.Drawing.Size(191, 19);
            this.qmcWarnIndependentSamplingCheckBox.TabIndex = 28;
            this.qmcWarnIndependentSamplingCheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.qmcWarnIndependentSamplingCheckBox, "If this is True, \r\na warning message is emitted when the value of a parameter\r\nis" +
        " sampled by using an independent sampler.");
            this.qmcWarnIndependentSamplingCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.miscDefaultButton);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.runGarbageCollectionLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(268, 258);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Misc";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // miscDefaultButton
            // 
            this.miscDefaultButton.Location = new System.Drawing.Point(200, 233);
            this.miscDefaultButton.Margin = new System.Windows.Forms.Padding(2);
            this.miscDefaultButton.Name = "miscDefaultButton";
            this.miscDefaultButton.Size = new System.Drawing.Size(67, 25);
            this.miscDefaultButton.TabIndex = 36;
            this.miscDefaultButton.Text = "Default";
            this.toolTip1.SetToolTip(this.miscDefaultButton, "Set to Tunny\'s default value.");
            this.miscDefaultButton.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Always",
            "HasGeometry",
            "NoExecute"});
            this.comboBox1.Location = new System.Drawing.Point(157, 9);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(114, 23);
            this.comboBox1.TabIndex = 34;
            this.comboBox1.Text = "HasGeometry";
            // 
            // runGarbageCollectionLabel
            // 
            this.runGarbageCollectionLabel.AutoSize = true;
            this.runGarbageCollectionLabel.Location = new System.Drawing.Point(4, 11);
            this.runGarbageCollectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.runGarbageCollectionLabel.Name = "runGarbageCollectionLabel";
            this.runGarbageCollectionLabel.Size = new System.Drawing.Size(142, 15);
            this.runGarbageCollectionLabel.TabIndex = 33;
            this.runGarbageCollectionLabel.Text = "Run Garbage Collection";
            this.toolTip1.SetToolTip(this.runGarbageCollectionLabel, "Setting whether or not per-trial data is targeted for garbage collection");
            // 
            // fileTabPage
            // 
            this.fileTabPage.Controls.Add(this.groupBox1);
            this.fileTabPage.Controls.Add(this.licenseGroupBox);
            this.fileTabPage.Location = new System.Drawing.Point(4, 24);
            this.fileTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.fileTabPage.Name = "fileTabPage";
            this.fileTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.fileTabPage.Size = new System.Drawing.Size(277, 289);
            this.fileTabPage.TabIndex = 4;
            this.fileTabPage.Text = "File";
            this.fileTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.setResultFilePathButton);
            this.groupBox1.Controls.Add(this.clearResultButton);
            this.groupBox1.Controls.Add(this.openResultFolderButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(252, 133);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // setResultFilePathButton
            // 
            this.setResultFilePathButton.Location = new System.Drawing.Point(39, 23);
            this.setResultFilePathButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.setResultFilePathButton.Name = "setResultFilePathButton";
            this.setResultFilePathButton.Size = new System.Drawing.Size(176, 26);
            this.setResultFilePathButton.TabIndex = 7;
            this.setResultFilePathButton.Text = "Set file path";
            this.setResultFilePathButton.UseVisualStyleBackColor = true;
            this.setResultFilePathButton.Click += new System.EventHandler(this.SetResultFilePathButton_Click);
            // 
            // clearResultButton
            // 
            this.clearResultButton.Location = new System.Drawing.Point(39, 91);
            this.clearResultButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clearResultButton.Name = "clearResultButton";
            this.clearResultButton.Size = new System.Drawing.Size(176, 28);
            this.clearResultButton.TabIndex = 5;
            this.clearResultButton.Text = "Clear flie";
            this.clearResultButton.UseVisualStyleBackColor = true;
            this.clearResultButton.Click += new System.EventHandler(this.ClearResultButton_Click);
            // 
            // openResultFolderButton
            // 
            this.openResultFolderButton.Location = new System.Drawing.Point(39, 57);
            this.openResultFolderButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.openResultFolderButton.Name = "openResultFolderButton";
            this.openResultFolderButton.Size = new System.Drawing.Size(176, 26);
            this.openResultFolderButton.TabIndex = 6;
            this.openResultFolderButton.Text = "Open folder";
            this.openResultFolderButton.UseVisualStyleBackColor = true;
            this.openResultFolderButton.Click += new System.EventHandler(this.OpenResultFolderButton_Click);
            // 
            // licenseGroupBox
            // 
            this.licenseGroupBox.Controls.Add(this.showThirdPartyLicenseButton);
            this.licenseGroupBox.Controls.Add(this.showTunnyLicenseButton);
            this.licenseGroupBox.Location = new System.Drawing.Point(15, 157);
            this.licenseGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.licenseGroupBox.Name = "licenseGroupBox";
            this.licenseGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.licenseGroupBox.Size = new System.Drawing.Size(252, 121);
            this.licenseGroupBox.TabIndex = 8;
            this.licenseGroupBox.TabStop = false;
            this.licenseGroupBox.Text = "License";
            // 
            // showThirdPartyLicenseButton
            // 
            this.showThirdPartyLicenseButton.Location = new System.Drawing.Point(39, 65);
            this.showThirdPartyLicenseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.showThirdPartyLicenseButton.Name = "showThirdPartyLicenseButton";
            this.showThirdPartyLicenseButton.Size = new System.Drawing.Size(176, 28);
            this.showThirdPartyLicenseButton.TabIndex = 8;
            this.showThirdPartyLicenseButton.Text = "Show Third Party License";
            this.showThirdPartyLicenseButton.UseVisualStyleBackColor = true;
            this.showThirdPartyLicenseButton.Click += new System.EventHandler(this.ShowThirdPartyLicenseButton_Click);
            // 
            // showTunnyLicenseButton
            // 
            this.showTunnyLicenseButton.Location = new System.Drawing.Point(39, 29);
            this.showTunnyLicenseButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.showTunnyLicenseButton.Name = "showTunnyLicenseButton";
            this.showTunnyLicenseButton.Size = new System.Drawing.Size(176, 28);
            this.showTunnyLicenseButton.TabIndex = 7;
            this.showTunnyLicenseButton.Text = "Show Tunny License";
            this.showTunnyLicenseButton.UseVisualStyleBackColor = true;
            this.showTunnyLicenseButton.Click += new System.EventHandler(this.ShowTunnyLicenseButton_Click);
            // 
            // OptimizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(285, 317);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumUpDown)).EndInit();
            this.visualizeTabPage.ResumeLayout(false);
            this.visualizeTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizeClusterNumUpDown)).EndInit();
            this.outputTabPage.ResumeLayout(false);
            this.outputTabPage.PerformLayout();
            this.outputUseModelNumberGroupBox.ResumeLayout(false);
            this.outputUseModelNumberGroupBox.PerformLayout();
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabControl.ResumeLayout(false);
            this.TPE.ResumeLayout(false);
            this.TPE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpeEINumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpeStartupNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpePriorNumUpDown)).EndInit();
            this.GP.ResumeLayout(false);
            this.GP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boTorchStartupNumUpDown)).EndInit();
            this.NSGAII.ResumeLayout(false);
            this.NSGAII.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaPopulationSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaSwappingProbUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaCrossoverProbUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaMutationProbUpDown)).EndInit();
            this.CMAES.ResumeLayout(false);
            this.CMAES.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsPopulationSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsStartupNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsIncPopSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaEsSigmaNumUpDown)).EndInit();
            this.QMC.ResumeLayout(false);
            this.QMC.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.fileTabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.licenseGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button optimizeRunButton;
        private System.ComponentModel.BackgroundWorker optimizeBackgroundWorker;
        private System.Windows.Forms.Button optimizeStopButton;
        private System.Windows.Forms.NumericUpDown nTrialNumUpDown;
        private System.Windows.Forms.CheckBox continueStudyCheckBox;
        private System.Windows.Forms.ProgressBar optimizeProgressBar;
        private System.Windows.Forms.ComboBox samplerComboBox;
        private System.Windows.Forms.Label samplerTypeText;
        private System.Windows.Forms.Label nTrialText;
        private System.Windows.Forms.Label studyNameLabel;
        private System.Windows.Forms.TextBox studyNameTextBox;
        private System.Windows.Forms.TabControl optimizeTabControl;
        private System.Windows.Forms.TabPage optimizeTabPage;
        private System.Windows.Forms.TabPage visualizeTabPage;
        private System.Windows.Forms.Button visualizeShowPlotButton;
        private System.Windows.Forms.ComboBox visualizeTypeComboBox;
        private System.ComponentModel.BackgroundWorker outputResultBackgroundWorker;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.Button dashboardButton;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.Button outputAllTrialsButton;
        private System.Windows.Forms.Button outputParatoSolutionButton;
        private System.Windows.Forms.Button reflectToSliderButton;
        private System.Windows.Forms.Button outputStopButton;
        private System.Windows.Forms.ProgressBar outputProgressBar;
        private System.Windows.Forms.Button outputModelNumberButton;
        private System.Windows.Forms.TextBox outputModelNumTextBox;
        private System.Windows.Forms.TabPage fileTabPage;
        private System.Windows.Forms.Button openResultFolderButton;
        private System.Windows.Forms.Button clearResultButton;
        private System.Windows.Forms.NumericUpDown timeoutNumUpDown;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label Timeout;
        private System.Windows.Forms.TabControl settingsTabControl;
        private System.Windows.Forms.TabPage TPE;
        private System.Windows.Forms.TabPage GP;
        private System.Windows.Forms.TabPage NSGAII;
        private System.Windows.Forms.TabPage CMAES;
        private System.Windows.Forms.TabPage QMC;
        private System.Windows.Forms.Label tpeNEICandidatesLabel;
        private System.Windows.Forms.Label tpeNStartupTrialsLabel;
        private System.Windows.Forms.Label tpePriorWeightLabel;
        private System.Windows.Forms.NumericUpDown tpeEINumUpDown;
        private System.Windows.Forms.NumericUpDown tpeStartupNumUpDown;
        private System.Windows.Forms.NumericUpDown tpePriorNumUpDown;
        private System.Windows.Forms.CheckBox tpeConstantLiarCheckBox;
        private System.Windows.Forms.CheckBox tpeWarnIndependentSamplingCheckBox;
        private System.Windows.Forms.CheckBox tpeGroupCheckBox;
        private System.Windows.Forms.CheckBox tpeMultivariateCheckBox;
        private System.Windows.Forms.CheckBox tpeConsiderEndpointsCheckBox;
        private System.Windows.Forms.CheckBox tpeConsiderMagicClipCheckBox;
        private System.Windows.Forms.CheckBox tpeConsiderPriorCheckBox;
        private System.Windows.Forms.Label boTorchNStartupTrialsLabel;
        private System.Windows.Forms.NumericUpDown boTorchStartupNumUpDown;
        private System.Windows.Forms.CheckBox nsgaMutationProbCheckBox;
        private System.Windows.Forms.Label nsgaPopulationSizeLabel;
        private System.Windows.Forms.NumericUpDown nsgaPopulationSizeUpDown;
        private System.Windows.Forms.Label nsgaSwappingProbLabel;
        private System.Windows.Forms.NumericUpDown nsgaSwappingProbUpDown;
        private System.Windows.Forms.Label nsgaCrossoverProbLabel;
        private System.Windows.Forms.NumericUpDown nsgaCrossoverProbUpDown;
        private System.Windows.Forms.NumericUpDown nsgaMutationProbUpDown;
        private System.Windows.Forms.CheckBox cmaEsRestartCheckBox;
        private System.Windows.Forms.CheckBox cmaEsUseSaparableCmaCheckBox;
        private System.Windows.Forms.Label cmaEsNStartupTrialsLabel;
        private System.Windows.Forms.NumericUpDown cmaEsStartupNumUpDown;
        private System.Windows.Forms.CheckBox cmaEsConsiderPruneTrialsCheckBox;
        private System.Windows.Forms.CheckBox cmaEsWarnIndependentSamplingCheckBox;
        private System.Windows.Forms.Label cmaEsIncPopsizeLabel;
        private System.Windows.Forms.NumericUpDown cmaEsIncPopSizeUpDown;
        private System.Windows.Forms.CheckBox cmaEsSigmaCheckBox;
        private System.Windows.Forms.NumericUpDown cmaEsSigmaNumUpDown;
        private System.Windows.Forms.CheckBox qmcWarnIndependentSamplingCheckBox;
        private System.Windows.Forms.ComboBox qmcTypeComboBox;
        private System.Windows.Forms.Label qmcTypeLabel;
        private System.Windows.Forms.CheckBox qmcWarnAsyncSeedingCheckBox;
        private System.Windows.Forms.CheckBox qmcScrambleCheckBox;
        private System.Windows.Forms.Button tpeDefaultButton;
        private System.Windows.Forms.Button boTorchDefaultButton;
        private System.Windows.Forms.Button nsgaDefaultButton;
        private System.Windows.Forms.Button cmaEsDefaultButton;
        private System.Windows.Forms.Button qmcDefaultButton;
        private System.Windows.Forms.Label visualizeNumClusterLabel;
        private System.Windows.Forms.NumericUpDown visualizeClusterNumUpDown;
        private System.Windows.Forms.ComboBox nsgaCrossoverComboBox;
        private System.Windows.Forms.CheckBox nsgaCrossoverCheckBox;
        private System.Windows.Forms.NumericUpDown cmaEsPopulationSizeUpDown;
        private System.Windows.Forms.Label cmaEsPopulationSizeLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox licenseGroupBox;
        private System.Windows.Forms.Button showThirdPartyLicenseButton;
        private System.Windows.Forms.Button showTunnyLicenseButton;
        private System.Windows.Forms.Button visualizeSavePlotButton;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button miscDefaultButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label runGarbageCollectionLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label optimizeBestValueLabel;
        private System.Windows.Forms.Label optimizeTrialNumLabel;
        private System.Windows.Forms.CheckBox copyStudyCheckBox;
        private System.Windows.Forms.Button setResultFilePathButton;
        private System.Windows.Forms.Label existedStudyNameLabel;
        private System.Windows.Forms.ComboBox existingStudyComboBox;
        private System.Windows.Forms.Label visualizeTargetVariableLabel;
        private System.Windows.Forms.Label visualizeTargetObjectiveLabel;
        private System.Windows.Forms.Label visualizeTypeLabel;
        private System.Windows.Forms.Label visualizeTargetStudyNameLabel;
        private System.Windows.Forms.ComboBox visualizeTargetStudyComboBox;
        private System.Windows.Forms.ListBox visualizeVariableListBox;
        private System.Windows.Forms.ListBox visualizeObjectiveListBox;
        private System.Windows.Forms.CheckBox visualizeIncludeDominatedCheckBox;
        private System.Windows.Forms.GroupBox outputUseModelNumberGroupBox;
        private System.Windows.Forms.ComboBox outputTargetStudyComboBox;
        private System.Windows.Forms.Label outputTargetStudyLabel;
        private System.Windows.Forms.ComboBox cmaEsWarmStartComboBox;
        private System.Windows.Forms.CheckBox cmaEsWarmStartCmaEsCheckBox;
    }
}

