
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
            this.loadIfExistsCheckBox = new System.Windows.Forms.CheckBox();
            this.optimizeProgressBar = new System.Windows.Forms.ProgressBar();
            this.samplerComboBox = new System.Windows.Forms.ComboBox();
            this.samplerTypeText = new System.Windows.Forms.Label();
            this.studyNameLabel = new System.Windows.Forms.Label();
            this.studyNameTextBox = new System.Windows.Forms.TextBox();
            this.optimizeTabControl = new System.Windows.Forms.TabControl();
            this.optimizeTabPage = new System.Windows.Forms.TabPage();
            this.timeoutNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.Timeout = new System.Windows.Forms.Label();
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
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.TPE = new System.Windows.Forms.TabPage();
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
            this.botorchNStartupTrialsLabel = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.NSGAII = new System.Windows.Forms.TabPage();
            this.nsgaMutationProbCheckBox = new System.Windows.Forms.CheckBox();
            this.nsgaPopulationSizeLabel = new System.Windows.Forms.Label();
            this.nsgaPopulationSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaSwappingProbLabel = new System.Windows.Forms.Label();
            this.nsgaSwappingProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaCrossoverProbLabel = new System.Windows.Forms.Label();
            this.nsgaCrossoverProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.nsgaMutationProbUpDown = new System.Windows.Forms.NumericUpDown();
            this.CMAES = new System.Windows.Forms.TabPage();
            this.cmaesRestartCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaesUseSaparableCmaCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaesNStartupTrialsLabel = new System.Windows.Forms.Label();
            this.cmaesNStartup = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.cmaesStartupNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmaesConsiderPruneTrialsCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaesWarnIndependentSamplingCheckBox = new System.Windows.Forms.CheckBox();
            this.cmaesIncPopsizeLabel = new System.Windows.Forms.Label();
            this.cmaesIncPopSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmaesSigmaCheckBox = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.QMC = new System.Windows.Forms.TabPage();
            this.qmcTypeComboBox = new System.Windows.Forms.ComboBox();
            this.qmcTypeLabel = new System.Windows.Forms.Label();
            this.qmcWarnAsyncSeedingCheckBox = new System.Windows.Forms.CheckBox();
            this.qmcScrambleCheckBox = new System.Windows.Forms.CheckBox();
            this.qmcWarnIndependentSamplingcheckBox = new System.Windows.Forms.CheckBox();
            this.fileTabPage = new System.Windows.Forms.TabPage();
            this.openResultFolderButton = new System.Windows.Forms.Button();
            this.clearResultButton = new System.Windows.Forms.Button();
            this.outputResultBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nTrialNumUpDown)).BeginInit();
            this.optimizeTabControl.SuspendLayout();
            this.optimizeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumUpDown)).BeginInit();
            this.visualizeTabPage.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.TPE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpeEINumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpeStartupNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpePriorNumUpDown)).BeginInit();
            this.GP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.NSGAII.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaPopulationSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaSwappingProbUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaCrossoverProbUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaMutationProbUpDown)).BeginInit();
            this.CMAES.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaesStartupNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaesIncPopSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.QMC.SuspendLayout();
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
            this.loadIfExistsCheckBox.Location = new System.Drawing.Point(19, 156);
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
            "BayesianOptimization (TPE)",
            "BayesianOptimization (GP)",
            "GeneticAlgorithm (NSGA-II)",
            "EvolutionStrategy (CMA-ES)",
            "Quasi-MonteCarlo",
            "Random",
            "Grid"});
            this.samplerComboBox.Location = new System.Drawing.Point(104, 12);
            this.samplerComboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.samplerComboBox.Name = "samplerComboBox";
            this.samplerComboBox.Size = new System.Drawing.Size(258, 31);
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
            this.studyNameLabel.Location = new System.Drawing.Point(14, 193);
            this.studyNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.studyNameLabel.Name = "studyNameLabel";
            this.studyNameLabel.Size = new System.Drawing.Size(116, 23);
            this.studyNameLabel.TabIndex = 9;
            this.studyNameLabel.Text = "Study Name";
            // 
            // studyNameTextBox
            // 
            this.studyNameTextBox.Location = new System.Drawing.Point(151, 187);
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
            this.optimizeTabPage.Controls.Add(this.timeoutNumUpDown);
            this.optimizeTabPage.Controls.Add(this.Timeout);
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
            // timeoutNumUpDown
            // 
            this.timeoutNumUpDown.Location = new System.Drawing.Point(197, 102);
            this.timeoutNumUpDown.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.timeoutNumUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.timeoutNumUpDown.Name = "timeoutNumUpDown";
            this.timeoutNumUpDown.Size = new System.Drawing.Size(165, 30);
            this.timeoutNumUpDown.TabIndex = 12;
            this.timeoutNumUpDown.ThousandsSeparator = true;
            this.toolTip1.SetToolTip(this.timeoutNumUpDown, "After this time has elapsed, optimization stops.\r\nIf 0 is entered, no stop by tim" +
        "e is performed.");
            // 
            // Timeout
            // 
            this.Timeout.AutoSize = true;
            this.Timeout.Location = new System.Drawing.Point(16, 102);
            this.Timeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Timeout.Name = "Timeout";
            this.Timeout.Size = new System.Drawing.Size(132, 23);
            this.Timeout.TabIndex = 11;
            this.Timeout.Text = "Timeout (sec)";
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
            "slice",
            "hypervolume"});
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
            this.settingsTabPage.Controls.Add(this.settingsTabControl);
            this.settingsTabPage.Location = new System.Drawing.Point(4, 60);
            this.settingsTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.Size = new System.Drawing.Size(379, 374);
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
            this.settingsTabControl.Location = new System.Drawing.Point(3, 3);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(376, 371);
            this.settingsTabControl.TabIndex = 0;
            // 
            // TPE
            // 
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
            this.TPE.Location = new System.Drawing.Point(4, 32);
            this.TPE.Name = "TPE";
            this.TPE.Padding = new System.Windows.Forms.Padding(3);
            this.TPE.Size = new System.Drawing.Size(368, 335);
            this.TPE.TabIndex = 0;
            this.TPE.Text = "TPE";
            this.toolTip1.SetToolTip(this.TPE, "aa");
            this.TPE.ToolTipText = "aaaaa";
            this.TPE.UseVisualStyleBackColor = true;
            // 
            // tpeNEICandidatesLabel
            // 
            this.tpeNEICandidatesLabel.AutoSize = true;
            this.tpeNEICandidatesLabel.Location = new System.Drawing.Point(5, 44);
            this.tpeNEICandidatesLabel.Name = "tpeNEICandidatesLabel";
            this.tpeNEICandidatesLabel.Size = new System.Drawing.Size(225, 23);
            this.tpeNEICandidatesLabel.TabIndex = 12;
            this.tpeNEICandidatesLabel.Text = "Number of EI candidates";
            this.toolTip1.SetToolTip(this.tpeNEICandidatesLabel, "Number of candidate samples used to calculate the expected improvement.");
            // 
            // tpeNStartupTrialsLabel
            // 
            this.tpeNStartupTrialsLabel.AutoSize = true;
            this.tpeNStartupTrialsLabel.Location = new System.Drawing.Point(5, 8);
            this.tpeNStartupTrialsLabel.Name = "tpeNStartupTrialsLabel";
            this.tpeNStartupTrialsLabel.Size = new System.Drawing.Size(219, 23);
            this.tpeNStartupTrialsLabel.TabIndex = 11;
            this.tpeNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.tpeNStartupTrialsLabel, "The random sampling is used instead of the TPE algorithm until the given number o" +
        "f trials finish in the same study.");
            // 
            // tpePriorWeightLabel
            // 
            this.tpePriorWeightLabel.AutoSize = true;
            this.tpePriorWeightLabel.Location = new System.Drawing.Point(5, 80);
            this.tpePriorWeightLabel.Name = "tpePriorWeightLabel";
            this.tpePriorWeightLabel.Size = new System.Drawing.Size(118, 23);
            this.tpePriorWeightLabel.TabIndex = 10;
            this.tpePriorWeightLabel.Text = "Prior Weight";
            this.toolTip1.SetToolTip(this.tpePriorWeightLabel, "The weight of the prior.");
            // 
            // tpeEINumUpDown
            // 
            this.tpeEINumUpDown.Location = new System.Drawing.Point(253, 42);
            this.tpeEINumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tpeEINumUpDown.Name = "tpeEINumUpDown";
            this.tpeEINumUpDown.Size = new System.Drawing.Size(94, 30);
            this.tpeEINumUpDown.TabIndex = 9;
            this.tpeEINumUpDown.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // tpeStartupNumUpDown
            // 
            this.tpeStartupNumUpDown.Location = new System.Drawing.Point(253, 6);
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
            this.tpeStartupNumUpDown.Size = new System.Drawing.Size(94, 30);
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
            this.tpePriorNumUpDown.Location = new System.Drawing.Point(253, 78);
            this.tpePriorNumUpDown.Name = "tpePriorNumUpDown";
            this.tpePriorNumUpDown.Size = new System.Drawing.Size(94, 30);
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
            this.tpeConstantLiarCheckBox.Location = new System.Drawing.Point(210, 203);
            this.tpeConstantLiarCheckBox.Name = "tpeConstantLiarCheckBox";
            this.tpeConstantLiarCheckBox.Size = new System.Drawing.Size(152, 27);
            this.tpeConstantLiarCheckBox.TabIndex = 6;
            this.tpeConstantLiarCheckBox.Text = "Constant Liar";
            this.toolTip1.SetToolTip(this.tpeConstantLiarCheckBox, "If True, penalize running trials to avoid suggesting parameter configurations nea" +
        "rby.");
            this.tpeConstantLiarCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeWarnIndependentSamplingCheckBox
            // 
            this.tpeWarnIndependentSamplingCheckBox.AutoSize = true;
            this.tpeWarnIndependentSamplingCheckBox.Checked = true;
            this.tpeWarnIndependentSamplingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeWarnIndependentSamplingCheckBox.Location = new System.Drawing.Point(5, 239);
            this.tpeWarnIndependentSamplingCheckBox.Name = "tpeWarnIndependentSamplingCheckBox";
            this.tpeWarnIndependentSamplingCheckBox.Size = new System.Drawing.Size(284, 27);
            this.tpeWarnIndependentSamplingCheckBox.TabIndex = 5;
            this.tpeWarnIndependentSamplingCheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.tpeWarnIndependentSamplingCheckBox, "If this is True and multivariate=True, a warning message is emitted when the valu" +
        "e of a parameter is sampled by using an independent sampler. If multivariate=Fal" +
        "se, this flag has no effect.");
            this.tpeWarnIndependentSamplingCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeGroupCheckBox
            // 
            this.tpeGroupCheckBox.AutoSize = true;
            this.tpeGroupCheckBox.Location = new System.Drawing.Point(210, 170);
            this.tpeGroupCheckBox.Name = "tpeGroupCheckBox";
            this.tpeGroupCheckBox.Size = new System.Drawing.Size(89, 27);
            this.tpeGroupCheckBox.TabIndex = 4;
            this.tpeGroupCheckBox.Text = "Group";
            this.toolTip1.SetToolTip(this.tpeGroupCheckBox, "If this and multivariate are True, the multivariate TPE with the group decomposed" +
        " search space is used when suggesting parameters. ");
            this.tpeGroupCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeMultivariateCheckBox
            // 
            this.tpeMultivariateCheckBox.AutoSize = true;
            this.tpeMultivariateCheckBox.Location = new System.Drawing.Point(210, 137);
            this.tpeMultivariateCheckBox.Name = "tpeMultivariateCheckBox";
            this.tpeMultivariateCheckBox.Size = new System.Drawing.Size(137, 27);
            this.tpeMultivariateCheckBox.TabIndex = 3;
            this.tpeMultivariateCheckBox.Text = "Maltivariate";
            this.toolTip1.SetToolTip(this.tpeMultivariateCheckBox, "If this is True, the multivariate TPE is used when suggesting parameters. The mul" +
        "tivariate TPE is reported to outperform the independent TPE.");
            this.tpeMultivariateCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeConsiderEndpointsCheckBox
            // 
            this.tpeConsiderEndpointsCheckBox.AutoSize = true;
            this.tpeConsiderEndpointsCheckBox.Location = new System.Drawing.Point(5, 170);
            this.tpeConsiderEndpointsCheckBox.Name = "tpeConsiderEndpointsCheckBox";
            this.tpeConsiderEndpointsCheckBox.Size = new System.Drawing.Size(205, 27);
            this.tpeConsiderEndpointsCheckBox.TabIndex = 2;
            this.tpeConsiderEndpointsCheckBox.Text = "Consider Endpoints";
            this.toolTip1.SetToolTip(this.tpeConsiderEndpointsCheckBox, "Take endpoints of domains into account when calculating variances of Gaussians in" +
        " Parzen estimator.");
            this.tpeConsiderEndpointsCheckBox.UseVisualStyleBackColor = true;
            // 
            // tpeConsiderMagicClipCheckBox
            // 
            this.tpeConsiderMagicClipCheckBox.AutoSize = true;
            this.tpeConsiderMagicClipCheckBox.Checked = true;
            this.tpeConsiderMagicClipCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tpeConsiderMagicClipCheckBox.Location = new System.Drawing.Point(5, 203);
            this.tpeConsiderMagicClipCheckBox.Name = "tpeConsiderMagicClipCheckBox";
            this.tpeConsiderMagicClipCheckBox.Size = new System.Drawing.Size(207, 27);
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
            this.tpeConsiderPriorCheckBox.Location = new System.Drawing.Point(5, 137);
            this.tpeConsiderPriorCheckBox.Name = "tpeConsiderPriorCheckBox";
            this.tpeConsiderPriorCheckBox.Size = new System.Drawing.Size(159, 27);
            this.tpeConsiderPriorCheckBox.TabIndex = 0;
            this.tpeConsiderPriorCheckBox.Text = "Consider Prior";
            this.toolTip1.SetToolTip(this.tpeConsiderPriorCheckBox, "Enhance the stability of Parzen estimator by imposing a Gaussian prior when True." +
        "");
            this.tpeConsiderPriorCheckBox.UseVisualStyleBackColor = true;
            // 
            // GP
            // 
            this.GP.Controls.Add(this.botorchNStartupTrialsLabel);
            this.GP.Controls.Add(this.numericUpDown1);
            this.GP.Location = new System.Drawing.Point(4, 32);
            this.GP.Name = "GP";
            this.GP.Padding = new System.Windows.Forms.Padding(3);
            this.GP.Size = new System.Drawing.Size(368, 335);
            this.GP.TabIndex = 1;
            this.GP.Text = "GP";
            this.GP.UseVisualStyleBackColor = true;
            // 
            // botorchNStartupTrialsLabel
            // 
            this.botorchNStartupTrialsLabel.AutoSize = true;
            this.botorchNStartupTrialsLabel.Location = new System.Drawing.Point(6, 8);
            this.botorchNStartupTrialsLabel.Name = "botorchNStartupTrialsLabel";
            this.botorchNStartupTrialsLabel.Size = new System.Drawing.Size(219, 23);
            this.botorchNStartupTrialsLabel.TabIndex = 13;
            this.botorchNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.botorchNStartupTrialsLabel, "Number of initial trials, that is the number of trials to resort to independent s" +
        "ampling.");
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(254, 6);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(94, 30);
            this.numericUpDown1.TabIndex = 12;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NSGAII
            // 
            this.NSGAII.Controls.Add(this.nsgaMutationProbCheckBox);
            this.NSGAII.Controls.Add(this.nsgaPopulationSizeLabel);
            this.NSGAII.Controls.Add(this.nsgaPopulationSizeUpDown);
            this.NSGAII.Controls.Add(this.nsgaSwappingProbLabel);
            this.NSGAII.Controls.Add(this.nsgaSwappingProbUpDown);
            this.NSGAII.Controls.Add(this.nsgaCrossoverProbLabel);
            this.NSGAII.Controls.Add(this.nsgaCrossoverProbUpDown);
            this.NSGAII.Controls.Add(this.nsgaMutationProbUpDown);
            this.NSGAII.Location = new System.Drawing.Point(4, 32);
            this.NSGAII.Name = "NSGAII";
            this.NSGAII.Size = new System.Drawing.Size(368, 335);
            this.NSGAII.TabIndex = 3;
            this.NSGAII.Text = "NSGAII";
            this.NSGAII.UseVisualStyleBackColor = true;
            // 
            // nsgaMutationProbCheckBox
            // 
            this.nsgaMutationProbCheckBox.AutoSize = true;
            this.nsgaMutationProbCheckBox.Location = new System.Drawing.Point(13, 8);
            this.nsgaMutationProbCheckBox.Name = "nsgaMutationProbCheckBox";
            this.nsgaMutationProbCheckBox.Size = new System.Drawing.Size(212, 27);
            this.nsgaMutationProbCheckBox.TabIndex = 22;
            this.nsgaMutationProbCheckBox.Text = "Mutation Probability";
            this.toolTip1.SetToolTip(this.nsgaMutationProbCheckBox, "If False, the solver automatically calculates mutation probability.");
            this.nsgaMutationProbCheckBox.UseVisualStyleBackColor = true;
            // 
            // nsgaPopulationSizeLabel
            // 
            this.nsgaPopulationSizeLabel.AutoSize = true;
            this.nsgaPopulationSizeLabel.Location = new System.Drawing.Point(9, 117);
            this.nsgaPopulationSizeLabel.Name = "nsgaPopulationSizeLabel";
            this.nsgaPopulationSizeLabel.Size = new System.Drawing.Size(144, 23);
            this.nsgaPopulationSizeLabel.TabIndex = 21;
            this.nsgaPopulationSizeLabel.Text = "Population Size";
            this.toolTip1.SetToolTip(this.nsgaPopulationSizeLabel, "Number of individuals (trials) in a generation.");
            // 
            // nsgaPopulationSizeUpDown
            // 
            this.nsgaPopulationSizeUpDown.Location = new System.Drawing.Point(257, 115);
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
            this.nsgaPopulationSizeUpDown.Size = new System.Drawing.Size(94, 30);
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
            this.nsgaSwappingProbLabel.Location = new System.Drawing.Point(9, 81);
            this.nsgaSwappingProbLabel.Name = "nsgaSwappingProbLabel";
            this.nsgaSwappingProbLabel.Size = new System.Drawing.Size(194, 23);
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
            this.nsgaSwappingProbUpDown.Location = new System.Drawing.Point(257, 79);
            this.nsgaSwappingProbUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaSwappingProbUpDown.Name = "nsgaSwappingProbUpDown";
            this.nsgaSwappingProbUpDown.Size = new System.Drawing.Size(94, 30);
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
            this.nsgaCrossoverProbLabel.Location = new System.Drawing.Point(9, 45);
            this.nsgaCrossoverProbLabel.Name = "nsgaCrossoverProbLabel";
            this.nsgaCrossoverProbLabel.Size = new System.Drawing.Size(195, 23);
            this.nsgaCrossoverProbLabel.TabIndex = 17;
            this.nsgaCrossoverProbLabel.Text = "Crossover Probability";
            this.toolTip1.SetToolTip(this.nsgaCrossoverProbLabel, "Probability that a crossover (parameters swapping between parents) will occur whe" +
        "n creating a new individual.");
            // 
            // nsgaCrossoverProbUpDown
            // 
            this.nsgaCrossoverProbUpDown.DecimalPlaces = 2;
            this.nsgaCrossoverProbUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nsgaCrossoverProbUpDown.Location = new System.Drawing.Point(257, 43);
            this.nsgaCrossoverProbUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nsgaCrossoverProbUpDown.Name = "nsgaCrossoverProbUpDown";
            this.nsgaCrossoverProbUpDown.Size = new System.Drawing.Size(94, 30);
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
            this.nsgaMutationProbUpDown.Location = new System.Drawing.Point(257, 7);
            this.nsgaMutationProbUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nsgaMutationProbUpDown.Name = "nsgaMutationProbUpDown";
            this.nsgaMutationProbUpDown.Size = new System.Drawing.Size(94, 30);
            this.nsgaMutationProbUpDown.TabIndex = 14;
            // 
            // CMAES
            // 
            this.CMAES.Controls.Add(this.cmaesRestartCheckBox);
            this.CMAES.Controls.Add(this.cmaesUseSaparableCmaCheckBox);
            this.CMAES.Controls.Add(this.cmaesNStartupTrialsLabel);
            this.CMAES.Controls.Add(this.cmaesNStartup);
            this.CMAES.Controls.Add(this.numericUpDown4);
            this.CMAES.Controls.Add(this.cmaesStartupNumUpDown);
            this.CMAES.Controls.Add(this.cmaesConsiderPruneTrialsCheckBox);
            this.CMAES.Controls.Add(this.cmaesWarnIndependentSamplingCheckBox);
            this.CMAES.Controls.Add(this.cmaesIncPopsizeLabel);
            this.CMAES.Controls.Add(this.cmaesIncPopSizeUpDown);
            this.CMAES.Controls.Add(this.cmaesSigmaCheckBox);
            this.CMAES.Controls.Add(this.numericUpDown2);
            this.CMAES.Location = new System.Drawing.Point(4, 32);
            this.CMAES.Name = "CMAES";
            this.CMAES.Size = new System.Drawing.Size(368, 335);
            this.CMAES.TabIndex = 2;
            this.CMAES.Text = "CMA-ES";
            this.CMAES.UseVisualStyleBackColor = true;
            // 
            // cmaesRestartCheckBox
            // 
            this.cmaesRestartCheckBox.AutoSize = true;
            this.cmaesRestartCheckBox.Location = new System.Drawing.Point(11, 203);
            this.cmaesRestartCheckBox.Name = "cmaesRestartCheckBox";
            this.cmaesRestartCheckBox.Size = new System.Drawing.Size(171, 27);
            this.cmaesRestartCheckBox.TabIndex = 32;
            this.cmaesRestartCheckBox.Text = "RestartStrategy";
            this.toolTip1.SetToolTip(this.cmaesRestartCheckBox, "If given False, CMA-ES will not restart.\r\nStrategy for restarting CMA-ES optimiza" +
        "tion when converges to a local minimum. ");
            this.cmaesRestartCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaesUseSaparableCmaCheckBox
            // 
            this.cmaesUseSaparableCmaCheckBox.AutoSize = true;
            this.cmaesUseSaparableCmaCheckBox.Location = new System.Drawing.Point(11, 152);
            this.cmaesUseSaparableCmaCheckBox.Name = "cmaesUseSaparableCmaCheckBox";
            this.cmaesUseSaparableCmaCheckBox.Size = new System.Drawing.Size(204, 27);
            this.cmaesUseSaparableCmaCheckBox.TabIndex = 31;
            this.cmaesUseSaparableCmaCheckBox.Text = "Use Separable CMA";
            this.toolTip1.SetToolTip(this.cmaesUseSaparableCmaCheckBox, resources.GetString("cmaesUseSaparableCmaCheckBox.ToolTip"));
            this.cmaesUseSaparableCmaCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaesNStartupTrialsLabel
            // 
            this.cmaesNStartupTrialsLabel.AutoSize = true;
            this.cmaesNStartupTrialsLabel.Location = new System.Drawing.Point(7, 7);
            this.cmaesNStartupTrialsLabel.Name = "cmaesNStartupTrialsLabel";
            this.cmaesNStartupTrialsLabel.Size = new System.Drawing.Size(219, 23);
            this.cmaesNStartupTrialsLabel.TabIndex = 30;
            this.cmaesNStartupTrialsLabel.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.cmaesNStartupTrialsLabel, "The independent sampling is used instead of the CMA-ES algorithm until the given " +
        "number of trials finish in the same study.");
            // 
            // cmaesNStartup
            // 
            this.cmaesNStartup.AutoSize = true;
            this.cmaesNStartup.Location = new System.Drawing.Point(7, 5);
            this.cmaesNStartup.Name = "cmaesNStartup";
            this.cmaesNStartup.Size = new System.Drawing.Size(219, 23);
            this.cmaesNStartup.TabIndex = 30;
            this.cmaesNStartup.Text = "Number of startup trials";
            this.toolTip1.SetToolTip(this.cmaesNStartup, "The independent sampling is used instead of the CMA-ES algorithm until the given " +
        "number of trials finish in the same study.");
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(258, 5);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(94, 30);
            this.numericUpDown4.TabIndex = 29;
            this.numericUpDown4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmaesStartupNumUpDown
            // 
            this.cmaesStartupNumUpDown.Location = new System.Drawing.Point(258, 3);
            this.cmaesStartupNumUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.cmaesStartupNumUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmaesStartupNumUpDown.Name = "cmaesStartupNumUpDown";
            this.cmaesStartupNumUpDown.Size = new System.Drawing.Size(94, 30);
            this.cmaesStartupNumUpDown.TabIndex = 29;
            this.cmaesStartupNumUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmaesConsiderPruneTrialsCheckBox
            // 
            this.cmaesConsiderPruneTrialsCheckBox.AutoSize = true;
            this.cmaesConsiderPruneTrialsCheckBox.Location = new System.Drawing.Point(11, 119);
            this.cmaesConsiderPruneTrialsCheckBox.Name = "cmaesConsiderPruneTrialsCheckBox";
            this.cmaesConsiderPruneTrialsCheckBox.Size = new System.Drawing.Size(230, 27);
            this.cmaesConsiderPruneTrialsCheckBox.TabIndex = 28;
            this.cmaesConsiderPruneTrialsCheckBox.Text = "Consider Pruned Trials";
            this.toolTip1.SetToolTip(this.cmaesConsiderPruneTrialsCheckBox, "If this is True, the PRUNED trials are considered for sampling.");
            this.cmaesConsiderPruneTrialsCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaesWarnIndependentSamplingCheckBox
            // 
            this.cmaesWarnIndependentSamplingCheckBox.AutoSize = true;
            this.cmaesWarnIndependentSamplingCheckBox.Checked = true;
            this.cmaesWarnIndependentSamplingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmaesWarnIndependentSamplingCheckBox.Location = new System.Drawing.Point(11, 86);
            this.cmaesWarnIndependentSamplingCheckBox.Name = "cmaesWarnIndependentSamplingCheckBox";
            this.cmaesWarnIndependentSamplingCheckBox.Size = new System.Drawing.Size(284, 27);
            this.cmaesWarnIndependentSamplingCheckBox.TabIndex = 27;
            this.cmaesWarnIndependentSamplingCheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.cmaesWarnIndependentSamplingCheckBox, "If this is True and multivariate=True, a warning message is emitted when the valu" +
        "e of a parameter is sampled by using an independent sampler. If multivariate=Fal" +
        "se, this flag has no effect.");
            this.cmaesWarnIndependentSamplingCheckBox.UseVisualStyleBackColor = true;
            // 
            // cmaesIncPopsizeLabel
            // 
            this.cmaesIncPopsizeLabel.AutoSize = true;
            this.cmaesIncPopsizeLabel.Location = new System.Drawing.Point(7, 233);
            this.cmaesIncPopsizeLabel.Name = "cmaesIncPopsizeLabel";
            this.cmaesIncPopsizeLabel.Size = new System.Drawing.Size(240, 23);
            this.cmaesIncPopsizeLabel.TabIndex = 26;
            this.cmaesIncPopsizeLabel.Text = "Increasing Population Size";
            this.toolTip1.SetToolTip(this.cmaesIncPopsizeLabel, "Multiplier for increasing population size before each restart.");
            // 
            // cmaesIncPopSizeUpDown
            // 
            this.cmaesIncPopSizeUpDown.Enabled = false;
            this.cmaesIncPopSizeUpDown.Location = new System.Drawing.Point(258, 231);
            this.cmaesIncPopSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmaesIncPopSizeUpDown.Name = "cmaesIncPopSizeUpDown";
            this.cmaesIncPopSizeUpDown.Size = new System.Drawing.Size(94, 30);
            this.cmaesIncPopSizeUpDown.TabIndex = 25;
            this.cmaesIncPopSizeUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cmaesSigmaCheckBox
            // 
            this.cmaesSigmaCheckBox.AutoSize = true;
            this.cmaesSigmaCheckBox.Location = new System.Drawing.Point(11, 39);
            this.cmaesSigmaCheckBox.Name = "cmaesSigmaCheckBox";
            this.cmaesSigmaCheckBox.Size = new System.Drawing.Size(101, 27);
            this.cmaesSigmaCheckBox.TabIndex = 24;
            this.cmaesSigmaCheckBox.Text = "Sigma0";
            this.toolTip1.SetToolTip(this.cmaesSigmaCheckBox, "Initial standard deviation of CMA-ES. By default, sigma0 is set to min_range / 6," +
        " where min_range denotes the minimum range of the distributions in the search sp" +
        "ace.");
            this.cmaesSigmaCheckBox.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Enabled = false;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown2.Location = new System.Drawing.Point(258, 39);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(94, 30);
            this.numericUpDown2.TabIndex = 23;
            // 
            // QMC
            // 
            this.QMC.Controls.Add(this.qmcTypeComboBox);
            this.QMC.Controls.Add(this.qmcTypeLabel);
            this.QMC.Controls.Add(this.qmcWarnAsyncSeedingCheckBox);
            this.QMC.Controls.Add(this.qmcScrambleCheckBox);
            this.QMC.Controls.Add(this.qmcWarnIndependentSamplingcheckBox);
            this.QMC.Location = new System.Drawing.Point(4, 32);
            this.QMC.Name = "QMC";
            this.QMC.Size = new System.Drawing.Size(368, 335);
            this.QMC.TabIndex = 4;
            this.QMC.Text = "QMC";
            this.QMC.UseVisualStyleBackColor = true;
            // 
            // qmcTypeComboBox
            // 
            this.qmcTypeComboBox.FormattingEnabled = true;
            this.qmcTypeComboBox.Items.AddRange(new object[] {
            "sobol",
            "halton"});
            this.qmcTypeComboBox.Location = new System.Drawing.Point(229, 10);
            this.qmcTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.qmcTypeComboBox.Name = "qmcTypeComboBox";
            this.qmcTypeComboBox.Size = new System.Drawing.Size(135, 31);
            this.qmcTypeComboBox.TabIndex = 32;
            this.qmcTypeComboBox.Text = "sobol";
            // 
            // qmcTypeLabel
            // 
            this.qmcTypeLabel.AutoSize = true;
            this.qmcTypeLabel.Location = new System.Drawing.Point(3, 13);
            this.qmcTypeLabel.Name = "qmcTypeLabel";
            this.qmcTypeLabel.Size = new System.Drawing.Size(97, 23);
            this.qmcTypeLabel.TabIndex = 31;
            this.qmcTypeLabel.Text = "QMC Type";
            this.toolTip1.SetToolTip(this.qmcTypeLabel, "The type of QMC sequence to be sampled. This must be one of “halton” and “sobol”." +
        " ");
            // 
            // qmcWarnAsyncSeedingCheckBox
            // 
            this.qmcWarnAsyncSeedingCheckBox.AutoSize = true;
            this.qmcWarnAsyncSeedingCheckBox.Checked = true;
            this.qmcWarnAsyncSeedingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qmcWarnAsyncSeedingCheckBox.Location = new System.Drawing.Point(7, 121);
            this.qmcWarnAsyncSeedingCheckBox.Name = "qmcWarnAsyncSeedingCheckBox";
            this.qmcWarnAsyncSeedingCheckBox.Size = new System.Drawing.Size(284, 27);
            this.qmcWarnAsyncSeedingCheckBox.TabIndex = 30;
            this.qmcWarnAsyncSeedingCheckBox.Text = "Warn Asynchronous Seeding";
            this.toolTip1.SetToolTip(this.qmcWarnAsyncSeedingCheckBox, "If this is True, a warning message is emitted when the scrambling (randomization)" +
        " is applied to the QMC sequence and the random seed of the sampler is not set ma" +
        "nually.");
            this.qmcWarnAsyncSeedingCheckBox.UseVisualStyleBackColor = true;
            // 
            // qmcScrambleCheckBox
            // 
            this.qmcScrambleCheckBox.AutoSize = true;
            this.qmcScrambleCheckBox.Location = new System.Drawing.Point(7, 55);
            this.qmcScrambleCheckBox.Name = "qmcScrambleCheckBox";
            this.qmcScrambleCheckBox.Size = new System.Drawing.Size(116, 27);
            this.qmcScrambleCheckBox.TabIndex = 29;
            this.qmcScrambleCheckBox.Text = "Scramble";
            this.toolTip1.SetToolTip(this.qmcScrambleCheckBox, "If this option is True, scrambling (randomization) is applied to the QMC sequence" +
        "s.");
            this.qmcScrambleCheckBox.UseVisualStyleBackColor = true;
            // 
            // qmcWarnIndependentSamplingcheckBox
            // 
            this.qmcWarnIndependentSamplingcheckBox.AutoSize = true;
            this.qmcWarnIndependentSamplingcheckBox.Checked = true;
            this.qmcWarnIndependentSamplingcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qmcWarnIndependentSamplingcheckBox.Location = new System.Drawing.Point(7, 88);
            this.qmcWarnIndependentSamplingcheckBox.Name = "qmcWarnIndependentSamplingcheckBox";
            this.qmcWarnIndependentSamplingcheckBox.Size = new System.Drawing.Size(284, 27);
            this.qmcWarnIndependentSamplingcheckBox.TabIndex = 28;
            this.qmcWarnIndependentSamplingcheckBox.Text = "Warn Independent Sampling";
            this.toolTip1.SetToolTip(this.qmcWarnIndependentSamplingcheckBox, "If this is True, a warning message is emitted when the value of a parameter is sa" +
        "mpled by using an independent sampler.");
            this.qmcWarnIndependentSamplingcheckBox.UseVisualStyleBackColor = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumUpDown)).EndInit();
            this.visualizeTabPage.ResumeLayout(false);
            this.visualizeTabPage.PerformLayout();
            this.outputTabPage.ResumeLayout(false);
            this.outputTabPage.PerformLayout();
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabControl.ResumeLayout(false);
            this.TPE.ResumeLayout(false);
            this.TPE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tpeEINumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpeStartupNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tpePriorNumUpDown)).EndInit();
            this.GP.ResumeLayout(false);
            this.GP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.NSGAII.ResumeLayout(false);
            this.NSGAII.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaPopulationSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaSwappingProbUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaCrossoverProbUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nsgaMutationProbUpDown)).EndInit();
            this.CMAES.ResumeLayout(false);
            this.CMAES.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaesStartupNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmaesIncPopSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.QMC.ResumeLayout(false);
            this.QMC.PerformLayout();
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
        private System.Windows.Forms.Label botorchNStartupTrialsLabel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox nsgaMutationProbCheckBox;
        private System.Windows.Forms.Label nsgaPopulationSizeLabel;
        private System.Windows.Forms.NumericUpDown nsgaPopulationSizeUpDown;
        private System.Windows.Forms.Label nsgaSwappingProbLabel;
        private System.Windows.Forms.NumericUpDown nsgaSwappingProbUpDown;
        private System.Windows.Forms.Label nsgaCrossoverProbLabel;
        private System.Windows.Forms.NumericUpDown nsgaCrossoverProbUpDown;
        private System.Windows.Forms.NumericUpDown nsgaMutationProbUpDown;
        private System.Windows.Forms.CheckBox cmaesRestartCheckBox;
        private System.Windows.Forms.CheckBox cmaesUseSaparableCmaCheckBox;
        private System.Windows.Forms.Label cmaesNStartupTrialsLabel;
        private System.Windows.Forms.Label cmaesNStartup;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown cmaesStartupNumUpDown;
        private System.Windows.Forms.CheckBox cmaesConsiderPruneTrialsCheckBox;
        private System.Windows.Forms.CheckBox cmaesWarnIndependentSamplingCheckBox;
        private System.Windows.Forms.Label cmaesIncPopsizeLabel;
        private System.Windows.Forms.NumericUpDown cmaesIncPopSizeUpDown;
        private System.Windows.Forms.CheckBox cmaesSigmaCheckBox;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox qmcWarnIndependentSamplingcheckBox;
        private System.Windows.Forms.ComboBox qmcTypeComboBox;
        private System.Windows.Forms.Label qmcTypeLabel;
        private System.Windows.Forms.CheckBox qmcWarnAsyncSeedingCheckBox;
        private System.Windows.Forms.CheckBox qmcScrambleCheckBox;
    }
}

