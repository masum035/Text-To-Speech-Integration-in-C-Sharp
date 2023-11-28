using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Python.Runtime;
// using Microsoft.ML;
// using Microsoft.ML.Data;

namespace TTSIntegration
{
    public class InputData
    {
        public PyObject Data { get; set; }
    }

    public class OutputData
    {
        [ColumnName("output")] public float[][] Data { get; set; }
    }

    public class ONNXModelRunner
    {
        public static PyObject GetTokenizer()
        {
            Runtime.PythonDLL = @"C:\Users\Malware\AppData\Local\Programs\Python\Python311\python311.dll";
            PythonEngine.Initialize();
            dynamic result;
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append("C://Users//Malware//PycharmProjects//VitsModelTesting");

                dynamic pyModule = Py.Import("TokenizerForTTS");

                result = pyModule.banglaTokenizer();
            }

            PythonEngine.Shutdown();
            return result;
        }

        public static float[] csArray;

        public static List<string> SliceString(string input, int maxLength = 450)
        {
            var slices = new List<string>();

            for (int i = 0; i < input.Length; i += maxLength)
            {
                // The Math.Min method ensures you don't go out of bounds on the last slice
                int length = Math.Min(maxLength, input.Length - i);
                slices.Add(input.Substring(i, length));
            }

            return slices;
        }

        public static List<float[]> BanglaAudioGetter(string BanglaText)
        {
            var audioDataList = new List<float[]> { };
            // Wrong here, need to specify Python.Runtime.dll path
            Runtime.PythonDLL = @"C:\Users\Malware\AppData\Local\Programs\Python\Python311\python311.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append("C://Users//Malware//PycharmProjects//VitsModelTesting");
                dynamic pyModule = Py.Import("FullModelRunner");

                List<string> slices = SliceString(BanglaText);
                int StringSliceCount = 0;
                foreach (string slice in slices)
                {
                    StringSliceCount++;
                    var banglaInput = new PyString(slice);
                    // var banglaInput = new PyString(BanglaText);
                    dynamic result =
                        pyModule.InvokeMethod("bangla_sentence_processing", new PyObject[] { banglaInput });

                    dynamic pyList = result.tolist();

                    int length = (int)pyList.Length();

                    csArray = new float[length];
                    for (int i = 0; i < length; i++)
                    {
                        csArray[i] = (float)pyList[i];
                    }

                    audioDataList.Add(csArray);
                }
                Console.WriteLine($"Total String Slices: {StringSliceCount}");
            }

            PythonEngine.Shutdown();
            return audioDataList;
        }

        public static List<float[]> JustFetchAudioArray(ref dynamic pyModule, string inputText)
        {
            var audioDataList = new List<float[]> { };
            try
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    var input = new PyString(inputText);
                    
                    dynamic result = pyModule.InvokeMethod("text_to_byte_array", new PyObject[] { input });

                    dynamic pyList = result.tolist();

                    int length = (int)pyList.Length();

                    csArray = new float[length];
                    for (int i = 0; i < length; i++)
                    {
                        csArray[i] = (float)pyList[i];
                    }

                    audioDataList.Add(csArray);
                }
                PythonEngine.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return audioDataList;

        }
        public static List<float[]> bengaliByteArrayGetter(string txt)
        {
            var audioDataList = new List<float[]> { };
            // Wrong here, need to specify Python.Runtime.dll path
            try
            {
                Runtime.PythonDLL = @"C:\Users\Malware\AppData\Local\Programs\Python\Python311\python311.dll";
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(
                        "C://Users//Malware//RiderProjects//Meta-AI-TTS-Integration//Python-TTS-Integration");
                    // dynamic pyModule = Py.Import("Main_Converter");
                    dynamic pyModule = Py.Import("onnx_model_runner");
                    
                    var banglaInput = new PyString(txt);
                    // var banglaInput = new PyString(BanglaText);
                    // dynamic result = pyModule.InvokeMethod("bangla_sentence_processing", new PyObject[] { banglaInput });
                    dynamic result = pyModule.InvokeMethod("text_to_byte_array", new PyObject[] { banglaInput });

                    dynamic pyList = result.tolist();

                    int length = (int)pyList.Length();

                    csArray = new float[length];
                    for (int i = 0; i < length; i++)
                    {
                        csArray[i] = (float)pyList[i];
                    }

                    audioDataList.Add(csArray);
                    // List<string> slices = SliceString(txt);
                    // int stringSliceCount = 0;
                    // foreach (string slice in slices)
                    // {
                    //     stringSliceCount++;
                    //     var banglaInput = new PyString(slice);
                    //     // var banglaInput = new PyString(BanglaText);
                    //     // dynamic result = pyModule.InvokeMethod("bangla_sentence_processing", new PyObject[] { banglaInput });
                    //     dynamic result = pyModule.InvokeMethod("text_to_byte_array", new PyObject[] { banglaInput });
                    //
                    //     dynamic pyList = result.tolist();
                    //
                    //     int length = (int)pyList.Length();
                    //
                    //     csArray = new float[length];
                    //     for (int i = 0; i < length; i++)
                    //     {
                    //         csArray[i] = (float)pyList[i];
                    //     }
                    //
                    //     audioDataList.Add(csArray);
                    // }

                    // Console.WriteLine($@"Total String Slices: {stringSliceCount}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return audioDataList;
        }

        public void RunONNXModel()
        {
            var context = new MLContext();

            string modelPath = "G:\\Apurba_Tech\\TTS_Model_Vits\\onnx_model\\vits_model.onnx";
            // Convert text to token IDs
            // string inputText = "দক্ষিণ এশিয়া";
            var tokenIds = GetTokenizer();

            // Define data view
            var data = new[] { new InputData { Data = tokenIds } };
            var dataView = context.Data.LoadFromEnumerable(data);

            // Define the pipeline
            var pipeline = context.Transforms.ApplyOnnxModel(
                outputColumnNames: new[] { "output" },
                inputColumnNames: new[] { "input" },
                modelFile: modelPath);

            // Create the transformer
            var transformer = pipeline.Fit(dataView);
            var transformedData = transformer.Transform(dataView);

            // Get the output
            var output = context.Data.CreateEnumerable<OutputData>(transformedData, reuseRowObject: false).First();

            Debug.WriteLine(output);
        }
    }
}