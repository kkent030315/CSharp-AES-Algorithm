using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    class Cryption
    {
        //AES128
        private static readonly uint NUM_KEY = 4;
        private static readonly uint NUM_ROUND = 10;

        private static readonly byte[] RCON =
        {
            0x00000000, 
            0x00000001, 
            0x00000002, 
            0x00000004, 
            0x00000008, 
            0x00000010, 
            0x00000020, 
            0x00000040, 
            0x00000080,
            0x0000001B,
            0x00000036
        };

        private static readonly byte[] CIPHER_KEY =
        {
            0x2b, 0x28, 0xab, 0x09,
            0x7e, 0xae, 0xf7, 0xcf,
            0x15, 0xd2, 0x15, 0x4f,
            0x16, 0xa6, 0x88, 0x3C
        };

        private static byte[] CURRENT_CHIPHER_KEY = CIPHER_KEY;

        public static uint KeySize = 0;

        public Cryption(){}

        public static string MyAesEncrypt()
        {
            byte[] input =
            {
                0x32, 0x88, 0x31, 0xe0,
                0x43, 0x5a, 0x31, 0x37,
                0xf6, 0x30, 0x98, 0x07,
                0xa8, 0x8d, 0xa2, 0x34
            };

            //if(!String.IsNullOrEmpty(key))
            //{
            //    if(key.Length == 16)
            //    {
            //        CURRENT_CHIPHER_KEY = System.Text.Encoding.Default.GetBytes(key);
            //        if(CURRENT_CHIPHER_KEY.Length < 16)
            //        {
            //            Logger.WriteLine("Input key must be 16 length string.", Color.Red);
            //        }
            //        else
            //        {
            //            Logger.WriteLine("Key: " + BitConverter.ToString(CURRENT_CHIPHER_KEY).Replace("-", " "));
            //        }
            //    }
            //}

            //if (!String.IsNullOrEmpty(plainText))
            //{
            //    if (plainText.Length == 16)
            //    {
            //        input = System.Text.Encoding.Default.GetBytes(plainText);
            //        if(input.Length < 16)
            //        {
            //            Logger.WriteLine("Input text must be 16 length string.", Color.Red);
            //        }
            //        else
            //        {
            //            Logger.WriteLine("Input text bytes: " + BitConverter.ToString(input).Replace("-", " "));
            //        }
            //    }
            //}

            bool completed = false;
            byte[] tmp = input;
            byte[] output = new byte[input.Length];
            for(int round = 0; round < 10; round++)
            {
                Logger.WriteLine("======================================================", Color.Aqua);
                Logger.WriteLine("                                 ROUND: " + (round+1).ToString(), Color.Aqua);

                tmp = AddRoundKey(tmp, round);
                tmp = SubBytes(tmp);
                tmp = ShiftRows(tmp);

                if(round != 9)
                {
                    tmp = MixColumns(tmp);
                }
                else
                {
                    output = Crypt(tmp);
                    completed = true;
                }

                Logger.WriteLine("======================================================" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine, Color.Aqua);
            }

            if(completed)
            {
                byte[] i1 = { output[0], output[1], output[2], output[3] };
                byte[] i2 = { output[4], output[5], output[6], output[7] };
                byte[] i3 = { output[8], output[9], output[10], output[11] };
                byte[] i4 = { output[12], output[13], output[14], output[15] };

                Logger.WriteLine("Complete!", Color.Lime);
                Logger.WriteLine(BitConverter.ToString(i1).Replace("-", " "), Color.Lime);
                Logger.WriteLine(BitConverter.ToString(i2).Replace("-", " "), Color.Lime);
                Logger.WriteLine(BitConverter.ToString(i3).Replace("-", " "), Color.Lime);
                Logger.WriteLine(BitConverter.ToString(i4).Replace("-", " "), Color.Lime);

                Logger.WriteLine("Crypted text: " + Encoding.Default.GetString(output));
                Logger.WriteLine("Crypted bytes: " + BitConverter.ToString(output).Replace("-", " "));
            }

            byte[] ptxt = new byte[16];
            ptxt[ 0] = 0x19; ptxt[ 1] = 0xa0; ptxt[ 2] = 0x9a; ptxt[ 3] = 0xe9;
            ptxt[ 4] = 0x3d; ptxt[ 5] = 0xf4; ptxt[ 6] = 0xc6; ptxt[ 7] = 0xf8;
            ptxt[ 8] = 0xe3; ptxt[ 9] = 0xe2; ptxt[10] = 0x8d; ptxt[11] = 0x48;
            ptxt[12] = 0xbe; ptxt[13] = 0x2b; ptxt[14] = 0x2a; ptxt[15] = 0x08;

            return "";
        }

        private static void TestFunc()
        {
            byte[] b1 =
{
                0x04, 0xe0, 0x48, 0x28,
                0x66, 0xcb, 0xf8, 0x06,
                0x81, 0x19, 0xd3, 0x26,
                0xe5, 0x9a, 0x7a, 0x4c
            };

            byte[] b2 =
            {
                0xa0, 0x88, 0x23, 0x2a,
                0xfa, 0x54, 0xa3, 0x6c,
                0xfe, 0x2c, 0x39, 0x76,
                0x17, 0xb1, 0x39, 0x05
            };

            byte[] rere = new byte[16];

            for (int x = 0; x < b1.Length; x++)
            {
                rere[x] = (byte)(b1[x] ^ b2[x]);
            }

            byte[] _1 = { rere[0], rere[1], rere[2], rere[3] };
            byte[] _2 = { rere[4], rere[5], rere[6], rere[7] };
            byte[] _3 = { rere[8], rere[9], rere[10], rere[11] };
            byte[] _4 = { rere[12], rere[13], rere[14], rere[15] };
            Logger.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Logger.WriteLine(BitConverter.ToString(_1).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_2).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_3).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_4).Replace("-", " "));
            Logger.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        }

        private static byte[] SubBytes(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            for(int x = 0; x < bytes.Length; x++)
            {
                result[x] = SBOX.Convert(bytes[x]);
            }

            byte[] _1 = { result[0], result[1], result[2], result[3] };
            byte[] _2 = { result[4], result[5], result[6], result[7] };
            byte[] _3 = { result[8], result[9], result[10], result[11] };
            byte[] _4 = { result[12], result[13], result[14], result[15] };
            Logger.WriteLine("====SubBytes====");
            Logger.WriteLine(BitConverter.ToString(_1).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_2).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_3).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_4).Replace("-", " "));
            Logger.WriteLine("================");
            return result;
        }

        private static byte[] ShiftRows(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];

            result[ 0] = bytes[ 0]; result[ 1] = bytes[ 1]; result[ 2] = bytes[ 2]; result[ 3] = bytes[ 3];

            result[ 4] = bytes[ 5]; result[ 5] = bytes[ 6]; result[ 6] = bytes[ 7]; result[ 7] = bytes[ 4];

            result[ 8] = bytes[10]; result[ 9] = bytes[11]; result[10] = bytes[ 8]; result[11] = bytes[ 9];

            result[12] = bytes[15]; result[13] = bytes[12]; result[14] = bytes[13]; result[15] = bytes[14];

            byte[] _1 = { result[0], result[1], result[2], result[3] };
            byte[] _2 = { result[4], result[5], result[6], result[7] };
            byte[] _3 = { result[8], result[9], result[10], result[11] };
            byte[] _4 = { result[12], result[13], result[14], result[15] };
            Logger.WriteLine("====ShiftRows====");
            Logger.WriteLine(BitConverter.ToString(_1).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_2).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_3).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(_4).Replace("-", " "));
            Logger.WriteLine("=================");

            return result;
        }

        private static byte[] MixColumns(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            byte[,] bytes2d = new byte[4, 4];

            bytes2d = Bytes16To2DBytes4(bytes);

            byte[] matrix = 
            { 
                0x02, 0x03, 0x01, 0x01,
                0x01, 0x02, 0x03, 0x01,
                0x01, 0x01, 0x02, 0x03,
                0x03, 0x01, 0x01, 0x02
            };

            byte[,] matrix2d = new byte[4, 4];
            matrix2d = Bytes16To2DBytes4(matrix);

            byte[,] resultBytes2d = new byte[4, 4];
            for (int c = 0; c <= 3; c++)
            {
                resultBytes2d[0, c] = (byte)((GMul(0x02, bytes2d[0, c])) ^ (GMul(0x03, bytes2d[1, c])) ^ (GMul(0x01, bytes2d[2, c])) ^ (GMul(0x01, bytes2d[3, c])));
                resultBytes2d[1, c] = (byte)((GMul(0x01, bytes2d[0, c])) ^ (GMul(0x02, bytes2d[1, c])) ^ (GMul(0x03, bytes2d[2, c])) ^ (GMul(0x01, bytes2d[3, c])));
                resultBytes2d[2, c] = (byte)((GMul(0x01, bytes2d[0, c])) ^ (GMul(0x01, bytes2d[1, c])) ^ (GMul(0x02, bytes2d[2, c])) ^ (GMul(0x03, bytes2d[3, c])));
                resultBytes2d[3, c] = (byte)((GMul(0x03, bytes2d[0, c])) ^ (GMul(0x01, bytes2d[1, c])) ^ (GMul(0x01, bytes2d[2, c])) ^ (GMul(0x02, bytes2d[3, c])));
            }

            result = Bytes2D4ToBytes16(resultBytes2d);

            byte[] i1 = { result[ 0], result[ 1], result[ 2], result[ 3] };
            byte[] i2 = { result[ 4], result[ 5], result[ 6], result[ 7] };
            byte[] i3 = { result[ 8], result[ 9], result[10], result[11] };
            byte[] i4 = { result[12], result[13], result[14], result[15] };

            Logger.WriteLine("====MixColumns====");
            Logger.WriteLine(BitConverter.ToString(i1).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(i2).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(i3).Replace("-", " "));
            Logger.WriteLine(BitConverter.ToString(i4).Replace("-", " "));
            Logger.WriteLine("==================");

            return result;
        }

        public static byte GMul(byte a, byte b)
        {
            // Galois Field (256) Multiplication of two Bytes
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= unchecked((byte)0x11B); /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }

        private static byte[,] Bytes16To2DBytes4(byte[] input)
        {
            byte[,] result2d = new byte[4, 4];
            int mtx = 0;
            for (uint x = 0; x < 4; x++)
            {
                for (uint y = 0; y < 4; y++)
                {
                    mtx++;
                    result2d[x, y] = input[mtx - 1];

                }
            }
            return result2d;
        }

        private static byte[] Bytes2D4ToBytes16(byte[,] input)
        {
            byte[] result = new byte[16];
            int mtx = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    mtx++;
                    result[mtx - 1] = input[x, y];
                }
            }
            return result;
        }

        private static byte[] AddRoundKey(byte[] bytes, int currentRound)
        {
            //Logger.WriteLine("====AddRoundKey====", Color.Green);
            byte[] result = new byte[bytes.Length];
            if (currentRound == 0)
            {
                for (int x = 0; x < bytes.Length; x++)
                {
                    result[x] = (byte)(bytes[x] ^ CIPHER_KEY[x]);
                }

                byte[] i1 = { result[0], result[1], result[2], result[3] };
                byte[] i2 = { result[4], result[5], result[6], result[7] };
                byte[] i3 = { result[8], result[9], result[10], result[11] };
                byte[] i4 = { result[12], result[13], result[14], result[15] };

                Logger.WriteLine("====RoundOne====", Color.Yellow);
                Logger.WriteLine(BitConverter.ToString(i1).Replace("-", " "), Color.Yellow);
                Logger.WriteLine(BitConverter.ToString(i2).Replace("-", " "), Color.Yellow);
                Logger.WriteLine(BitConverter.ToString(i3).Replace("-", " "), Color.Yellow);
                Logger.WriteLine(BitConverter.ToString(i4).Replace("-", " "), Color.Yellow);
                Logger.WriteLine("================", Color.Yellow);

                KeySchedule(result, 1);

                return result;
            }
            else
            {
                result = Crypt(bytes);
                KeySchedule(bytes, currentRound+1);
            }
            //Logger.WriteLine("===================", Color.Green);
            return result;
        }

        private static byte[] Crypt(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            byte[] i1 = { CURRENT_CHIPHER_KEY[0], CURRENT_CHIPHER_KEY[1], CURRENT_CHIPHER_KEY[2], CURRENT_CHIPHER_KEY[3] };
            byte[] i2 = { CURRENT_CHIPHER_KEY[4], CURRENT_CHIPHER_KEY[5], CURRENT_CHIPHER_KEY[6], CURRENT_CHIPHER_KEY[7] };
            byte[] i3 = { CURRENT_CHIPHER_KEY[8], CURRENT_CHIPHER_KEY[9], CURRENT_CHIPHER_KEY[10], CURRENT_CHIPHER_KEY[11] };
            byte[] i4 = { CURRENT_CHIPHER_KEY[12], CURRENT_CHIPHER_KEY[13], CURRENT_CHIPHER_KEY[14], CURRENT_CHIPHER_KEY[15] };
            Logger.WriteLine("Cipher key", Color.Purple);
            Logger.WriteLine(BitConverter.ToString(i1).Replace("-", " "), Color.Purple);
            Logger.WriteLine(BitConverter.ToString(i2).Replace("-", " "), Color.Purple);
            Logger.WriteLine(BitConverter.ToString(i3).Replace("-", " "), Color.Purple);
            Logger.WriteLine(BitConverter.ToString(i4).Replace("-", " "), Color.Purple);
            for (int x = 0; x < bytes.Length; x++)
            {
                result[x] = (byte)(bytes[x] ^ CURRENT_CHIPHER_KEY[x]);
            }
            byte[] i11 = { result[0], result[1], result[2], result[3] };
            byte[] i22 = { result[4], result[5], result[6], result[7] };
            byte[] i33 = { result[8], result[9], result[10], result[11] };
            byte[] i44 = { result[12], result[13], result[14], result[15] };
            Logger.WriteLine("====Crypted====", Color.Orange);
            Logger.WriteLine(BitConverter.ToString(i11).Replace("-", " "), Color.Orange);
            Logger.WriteLine(BitConverter.ToString(i22).Replace("-", " "), Color.Orange);
            Logger.WriteLine(BitConverter.ToString(i33).Replace("-", " "), Color.Orange);
            Logger.WriteLine(BitConverter.ToString(i44).Replace("-", " "), Color.Orange);
            Logger.WriteLine("===============", Color.Orange);
            return result;
        }

        private static void KeySchedule(byte[] bytes, int currentRound)
        {
            byte[,] bytes2d = Bytes16To2DBytes4(bytes);
            byte[,] round_key = new byte[4, 4];
            byte[] r = new byte[16];
            byte[] r_ = new byte[16];

            //Rot Word
            //一旦値を格納
            r[3] = CURRENT_CHIPHER_KEY[3]; //09
            r[7] = CURRENT_CHIPHER_KEY[7]; //cf
            r[11] = CURRENT_CHIPHER_KEY[11]; //4f
            r[15] = CURRENT_CHIPHER_KEY[15]; //3c
            //Logger.WriteLine("一番左の行(S-BOX変換前): " + BitConverter.ToString(r), Color.CadetBlue);
            //↓ 一番下を一番上に持ってくる
            r_[3] = CURRENT_CHIPHER_KEY[7]; //cf
            r_[7] = CURRENT_CHIPHER_KEY[11]; //4f
            r_[11] = CURRENT_CHIPHER_KEY[15]; //3c
            r_[15] = CURRENT_CHIPHER_KEY[3]; //09
            //↓ s-box変換
            r_[3] = SBOX.Convert(r_[3]); //8a
            r_[7] = SBOX.Convert(r_[7]); //84
            r_[11] = SBOX.Convert(r_[11]); //eb
            r_[15] = SBOX.Convert(r_[15]); //01

            //Logger.WriteLine("一番左の行(S-BOX変換後): " + BitConverter.ToString(r_), Color.Orange);

            r[0] = CURRENT_CHIPHER_KEY[0];
            r[4] = CURRENT_CHIPHER_KEY[4];
            r[8] = CURRENT_CHIPHER_KEY[8];
            r[12] = CURRENT_CHIPHER_KEY[12];

            round_key[0, 0] = (byte)(CURRENT_CHIPHER_KEY[0] ^ r_[3] ^ RCON[currentRound]);
            round_key[1, 0] = (byte)(CURRENT_CHIPHER_KEY[4] ^ r_[7] ^ RCON[0]);
            round_key[2, 0] = (byte)(CURRENT_CHIPHER_KEY[8] ^ r_[11] ^ RCON[0]);
            round_key[3, 0] = (byte)(CURRENT_CHIPHER_KEY[12] ^ r_[15] ^ RCON[0]);

            byte[] aaa = new byte[4];
            aaa[0] = (byte)int.Parse(round_key[0, 0].ToString());
            aaa[1] = (byte)int.Parse(round_key[1, 0].ToString());
            aaa[2] = (byte)int.Parse(round_key[2, 0].ToString());
            aaa[3] = (byte)int.Parse(round_key[3, 0].ToString());
            //Logger.WriteLine(BitConverter.ToString(aaa), Color.CadetBlue);

            for (int x = 0; x <= 3; x++) //0,1,2 (3)
            {
                //0は除きたい（横インデックス1から埋め込んでいく）ので x は 1 2 3 のみに絞る
                if (x == 0) continue;
                round_key[0, x] = (byte)(CURRENT_CHIPHER_KEY[0 + (1 * x)] ^ round_key[0, x - 1]);
                round_key[1, x] = (byte)(CURRENT_CHIPHER_KEY[4 + (1 * x)] ^ round_key[1, x - 1]);
                round_key[2, x] = (byte)(CURRENT_CHIPHER_KEY[8 + (1 * x)] ^ round_key[2, x - 1]);
                round_key[3, x] = (byte)(CURRENT_CHIPHER_KEY[12 + (1 * x)] ^ round_key[3, x - 1]);
            }

            CURRENT_CHIPHER_KEY = Bytes2D4ToBytes16(round_key);

            //Logger.WriteLine("ROUND_KEY: " + BitConverter.ToString(Bytes2D4ToBytes16(round_key)), Color.Yellow);
            byte[] _1 = { CURRENT_CHIPHER_KEY[0], CURRENT_CHIPHER_KEY[1], CURRENT_CHIPHER_KEY[2], CURRENT_CHIPHER_KEY[3] };
            byte[] _2 = { CURRENT_CHIPHER_KEY[4], CURRENT_CHIPHER_KEY[5], CURRENT_CHIPHER_KEY[6], CURRENT_CHIPHER_KEY[7] };
            byte[] _3 = { CURRENT_CHIPHER_KEY[8], CURRENT_CHIPHER_KEY[9], CURRENT_CHIPHER_KEY[10], CURRENT_CHIPHER_KEY[11] };
            byte[] _4 = { CURRENT_CHIPHER_KEY[12], CURRENT_CHIPHER_KEY[13], CURRENT_CHIPHER_KEY[14], CURRENT_CHIPHER_KEY[15] };
            //Logger.WriteLine("ROUND_KEY: " + BitConverter.ToString(CURRENT_CHIPHER_KEY), Color.DeepSkyBlue);

            Logger.WriteLine("----ROUND KEY----", Color.Red);
            Logger.WriteLine(BitConverter.ToString(_1).Replace("-", " "), Color.Red);
            Logger.WriteLine(BitConverter.ToString(_2).Replace("-", " "), Color.Red);
            Logger.WriteLine(BitConverter.ToString(_3).Replace("-", " "), Color.Red);
            Logger.WriteLine(BitConverter.ToString(_4).Replace("-", " "), Color.Red);
            Logger.WriteLine("-----------------", Color.Red);
        }

        public static string Random(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
