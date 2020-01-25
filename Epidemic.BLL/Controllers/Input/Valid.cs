using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epidemic.BLL.Controllers.Input;

namespace Vaccination.Controllers
{
    public class Valid
    {
        public static double CheckA1(string s)
        {
            double a1 = Transform.ToDouble(s);
            if (a1 >= 0 && a1 <= 1) return a1;
            throw new Exception("Параметр а1 приймає недопустиме значення: " + a1);
        }

        public static double CheckA2(string s)
        {
            double a2 = Transform.ToDouble(s);
            if (a2 >= 0 && a2 <= 1) return a2;
            throw new Exception("Параметр а2 приймає недопустиме значення: " + a2);
        }

        public static double CheckA3(string s)
        {
            double a3 = Transform.ToDouble(s);
            if (a3 > 0 && a3 <= 1) return a3;
            throw new Exception("Параметр а3 приймає недопустиме значення: " + a3);
        }

        public static double CheckA4(string s)
        {
            double a4 = Transform.ToDouble(s);
            if (a4 > 0 && a4 <= 1) return a4;
            throw new Exception("Параметр а4 приймає недопустиме значення: " + a4);
        }

        public static double CheckEf(string s)
        {
            double ef = Transform.ToDouble(s);
            if (ef > 0 && ef <= 1) return ef;
            throw new Exception("Параметр ef приймає недопустиме значення: " + ef);
        }

        public static double CheckCv1(string s)
        {
            double cv1 = Transform.ToDouble(s);
            if (cv1 > 0) return cv1;
            throw new Exception("Параметр Cv1 приймає недопустиме значення: " + cv1);
        }

        public static double CheckCv2(string s)
        {
            double cv2 = Transform.ToDouble(s);
            if (cv2 > 0) return cv2;
            throw new Exception("Параметр Cv2 приймає недопустиме значення: " + cv2);
        }

        public static void CheckCv(string s1, string s2)
        {
            double cv1 = CheckCv1(s1);
            double cv2 = CheckCv2(s2);
            if (cv1 > cv2)
                throw new Exception("Параметр Cv1 не повинен бути більшим за Cv2");
        }

        public static double CheckCi(string s)
        {
            double ci = Transform.ToDouble(s);
            if (ci > 0) return ci;
            throw new Exception("Параметр Ci приймає недопустиме значення: " + ci);
        }

        public static double CheckInflation(string s)
        {
            double inflation = Transform.ToDouble(s);
            return inflation;
            //throw new Exception("Інфляція приймає недопустиме значення: " + inflation);
        }

        public static double CheckP1(string s)
        {
            double p1 = Transform.ToDouble(s);
            if (p1 >= 0 && p1 <= 1) return p1;
            throw new Exception("Параметр p1 приймає недопустиме значення: " + p1);
        }

        public static double CheckP2(string s)
        {
            double p2 = Transform.ToDouble(s);
            if (p2 >= 0 && p2 <= 1) return p2;
            throw new Exception("Параметр p2 приймає недопустиме значення: " + p2);
        }

        public static void CheckP(string s1, string s2)
        {
            double p1 = CheckP1(s1);
            double p2 = CheckP2(s2);
            if (p1 + p2 > 1)
                throw new Exception("Параметри p1 і p2 сумарно не повинні бути більшими за 1");
        }

        public static double CheckBirthPercent(string s)
        {
            double birthPercent = Transform.ToDouble(s);
            if (birthPercent >= 0) return birthPercent;
            throw new Exception("Параметр народжуваності не може бути від'ємним: " + birthPercent);
        }

        public static double CheckDeathPercent(string s)
        {
            double deathPercent = Transform.ToDouble(s);
            if (deathPercent >= 0) return deathPercent;
            throw new Exception("Параметр смертності не може бути від'ємним: " + deathPercent);
        }

        public static int CheckBirth(string s)
        {
            int birth = Transform.ToInt32(s);
            if (birth >= 0) return birth;
            throw new Exception("Параметр народжуваності не може бути від'ємним: " + birth);
        }

        public static int CheckDeath(string s)
        {
            int death = Transform.ToInt32(s);
            if (death >= 0) return death;
            throw new Exception("Параметр смертності не може бути від'ємним: " + death);
        }
    }
}
