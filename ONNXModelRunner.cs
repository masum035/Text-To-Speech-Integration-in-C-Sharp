using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
// using Microsoft.ML;
// using Microsoft.ML.Data;
using Python.Runtime;

namespace BanglaTTSIntegration
{
    public class InputData
    {
        public PyObject Data { get; set; }
    }
    
    public class OutputData
    {
        [ColumnName("output")] 
        public float[][] Data { get; set; }
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
        public static float[] BanglaAudioGetter(string BanglaText)
        {
            Runtime.PythonDLL = @"C:\Users\Malware\AppData\Local\Programs\Python\Python311\python311.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append("C://Users//Malware//PycharmProjects//VitsModelTesting");
                dynamic pyModule = Py.Import("FullModelRunner");

                var banglaInput = new PyString(BanglaText);
                dynamic result = pyModule.InvokeMethod("bangla_sentence_processing",new PyObject[] {banglaInput});
                
                dynamic pyList = result.tolist();
                
                int length = (int)pyList.Length();
                
                csArray = new float[length];
                for (int i = 0; i < length; i++)
                {
                    csArray[i] = (float) pyList[i];
                }
            }
            PythonEngine.Shutdown();
            return csArray;
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