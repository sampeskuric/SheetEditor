using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SheetEditor
{
    class Cell
    {
        private bool highlighted;

        private string input;
        private string output;

        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                Set(value);
            }
        }
        public string Output
        {
            get
            {
                return output;
            }
        }

        public Cell()
        {
            highlighted = false;
            input = "";
            output = "";
        }

        public Cell(string text)
        {
            highlighted = false;
            input = text;
            output = Formula.Calculate(text);
        }

        public void Set(string text)
        {
            input = text;
            string new_output = Formula.Calculate(text);
            if (new_output != output)
            {
                //Set to be redrawn
            }
            output = new_output;
        }
    }
}
