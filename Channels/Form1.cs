using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Channels
{
    public partial class Form1 : Form
    {

        private Button pressedBtn; 
        public Form1()
        {
            InitializeComponent();
            Helper.verifyAceStreamIsRunning();
            //pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\Resources\\TV.jpg";
            loadButtons();
            loadFavorites();
            tabControl1.Controls[0].Text = "Channels";
            tabControl1.Controls[1].Text = "Favorites";
            for (int i = 0;i<Helper.categories.Count;i++)
            {
                TabPage tabPageT = new TabPage();
                FlowLayoutPanel flowLayoutPanelT = new FlowLayoutPanel();
                flowLayoutPanelT.AutoScroll = true;
                flowLayoutPanelT.Location = new System.Drawing.Point(6, 6);
                flowLayoutPanelT.Name = Helper.categories[i] + "_flowLayoutPanel";
                flowLayoutPanelT.Size = new System.Drawing.Size(410, 562);
                flowLayoutPanelT.TabIndex = 0;
               
                tabPageT.Controls.Add(flowLayoutPanelT);
                tabPageT.Location = new System.Drawing.Point(4, 22);
                tabPageT.Name = Helper.categories[i]+"_TabPage";
                tabPageT.Padding = new System.Windows.Forms.Padding(3);
                tabPageT.Size = new System.Drawing.Size(422, 574);
                tabPageT.TabIndex = 1;
                tabPageT.Text = Helper.categories[i];
                tabPageT.UseVisualStyleBackColor = true;
                tabControl1.TabPages.Add(tabPageT);
                tabControl1.Controls[i + 2].Controls.Add(new FlowLayoutPanel());

            }
            fillCategories();



        }
        

public  void fillCategories()
        {
            if (Helper.categories.Count > 0)
            {
                for (int i = 0; i < Helper.categories.Count; i++)
                {
                    foreach (Channel channel in Helper.channels)
                    {
                        if (channel.category.Contains(Helper.categories[i]))
                        { 
                        Button temp = new Button();
                        temp.Click += Temp_Click;
                        temp.Size = new System.Drawing.Size(124, 63);
                        temp.UseVisualStyleBackColor = true;
                        temp.Text = channel.name;
                        temp.MouseDown += button1_MouseDown;

                        tabControl1.Controls[i + 2].Controls[0].Controls.Add(temp);
                        }
                    }
                }
            }
        }

        public void loadButtons()
        {
            Helper.loadChannels("Channels.ini");
            flowLayoutPanel1.Controls.Clear();
            foreach (Channel channel in Helper.channels)
            {
                Button temp = new Button();
                temp.Click += Temp_Click; 
                temp.Size = new System.Drawing.Size(124, 63);      
                temp.UseVisualStyleBackColor = true;       
                temp.Text = channel.name;
                temp.MouseDown += button1_MouseDown;
                flowLayoutPanel1.Controls.Add(temp);
            }

        }

        public void loadFavorites()
        {
            flowLayoutPanel2.Controls.Clear();
    
            foreach (Channel channel in Helper.Favorites)
            {
                Button temp = new Button();
                temp.Click += Temp_Click;
                temp.Size = new System.Drawing.Size(124, 63);
                temp.UseVisualStyleBackColor = true;
                temp.Text = channel.name;
                temp.MouseDown += button1_MouseDown;
                flowLayoutPanel2.Controls.Add(temp);
            }

        }

        private void Temp_Click(object sender, EventArgs e)
        {
            
            Button btn = sender as Button;
            axVLCPlugin21.playlist.stop();
            axVLCPlugin21.Visible = true;
            button1.Visible = true;
            lbl_nowPlaying.Visible = true;
            btn_aspect.Visible = true;
            axVLCPlugin21.playlist.items.clear();
            lbl_nowPlaying.Text = getURL(btn.Text.ToString()).name;
            axVLCPlugin21.playlist.add(getURL(btn.Text.ToString()).url);
            axVLCPlugin21.playlist.play();
            
            //aspect= axVLCPlugin21.video.aspectRatio;
            axVLCPlugin21.video.aspectRatio = "Default";
            //axVLCPlugin21.Visible = true;
        }

        private Channel getURL(string name)
        {
            int index = Helper.channels.FindIndex(channel => (channel.name == name));
            return Helper.channels[index];

        }


    

        private void axVLCPlugin21_MediaPlayerStopped(object sender, EventArgs e)
        {
            //axVLCPlugin21.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            axVLCPlugin21.playlist.stop();
            button1.Visible = false;
            axVLCPlugin21.Visible = false;
            btn_aspect.Visible = false;
            lbl_nowPlaying.Visible = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void panel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void axVLCPlugin21_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void btn_aspect_Click(object sender, EventArgs e)
        {
            string temp = axVLCPlugin21.video.aspectRatio;
            if (btn_aspect.Text == "Aspect Ratio = " + "Default")
            {
                axVLCPlugin21.video.aspectRatio = "16:9";
                btn_aspect.Text = "Aspect Ratio = " + "16:9";
                
            }
            else if (btn_aspect.Text == "Aspect Ratio = " + "16:9")
            {
                axVLCPlugin21.video.aspectRatio = "4:3";
                btn_aspect.Text = "Aspect Ratio = " + "4:3";
            }
            else if (btn_aspect.Text == "Aspect Ratio = " + "4:3")
            {
                axVLCPlugin21.video.aspectRatio = "1:1";
                btn_aspect.Text = "Aspect Ratio = " + "1:1";
            }
            else if (btn_aspect.Text == "Aspect Ratio = " + "1:1")
            {
                axVLCPlugin21.video.aspectRatio = "16:10";
                btn_aspect.Text = "Aspect Ratio = " + "16:10";
            }
            else if (btn_aspect.Text == "Aspect Ratio = " + "16:10")
            {
                axVLCPlugin21.video.aspectRatio = "5:4";
                btn_aspect.Text = "Aspect Ratio = " + "5:4";
            }
            else if (btn_aspect.Text == "Aspect Ratio = " + "5:4")
            {
                //axVLCPlugin21.video.aspectRatio = "Default";
                axVLCPlugin21.video.aspectRatio = "16:9";
                btn_aspect.Text = "Aspect Ratio = " + "Default";
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                //do something
            }
            if (e.Button == MouseButtons.Right)
            {
                //do something
                ContextMenu cm = new ContextMenu();
                pressedBtn = btn;
                cm.MenuItems.Add("Add To Favorites", new EventHandler(addToFav_Click));
                cm.MenuItems.Add("Remove From Favorites", new EventHandler(remFromFav_Click));
                cm.MenuItems.Add("Move Up",new EventHandler(moveUp_Click));
                cm.MenuItems.Add("Move Down", new EventHandler(moveDown_Click));
                pressedBtn.ContextMenu = cm;
            }
        }

        private void moveDown_Click(object sender, EventArgs e)
        {
           
            int index = Helper.Favorites.FindIndex(x => (x.name == pressedBtn.Text));
            Channel temp = Helper.Favorites[index];
            if (index <Helper.Favorites.Count-1)
            {
                Helper.Favorites.RemoveAt(index);
                Helper.Favorites.Insert(index + 1, temp);
            }
            loadFavorites();
        }

        private void moveUp_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int index = Helper.Favorites.FindIndex(x => (x.name == pressedBtn.Text));
            Channel temp = Helper.Favorites[index];
            if (index > 0)
            { 
            Helper.Favorites.RemoveAt(index);
            Helper.Favorites.Insert(index - 1, temp);
            }
            loadFavorites();
            //int index= flowLayoutPanel2.Controls.IndexOf(pressedBtn);
            //if (index > 0)
            //{
            //    flowLayoutPanel2.Controls.SetChildIndex(flowLayoutPanel2.Controls[index], index - 1);
            //    flowLayoutPanel2.Controls.SetChildIndex(flowLayoutPanel2.Controls[index - 1], index);
            //    flowLayoutPanel2.Update();
            //}
        }

        private void remFromFav_Click(object sender, EventArgs e)
        {
            Helper.remFromFav(pressedBtn);
            loadFavorites();
        }

        private void addToFav_Click(object sender, EventArgs e)
        {
            Helper.addToFav(pressedBtn);
            loadFavorites();
        }

        private void btn_AddToFavs_Click(object sender, EventArgs e)
        {
            AddChannel add = new AddChannel(this);
            add.Show();
        }
    }
}
