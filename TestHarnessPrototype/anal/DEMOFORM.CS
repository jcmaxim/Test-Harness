using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace DemoForm
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button Browse;
		private System.ComponentModel.Container components = null;

    string path;

		public Form1()
		{
			InitializeComponent();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.Browse = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(40, 24);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(360, 20);
      this.textBox1.TabIndex = 0;
      this.textBox1.Text = "";
      // 
      // Browse
      // 
      this.Browse.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.Browse.Location = new System.Drawing.Point(424, 24);
      this.Browse.Name = "Browse";
      this.Browse.Size = new System.Drawing.Size(128, 23);
      this.Browse.TabIndex = 1;
      this.Browse.Text = "Browse for Tests";
      this.Browse.Click += new System.EventHandler(this.Browse_Click);
      // 
      // Form1
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(592, 262);
      this.Controls.Add(this.Browse);
      this.Controls.Add(this.textBox1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);

    }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

    private void Form1_Load(object sender, System.EventArgs e)
    {
    
    }

    private void Browse_Click(object sender, System.EventArgs e)
    {
      FolderBrowserDialog fb = new FolderBrowserDialog();
      fb.SelectedPath = Directory.GetCurrentDirectory();
      fb.ShowNewFolderButton = false;
      DialogResult result = fb.ShowDialog(this);
      if(result == DialogResult.OK)
        path = fb.SelectedPath;    
    }
	}
}
