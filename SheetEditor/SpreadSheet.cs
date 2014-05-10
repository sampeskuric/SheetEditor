using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SheetEditor
{
    class SpreadSheet : Control
    {
        private const int DEFAULT_COLUMN_WIDTH = 50;
        private const int DEFAULT_ROW_HEIGHT = 20;

        private Color DEFAULT_CELL_BACKGROUND_COLOR = Color.White;
        private Color DEFAULT_CELL_SELECTED_COLOR = Color.Blue;
        private Color DEFAULT_FROZEN_BACKGROUND_COLOR = Color.Gray;
        private Color DEFAULT_FROZEN_SELECTED_COLOR = Color.DarkBlue;
        private Color DEFAULT_CELL_BORDER_COLOR = Color.Black;
        private Color DEFAULT_FONT_COLOR = Color.Black;

        private List<int> column_width;
        private List<int> row_height;
        private List<List<Cell>> cells;
        private int total_width
        {
            get
            {
                int q = 0;
                for (int i = 0; i < column_width.Count; i++) q += column_width[i];
                return q;
            }
        }
        private int total_height
        {
            get
            {
                int q = 0;
                for (int i = 0; i < row_height.Count; i++) q += row_height[i];
                return q;
            }
        }

        private Image buffer;
        private Graphics buffer_graphics;


        public SpreadSheet()
        {
            set_table(12,16);
            create_buffer();
            
        }

        private void set_table(int width, int height)
        {
            column_width = new List<int>();
            AddColumns(1, 30);
            AddColumns(width);
            row_height = new List<int>();
            AddRows(1, 20);
            AddRows(height);
            cells = new List<List<Cell>>();
            for (int i = 1; i <= height; i++)
            {
                for (int j = 1; j <= width; j++)
                {
                    SetCell(j, i, "(" + j + ", " + i + ")");
                }
            }
        }

        private void create_buffer()
        {
            buffer = new Bitmap(total_width, total_height);
            buffer_graphics = Graphics.FromImage(buffer);
            RedrawBuffer();
        }

        //Redraws every cell onto the buffer
        private void RedrawBuffer()
        {
            RedrawBuffer(0, 0, column_width.Count, row_height.Count);
        }
        //Redraws from cell x1,y1 to cell x2,y2 onto the buffer
        private void RedrawBuffer(int x1, int y1, int x2, int y2)
        {
            int sx = 0;
            int gx, gy;
            for (int i = 0; i < x1; i++) sx += column_width[i];
            gx = sx;
            gy = 0;
            for (int i = 0; i < y1; i++) gy += row_height[i];
            for (int y = y1; y < y2; y++)
            {
                gx = sx;
                for (int x = x1; x < x2; x++)
                {
                    RedrawBufferCell(x, y, gx, gy);
                    gx += column_width[x];
                }
                gy += row_height[y];
            }
        }
        //Redraws cell x,y onto the buffer
        private void RedrawBufferCell(int x, int y)
        {
            int gx = 0, gy = 0;
            for (int i = 0; i < x; i++) gx += column_width[i];
            for (int i = 0; i < y; i++) gy += row_height[i];
            RedrawBufferCell(x, y, gx, gy);
        }
        //Draw Cell
        private void RedrawBufferCell(int x, int y, int gx, int gy)
        {
            //Change color to appropriate background color for cell
            using (Brush brush = new SolidBrush(DEFAULT_CELL_BACKGROUND_COLOR))
            {
                buffer_graphics.FillRectangle(brush, gx, gy, column_width[x] - 1, row_height[y] - 1);
                string celltext = GetCellOutput(x,y);
                using(Brush fontbrush = new SolidBrush(DEFAULT_FONT_COLOR)){
                    buffer_graphics.DrawString(celltext, this.Font, fontbrush, gx, gy);
                }
            }
        }

        public void AddColumns(int n = 1, int width = DEFAULT_COLUMN_WIDTH)
        {
            for (int i = 0; i < n; i++) column_width.Add(width);
        }
        public void AddRows(int n = 1, int height = DEFAULT_ROW_HEIGHT)
        {
            for (int i = 0; i < n; i++) row_height.Add(height);
        }

        public void SetCell(int x, int y, string text)
        {
            while (y >= cells.Count) cells.Add(new List<Cell>());
            while (x > cells[y].Count) cells[y].Add(new Cell());
            cells[y].Add(new Cell(text));
        }
        public string GetCellInput(int x, int y)
        {
            if (x == 0)
            {
                if (y == 0) return "";
                return "" + y;
            }
            if (y == 0)
            {
                return Formula.ColumnName(x);
            }
            if (y >= cells.Count || x >= cells[y].Count) return "";
            return cells[y][x].Input;
        }
        public string GetCellOutput(int x, int y)
        {
            if (x == 0)
            {
                if (y == 0) return "";
                return "" + y;
            }
            if (y == 0)
            {
                return Formula.ColumnName(x);
            }
            if (y >= cells.Count || x >= cells[y].Count) return "";
            return cells[y][x].Output;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(buffer, 0, 0);
        }
    }
}
