using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Accessibility;
using BanglaTTSIntegration;
using ManagedWinapi.Accessibility;
using NAudio.Wave;

namespace TTSIntegration
{
    public partial class Form2 : Form
    {
        private string audioFilePath = "sliced_vits_tts_example.wav";
        private SpeechSynthesizer synth;
        public Form2()
        {
            synth = new SpeechSynthesizer();
            InitializeComponent();
            inputText.Text = "প্রাকৃতিক রূপবৈচিত্র্যে ভরা আমাদের এই বাংলাদেশ। এই দেশে পরিচিত অপরিচিত অনেক পর্যটক-আকর্ষক স্থান আছে। এর মধ্যে প্রত্নতাত্ত্বিক নিদর্শন, ঐতিহাসিক মসজিদ এবং মিনার, পৃথিবীর দীর্ঘতম প্রাকৃতিক সমুদ্র সৈকত, পাহাড়, অরণ্য ইত্যাদি অন্যতম। এদেশের প্রাকৃতিক সৌন্দর্য পর্যটকদের মুগ্ধ করে। ";
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

        public static void ConcatenateAndSave(List<float[]> audioDataList, string path, int sampleRate = 22050)
        {
            // Concatenate the multiple arrays of audio data into a single array
            int totalLength = 0;
            foreach (var data in audioDataList)
            {
                totalLength += data.Length;
            }
            
            float[] concatenatedData = new float[totalLength];
            int offset = 0;
            foreach (var data in audioDataList)
            {
                Array.Copy(data, 0, concatenatedData, offset, data.Length);
                offset += data.Length;
                
            }

            // Save the concatenated audio data as a WAV file
            // Normalize the WAV data
            float maxAbsValue = Math.Max(0.01f, MaxAbs(concatenatedData));
            short[] wavNormalized = new short[concatenatedData.Length];
            for (int i = 0; i < concatenatedData.Length; i++)
            {
                wavNormalized[i] = (short)(concatenatedData[i] * (32767 / maxAbsValue));
            }

            using (var waveFileWriter = new WaveFileWriter(path, new WaveFormat(sampleRate, 16, 1)))
            {
                waveFileWriter.WriteSamples(wavNormalized, 0, wavNormalized.Length);
            }
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                string text = inputText.Text;
                List<float[]> wavData = ONNXModelRunner.BanglaAudioGetter(text);
                ConcatenateAndSave(audioDataList: wavData, path: audioFilePath, sampleRate: 22050);

                string Caption = "Convertion of Text to Audio Completed";
                DialogResult result = MessageBox.Show("Your input text has been successfully converted to audio.",Caption
                    , MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception we)
            {
                Debug.WriteLine("\nException Caught!");
                Debug.WriteLine("Message :{0} ", we.Message);
            }
        }
        
        [DllImport("oleacc.dll")]
        public static extern int AccessibleObjectFromWindow(IntPtr hwnd, uint id, ref Guid iid, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object ppvObject);

        static List<Tuple<SystemAccessibleObject, int>> saoList = new List<Tuple<SystemAccessibleObject, int>>();
        public static void ProcessAccessibilityTree(SystemAccessibleObject sao, int level)
        {
            try
            {
                if ((int)sao.Role == 30)
                {
                    saoList.Add(new Tuple<SystemAccessibleObject, int>(sao, level));
                    Console.WriteLine($"Name: {sao.Name}, Description: {sao.Description}, Value: {sao.Value}");
                    foreach (SystemAccessibleObject child in sao.Children)
                    {
                        Console.WriteLine(child.Value);
                        ProcessAccessibilityTree(child, level + 1);
                    }
                }
                
            }
            catch (Exception e)
            {
                // Log the error (you can replace this with more sophisticated error handling)
                Console.WriteLine($"Error processing an accessible object: {e.Message}");
            }
        }
        public static SystemAccessibleObject GetAccessibleObjectFromWindowHandle(IntPtr hwnd)
        {
            object acc = null;
            Guid IID_IAccessible = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
            const int OBJID_WINDOW = 0x00000000;
            if (AccessibleObjectFromWindow(hwnd, OBJID_WINDOW, ref IID_IAccessible, ref acc) >= 0)
            {
                IAccessible iAccessible = (IAccessible)acc;
                return new SystemAccessibleObject(iAccessible, OBJID_WINDOW);
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var audioFileReader = new AudioFileReader(audioFilePath))
            using (var waveOut = new WaveOutEvent())
            {
                // Initialize the playback
                waveOut.Init(audioFileReader);

                // Start playback
                waveOut.Play();

                // Keep the program alive while audio plays
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}