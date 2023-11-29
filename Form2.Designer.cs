using System.ComponentModel;

namespace TTSIntegration
{
    partial class Form2
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
            this.audioGeneratorBtn = new System.Windows.Forms.Button();
            this.inputText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.audioPlayerBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // audioGeneratorBtn
            // 
            this.audioGeneratorBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioGeneratorBtn.Location = new System.Drawing.Point(103, 279);
            this.audioGeneratorBtn.Margin = new System.Windows.Forms.Padding(2);
            this.audioGeneratorBtn.Name = "audioGeneratorBtn";
            this.audioGeneratorBtn.Size = new System.Drawing.Size(116, 53);
            this.audioGeneratorBtn.TabIndex = 0;
            this.audioGeneratorBtn.Text = "Convert To Bangla Audio";
            this.audioGeneratorBtn.UseVisualStyleBackColor = true;
            this.audioGeneratorBtn.Click += new System.EventHandler(this.AudioGenerateBtnClick);
            // 
            // inputText
            // 
            this.inputText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputText.Location = new System.Drawing.Point(24, 34);
            this.inputText.Margin = new System.Windows.Forms.Padding(2);
            this.inputText.Name = "inputText";
            this.inputText.Size = new System.Drawing.Size(485, 204);
            this.inputText.TabIndex = 1;
            this.inputText.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(223, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input Your Text";
            // 
            // audioPlayerBtn
            // 
            this.audioPlayerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.audioPlayerBtn.Location = new System.Drawing.Point(345, 280);
            this.audioPlayerBtn.Name = "audioPlayerBtn";
            this.audioPlayerBtn.Size = new System.Drawing.Size(97, 52);
            this.audioPlayerBtn.TabIndex = 3;
            this.audioPlayerBtn.Text = "Play the Audio";
            this.audioPlayerBtn.UseVisualStyleBackColor = true;
            this.audioPlayerBtn.Visible = false;
            this.audioPlayerBtn.Click += new System.EventHandler(this.SpeakOutBtnClick);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(123, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 37);
            this.label2.TabIndex = 4;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 344);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.audioPlayerBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputText);
            this.Controls.Add(this.audioGeneratorBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bangla TTS Integration";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button audioPlayerBtn;

        #endregion

        private System.Windows.Forms.Button audioGeneratorBtn;
        private System.Windows.Forms.RichTextBox inputText;
        private System.Windows.Forms.Label label1;
    }
}