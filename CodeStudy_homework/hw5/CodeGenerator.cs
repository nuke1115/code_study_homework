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

        public string GetString()
        {
            return _code;
        }
    }
}
