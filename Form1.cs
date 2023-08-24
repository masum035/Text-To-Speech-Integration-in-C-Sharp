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

        private void button1_Click(object sender, EventArgs e)
        {
            // string inputData = "দারিদ্রপীড়িত বাংলাদেশে অপুষ্টি একটি দুরূহ সমস্যা যা স্বাস্থ্য খাতে ব্যাপকভাবে প্রভাব বিস্তার করেছে। অপুষ্টিজনিত কারণে বাংলাদেশের ভবিষ্যৎ প্রজন্ম হিসাবে পরিচিত শিশুরা বিশ্ব ব্যাংকের জরীপে বিশ্বে শীর্ষস্থান দখল করেছে যা মোটেই কাঙ্ক্ষিত নয়। মোট জনগোষ্ঠীর ২৬% অপুষ্টিতে ভুগছে। ৪৬% শিশু মাঝারি থেকে গভীরতর পর্যায়ে ওজনজনিত সমস্যায় ভুগছে। ৫ বছর বয়সের পূর্বেই ৪৩% শিশু মারা যায়। প্রতি পাঁচ শিশুর একজন ভিটামিন এ এবং প্রতি দুইজনের একজন রক্তস্বল্পতাজনিত রোগে ভুগছে। ";
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            string inputData =
                "প্রাকৃতিক রূপবৈচিত্র্যে ভরা আমাদের এই বাংলাদেশ। এই দেশে পরিচিত অপরিচিত অনেক পর্যটক-আকর্ষক স্থান আছে। এর মধ্যে প্রত্নতাত্ত্বিক নিদর্শন, ঐতিহাসিক মসজিদ এবং মিনার, পৃথিবীর দীর্ঘতম প্রাকৃতিক সমুদ্র সৈকত, পাহাড়, অরণ্য ইত্যাদি অন্যতম। এদেশের প্রাকৃতিক সৌন্দর্য পর্যটকদের মুগ্ধ করে। বাংলাদেশের প্রত্যেকটি এলাকা বিভিন্ন স্বতন্ত্র্র বৈশিষ্ট্যে বিশেষায়িত । বাংলাদেশ দক্ষিণ এশিয়ার উত্তর পূর্ব অংশে অবস্থিত। বাংলাদেশের উত্তর সীমানা থেকে কিছু দূরে হিমালয় পর্বতমালা এবং দক্ষিণে বঙ্গোপসাগর। পশ্চিমে ভারতের পশ্চিমবঙ্গ, পূর্বে ভারতের ত্রিপুরা, মিজোরাম রাজ্য এবং মায়ানমারের পাহাড়ী এলাকা। অসংখ্য নদ-নদী পরিবেষ্টিত বাংলাদেশ প্রধানত সমতল ভূমি। দেশের উল্লেখযোগ্য নদ-নদী হলো- পদ্মা, ব্রহ্মপুত্র, সুরমা, কুশিয়ারা, মেঘনা ও কর্ণফুলী। একেকটি অঞ্চলের প্রাকৃতিক সৌন্দর্য ও খাদ্যাভ্যাস বিভিন্ন ধরনের। বাংলাদেশ রয়েল বেঙ্গল টাইগারের দেশ যার বাস সুন্দরবনে। এছাড়াও এখানে রয়েছে লাল মাটি দিয়ে নির্মিত মন্দির। এদেশে উল্লেখযোগ্য পর্যটন এলাকার মধ্যে রয়েছে: শ্র্রীমঙ্গল, যেখানে মাইলের পর মাইল জুড়ে রয়েছে চা বাগান। প্রত্নতাত্ত্বিক নিদর্শনের স্থানগুলোর মধ্যে রয়েছে–ময়নামতি, মহাস্থানগড় এবং পাহাড়পুর। রাঙ্গামাট, কাপ্তাই এবং কক্সবাজার প্রাকৃতিক দৃশ্যের জন্য খ্যাত। সুন্দরবনে আছে বন্য প্রাণী এবং পৃথিবীখ্যাত ম্যানগ্রোভ ফরেস্ট এ বনাঞ্চলে অবস্থিত । ";

            List<float[]> wavData = ONNXModelRunner.BanglaAudioGetter(inputData);
            ConcatenateAndSave(audioDataList: wavData, path: "concatenated.wav", sampleRate: 22050);


            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} seconds", stopwatch.ElapsedMilliseconds / 1000);
            //
            // using (var writer = new WaveFileWriter("output.wav", waveFormat))
            // {
            //     writer.WriteSamples(samples:xyz, offset:0, count:xyz.Length);
            // }
        }
    }
}