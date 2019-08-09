using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpriteUnitor
{
    class Program
    {
        static string path = Directory.GetCurrentDirectory();
        static int delay = 1;
        static int userWidth;
        static int userHeight;
        static int rows = 1;
        static int cols = 1;

        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{
            //    string[] lines = File.ReadAllLines("settings.txt");
            //    if (lines.Length > 0)
            //    {
            //        path = lines[0];
            //    }
            //}
            //catch (FileNotFoundException e)
            //{
            //    path = Directory.GetCurrentDirectory();
            //}
            //catch (IOException e)
            //{
            //    Console.Write("Exception by reading of file");
            //    Console.ReadKey();
            //    return;
            //}

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "image files (*.png)|*.png";
                //openFileDialog.FilterIndex = 2;
                //openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string[] fileNames = openFileDialog.FileNames;
                    Bitmap[] bmps = new Bitmap[fileNames.Length];

                    Console.Write("input delay: ");
                    string command = Console.ReadLine();
                    delay = int.Parse(command);

                    Console.Write("Input size (w,h): ");
                    string command2 = Console.ReadLine();
                    string[] tokens = command2.Split(new char[] { ' ' });
                    if(tokens.Length != 2)
                    {
                        Console.Write("Incorrect data!");
                        return;
                    }

                    //Parse element 0
                    userWidth = int.Parse(tokens[0]);
                    //Parse element 1
                    userHeight = int.Parse(tokens[1]);

                    Console.Write("Input rows: ");
                    string command3 = Console.ReadLine();
                    rows = int.Parse(command3);

                    //int generalWidth = 0;
                    //int generalHeight = 0;
                    int index = 0;
                    for (int i = 0; i < bmps.Length; i++)
                    {
                        if (i % delay == 0)
                        {
                            Bitmap sourceBmp = new Bitmap(fileNames[i]);
                            Bitmap resizedBmp = new Bitmap(sourceBmp, new Size(userWidth, userHeight));
                            bmps[index] = resizedBmp;

                            //generalWidth += resizedBmp.Width;
                            //int H = resizedBmp.Height;
                            //if (H > generalHeight) generalHeight = H;
                            index++;
                        }
                    }
                    int bmpCount = index;

                    cols = bmpCount / rows;

                    int generalWidth = cols * userWidth;
                    int generalHeight = rows * userHeight;

                    Bitmap generalBmp = new Bitmap(generalWidth, generalHeight);

                    //int currentX = 0;
                    using (Graphics gr = Graphics.FromImage(generalBmp))
                    {
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                int localIndex = i * cols + j;
                                if (localIndex < bmps.Length)
                                {
                                    Bitmap bmp = bmps[localIndex];
                                    gr.DrawImage(bmp, j * userWidth, i * userHeight);
                                    bmp.Dispose();
                                }
                            }
                        }
                    }
                    
                    generalBmp.Save(path + "\\result[" + rows + "_" + cols + "].png");
                    Process.Start("explorer.exe", path);

                    generalBmp.Dispose();
                }
            }
            Console.Write("Procedure finished! Press any key...");
            Console.ReadKey();
        }
    }
}
