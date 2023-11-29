using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Python.Runtime;

namespace TTSIntegration
{
    public class ModelRunner
    {
        private static float[] _csArray;
        public ModelRunner(ref dynamic pyModule)
        {
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
                    pyModule = Py.Import("onnx_model_runner");
                }
                PythonEngine.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task<List<float[]>> JustFetchAudioArray(DynamicWrapper pyModuleWrapper, string inputText)
        {
            return await Task.Run(() =>
            {
                List<float[]> audioDataList = new List<float[]>();
                try
                {
                    PythonEngine.Initialize();
                    using (Py.GIL())
                    {
                        var input = new PyString(inputText);

                        dynamic result =
                            pyModuleWrapper.Value.InvokeMethod("text_to_byte_array", new PyObject[] { input });

                        dynamic pyList = result.tolist();

                        int length = (int)pyList.Length();

                        _csArray = new float[length];
                        for (int i = 0; i < length; i++)
                        {
                            _csArray[i] = (float)pyList[i];
                        }

                        audioDataList.Add(_csArray);
                    }

                    PythonEngine.Shutdown();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return audioDataList;
            });
        }
    }
}