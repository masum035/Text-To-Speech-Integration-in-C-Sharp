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

        private static float MaxAbs(float[] data)
        {
            float max = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (Math.Abs(data[i]) > max)
                {
                    max = Math.Abs(data[i]);
                }
            }
            return max;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string inputData = "দক্ষিণ এশিয়ার প্রাচীন ও ধ্রুপদী যুগে বাংলাদেশ অঞ্চলটিতে বঙ্গ, পুণ্ড্র, গৌড়, গঙ্গাঋদ্ধি, সমতট ও হরিকেল নামক জনপদ";
            
            float[] wavData = ONNXModelRunner.BanglaAudioGetter(inputData);
            
            // Normalize the WAV data
            float maxAbsValue = Math.Max(0.01f, MaxAbs(wavData));
            short[] wavNormalized = new short[wavData.Length];
            for (int i = 0; i < wavData.Length; i++)
            {
                wavNormalized[i] = (short)(wavData[i] * (32767 / maxAbsValue));
            }
            
            using (var waveFileWriter = new WaveFileWriter("normalized_output.wav", new WaveFormat(22050, 16, 1)))
            {
                waveFileWriter.WriteSamples(wavNormalized, 0, wavNormalized.Length);
            }
            
            //
            // using (var writer = new WaveFileWriter("output.wav", waveFormat))
            // {
            //     writer.WriteSamples(samples:xyz, offset:0, count:xyz.Length);
            // }
        }
    }
}