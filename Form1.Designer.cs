using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TranferDataToSQLApp
{
    partial class UserInterface
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
            this.Browse = new System.Windows.Forms.Button();
            this.JobPos = new System.Windows.Forms.ComboBox();
            this.Title = new System.Windows.Forms.Label();
            this.FilePath = new System.Windows.Forms.TextBox();
            this.Submit = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.TaskListLabel = new System.Windows.Forms.Label();
            this.CurrentTask = new System.Windows.Forms.Label();
            this.TaskStep = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Enabled = false;
            this.Browse.Location = new System.Drawing.Point(394, 118);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(69, 24);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // JobPos
            // 
            this.JobPos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.JobPos.FormattingEnabled = true;
            this.JobPos.Location = new System.Drawing.Point(50, 57);
            this.JobPos.Name = "JobPos";
            this.JobPos.Size = new System.Drawing.Size(316, 21);
            this.JobPos.TabIndex = 1;
            this.JobPos.SelectedIndexChanged += new System.EventHandler(this.JobPos_SelectedIndexChanged);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(47, 29);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(27, 13);
            this.Title.TabIndex = 2;
            this.Title.Text = "Title";
            // 
            // FilePath
            // 
            this.FilePath.Enabled = false;
            this.FilePath.Location = new System.Drawing.Point(50, 122);
            this.FilePath.Name = "FilePath";
            this.FilePath.Size = new System.Drawing.Size(316, 20);
            this.FilePath.TabIndex = 3;
            this.FilePath.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // Submit
            // 
            this.Submit.Enabled = false;
            this.Submit.Location = new System.Drawing.Point(50, 174);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(79, 22);
            this.Submit.TabIndex = 5;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.Click += new System.EventHandler(this.Submit_ClickAsync);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(275, 174);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(78, 22);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "File";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.Control;
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(50, 239);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(316, 117);
            this.listBox1.TabIndex = 10;
            this.listBox1.Visible = false;
            // 
            // TaskListLabel
            // 
            this.TaskListLabel.AutoSize = true;
            this.TaskListLabel.Location = new System.Drawing.Point(47, 214);
            this.TaskListLabel.Name = "TaskListLabel";
            this.TaskListLabel.Size = new System.Drawing.Size(37, 13);
            this.TaskListLabel.TabIndex = 11;
            this.TaskListLabel.Text = "Steps:\r\n";
            this.TaskListLabel.Visible = false;
            // 
            // CurrentTask
            // 
            this.CurrentTask.AutoSize = true;
            this.CurrentTask.Location = new System.Drawing.Point(47, 368);
            this.CurrentTask.Name = "CurrentTask";
            this.CurrentTask.Size = new System.Drawing.Size(95, 13);
            this.CurrentTask.TabIndex = 12;
            this.CurrentTask.Text = "Processing Task:  ";
            this.CurrentTask.Visible = false;
            // 
            // TaskStep
            // 
            this.TaskStep.AutoSize = true;
            this.TaskStep.Location = new System.Drawing.Point(138, 368);
            this.TaskStep.Name = "TaskStep";
            this.TaskStep.Size = new System.Drawing.Size(41, 13);
            this.TaskStep.TabIndex = 13;
            this.TaskStep.Text = "(empty)";
            this.TaskStep.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(50, 394);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(331, 23);
            this.progressBar1.TabIndex = 14;
            this.progressBar1.Visible = false;
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 441);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.TaskStep);
            this.Controls.Add(this.CurrentTask);
            this.Controls.Add(this.TaskListLabel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.JobPos);
            this.Controls.Add(this.Browse);
            this.Name = "UserInterface";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.UserInterface_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.ComboBox JobPos;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Button Submit;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label TaskListLabel;
        private System.Windows.Forms.Label CurrentTask;
        private System.Windows.Forms.Label TaskStep;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

