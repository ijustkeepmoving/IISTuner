﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IISTuner
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        public static string InetDir = string.Empty;
        public static string AdminMessage = "Cannot access to the required folder or registery key.\nIIS Tuner tool needs access to registery to achieve required tunings.\nRunning this application under 'Administrator Mode' may solve this issue.";
        public static RegistryKey HKLM = null;
        internal RegistryKey IIS = null;
        public static int IISVersion = 0;

        bool LocateIIS()
        {
            try
            {

                HKLM = Registry.LocalMachine;

                IIS = HKLM.OpenSubKey("Software\\Microsoft\\InetStp");
                if (IIS == null)
                {
                    MessageBox.Show("IIS Not Installed!");
                    return false;
                }
                IISVersion = Convert.ToInt16(IIS.GetValue("MajorVersion"));
                if (IISVersion < 6)
                {
                    MessageBox.Show("This application compatible with the IIS versions 6 and 7");
                    return false;
                }

                if (IISVersion == 7)
                    InetDir = IIS.GetValue("InstallPath").ToString();
            }
            catch
            {
                MessageBox.Show(Loading.AdminMessage);
                return false;
            }

            return true;
        }

        public static string Net2Dir = string.Empty;
        public static string Net4Dir = string.Empty;
        public static string Net264Dir = string.Empty;
        public static string Net464Dir = string.Empty;

        public static bool Net264Exist = false;
        public static bool Net464Exist = false;
        public static bool Net2Exist = false;
        public static bool Net4Exist = false;

        private void StartFrom_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();
            this.Visible = false;
            string windir = System.Environment.GetEnvironmentVariable("windir");
            Net2Dir = windir + "\\Microsoft.NET\\Framework\\v2.0.50727";
            Net4Dir = windir + "\\Microsoft.NET\\Framework\\v4.0.30319";
            Net2Exist = Directory.Exists(Net2Dir);
            Net4Exist = Directory.Exists(Net4Dir);
            Net264Dir = windir + "\\Microsoft.NET\\Framework64\\v2.0.50727";
            Net464Dir = windir + "\\Microsoft.NET\\Framework64\\v4.0.30319";
            Net264Exist = Directory.Exists(Net264Dir);
            Net464Exist = Directory.Exists(Net464Dir);

            if (!Net2Exist && !Net4Exist)
            {
                MessageBox.Show(".Net Framework Installation not Found!");
            }
            else if (LocateIIS())
            {
                new Form1().ShowDialog();
            }
            this.Close();
            Application.Exit();
        }

    }
}
