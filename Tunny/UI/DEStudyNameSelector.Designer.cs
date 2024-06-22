
namespace Tunny.UI
{
    partial class DEStudyNameSelector 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DEStudyNameSelector));
            this.okButton = new System.Windows.Forms.Button();
            this.studyNameComboBox = new System.Windows.Forms.ComboBox();
            this.selectTargetStudyNameLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(121, 117);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(105, 37);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // studyNameComboBox
            // 
            this.studyNameComboBox.FormattingEnabled = true;
            this.studyNameComboBox.Location = new System.Drawing.Point(36, 69);
            this.studyNameComboBox.Name = "studyNameComboBox";
            this.studyNameComboBox.Size = new System.Drawing.Size(430, 31);
            this.studyNameComboBox.TabIndex = 1;
            // 
            // selectTargetStudyNameLabel
            // 
            this.selectTargetStudyNameLabel.AutoSize = true;
            this.selectTargetStudyNameLabel.Location = new System.Drawing.Point(32, 25);
            this.selectTargetStudyNameLabel.Name = "selectTargetStudyNameLabel";
            this.selectTargetStudyNameLabel.Size = new System.Drawing.Size(325, 23);
            this.selectTargetStudyNameLabel.TabIndex = 2;
            this.selectTargetStudyNameLabel.Text = "Please select the target StudyName.";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(277, 117);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(105, 37);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // DEStudyNameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(495, 166);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectTargetStudyNameLabel);
            this.Controls.Add(this.studyNameComboBox);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "DEStudyNameSelector";
            this.Text = "DE StudyName Selector";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingXButton);
            this.Load += new System.EventHandler(this.Window_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox studyNameComboBox;
        private System.Windows.Forms.Label selectTargetStudyNameLabel;
        private System.Windows.Forms.Button cancelButton;
    }
}

