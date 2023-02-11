using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace LAB5_6
{
    public partial class Form1 : Form
    {

        private string path = @"F:\Visual Studio\OP\LAB5-6\Files\";
        public string name = "My_File";
        private bool rewrite = true;
        private string key = "<RSAKeyValue><Modulus>vz9MkMIjrKUm+rJi7j7iv5RvLljf+svVXvlcCtYDCjtJ2rMesjaWh8lUx0wcXcjSau7cVr2B4qdoLjPJCSQctrvbxr2B13bk8MLY80eP2PFjO5baQyYqrCYVUcUPWc1/GpE4IHz+Vvp8TAPQN0VVu2Rda/wlOBx0p9gHcTNcvvE=</Modulus><Exponent>AQAB</Exponent><P>x08GVT9OkiNiEn9uWrFwXtoJlCcDOU+0mxUi4c1QECuDNtIhE4FWS4MckxKCJfE8FpTwsAEkTGrz8jJmOZGQkw==</P><Q>9aVEzuzEoQ5So+ojMcCmy/oHcd//3bSvkkOixiATcDi7pJZR3DI92GcWvMiviXGf9ruWu8KQ/oNchoAjmdvY6w==</Q><DP>shJ+/AdV8ee/JfvI4ysJ1RVF3aKFlN0L5kuaINjJocjesUpI1x6FtW3tR9IPikrgfuSPrxE2FzivwEMbZnASFQ==</DP><DQ>D55lhJ6rVe46NO/jMvuj315TcNskQq2FaFBiniRV+BGzZKCmLdEH+6V6XaCJAL03xZFh4Sha3cPucyLcoKDI5w==</DQ><InverseQ>Wuj88z643ga3eKQGqcTjC5fyx0FwiwNOOjENhpN4QOZ/NtxUa2wdAow20Ycm4m0y3q4O2CNG1AK4sdIKSJDx/A==</InverseQ><D>vnfWU11mli0tMwSyjsDGpzK3wBr3hxm2eY4zYv9dq7T8ivlVmvvO05FqA8sDfUvidGlLvH+keg1sLoeYsqfqh0BPXsOJrZwXR7jCvTzUGAce0q/GFMiBwbAyaAEjr99+3k6mdRyA1qdIfZ3MazG530EdMA2zb8wo1Z+2OXFMNXk=</D></RSAKeyValue>";

        public Form1()
        {
            InitializeComponent();
            nameFileLable.Text = name;
            pathTextBox.Text = path + NameFile(name);
        }

        private string NameFile(string name)
        {
            name = name + ".txt";
            return name;
        }

        private void WriteTextFile(string path, bool b)
        {
            using (StreamWriter sw = new StreamWriter(path, b))
            {
                sw.WriteLine(textBox1.Text);
            }
        }

        private void ReadFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                textBox1.Text = sr.ReadToEnd();
            }
        }


        private void CompressFile(string path)
        {
            using (FileStream originalFileStream = File.Open(path, FileMode.Open))
            {
                using (var compressedFileStream = originalFileStream)
                {
                    using (var compressor = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressor);
                        originalFileStream.SetLength(0);
                    }
                }

            }

        }

        private void DecompressFile(string path)
        {
            using (FileStream compressedFileStream = File.Open(path, FileMode.Open))
            {
                using (var decompressor = new DeflateStream(compressedFileStream, CompressionMode.Decompress))
                {
                    using (var memory = new MemoryStream())
                    {
                        decompressor.CopyTo(memory);
                        compressedFileStream.SetLength(0);
                        memory.Position = 0;
                        memory.CopyTo(compressedFileStream);
                    }
                }
            }
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            rewrite = false;
            yesButton.BackColor = Color.IndianRed;
            noButton.BackColor = Color.Transparent;
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            rewrite = true;
            noButton.BackColor = Color.IndianRed;
            yesButton.BackColor = Color.Transparent;
        }

        private void PathFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = ofd.FileName;
            }
            ofd.Dispose();
        }

        void refresh()
        {
            nameFileLable.Text = name;
            pathTextBox.Text = path + NameFile(name);

        }
                                                
        private string Encrypt(string text)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(text), true));
            }
        }

        private string Decrypt(string text)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(text), true));
            }

        }
       

        private void writeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                WriteTextFile(pathTextBox.Text, rewrite);
                textBox1.Text = "";

            }
            else
            {
                MessageBox.Show("Error.The field is empty!");
            }
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ReadFile(pathTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Error!");

            }
        }

        private void reviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PathFile();
                ReadFile(pathTextBox.Text);

            }
            catch
            {
                MessageBox.Show("Error!");
            }
        }

        private void fileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Owner = this;
            f.ShowDialog();
            refresh();
        }

        private void comperssionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CompressFile(pathTextBox.Text);               
                long compressedSize = new FileInfo(path + NameFile(name)).Length;
                MessageBox.Show($"The compressed  file '{path + NameFile(name)}' weighs {compressedSize} bytes.");
            }
            catch
            {
                MessageBox.Show("Error.File not found!");
            }
        }

        private void decompressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DecompressFile(pathTextBox.Text);
                long decompressedSize = new FileInfo(path + NameFile(name)).Length;
                MessageBox.Show($"The decompressed file '{path + NameFile(name)}' weighs {decompressedSize} bytes.");
            }catch
            {
                MessageBox.Show("Error.File not found!");
            }

        }

        

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string a;
                using (StreamReader sr = new StreamReader(pathTextBox.Text))
                {
                    a = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(pathTextBox.Text, false))
                {
                    sw.WriteLine(Encrypt(a));
                }
                MessageBox.Show("File Encrypted");
            }
            catch
            {
                MessageBox.Show("Error.File not found!");
            }

        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string a;
                using (StreamReader sr = new StreamReader(pathTextBox.Text))
                {
                    a = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(pathTextBox.Text, false))
                {
                    sw.WriteLine(Decrypt(a));
                }
                MessageBox.Show("File decrypted");
            }
            catch
            {
                MessageBox.Show("Error.File not found!");
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            pathTextBox.Text = path + NameFile(name);
        }
    }
}