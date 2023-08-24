using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BanglaTTSIntegration;
using NAudio.Wave;

namespace TTSIntegration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputData = "দক্ষিণ এশিয়ার প্রাচীন ও ধ্রুপদী যুগে বাংলাদেশ অঞ্চলটিতে বঙ্গ, পুণ্ড্র, গৌড়, গঙ্গাঋদ্ধি, সমতট ও হরিকেল নামক জনপদ";
            
            float[] xyz = ONNXModelRunner.BanglaAudioGetter(inputData);
            
            var waveFormat = new WaveFormat(sampleRate:22050, channels:1);
            
            using (var writer = new WaveFileWriter("output.wav", waveFormat))
            {
                writer.WriteSamples(samples:xyz, offset:0, count:xyz.Length);
            }
        }
    }
}