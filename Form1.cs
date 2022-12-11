using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Path = System.IO.Path;
using System.Reflection;
using Humanizer;
using System.Globalization;
using System.Runtime.InteropServices;

namespace TRAINER___2023___ZELDA
{
    public partial class Form1 : Form
    {
        private readonly ProcessWrapper _processWrapper;
        public Form1()
        {
            InitializeComponent();
            _processWrapper = new ProcessWrapper("Cemu");
        }

        public string GetCemuPath()
        {
            return _processWrapper.GetProcessLocation();
        }

        public bool DisableWrite { get; set; }

        public void SetBaseAddress(long baseAddress)
        {
            _processWrapper.SetBaseAddress(Program.Globals.baseAdress);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (!_processWrapper.OpenProcess())
            {
                throw new Exception("Failed to open cemu process. Is it running ?");
            }
            textBox1.Text = GetCemuPath();

            string fileName = "log.txt";
            string fileName2 = "log2.txt";
            string sourcePath = textBox1.Text.Substring(0, textBox1.Text.Length - 9);
            string targetPath = textBox1.Text.Substring(0, textBox1.Text.Length - 9);

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName2);

            // To copy a folder's contents to a new location:
            // Create a new target folder.
            // If the directory already exists, this method does not create a new directory.
            System.IO.Directory.CreateDirectory(targetPath);

            // To copy a file to another location and
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

            string textFile = targetPath + "\\log2.txt";
            string text = File.ReadAllText(textFile);
            //string line = File.ReadLines(text).Skip(2).Take(1).First();
            var result = text.Substring(text.LastIndexOf("(base: ") + 7);
            string base1 = result.Substring(0, 13);
            textBox2.Text = base1;

            // long i = (long)Convert.ToDouble(base1);
            long i = Convert.ToInt64(base1, 16);

            Program.Globals.baseAdress = i;
            button2.Visible = true;
            button2.PerformClick();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = true;


            //Update Player data: Health, Speed, Stamina and Rupees
            textBox3.Text = (ReadOffset(GameOffsets.HEALTH)).ToString();
            textBox4.Text = (ReadOffset(GameOffsets.STAMINA)).ToString();

            label6.Text = (ReadOffset(GameOffsets.Speed)).ToString();


            ReadOffset(GameOffsets.RUPEES);
            textBox5.Text = (ReadOffset(GameOffsets.RUPEES)).ToString();
            if (textBox5.Text.Contains("-") == true)
            {
                textBox5.Text = (ReadOffset(GameOffsets.RUPEES)).ToString("x");
                var text = textBox5.Text;
                var word = "ff";
                int first = text.IndexOf(word);
                int last = text.LastIndexOf(word);
                textBox5.Text = textBox5.Text.Remove(0, last + 2);
            }

        }

        public int GetRupee()
        {
            return ReadOffset(GameOffsets.RUPEES);
        }

        public bool UpdateRupees(int quantity)
        {
            return WriteOffset(GameOffsets.RUPEES, quantity);
        }

        public int GetStamina()
        {
            return ReadOffset(GameOffsets.STAMINA);
        }

        public bool UpdateStamina(int quantity)
        {
            return WriteOffset(GameOffsets.STAMINA, quantity);
        }

        public int GetHealth()
        {
            return ReadOffset(GameOffsets.HEALTH);
        }

        public bool UpdateHealth(int quantity)
        {
            return WriteOffset(GameOffsets.HEALTH, quantity);
        }

        public int GetSpeed()
        {
            return ReadOffset(GameOffsets.Speed);
        }

        public bool UpdateSpeed(int value)
        {
            return WriteOffset(GameOffsets.Speed, value);
        }

        //Coordinates
        //Position
        public float GetXPosition()
        {
            return ReadOffset(GameOffsets.Xposition);
        }
        public bool UpdateXPosition(int value)
        {
            return WriteOffset(GameOffsets.Xposition, value);
        }
        public int GetYPosition()
        {
            return ReadOffset(GameOffsets.Yposition);
        }
        public bool UpdateYPosition(int value)
        {
            return WriteOffset(GameOffsets.Yposition, value);
        }
        public int GetZPosition()
        {
            return ReadOffset(GameOffsets.Zposition);
        }
        public bool UpdateZPosition(int value)
        {
            return WriteOffset(GameOffsets.Zposition, value);
        }



        private static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        private bool WriteOffset(Offset offset, int value)
        {
            if (DisableWrite) return false;

            byte[] data;
            if (offset.Size == 1)
            {
                if (value > 255) return false;

                data = new byte[] { (byte)value };
            }
            else if (offset.Size == 2)
            {
                if (value > 65535) return false;
                data = BitConverter.GetBytes((short)value);
            }
            else
            {
                throw new Exception("32 Bits and 64 Bits types are currently not handled");
            }

            return _processWrapper.OverwriteMemoryProtection(offset.Address, offset.Size) && _processWrapper.WriteMemory(offset, data);
        }

        private bool WriteOffset(Offset offset, bool[] bitsValues)
        {
            if (DisableWrite) return false;
            if (offset.Size != 1) throw new Exception("Unsupported for now.");

            BitArray bits = new BitArray(8);
            for (int i = 0; i < bitsValues.Length; i++) bits[i] = bitsValues[i];

            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);

            return WriteOffset(offset, bytes[0]);
        }

        private bool WriteSingleBit(Offset offset, int bit, bool value)
        {
            if (DisableWrite) return false;
            if (offset.Size != 1) throw new Exception("Unsupported for now.");
            if (bit > 7) throw new Exception("Bit index are between 0 and 7");

            int currentValue = ReadOffset(offset);
            BitArray bits = new BitArray(new byte[] { (byte)currentValue });
            bits[bit] = value;

            byte[] newByte = new byte[1];
            bits.CopyTo(newByte, 0);

            return WriteOffset(offset, newByte[0]);
        }

        private int ReadOffset(Offset offset)
        {
            byte[] data = _processWrapper.ReadMemory(offset);

            if (data.Length != offset.Size) return -1;
            else if (data.Length == 1) return data[0];
            else if (data.Length == 2) return BitConverter.ToInt16(data, 0);

            throw new Exception("32 Bits and 64 Bits types are currently not handled");
        }


        private void button3_Click(object sender, EventArgs e)
        {
            UpdateRupees(Convert.ToInt32(textBox5.Text));
        }
               

        private void timer1_Tick(object sender, EventArgs e)
        {            

            if (checkBox1.Checked==true && textBox3.Text != "")
            {
                UpdateHealth(Convert.ToInt32(textBox3.Text));
            }
            if (checkBox2.Checked == true && textBox4.Text != "")
            {
                UpdateStamina(Convert.ToInt32(textBox4.Text));
            }

            if (checkBox4.Checked == true)
            {
                textBox6.Text = GetXPosition().ToString();
                textBox7.Text = GetYPosition().ToString();
                textBox8.Text = GetZPosition().ToString();
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int runningSpeed = 16256 + (16 * Convert.ToInt16(trackBar1.Value.ToString()));
            textBox9.Text = trackBar1.Value.ToString();
            
            label6.Text = runningSpeed.ToString();
            UpdateSpeed(Convert.ToInt32(label6.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {            
            UpdateXPosition(Convert.ToInt32(textBox6.Text));
            UpdateXPosition(Convert.ToInt32(textBox7.Text));
            UpdateXPosition(Convert.ToInt32(textBox8.Text));
        }
    }
}
