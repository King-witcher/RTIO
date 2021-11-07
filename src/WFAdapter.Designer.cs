
namespace RTIO
{
    // This is a complete cheat. I use a windows forms window with a PictureBox to render everything =]
    // Unfortunately that's the only practical way I know yet.
    partial class WFAdapater
    {
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WFAdapater));
            this.outPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.outPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // outputBox
            // 
            this.outPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outPictureBox.Location = new System.Drawing.Point(0, 0);
            this.outPictureBox.Name = "outputBox";
            // this.outPictureBox.Size = new System.Drawing.Size(640, 360);
            this.outPictureBox.TabIndex = 0;
            this.outPictureBox.TabStop = false;
            // 
            // GLTechWindowForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.WhiteSpace;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            // this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.outPictureBox);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WindowsFormsAdapter";
            this.Text = "RTIO Window";
            ((System.ComponentModel.ISupportInitialize)(this.outPictureBox)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
