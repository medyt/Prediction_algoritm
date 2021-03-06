﻿using System;
using System.IO;
using System.Linq;

namespace ValeurVenale
{
    class Program
    {
        //Type of properties
        public enum HouseTypeEnum
        {
            Other = 0,
            House = 1,
            Commercial = 2,
            Apartment = 3,
            Garage = 4,
            InvestmentBuilding = 5,
            Storage = 6,
            Office = 7,
            Project = 8
        }
        //statistical variable
        static double[] priceDifference;
        static double[] procentOfEroor;
        //algorithmic variable
        static int numberOfProperty;
        static string [] Country;
        static double [] LivingSpaceSurface;
        static int[] NumberOfFacades;
        static int[] ConstructionYears;
        static double[] LandSurfaces;
        static double[] Prices;
        static int[] Types;
        static string[] ApartmentTypes;
        static string[] Conditions;
        static string[] Garages;
        static string[] Basements;//subsolul
        static string[] Gardens;//gradina exterioara
        static string[] OutsideFeatures; //alte anexe
        static double[] Terrace;//cati metrii are terasa.
        static string[] CheckOutsideFeaturesBalcony;// daca este balcon in apartament.
        static string[] CheckOutsideFeaturesParking;// daca este loc de parcare pentru apartament
        static double[] GardenSurface;//suprafata gradinii amenajate.
        static string[] Parlophone;//interfon appartment
        static string[] Elevator;//lift appartment
        static string[] ArmouredDoor; // usa metalica appartment
        static string[] Windows;// geamurile apartamentului
        static void Main(string[] args)
        {
            readFromDatabase();
            //readFromCalin2017();
            int i;
            double VN;
            double SUP;
            double indice = 1.15;
            double Tc = 10000;
            //ultima valoare din istoricul indicelui ABEX din istoricul sau 
            double ABEX = 775;

            double VGlob;
            double UG;
            double UT;

            double CG = 1;

            double Vrt;
            double pricepermeter;

            double Values;

            double CI;

            double VV;
            string newProperty = "My estimation" + "," + "Old Price" + "," + "Diferenta de preturi" + "," + "Procentul erorii" + "," + "\n";

            //statistical variable initialization;
            priceDifference = new double[1000];
            procentOfEroor = new double[1000];

            int countPropertyEvaluate = 0;
            double procentageaverage = 0;

            for (i = 1; i <= numberOfProperty; i++)
            {
                if(Country[i] != null && Prices[i] > 3500 && (Types[i] == 1 || Types[i] == 3))
                {
                    if(LivingSpaceSurface[i] != 0)
                    {
                        SUP = LivingSpaceSurface[i];
                        if (Types[i] == 1)
                        {
                            if (NumberOfFacades[i] != 0)
                            {
                                if (NumberOfFacades[i] == 2)
                                {
                                    if (Conditions[i] == "New")
                                    {
                                        Tc =1.355;
                                    }
                                    else
                                    {
                                        if(Conditions[i] == "ConditionNeedsImprovement")
                                        {
                                            Tc = 1.153;
                                        }
                                        else
                                        {
                                            if (Conditions[i] == "OldToRenovate")
                                            {
                                                Tc = 1.18;
                                            }
                                            else
                                            {
                                                if (Conditions[i] == "DescConditionGood")
                                                {
                                                    Tc = 1.3;
                                                }
                                                else
                                                {
                                                    if (Conditions[i] == "ConditionOther")
                                                    {
                                                        Tc = 1.25;
                                                    }
                                                    else
                                                    {
                                                        Tc = 1.153;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (NumberOfFacades[i] == 3)
                                    {
                                        if (Conditions[i] == "DescConditionGood")
                                        {
                                            Tc = 1.3;
                                        }
                                        else
                                        {
                                            if (Conditions[i] == "ConditionNeedsImprovement")
                                            {
                                                Tc = 1.25;
                                            }
                                            else
                                            {
                                                if (Conditions[i] == "OldToRenovate")
                                                {
                                                    Tc = 1.45;
                                                }
                                                else
                                                {
                                                    Tc = 1.55;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (NumberOfFacades[i] == 4)
                                        {
                                            if (Conditions[i] == "New")
                                            {
                                                Tc = 1.85;
                                            }
                                            else
                                            {
                                                if (Conditions[i] == "OldToRenovate")
                                                {
                                                    Tc = 1.25;
                                                }
                                                else
                                                {
                                                    if (Conditions[i] == "DescConditionGood")
                                                    {
                                                        Tc = 1.4;
                                                    }
                                                    else
                                                    {
                                                        Tc = 1.65;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Tc = 0;
                            }
                        } else
                        {
                            if (Types[i] == 3)
                            {
                                if(ApartmentTypes[i] == "AppartmentTypeTriplex")
                                {
                                    if (Conditions[i] == "OldToRenovate")
                                    {
                                        Tc = 1.65;
                                    }
                                    else
                                    {
                                        if (Conditions[i] == "DescConditionGood")
                                        {
                                            Tc = 1.2;
                                        }
                                        else
                                        {
                                            Tc = 1.35;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ApartmentTypes[i] == "AppartmentTypeSimple")
                                    {
                                        Tc = 1.85;
                                        if (Conditions[i] == "New")
                                        {
                                            Tc = 2.3;
                                        }
                                        if(Conditions[i] == "OldRefurbished")
                                        {
                                            Tc = 2.1;
                                        }
                                        if(Conditions[i] == "DescConditionGood")
                                        {
                                            Tc = 1.95;
                                        }
                                        if(Conditions[i] == "OldToRenovate")
                                        {
                                            Tc = 2;
                                        }
                                        if(Conditions[i] == "Unknown")
                                        {
                                            Tc = 1.6;
                                        }
                                        if(Conditions[i] == "ConditionOther")
                                        {
                                            Tc = 2.10;
                                        }
                                    }
                                    else
                                    {      
                                        if(ApartmentTypes[i] == "AppartmentTypeStudio")
                                        {
                                            if (Conditions[i] == "OldRefurbished")
                                            {
                                                Tc = 3.2;
                                            }
                                            else
                                            {
                                                Tc = 2.15;
                                            }
                                        }
                                        else
                                        {
                                            if (ApartmentTypes[i] == "AppartmentTypeDuplex")
                                            {
                                                if (Conditions[i] == "OldRefurbished")
                                                {
                                                    Tc = 2;
                                                }
                                                else
                                                {
                                                    if (Conditions[i] == "New")
                                                    {
                                                        Tc = 2;
                                                    }
                                                    else
                                                    {
                                                        if (Conditions[i] == "HeatingAirConditioning")
                                                        {
                                                            Tc = 1.7;
                                                        }
                                                        else
                                                        {
                                                            Tc = 1.5;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (ApartmentTypes[i] == "AppartamentOther")
                                                {
                                                    Tc = 2;
                                                    if (Conditions[i] == "New")
                                                    {
                                                        Tc = 2.25;
                                                    }
                                                }
                                                else
                                                {
                                                    Tc = 2;
                                                }
                                            }
                                        }                                        
                                    }
                                }                                
                            }
                        }
                        if (Tc != 0)
                        {
                            VN = computeValeurNeuf(SUP, indice, Tc, ABEX);
                            int procent = 0;
                            int differenceYear = 0;
                            if (ConstructionYears[i] != 0)
                            {
                                DateTime date = DateTime.Now;
                                differenceYear = date.Year - ConstructionYears[i];
                                if (differenceYear < 10)
                                    procent = differenceYear * 3;
                                else
                                {
                                    if (differenceYear < 15)
                                    {
                                        procent = differenceYear * 4;
                                    }
                                    else
                                    {
                                        procent = differenceYear * 5;
                                    }
                                }
                            }
                            if (procent > 100 || procent == 0)
                            {
                                UG = 0.092 * VN;
                                UT = 0;
                            }
                            else
                            {
                                UG = 0.08 * VN;
                                UT = procent / 1000 * VN;
                            }                            
                            if(Conditions[i] == "New")
                            {
                                UG = UT = 0;
                            }
                            VGlob = computeValeurGloballe(UT, UG);
                            

                            //where is CG 
                            CG = 1;

                            //Price per meter in functie de locatie
                            pricepermeter = 90;
                            Vrt = computeValeurReziduelle(LandSurfaces[i], pricepermeter);
                            if(GardenSurface[i] != 0 && Types[i] == 1)
                            {
                                Vrt = 150 * GardenSurface[i];
                            }                            
                            //trebuie observate garajul si alte chestii de pe langa casa sau chiar din ea.
                            if (Types[i] == 1)
                            {
                                Values = 0;
                                if (Garages[i] == "GarageNone")
                                {
                                    Values = Values - 6000;
                                }                                
                                else
                                {
                                    if(Garages[i] == "GarageOneCar")
                                        Values = Values + 6000;
                                    if (Garages[i] == "GarageTwoCar")
                                        Values = Values + 10000;
                                }
                                if(Basements[i] == "on")
                                {
                                    Values = Values + 6000;
                                }
                                if(Gardens[i] == "on")
                                {
                                    Values = Values + 10000;
                                }
                                else
                                {
                                    Values = Values - 2000;
                                }
                                if (OutsideFeatures[i] == "annexe")
                                {
                                    Values = Values + 0.3 * VN;
                                }
                            }
                            else
                            {
                                Values = 0;
                                if(Terrace[i] > 0)
                                {
                                    Values += 600 * Terrace[i];
                                }
                                if(CheckOutsideFeaturesBalcony[i] == "on")
                                {
                                    Values += 0.1 * VN;
                                }
                                if(CheckOutsideFeaturesParking[i] == "on")
                                {
                                    Values += 0.05 * VN;
                                }
                                if(OutsideFeatures[i] == "Cave")
                                {
                                    Values += 6000;
                                }
                                if(Parlophone[i] == "on")
                                {
                                    Values += 2000;
                                }
                                if(Elevator[i] == "on")
                                {
                                    Values += 0.005*VN;
                                }
                                if(ArmouredDoor[i] == "on")
                                {
                                    Values += 0.005 * VN;
                                }
                                if(Windows[i] == "WindowsTriple")
                                {
                                    Values += 7500;
                                }
                                if (Windows[i] == "WindowsDouble")
                                {
                                    Values += 5000;
                                }
                            }

                            //rata dobanzii unei cladiri
                            CI = 1;
                            VV = computeValeurVenale(VN, VGlob, CG, Vrt, Values, CI);
                            //output in CSV
                            priceDifference[i] = (Prices[i] > VV) ? Prices[i] - VV : VV - Prices[i];
                            procentOfEroor[i] = priceDifference[i] * 100 / Prices[i];
                            if (procentOfEroor[i] < 100)//daca eroarea este mai mare de 100% consider ca datele sunt introduse gresit
                            {
                                newProperty += VV + "," + Prices[i] + "," + priceDifference[i] + "," + procentOfEroor[i] + "," + "\n";
                                //calculez media procentelor
                                countPropertyEvaluate++;
                                procentageaverage += procentOfEroor[i];
                            }
                            if (procentOfEroor[i] > 15)
                            {
                                //am ceva aici
                                Console.WriteLine("da");
                            }
                        }
                    }
                }                
            }
            File.WriteAllText(@"C:\Users\Madalin\Desktop\ValeurVenale\predict.csv", newProperty);

            //fac fisierul pentru a desena precision recoll curves
            string newprag = "";
            double precision = 0;
            double recall = 0;
            for (i = 5; i <= 100; i = i + 5)
            {
                precision = computePrecisionLessThanParameter(i);
                recall = computeRecallBiggerThanParameter(i);
                newprag += precision + "\n" + recall + "\n";                
            }
            File.WriteAllText(@"C:\Users\Madalin\Desktop\ValeurVenale\precisionrecall.txt", newprag);

            procentageaverage /= countPropertyEvaluate;
            Console.WriteLine(procentageaverage);
        }
        static void readFromCalin2017()
        {
            String line;
            numberOfProperty = 0;
            Country = new string[1000];
            LivingSpaceSurface = new double[1000];
            NumberOfFacades = new int[1000];
            ConstructionYears = new int[1000];
            LandSurfaces = new double[1000];
            Prices = new double[1000];
            Types = new int[1000];
            ApartmentTypes = new string[1000];
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("C:\\Users\\Madalin\\Desktop\\ValeurVenale\\dataCalin.txt"))
                {
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains("/*"))
                        {
                            numberOfProperty++;
                            Console.WriteLine(line);
                            line = sr.ReadLine();
                            Types[numberOfProperty] = Convert.ToInt32(line);
                            Console.WriteLine(line);
                            line = sr.ReadLine();
                            NumberOfFacades[numberOfProperty] = Convert.ToInt32(line);
                            Console.WriteLine(line);
                            line = sr.ReadLine();
                            Prices[numberOfProperty] = Convert.ToDouble(line);
                            Console.WriteLine(line);
                            line = sr.ReadLine();
                            LivingSpaceSurface[numberOfProperty] = Convert.ToDouble(line);
                            Country[numberOfProperty] = "Belgium";
                        }
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        static void readFromDatabase()
        {
            numberOfProperty = 0;
            string[] words = new string[100];
            Country = new string[1000];
            LivingSpaceSurface = new double[1000];
            NumberOfFacades = new int[1000];
            ConstructionYears = new int[1000];
            LandSurfaces = new double[1000];
            Prices = new double[1000];
            Types = new int[1000];
            ApartmentTypes = new string[1000];
            Conditions = new string[1000];
            Garages = new string[1000];
            Basements = new string[1000];
            Gardens = new string[1000];
            OutsideFeatures = new string[1000];
            Terrace = new double[1000];
            CheckOutsideFeaturesBalcony = new string[1000];
            CheckOutsideFeaturesParking = new string[1000];
            GardenSurface = new double[1000];
            Parlophone = new string[1000];
            Elevator = new string[1000];
            ArmouredDoor = new string[1000];
            Windows = new string[1000];
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("C:\\Users\\Madalin\\Desktop\\ValeurVenale\\property.txt"))
                {
                    String line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains("/*"))
                        {
                            numberOfProperty++;
                        }
                        if (line.Contains("Country"))
                        {
                            words = line.Split('"');
                            Country[numberOfProperty] = words[3];
                        }
                        if (line.Contains("Windows"))
                        {
                            words = line.Split('"');
                            Windows[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckConfortArmouredDoor"))
                        {
                            words = line.Split('"');
                            ArmouredDoor[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckConfortParlophone" ))
                        {
                            words = line.Split('"');
                            Parlophone[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckConfortElevator"))
                        {
                            words = line.Split('"');
                            Elevator[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckOutsideFeaturesBalcony"))
                        {
                            words = line.Split('"');
                            CheckOutsideFeaturesBalcony[numberOfProperty] = words[3];                            
                        }
                        if (line.Contains("CheckOutsideFeaturesParking"))
                        {
                            words = line.Split('"');
                            CheckOutsideFeaturesParking[numberOfProperty] = words[3];
                        }                        
                        if (line.Contains("CheckOutsideFeaturesTerrace"))
                        {
                            line = sr.ReadLine();
                            if(line.Contains("EntryValue"))
                            {
                                words = line.Split('"');
                                Terrace[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                            }
                        }
                        if (line.Contains("OutsideFeaturesOther"))
                        {
                            words = line.Split('"');
                            OutsideFeatures[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckOutsideFeaturesYard") || line.Contains("CheckGardenBack"))
                        {
                            words = line.Split('"');
                            Gardens[numberOfProperty] = words[3];
                        }
                        if (line.Contains("CheckBasementCellar"))
                        {
                            words = line.Split('"');
                            Basements[numberOfProperty] = words[3];
                        }
                        if (line.Contains("GarageDropdown"))
                        {
                            words = line.Split('"');
                            Garages[numberOfProperty] = words[3];
                        }
                        if (line.Contains("Condition") && !line.Contains("IndividualConditions"))
                        {
                            words = line.Split('"');
                            Conditions[numberOfProperty] = words[3];
                        }
                        if (line.Contains("LivingSpaceSurface"))
                        {
                            words = line.Split('"');
                            LivingSpaceSurface[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("NumberOfFacades"))
                        {
                            words = line.Split('"');
                            NumberOfFacades[numberOfProperty] = Convert.ToInt32(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("ConstructionYear"))
                        {
                            words = line.Split('"');
                            if(words[3] == "Unknown")
                            {
                                ConstructionYears[numberOfProperty] = 0;
                            } else {
                                ConstructionYears[numberOfProperty] = Convert.ToInt32(words[3]);
                            }
                            
                        }
                        if (line.Contains("LandSurface"))
                        {
                            words = line.Split('"');
                            
                            if (words[2] == " : ")
                            {
                                LandSurfaces[numberOfProperty] = 0;
                            } else
                            {
                                LandSurfaces[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                            }                            
                        }
                        if (line.Contains("GardenSurface"))
                        {
                            line = sr.ReadLine();
                            if (line.Contains("EntryValue"))
                            {
                                words = line.Split('"');
                                GardenSurface[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                                Gardens[numberOfProperty] = "on";
                            }
                        }
                        if (line.Contains("Price"))
                        {
                            words = line.Split('"');
                            Prices[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("Type"))
                        {
                            words = line.Split('"');
                            if(words[1] == "Type")
                                Types[numberOfProperty] = Convert.ToInt32(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if(line.Contains("AppartmentType"))
                        {
                            words = line.Split('"');
                            ApartmentTypes[numberOfProperty] = words[3];
                        }
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        static double computeValeurVenale(double VN, double VGlob, double CG, double Vrt, double Values, double CI)
        {
            double x = ((VN - VGlob) * CG + Vrt + Values) * CI;
            return x;
        }
        static double computeValeurNeuf(double SUP, double indice, double Tc, double ABEX)
        {
            double x = SUP * indice * Tc * ABEX;
            return x;
        }
        static double computeValeurGloballe(double UT, double UG)
        {
            double x = UT + UG;
            return x;
        }
        static double computeValeurReziduelle(double terrainSurface, double pricepermeterTerrain)
        {
            double x = terrainSurface * 2 / 3 * pricepermeterTerrain;
            return x;
        }
        static double computePrecisionLessThanParameter(int x)
        {
            int i;
            int nr = 0, numberOfGoodProperties = 0 ;
            double precision;
            for (i = 0; i <= numberOfProperty; i++)
            {
                if (Country[i] != null && Prices[i] > 1500 && (Types[i] == 1 || Types[i] == 3))
                {
                    if (LivingSpaceSurface[i] != 0)
                    {
                        if (procentOfEroor[i] < 100)
                        {
                            numberOfGoodProperties++;
                            if (procentOfEroor[i] < x)
                            {
                                nr++;
                            }
                        }
                    }
                }
            }
            precision = nr * 100 / numberOfGoodProperties;
            precision = precision / 100;
            return precision;
        }
        static double computeRecallBiggerThanParameter(int x)
        {
            int i;
            int nr = 0, numberOfGoodProperties = 0;
            double recall;
            for (i = 0; i <= numberOfProperty; i++)
            {
                if (Country[i] != null && Prices[i] > 1500 && (Types[i] == 1 || Types[i] == 3))
                {
                    if (LivingSpaceSurface[i] != 0)
                    {
                        if (procentOfEroor[i] < 100)
                        {
                            numberOfGoodProperties++;
                            if (procentOfEroor[i] > x)
                            {
                                nr++;
                            }
                        }
                    }
                }
            }
            recall = nr * 100 / numberOfGoodProperties;
            recall = recall / 100;
            return recall;
        }

    }
}






/*Exemplul din paper
 VN = computeValeurNeuf(181.68, 1.15, 1.53, 729);
            UG = 0.08 * VN;
            VGlob = computeValeurGloballe(19500, UG);
            CG = 1;
            double terrainSurface = 197.97;
            Vrt = computeValeurReziduelle(terrainSurface, 90);
            Values = 6000;
            CI = 1;
            VV = computeValeurVenale(VN, VGlob, CG, Vrt, Values, CI);
            Console.WriteLine(VV);
 */
