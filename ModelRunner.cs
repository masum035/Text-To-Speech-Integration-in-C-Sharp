using Microsoft.ML.Data;
using Python.Runtime;

namespace TTSIntegration
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
    
    public class ModelRunner
    {
        
    }
}