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

namespace Channels
{
    public partial class AddChannel : Form
    {
        Form1 parent;
        public AddChannel(Form1 form1)
        {
            InitializeComponent();
            parent = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(tb_Name.Text.Equals("")||tb_URL.Equals(""))
            {
                MessageBox.Show("Name and URL cannot be empty", "Attention");
            }
            else
            {
                string url;
                if(tb_URL.Text.Contains("torrent-tv.ru/torrent-online.php?translation"))
                {
                    url = "http://127.0.0.1:6878/ace/getstream?url=http%3A%2F%2Fcontent.asplaylist.net%2F" + tb_URL.Text.Substring(tb_URL.Text.IndexOf("=") + 1)+ ".acelive";
                }
                else if(tb_URL.Text.Length<10)
                {
                    url = "http://127.0.0.1:6878/ace/getstream?url=http%3A%2F%2Fcontent.asplaylist.net%2F" + tb_URL.Text + ".acelive";
                }
                else
                {
                    url = tb_URL.Text;
                }
                Channel temp = new Channel();
                temp.url = url;
                temp.name = tb_Name.Text;
                temp.thumb = tb_Thumbnail.Text;
                Helper.channels.Add(temp);
                File.AppendAllText(Environment.CurrentDirectory + "//Channels.ini", Environment.NewLine + temp.name + " = " + temp.url);
                parent.loadButtons();

                if (checkBox1.CheckState==CheckState.Checked)
                {
                    Helper.Favorites.Add(temp);
                    File.AppendAllText(Environment.CurrentDirectory + "//Favorites.ini", Environment.NewLine + temp.name + " = " + temp.url);
                    parent.loadFavorites();
                }
                MessageBox.Show("Channel \"" + temp.name + "\" Added.", "Success");

            }
        }

        private void AddChannel_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            
            
        }
    }
}
