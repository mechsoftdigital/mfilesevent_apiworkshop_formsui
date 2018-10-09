using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MFilesEvent.Forms
{
    public partial class UserForm : Form
    {

        public UserForm(string username, string vaultname)
        {
            InitializeComponent();

            this.label1.Text = username;
            this.label4.Text = vaultname;
        }
    }
}
