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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace Image_Display
{
    public partial class Form1 : Form
    {
        private int imageSelect = 0; //keeps count of which image is being displayed in the folder
        private string[] fileList; //each string in array leads to an image path
        private Timer timer; //used to keep track of how long the timer will go for
        private int timeLeft; //time left in timer
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
                    
                    timer = new Timer(); //create a timer to count down
                    timeLeft = 60 * (int)timerNumeric.Value; //variable to store the length of timer
                    timer.Interval = 1000; //determines how frequently timer.tick is called (set to 1 sec)
                    timerLabel.Text = timerNumeric.Value + ":00"; //display time left
                    timer.Tick += ImageTimer_Tick; //call imageTimer_Tick each timer.Tick
                    timer.Start(); //start timer

                    displayImage(); //display the current image


                    Properties.Settings.Default.LastSelectedFolder = lastSelectedFolder; //store the last selected folder
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void backButton_Click(object sender, EventArgs e) //decrements the image selected
        {
            imageSelect--; //decrement the selected image
            displayImage(); //display previous image
        }

        private void pauseButton_Click(object sender, EventArgs e) //pauses the current timer
        {
            if(pauseButton.Text == "II") //pause symbol
            {
                timer.Stop();
                pauseButton.Text = "▶"; //play symbol
            }
            else
            {
                timer.Start();
                pauseButton.Text = "II"; //pause symbol
            }
        }

        private void nextButton_Click(object sender, EventArgs e) //increments the image selected
        {
            imageSelect++; //increment the selected image
            
            displayImage(); //display next image
        }
        private void displayImage() //displays the currently selected image
        {
            if (imageSelect < 0) //if the user is trying to select an image below zero skip to last image
                imageSelect += fileList.Length;
            else if (imageSelect >= fileList.Length) //if the user is trying to select an image past the last image skip to start
                imageSelect = 0;

            timeLeft = 60 * (int)timerNumeric.Value; //reset timer
            pictureBox.Image = Image.FromFile(fileList[imageSelect]);
        }
        private void ImageTimer_Tick(object sender, EventArgs e) //call method each timer interval
        {
            if (timeLeft > 0) //if there is still time left decrement timer
            {
                timeLeft--;
                UpdateTimeDisplay();
            }
            else
            {
                imageSelect++;
                displayImage();
            }
        }
        private void UpdateTimeDisplay() //update the image displayed
        {
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            timerLabel.Text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}
