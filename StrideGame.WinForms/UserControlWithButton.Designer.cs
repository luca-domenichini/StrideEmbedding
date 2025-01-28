namespace StrideGame.WinForms;

partial class UserControlWithButton
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Button = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // Button
        // 
        Button.Location = new System.Drawing.Point(50, 43);
        Button.Name = "Button";
        Button.Size = new System.Drawing.Size(75, 23);
        Button.TabIndex = 0;
        Button.Text = "Pause";
        Button.UseVisualStyleBackColor = true;
        Button.Click += Button_Click;
        // 
        // UserControlWithButton
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(Button);
        Name = "UserControlWithButton";
        Size = new System.Drawing.Size(628, 399);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button Button;
}
