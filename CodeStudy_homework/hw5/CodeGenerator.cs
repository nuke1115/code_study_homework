using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw5
{
    public class CodeGenerator
    {
        private string _code = string.Empty;
        public bool ReadFromFile(string filePath)
        {
            if(Path.Exists(filePath) == false)
            {
                return false;
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                try
                {
                    _code = sr.ReadToEnd();
                }
                catch
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool WriteToFile(string outputFolderPath, string outputFileName)
        {
            if (Path.Exists(outputFolderPath) == false)
            {
                return false;
            }

            outputFolderPath += outputFileName;

            using (StreamWriter sr = new StreamWriter(outputFolderPath))
            {
                try
                {
                    sr.Write(_code);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public void ReplaceWith(string placeHolder, string targetStr)
        {
            _code = _code.Replace(placeHolder, targetStr);
        }

        public void ResetGenerator()
        {
            _code = string.Empty;
        }

        public string GetString()
        {
            return _code;
        }
    }
}
