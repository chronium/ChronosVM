using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronosVM_2
{
    public class Registers
    {
        public byte AL;
        public byte AH;

        public short A
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { AL, AH }, 0);
            }
            set
            {
                AL = (byte)(value & 0x00FF);
                AH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte BL;
        public byte BH;

        public short B
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { BL, BH }, 0);
            }
            set
            {
                BL = (byte)(value & 0x00FF);
                BH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte CL;
        public byte CH;

        public short C
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { CL, CH }, 0);
            }
            set
            {
                CL = (byte)(value & 0x00FF);
                CH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte DL;
        public byte DH;

        public short D
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { DL, DH }, 0);
            }
            set
            {
                DL = (byte)(value & 0x00FF);
                DH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte EL;
        public byte EH;

        public short E
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { EL, EH }, 0);
            }
            set
            {
                EL = (byte)(value & 0x00FF);
                EH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte FL;
        public byte FH;

        public short F
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { FL, FH }, 0);
            }
            set
            {
                FL = (byte)(value & 0x00FF);
                FH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public byte GL;
        public byte GH;

        public short G
        {
            get
            {
                return BitConverter.ToInt16(new byte[] { GL, GH }, 0);
            }
            set
            {
                GL = (byte)(value & 0x00FF);
                GH = (byte)((value & 0xFF00) >> 8);
            }
        }

        public short X, Y;
        public ushort IP, SP, BP;

        public bool ZERO;
        public bool EQUAL;
        public bool GREATER;

        public Registers()
        {
            A = B = C = D = E = F = G = X = Y = 0;
            IP = SP = BP = 0;
            ZERO = EQUAL = GREATER = false;
        }

        public bool is16BitRegister(byte regNum)
        {
            return !(regNum == 0 || regNum == 1 || regNum == 3 || regNum == 4 || regNum == 6 || regNum == 7 || regNum == 9 || regNum == 10 || regNum == 12 || regNum == 13 || regNum == 15 || regNum == 16 || regNum == 18 || regNum == 19);
        }

        public int getRegister(byte regNum)
        {
            switch (regNum)
            {
                case 0:
                    return AL;
                case 1:
                    return AH;
                case 2:
                    return A;
                case 3:
                    return BL;
                case 4:
                    return BH;
                case 5:
                    return B;
                case 6:
                    return CL;
                case 7:
                    return CH;
                case 8:
                    return C;
                case 9:
                    return DL;
                case 10:
                    return DH;
                case 11:
                    return D;
                case 12:
                    return EL;
                case 13:
                    return EH;
                case 14:
                    return E;
                case 15:
                    return FL;
                case 16:
                    return FH;
                case 17:
                    return F;
                case 18:
                    return GL;
                case 19:
                    return GH;
                case 20:
                    return G;
                case 21:
                    return X;
                case 22:
                    return Y;
                case 23:
                    return IP;
                case 24:
                    return SP;
                case 25:
                    return BP;
            }
            return 0;
        }

        public void setRegister(byte regNum, int data)
        {
            switch (regNum)
            {
                case 0:
                    AL = (byte)data;
                    break;
                case 1:
                    AH = (byte)data;
                    break;
                case 2:
                    A = (short)data;
                    break;
                case 3:
                    BL = (byte)data;
                    break;
                case 4:
                    BH = (byte)data;
                    break;
                case 5:
                    B = (short)data;
                    break;
                case 6:
                    CL = (byte)data;
                    break;
                case 7:
                    CH = (byte)data;
                    break;
                case 8:
                    C = (short)data;
                    break;
                case 9:
                    DL = (byte)data;
                    break;
                case 10:
                    DH = (byte)data;
                    break;
                case 11:
                    D = (short)data;
                    break;
                case 12:
                    EL = (byte)data;
                    break;
                case 13:
                    EH = (byte)data;
                    break;
                case 14:
                    E = (short)data;
                    break;
                case 15:
                    FL = (byte)data;
                    break;
                case 16:
                    FH = (byte)data;
                    break;
                case 17:
                    F = (short)data;
                    break;
                case 18:
                    GL = (byte)data;
                    break;
                case 19:
                    GH = (byte)data;
                    break;
                case 20:
                    G = (short)data;
                    break;
                case 21:
                    X = (short)data;
                    break;
                case 22:
                    Y = (short)data;
                    break;
                case 23:
                    IP = (ushort)data;
                    break;
                case 24:
                    SP = (ushort)data;
                    break;
                case 25:
                    BP = (ushort)data;
                    break;
            }
        }
    }
}
