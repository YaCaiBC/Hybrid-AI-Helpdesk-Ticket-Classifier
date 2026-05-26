namespace HelpdeskTicketClassifier
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtIssue = new TextBox();
            btnClassify = new Button();
            label2 = new Label();
            txtResult = new TextBox();
            btnClear = new Button();
            btnSave = new Button();
            TicketHistory = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)TicketHistory).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 94);
            label1.Name = "label1";
            label1.Size = new Size(138, 25);
            label1.TabIndex = 0;
            label1.Text = "Enter user issue:";
            // 
            // txtIssue
            // 
            txtIssue.Location = new Point(186, 94);
            txtIssue.Multiline = true;
            txtIssue.Name = "txtIssue";
            txtIssue.Size = new Size(550, 195);
            txtIssue.TabIndex = 1;
            // 
            // btnClassify
            // 
            btnClassify.Location = new Point(186, 346);
            btnClassify.Name = "btnClassify";
            btnClassify.Size = new Size(129, 34);
            btnClassify.TabIndex = 2;
            btnClassify.Text = "Classify Ticket";
            btnClassify.UseVisualStyleBackColor = true;
            btnClassify.Click += btnClassify_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 437);
            label2.Name = "label2";
            label2.Size = new Size(170, 25);
            label2.TabIndex = 3;
            label2.Text = "Classification Result:";
            // 
            // txtResult
            // 
            txtResult.Location = new Point(209, 434);
            txtResult.Multiline = true;
            txtResult.Name = "txtResult";
            txtResult.Size = new Size(527, 213);
            txtResult.TabIndex = 4;
            txtResult.TextChanged += textBox2_TextChanged;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(389, 346);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(112, 34);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(599, 348);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save Ticket";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // TicketHistory
            // 
            TicketHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TicketHistory.Location = new Point(33, 709);
            TicketHistory.Name = "TicketHistory";
            TicketHistory.RowHeadersWidth = 62;
            TicketHistory.Size = new Size(703, 225);
            TicketHistory.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(806, 1003);
            Controls.Add(TicketHistory);
            Controls.Add(btnSave);
            Controls.Add(btnClear);
            Controls.Add(txtResult);
            Controls.Add(label2);
            Controls.Add(btnClassify);
            Controls.Add(txtIssue);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)TicketHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtIssue;
        private Button btnClassify;
        private Label label2;
        private TextBox txtResult;
        private Button btnClear;
        private Button btnSave;
        private DataGridView TicketHistory;
    }
}
