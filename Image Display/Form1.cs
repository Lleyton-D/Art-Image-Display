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

namespace Image_Display
{
    public partial class Form1 : Form
    {
        private int imageSelect = 0; //keeps count of which image is being displayed in the folder
        private string[] fileList; //each string in array leads to an image path
        private Timer timer; //used to keep track of how long the timer will go for
        private string lastSelectedFolder = string.Empty; //keeps track of the location of last accessed folder
        public Form1()
        {
            InitializeComponent();
            lastSelectedFolder = Properties.Settings.Default.LastSelectedFolder; //Store the location of the last accessed folder
        }
        

        private void selectButton_Click(object sender, EventArgs e) //selects folder to be used
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(lastSelectedFolder)) 
                {
                    fbd.SelectedPath = lastSelectedFolder; // Set the last selected folder as the initial path
                }

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    lastSelectedFolder = fbd.SelectedPath;
                    fileList = Directory.GetFiles(fbd.SelectedPath);
                    imageSelect = 0;
                    displayImage();
                    
                    timer = new Timer();
                    timer.Interval = 300000; // Set the interval to 2 seconds (2000 milliseconds)
                    timerLabel.Text = "5:00";
                    timer.Start();

                    Properties.Settings.Default.LastSelectedFolder = lastSelectedFolder;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void backButton_Click(object sender, EventArgs e) //decrements the image selected
        {
            imageSelect--;
            displayImage();
        }

        private void pauseButton_Click(object sender, EventArgs e) //pauses the current timer
        {
            if(pauseButton.Text == "II")
            {
                timer.Stop();
                pauseButton.Text = "▶";
            }
            else
            {
                timer.Start();
                pauseButton.Text = "II";
            }
        }

        private void nextButton_Click(object sender, EventArgs e) //increments the image selected
        {
            imageSelect++;
            displayImage();
        }
        private void displayImage() //displays the currently selected image
        {
            if (imageSelect < 0)
                imageSelect = fileList.Length;
            else if (imageSelect > fileList.Length)
                imageSelect = 0;
            
            pictureBox.Image = Image.FromFile(fileList[imageSelect]);
        }

    }
}
