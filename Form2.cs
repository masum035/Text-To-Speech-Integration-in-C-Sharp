using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Forms;
using NAudio.Wave;

namespace TTSIntegration
{
    public partial class Form2 : Form
    {
        private static int _audioCounter = 0;
        private string _audioFilePath = "generated_audio_output.wav";
        private ModelRunner _modelRunner;
        private dynamic _pyModule = null;
        private SpeechSynthesizer synth;
        public Form2()
        {
            _modelRunner = new ModelRunner(ref _pyModule);
            synth = new SpeechSynthesizer();
            InitializeComponent();
            // inputText.Text = "প্রাকৃতিক রূপবৈচিত্র্যে ভরা আমাদের এই বাংলাদেশ। এই দেশে পরিচিত অপরিচিত অনেক পর্যটক-আকর্ষক স্থান আছে। এর মধ্যে প্রত্নতাত্ত্বিক নিদর্শন, ঐতিহাসিক মসজিদ এবং মিনার, পৃথিবীর দীর্ঘতম প্রাকৃতিক সমুদ্র সৈকত, পাহাড়, অরণ্য ইত্যাদি অন্যতম। এদেশের প্রাকৃতিক সৌন্দর্য পর্যটকদের মুগ্ধ করে। ";
            // inputText.Text = @"Bangladesh shares land borders with India to the west, north, and east, and Myanmar to the southeast; to the south it has a coastline along the Bay of Bengal. It is narrowly separated from Bhutan and Nepal by the Siliguri Corridor; and from China by the Indian state of Sikkim in the north. Dhaka, the capital and largest city, is the nation's political, financial and cultural centre. Chittagong, the second-largest city, is the busiest port on the Bay of Bengal. The official language is Bengali.";
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

        private static void ConcatenateAndSave(List<float[]> audioDataList, string path, int sampleRate = 22050)
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
        
        
        public static string ToUTF8(string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }
        
        private void AudioGenerateBtnClick(object sender, EventArgs e)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                string text = inputText.Text;
                // string text = ToUTF8(inputText.Text);
                Console.WriteLine(@"Conversion Started...");
                // List<float[]> wavData = ONNXModelRunner.BanglaAudioGetter(text);
                // List<float[]> wavData = ONNXModelRunner.bengaliByteArrayGetter(text);
                List<float[]> wavData = _modelRunner.JustFetchAudioArray(ref _pyModule, inputText:text);
                stopwatch.Stop();
                _audioFilePath = $"generated_audio_{_audioCounter}_in_{stopwatch.ElapsedMilliseconds / 1000}_seconds.wav";
                ConcatenateAndSave(audioDataList: wavData, path: _audioFilePath, sampleRate: 22050); // for vits model
                // ConcatenateAndSave(audioDataList: wavData, path: audioFilePath, sampleRate:16000); // for meta_ai
                _audioCounter++;
                label2.Text = $@"Conversion Elapsed Time: {stopwatch.ElapsedMilliseconds / 1000} second";
                
                string Caption = "Conversion of Text to Audio Completed";
                DialogResult result = MessageBox.Show(@"Your input text has been successfully converted to audio.",Caption
                    , MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception we)
            {
                Debug.WriteLine("\nException Caught!");
                Debug.WriteLine("Message :{0} ", we.Message);
            }
        }

        private void SpeakOutBtnClick(object sender, EventArgs e)
        {
            using (var audioFileReader = new AudioFileReader(_audioFilePath))
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