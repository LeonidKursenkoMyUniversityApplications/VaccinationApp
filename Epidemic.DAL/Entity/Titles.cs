using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epidemic.DAL.Entity
{
    public class Titles
    {
        public static string Beta => "Параметр передачі збудника";
        public static string Birth => "Новонароджені особи";
        public static string Ci => "Вартість лікування Ci, грн";
        public static string Cv1 => "Вартість однієї дози вакцини Cv1, грн";
        public static string Cv1DivCi => "Коефіцієнт відношення витрат Cv1/Ci";
        public static string Cv2 => "Вартість двох доз вакцини Cv2, грн";
        public static string Cv2DivCi => "Коефіцієнт відношення витрат Cv2/Ci";
        public static string Date => "Дата";
        public static string Gamma => "Швидкість одужання";
        public static string Inf => "Інфіковані особи";
        public static string InfFromBirth => "Залежність інфікованих осіб від народжуваності";
        public static string Iv1 => "Носії РВІ вакциновані однією дозою вакцини";
        public static string Iv2 => "Носії РВІ вакциновані двома дозами вакцини";
        public static string P1 => "Охоплення осіб однією дозою вакцини";
        public static string P1Clinic => "Ймовірність захворювання";
        public static string P2 => "Охоплення осіб двома дозами вакцини";
        public static string Sus => "Сприйнятливі особи";
        public static string N => "Все населення";
        public static string InfTotal => "Всього інфікованих осіб";
        public static string V1 => "Вакциновані особи однією дозою вакцини";
        public static string V2 => "Вакциновані особи двома дозами вакцини";
        public static string PredictedInfetions => "Попереджені випадки РВІ";
    }
}
