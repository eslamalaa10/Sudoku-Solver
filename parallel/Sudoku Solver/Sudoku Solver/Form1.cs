using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[,] data;

        //------------------------show the grid on screen-------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            int size = 9;
            string filemame;
            filemame = files_list.Text;
            data = load_file("../../../" + filemame+".txt");
            for(int i = 0; i < size; i++)
            {
                for (int j=0;j<size;j++) {
                    flowLayoutPanel1.Controls.Add(create_grid(data[i,j]));
                }
            }
        }

        //---------------------------creat one cell and put the value in it---------------------------
        Label create_grid(int i) {
            Label l = new Label();
            l.Name = i.ToString();
            l.Width = 50;
            l.Height = 50;
            l.ForeColor = Color.Blue;
            if(i==0)
                l.Text = "";
            else
                l.Text = i.ToString();
            l.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            l.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            return l;
        }

        //------------------------load the txt file in 2d array----------------------------------------
        int[,] load_file(String path)
        {
            String input = File.ReadAllText(path);

            int i = 0, j = 0;
            int[,] result = new int[9, 9];
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            return result;
        }

        //-----------------------------find the position of empty cell----------------------------------
        int[] find_empty() {
            int[] position=new int[2];
            int flag = 1;
           for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9 ; j++)
                    if (data[i, j] == 0)
                    {
                        position[0] = i;
                        position[1] = j;
                        flag = 0;
                        break;   
                    }
                if (flag == 0)
                    break;
            }
            if (flag == 1)
            {
                position[0] =-1;
                position[1] = -1;
                return position;
            }
            else
                return position;
        }

        //------------------------------------check if the number is valid-------------------------------
        Boolean valid_row(int value, int[] position)
        {
            Boolean flag = true;
            //check the row
            for (int i = 0; i < 9; i++)
            {
                if (data[position[0], i] == value & position[1] != i)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        Boolean valid_column(int value, int[] position)
        {
            Boolean flag = true;
            //check the column
            for (int i = 0; i < 9; i++)
            {
                if (flag == false)
                    break;

                else if (data[i, position[1]] == value & position[0] != i)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        Boolean valid_box(int value, int[] position)
        {
            Boolean flag = true;
            //check box
            int x_box = position[1] / 3;
            int y_box = position[0] / 3;
            for (int i = (y_box * 3); i < ((y_box * 3) + 3); i++)
            {
                if (flag == false)
                    break;
                else
                    for (int j = (x_box * 3); j < ((x_box * 3) + 3); j++)
                    {

                        if (flag == false)
                        {
                            break;
                        }
                        else if (data[i, j] == value & position[0] != i & position[1] != j)
                        {
                            flag = false;
                            break;
                        }
                    }
            }
            return flag;
        }
        Boolean valid(int value,int[] position ) {
            Boolean row = new Boolean();
            Boolean col = new Boolean();
            Boolean box = new Boolean();
            Parallel.Invoke(() => {
                row = valid_row(value, position);
                col = valid_column(value, position);
                box = valid_box(value, position);
            });
            if (row & col & box)
                return true;
            else
                return false;
            
            
        }

        //----------------------------------solve the puzzel recursively------------------------------------
        bool solve() {
            int[] find = find_empty();
            if(find[0] == -1 & find[1] == -1) //if there is no empty cell so it solved
            {
                return true;
            }

            for(int i = 1; i < 10; i++)
            {
                if (valid( i, find))
                {
                    data[find[0], find[1]] = i;
                    if (solve())
                        return true;
                    data[find[0], find[1]] = 0;
                }
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch time=Stopwatch.StartNew();
            Boolean flag = solve();
            time.Stop();
            if (!flag)
                System.Windows.Forms.MessageBox.Show("cant solve");
            else
            {
                flowLayoutPanel1.Controls.Clear();
                int size = 9;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        flowLayoutPanel1.Controls.Add(create_grid(data[i, j]));
                    }
                }
                label1.Text = "Time = "+time.Elapsed.TotalSeconds.ToString();
                System.Windows.Forms.MessageBox.Show("solved :)");
            }

        }

       
    }
}
