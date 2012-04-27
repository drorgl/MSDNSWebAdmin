namespace DNSAdminGUI
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDumpServer = new System.Windows.Forms.Button();
            this.btnDomains = new System.Windows.Forms.Button();
            this.btnZones = new System.Windows.Forms.Button();
            this.btnCache = new System.Windows.Forms.Button();
            this.btnRootHints = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnServices = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnProtoServ = new System.Windows.Forms.Button();
            this.btnSigZone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(620, 486);
            this.textBox1.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(640, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDumpServer
            // 
            this.btnDumpServer.Location = new System.Drawing.Point(640, 43);
            this.btnDumpServer.Name = "btnDumpServer";
            this.btnDumpServer.Size = new System.Drawing.Size(97, 23);
            this.btnDumpServer.TabIndex = 2;
            this.btnDumpServer.Text = "Dump Server\r\n";
            this.btnDumpServer.UseVisualStyleBackColor = true;
            this.btnDumpServer.Click += new System.EventHandler(this.btnDumpServer_Click);
            // 
            // btnDomains
            // 
            this.btnDomains.Location = new System.Drawing.Point(640, 73);
            this.btnDomains.Name = "btnDomains";
            this.btnDomains.Size = new System.Drawing.Size(75, 23);
            this.btnDomains.TabIndex = 3;
            this.btnDomains.Text = "Domains";
            this.btnDomains.UseVisualStyleBackColor = true;
            this.btnDomains.Click += new System.EventHandler(this.btnDomains_Click);
            // 
            // btnZones
            // 
            this.btnZones.Location = new System.Drawing.Point(640, 103);
            this.btnZones.Name = "btnZones";
            this.btnZones.Size = new System.Drawing.Size(75, 23);
            this.btnZones.TabIndex = 4;
            this.btnZones.Text = "Zones";
            this.btnZones.UseVisualStyleBackColor = true;
            this.btnZones.Click += new System.EventHandler(this.btnZones_Click);
            // 
            // btnCache
            // 
            this.btnCache.Location = new System.Drawing.Point(640, 133);
            this.btnCache.Name = "btnCache";
            this.btnCache.Size = new System.Drawing.Size(75, 23);
            this.btnCache.TabIndex = 5;
            this.btnCache.Text = "Cache";
            this.btnCache.UseVisualStyleBackColor = true;
            this.btnCache.Click += new System.EventHandler(this.btnCache_Click);
            // 
            // btnRootHints
            // 
            this.btnRootHints.Location = new System.Drawing.Point(640, 163);
            this.btnRootHints.Name = "btnRootHints";
            this.btnRootHints.Size = new System.Drawing.Size(75, 23);
            this.btnRootHints.TabIndex = 6;
            this.btnRootHints.Text = "RootHints";
            this.btnRootHints.UseVisualStyleBackColor = true;
            this.btnRootHints.Click += new System.EventHandler(this.btnRootHints_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(640, 193);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(75, 23);
            this.btnStatistics.TabIndex = 7;
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnServices
            // 
            this.btnServices.Location = new System.Drawing.Point(640, 264);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(75, 23);
            this.btnServices.TabIndex = 8;
            this.btnServices.Text = "Services";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(640, 223);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "NSType";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnProtoServ
            // 
            this.btnProtoServ.Location = new System.Drawing.Point(640, 343);
            this.btnProtoServ.Name = "btnProtoServ";
            this.btnProtoServ.Size = new System.Drawing.Size(75, 23);
            this.btnProtoServ.TabIndex = 10;
            this.btnProtoServ.Text = "Proto/Serv";
            this.btnProtoServ.UseVisualStyleBackColor = true;
            this.btnProtoServ.Click += new System.EventHandler(this.btnProtoServ_Click);
            // 
            // btnSigZone
            // 
            this.btnSigZone.Location = new System.Drawing.Point(640, 418);
            this.btnSigZone.Name = "btnSigZone";
            this.btnSigZone.Size = new System.Drawing.Size(75, 23);
            this.btnSigZone.TabIndex = 11;
            this.btnSigZone.Text = "SIG Zone";
            this.btnSigZone.UseVisualStyleBackColor = true;
            this.btnSigZone.Click += new System.EventHandler(this.btnSigZone_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 542);
            this.Controls.Add(this.btnSigZone);
            this.Controls.Add(this.btnProtoServ);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnServices);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.btnRootHints);
            this.Controls.Add(this.btnCache);
            this.Controls.Add(this.btnZones);
            this.Controls.Add(this.btnDomains);
            this.Controls.Add(this.btnDumpServer);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDumpServer;
        private System.Windows.Forms.Button btnDomains;
        private System.Windows.Forms.Button btnZones;
        private System.Windows.Forms.Button btnCache;
        private System.Windows.Forms.Button btnRootHints;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnProtoServ;
        private System.Windows.Forms.Button btnSigZone;
    }
}

