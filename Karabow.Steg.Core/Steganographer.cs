using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Karabow.Steg.Core
{
    /*
     * Based largely on the source codes I got from a long-lost friend while in 300L in school (circa 2011).
     * I have included some modifications, like support for password, and return types on the core methods.
     */
    public class Steganographer
    {
        FeedbackEventArgs fdbkArgs;

        long _fileSize;
        int _height, _width;
        long _hashedPasswordSize;
        byte[] _fileContainer;
        string _hashedPassword;//_encImagePath

        Bitmap _bitmap;

        public Steganographer()
        {
            fdbk = new FeedbackEventHandler(Steganographer_Feedback);
        }

        public byte[] Dec(string encImagePath, string password)
        {
            try 
            {
                fdbkArgs = new FeedbackEventArgs(encImagePath, 0, "Preparing image...", FeedbackType.Begin);
                fdbk(this, fdbkArgs);
                Image image = Image.FromFile(encImagePath);
                _height = image.Height;
                _width = image.Width;
                _bitmap = new Bitmap(image);
                image.Dispose();
                FileInfo imgInfo = new FileInfo(encImagePath);
                float imgSz = (float)imgInfo.Length / 1024;

                fdbkArgs = new FeedbackEventArgs(encImagePath, 10, "Preparing file...", FeedbackType.Progress);
                fdbk(this, fdbkArgs);
                _hashedPassword = StegUtility.GetHashString(password);
                //double cansave = (8.0 * ((_height * (_width / 3) * 3) / 3 - 1)) / 1024;

                return DecryptLayer(encImagePath);
            }
            catch (Exception e)
            {
                fdbkArgs = new FeedbackEventArgs(encImagePath, 0, e.Message, FeedbackType.Error);
                fdbk(this, fdbkArgs);
                fdbkArgs = new FeedbackEventArgs(encImagePath, 0, "Decryption was aborted", FeedbackType.End);
                fdbk(this, fdbkArgs);
                return null;
            }
        }
        private byte[] DecryptLayer(string encImagePath)
        {
            fdbkArgs = new FeedbackEventArgs(encImagePath, 20, "Decrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            int i, j = 0;
            bool[] t = new bool[8];
            bool[] rb = new bool[8];
            bool[] gb = new bool[8];
            bool[] bb = new bool[8];
            Color pixel = new Color();
            byte r, g, b;

            pixel = _bitmap.GetPixel(_width - 1, _height - 1);
            long fSize = pixel.R + pixel.G * 100 + pixel.B * 10000;
            pixel = _bitmap.GetPixel(_width - 2, _height - 1);
            long pwdSize = pixel.R + pixel.G * 100 + pixel.B * 10000;
            byte[] res = new byte[fSize];
            string resPwd = "";
            byte temp;

            fdbkArgs = new FeedbackEventArgs(encImagePath, 40, "Decrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);
            //Read password:
            for (i = 0; i < _height && i * (_height / 3) < pwdSize; i++)
            {
                for (j = 0; j < (_width / 3) * 3 && i * (_height / 3) + (j / 3) < pwdSize; j++)
                {
                    pixel = _bitmap.GetPixel(j, i);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    Helpers.ByteToBool(r, ref rb);
                    Helpers.ByteToBool(g, ref gb);
                    Helpers.ByteToBool(b, ref bb);
                    if (j % 3 == 0)
                    {
                        t[0] = rb[7];
                        t[1] = gb[7];
                        t[2] = bb[7];
                    }
                    else if (j % 3 == 1)
                    {
                        t[3] = rb[7];
                        t[4] = gb[7];
                        t[5] = bb[7];
                    }
                    else
                    {
                        t[6] = rb[7];
                        t[7] = gb[7];
                        temp = Helpers.BoolToByte(t);
                        resPwd += (char)temp;
                    }
                }
            }

            //check password
            if (resPwd != _hashedPassword)
                throw new Exception("Invalid password!");

            //Read file on layer 8 (after password):
            int tempj = j;
            i--;

            fdbkArgs = new FeedbackEventArgs(encImagePath, 60, "Decrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            for (; i < _height && i * (_height / 3) < fSize + pwdSize; i++)
            {
                for (j = 0; j < (_width / 3) * 3 && i * (_height / 3) + (j / 3) < (_height * (_width / 3) * 3) / 3 - 1 && i * (_height / 3) + (j / 3) < fSize + pwdSize; j++)
                {
                    if (tempj != 0)
                    {
                        j = tempj;
                        tempj = 0;
                    }
                    pixel = _bitmap.GetPixel(j, i);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    Helpers.ByteToBool(r, ref rb);
                    Helpers.ByteToBool(g, ref gb);
                    Helpers.ByteToBool(b, ref bb);
                    if (j % 3 == 0)
                    {
                        t[0] = rb[7];
                        t[1] = gb[7];
                        t[2] = bb[7];
                    }
                    else if (j % 3 == 1)
                    {
                        t[3] = rb[7];
                        t[4] = gb[7];
                        t[5] = bb[7];
                    }
                    else
                    {
                        t[6] = rb[7];
                        t[7] = gb[7];
                        temp = Helpers.BoolToByte(t);
                        res[i * (_height / 3) + j / 3 - pwdSize] = temp;
                    }
                }
            }

            //Read file on other layers:
            long readedOnL8 = (_height * (_width / 3) * 3) / 3 - pwdSize - 1;

            fdbkArgs = new FeedbackEventArgs(encImagePath, 80, "Decrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            for (int layer = 6; layer >= 0 && readedOnL8 + (6 - layer) * ((_height * (_width / 3) * 3) / 3 - 1) < fSize; layer--)
            {
                for (i = 0; i < _height && i * (_height / 3) + readedOnL8 + (6 - layer) * ((_height * (_width / 3) * 3) / 3 - 1) < fSize; i++)
                {
                    for (j = 0; j < (_width / 3) * 3 && i * (_height / 3) + (j / 3) + readedOnL8 + (6 - layer) * ((_height * (_width / 3) * 3) / 3 - 1) < fSize; j++)
                    {
                        pixel = _bitmap.GetPixel(j, i);
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        Helpers.ByteToBool(r, ref rb);
                        Helpers.ByteToBool(g, ref gb);
                        Helpers.ByteToBool(b, ref bb);
                        if (j % 3 == 0)
                        {
                            t[0] = rb[layer];
                            t[1] = gb[layer];
                            t[2] = bb[layer];
                        }
                        else if (j % 3 == 1)
                        {
                            t[3] = rb[layer];
                            t[4] = gb[layer];
                            t[5] = bb[layer];
                        }
                        else
                        {
                            t[6] = rb[layer];
                            t[7] = gb[layer];
                            temp = Helpers.BoolToByte(t);
                            res[i * (_height / 3) + j / 3 + (6 - layer) * ((_height * (_width / 3) * 3) / 3 - 1) + readedOnL8] = temp;
                        }
                    }
                }
            }

            fdbkArgs = new FeedbackEventArgs(encImagePath, 90, "Decrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            fdbkArgs = new FeedbackEventArgs(encImagePath, 0, "Decryption has been successfully completed.", FeedbackType.End);
            fdbk(this, fdbkArgs);

            _bitmap.Dispose();
            return res;
        }

        public Bitmap Enc(string sourceFilePath, string maskImagePath, string password) 
        {
            try 
            {
                fdbkArgs = new FeedbackEventArgs(sourceFilePath, 0, "Preparing image...", FeedbackType.Begin);
                fdbk(this, fdbkArgs);
                Image image = Image.FromFile(maskImagePath);
                _height = image.Height;
                _width = image.Width;
                _bitmap = new Bitmap(image);
                image.Dispose();
                FileInfo imgInfo = new FileInfo(maskImagePath);
                float imgSz = (float)imgInfo.Length / 1024;
                //fdbkArgs = new FeedbackEventArgs(10, "Image file size: " + Helpers.SmallDecimal(imgSz.ToString(), 2) + " KB", FeedbackType.Information);
                //fdbk(this, fdbkArgs);
                //_encImagePath = encImagePath;

                fdbkArgs = new FeedbackEventArgs(sourceFilePath, 10, "Preparing file...", FeedbackType.Progress);
                fdbk(this, fdbkArgs);
                //double cansave = (8.0 * ((_height * (_width / 3) * 3) / 3 - 1)) / 1024;
                FileInfo fileInfo = new FileInfo(sourceFilePath);
                _fileSize = fileInfo.Length;
                _hashedPassword = StegUtility.GetHashString(password);
                _hashedPasswordSize = _hashedPassword.Length;

                fdbkArgs = new FeedbackEventArgs(sourceFilePath, 20, "Preparing to encrypt...", FeedbackType.Progress);
                fdbk(this, fdbkArgs);
                if (8 * ((_height * (_width / 3) * 3) / 3 - 1) < _fileSize + _hashedPasswordSize)
                {
                    throw new Exception("File size is too large!\nPlease use a larger image to hide this file.");
                }
                _fileContainer = File.ReadAllBytes(sourceFilePath);
                return EncryptLayer(sourceFilePath);
            }
            catch (Exception e)
            {
                fdbkArgs = new FeedbackEventArgs(sourceFilePath, 0, e.Message, FeedbackType.Error);
                fdbk(this, fdbkArgs);
                fdbkArgs = new FeedbackEventArgs(sourceFilePath, 0, "Encryption was aborted", FeedbackType.End);
                fdbk(this, fdbkArgs);
                return null;
            }
        }
        private Bitmap EncryptLayer(string sourceFilePath) 
        {
            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 30, "Encrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            long FSize = _fileSize;
            Bitmap encBitmap =
                EncryptLayer(sourceFilePath, 8, _bitmap, 0, (_height * (_width / 3) * 3) / 3 - _hashedPasswordSize - 1, true);
            FSize -= (_height * (_width / 3) * 3) / 3 - _hashedPasswordSize - 1;
            if (FSize > 0)
            {
                for (int i = 7; i >= 0 && FSize > 0; i--)
                {
                    encBitmap =
                        EncryptLayer(sourceFilePath, i, encBitmap,
                        (((8 - i) * _height * (_width / 3) * 3) / 3 - _hashedPasswordSize - (8 - i)),
                        (((9 - i) * _height * (_width / 3) * 3) / 3 - _hashedPasswordSize - (9 - i)), false);
                    FSize -= (_height * (_width / 3) * 3) / 3 - 1;
                }
            }

            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 0, "Encryption has been successfully completed.", FeedbackType.End);
            fdbk(this, fdbkArgs);

            return encBitmap;
        }
        private Bitmap EncryptLayer(string sourceFilePath,
            int layer, Bitmap inputBitmap, long startPosition, long endPosition, bool writePassword) 
        {
            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 40, "Encrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            Bitmap outputBitmap = inputBitmap;
            layer--;
            int i = 0, j = 0;
            long PWSize = 0;
            bool[] t = new bool[8];
            bool[] rb = new bool[8];
            bool[] gb = new bool[8];
            bool[] bb = new bool[8];
            Color pixel = new Color();
            byte r, g, b;

            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 50, "Encrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            if (writePassword)
            {
                PWSize = _hashedPasswordSize;

                //write password:
                for (i = 0; i < _height && i * (_height / 3) < _hashedPasswordSize; i++)
                {
                    for (j = 0; j < (_width / 3) * 3 && i * (_height / 3) + (j / 3) < _hashedPasswordSize; j++)
                    {
                        Helpers.ByteToBool((byte)_hashedPassword[i * (_height / 3) + j / 3], ref t);
                        pixel = inputBitmap.GetPixel(j, i);
                        r = pixel.R;
                        g = pixel.G;
                        b = pixel.B;
                        Helpers.ByteToBool(r, ref rb);
                        Helpers.ByteToBool(g, ref gb);
                        Helpers.ByteToBool(b, ref bb);
                        if (j % 3 == 0)
                        {
                            rb[7] = t[0];
                            gb[7] = t[1];
                            bb[7] = t[2];
                        }
                        else if (j % 3 == 1)
                        {
                            rb[7] = t[3];
                            gb[7] = t[4];
                            bb[7] = t[5];
                        }
                        else
                        {
                            rb[7] = t[6];
                            gb[7] = t[7];
                        }
                        Color result =
                            Color.FromArgb
                            (
                                (int)Helpers.BoolToByte(rb),
                                (int)Helpers.BoolToByte(gb),
                                (int)Helpers.BoolToByte(bb));
                        outputBitmap.SetPixel(j, i, result);
                    }
                }
                i--;
            }

            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 60, "Encrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);
            //write file (after password):
            int tempj = j;
            for (; i < _height && i * (_height / 3) < endPosition - startPosition + PWSize && startPosition + i * (_height / 3) < _fileSize + PWSize; i++)
            {
                for (j = 0; j < (_width / 3) * 3 && i * (_height / 3) + (j / 3) < endPosition - startPosition + PWSize && startPosition + i * (_height / 3) + (j / 3) < _fileSize + PWSize; j++)
                {
                    if (tempj != 0)
                    {
                        j = tempj;
                        tempj = 0;
                    }
                    Helpers.ByteToBool((byte)_fileContainer[startPosition + i * (_height / 3) + j / 3 - PWSize], ref t);
                    pixel = inputBitmap.GetPixel(j, i);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    Helpers.ByteToBool(r, ref rb);
                    Helpers.ByteToBool(g, ref gb);
                    Helpers.ByteToBool(b, ref bb);
                    if (j % 3 == 0)
                    {
                        rb[layer] = t[0];
                        gb[layer] = t[1];
                        bb[layer] = t[2];
                    }
                    else if (j % 3 == 1)
                    {
                        rb[layer] = t[3];
                        gb[layer] = t[4];
                        bb[layer] = t[5];
                    }
                    else
                    {
                        rb[layer] = t[6];
                        gb[layer] = t[7];
                    }
                    Color result = Color.FromArgb((int)Helpers.BoolToByte(rb), (int)Helpers.BoolToByte(gb), (int)Helpers.BoolToByte(bb));
                    outputBitmap.SetPixel(j, i, result);
                }
            }

            long tempFS = _fileSize, tempFNS = _hashedPasswordSize;
            r = (byte)(tempFS % 100);
            tempFS /= 100;
            g = (byte)(tempFS % 100);
            tempFS /= 100;
            b = (byte)(tempFS % 100);
            Color flenColor = Color.FromArgb(r, g, b);
            outputBitmap.SetPixel(_width - 1, _height - 1, flenColor);

            r = (byte)(tempFNS % 100);
            tempFNS /= 100;
            g = (byte)(tempFNS % 100);
            tempFNS /= 100;
            b = (byte)(tempFNS % 100);
            Color fnlenColor = Color.FromArgb(r, g, b);
            outputBitmap.SetPixel(_width - 2, _height - 1, fnlenColor);

            fdbkArgs = new FeedbackEventArgs(sourceFilePath, 95, "Encrypting... Please wait", FeedbackType.Progress);
            fdbk(this, fdbkArgs);

            return outputBitmap;
        }

        event FeedbackEventHandler fdbk;
        void Steganographer_Feedback(object sender, FeedbackEventArgs e) { }
        public event FeedbackEventHandler Feedback 
        {
            add 
            {
                if (fdbk == null)
                    fdbk = new FeedbackEventHandler(Steganographer_Feedback);
                fdbk += value;
            }
            remove 
            {
                if (fdbk == null)
                    fdbk = new FeedbackEventHandler(Steganographer_Feedback);
                fdbk -= value;
            }
        }

        private class StegUtility
        {
            private static byte[] GetHash(string input)
            {
                HashAlgorithm algol = SHA1.Create(); //MD5.Create();
                return algol.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
            public static string GetHashString(string input)
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in GetHash(input))
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }
    }

    public delegate void FeedbackEventHandler(object sender, FeedbackEventArgs e);

    public class FeedbackEventArgs : EventArgs
    {
        public FeedbackEventArgs(string filePath, int workProgress, string message, FeedbackType type)
        {
            file = filePath;
            WorkProgress = workProgress;
            msg = message;
            typ = type;
        }

        string file;
        public string FilePath
        {
            get { return file; }
        }

        int wkProg = 0;
        public int WorkProgress
        {
            get { return wkProg; }
            private set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                wkProg = value;
            }
        }

        string msg;
        public string Message
        {
            get { return msg; }
        }

        FeedbackType typ;
        public FeedbackType Type
        {
            get { return typ; }
        }
    }

    public enum FeedbackType
    {
        Begin,
        End,
        Progress,
        Information,
        Question,
        Warning,
        Error
    }
}
