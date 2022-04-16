using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//KRIZA LACSAMANA 0771310

namespace Weightlifting
{
    public class Athlete
    {
        private int id;
        private string fname;
        private string lname;
        private string country;
        private int weight;

        //getters and setters
        public string Fname { get => fname; set => fname = value; }
        public string Lname { get => lname; set => lname = value; }
        public string Country { get => country; set => country = value; }
        public int Weight { get => weight; set => weight = value; }

        //constructor with 5 parameters
        public Athlete(int id, string firstName, string lastName, string origin, int kg)
        {
            this.id = id;
            this.Fname = firstName;
            this.Lname = lastName;
            this.Country = origin;
            this.Weight = kg;
        }

        //constructor with 4 parameters
        public Athlete(string firstName, string lastName, string origin, int kg)
        {
            this.Fname = firstName;
            this.Lname = lastName;
            this.Country = origin;
            this.Weight = kg;
        }

        //getAthlete() method used to write in athleteFile
        public string getAthlete()
        {
            return (id + "/" + Fname + "/" + Lname + "/" + Country + "/" + Weight + "\n");
        }

        //fileAthlete() method used when transfering dictionary data to existing athleteFile
        public string fileAthlete()
        {
            return (Fname + "/" + Lname + "/" + Country + "/" + Weight + "\n");
        }
    }

    internal class Program
    {
        public string athleteFile;

        public Program(string fileName)
        {
            athleteFile = fileName;
        }

        static int choice = -1;
        static Program prog = new Program("athlete.txt");
        static void Main(string[] args)
        {
        Menu:
            Console.Clear();
            displayTitle();
            do
            {
                choice = menu();
            } while (choice < 0 || choice > 4);

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    displayTitle();
                    add();
                    prog.addAthlete();
                    goto Menu;
                case 2:
                    Console.Clear();
                    displayTitle();
                    delete();
                    prog.deleteAthlete();
                    goto Menu;
                case 3:
                    Console.Clear();
                    displayTitle();
                    modify();
                    prog.modifyAthlete();
                    goto Menu;
                case 4:
                    Console.Clear();
                    displayTitle();
                    display();
                    prog.displayAthletes();
                    Console.ReadKey();
                    goto Menu;
                case 0:
                    Console.Clear();
                    displayTitle();
                    exit();
                    break;
            }
        }

        public void addAthlete()
        {
            //ask user for athlete's informations
            Console.WriteLine("\nEnter ID: ");
            var userID = Console.ReadLine();

            //verify if user typed in numbers
            int idOutput;
            while((!int.TryParse(userID, out idOutput)) || (userID.Length != 3)) 
            {
                Console.WriteLine("Please enter 3 numbers.");
                userID = Console.ReadLine();
            }
        
            Console.WriteLine("Enter first name: ");
            string firstName = Console.ReadLine();
            //verify if user typed in letters
            while ((!Regex.IsMatch(firstName, @"^[a-zA-Z]+$")) || (!(firstName.Length >= 2)))
            {
                Console.WriteLine("Please enter a first name with more than 2 letters.");
                firstName = Console.ReadLine();
            }

            Console.WriteLine("Enter last name: ");
            string lastName = Console.ReadLine();
            //verify if user typed in letters
            while ((!Regex.IsMatch(lastName, @"^[a-zA-Z]+$")) || (!(lastName.Length >= 2)))
            {
                Console.WriteLine("Please enter a last name with more than 2 letters.");
                lastName = Console.ReadLine();
            }

            Console.WriteLine("Enter representing country: ");
            string represent = Console.ReadLine();
            //verify if user typed in letters
            while (!Regex.IsMatch(represent, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Please enter letters only.");
                represent = Console.ReadLine();
            }

            Console.WriteLine("Enter weight: ");
            var userWeight = Console.ReadLine();

            //verify if user typed in numbers
            int weightOutput;
            while (!int.TryParse(userWeight, out weightOutput))
            {
                Console.WriteLine("Please enter numbers only.");
                userWeight = Console.ReadLine();
            }

            //creation of new athlete
            Athlete newAthlete = new Athlete(idOutput, firstName, lastName, represent, weightOutput);

            //transfer athlete's information to athleteFile
            File.AppendAllText(athleteFile, newAthlete.getAthlete());
          
        }

        public void deleteAthlete()
        {
            //create dictionary with a int key(id) and object value(Athlete)
            //transfer data from file to dictionary
            Dictionary<int, Athlete> dictionaryAthletes = File.ReadLines(athleteFile)
                .Select(line => line.Split('/'))
                .Where(split => split[0] != "id")
                .ToDictionary(split => int.Parse(split[0]),
                    //creating new Athlete athD in order to stock all athlete's info then store it in dictionary
                    split => new Athlete(split[1], split[2], split[3], int.Parse(split[4])));

            //ask user for athlete's id in order to delete
            Console.WriteLine("\nEnter athlete to delete: ");
            var userDelete = Console.ReadLine();

            //verify if user typed in numbers
            int deleteOutput;
            while (!int.TryParse(userDelete, out deleteOutput))
            {
                Console.WriteLine("Please enter an existing ID.");
                userDelete = Console.ReadLine();
            }

            //search for int key to delete athlete
            if (dictionaryAthletes.ContainsKey(deleteOutput))
            {
                dictionaryAthletes.Remove(deleteOutput);
                //delete existing athlete file
                File.Delete(athleteFile);

                //for each int key and object value found in dictionary, transfer dictionary data to file (rewrite existing athleteFile)
                foreach (KeyValuePair<int, Athlete> kvp in dictionaryAthletes)
                {
                    //fileAthlete() method used so that it prints the correct attributes
                    File.AppendAllText(athleteFile, string.Format("{0}/{1}", kvp.Key, kvp.Value.fileAthlete()));
                }
            }
            //confirm if id exists
            else if (!(dictionaryAthletes.ContainsKey(deleteOutput)))
            {
                idInvalid();
                Console.WriteLine("PLEASE TRY AGAIN.");
                deleteAthlete();
            }
        }

        public void modifyAthlete()
        { 
            Athlete athD;

            //create dictionary with a int key(id) and object value(Athlete)
            Dictionary<int, Athlete> dictionaryAthletes = new Dictionary<int, Athlete>();

            //transfer data from file to dictionary
            foreach (string line in File.ReadAllLines(athleteFile))
            {
                string[] split = line.Split('/');
                if (split.Length == 5)
                {
                    int tmpID = int.Parse(split[0]);
                    string tmpName = split[1];
                    string tmpLName = split[2];
                    string tmpCountry = split[3];
                    int tmpWeight = int.Parse(split[4]);
                    //creating new Athlete athD in order to stock all athlete's info then store it in dictionary
                    athD = new Athlete(tmpName, tmpLName, tmpCountry, tmpWeight);
                    dictionaryAthletes[tmpID] = athD;
                }
            }

            //ask user for athlete's id in order to modify info
            Console.WriteLine("\nEnter athlete to modify: ");
            var userModify = Console.ReadLine();

            int modifyOutput;
            while (!int.TryParse(userModify, out modifyOutput))
            {
                Console.WriteLine("Please enter an existing ID.");
                userModify = Console.ReadLine();
            }

            //search for int key to modify athlete
            if (dictionaryAthletes.ContainsKey(modifyOutput))
            {
                //ask user to modify weight of specific athlete
                Console.WriteLine("Enter new weight lifted: ");
                //int newWeight = Convert.ToInt32(Console.ReadLine());
                var userWeight = Console.ReadLine();

                //verify if user typed in numbers
                int weightOutput;
                while (!int.TryParse(userWeight, out weightOutput))
                {
                    Console.WriteLine("Please enter a valid weight.");
                    userWeight = Console.ReadLine();
                }
                //newWeight entered by user is updated in dictionary
                dictionaryAthletes[(modifyOutput)].Weight = weightOutput;

                //delete existing athlete file
                File.Delete(athleteFile);

                //transfer dictionary data to file (rewrite existing athleteFile)
                for (int i = 0; i < dictionaryAthletes.Count; i++)
                {
                    int numD = dictionaryAthletes.ElementAt(i).Key;
                    string fnameD = dictionaryAthletes.ElementAt(i).Value.Fname;
                    string lnameD = dictionaryAthletes.ElementAt(i).Value.Lname;
                    string countryD = dictionaryAthletes.ElementAt(i).Value.Country;
                    int weightD = dictionaryAthletes.ElementAt(i).Value.Weight;
                    Athlete athleteD = new Athlete(numD, fnameD, lnameD, countryD, weightD);
                    File.AppendAllText(athleteFile, athleteD.getAthlete());
                }
            }
            //confirm if id exists
            else if (!(dictionaryAthletes.ContainsKey(modifyOutput)))
            {
                idInvalid();
                Console.WriteLine("PLEASE TRY AGAIN.");
                modifyAthlete();
            }
        }

        public void displayAthletes()
        {
            Dictionary<int, Athlete> dictionaryAthletes = new Dictionary<int, Athlete>();

            Athlete athD;

            //transfer data from file to dictionary
            foreach (string line in File.ReadAllLines(athleteFile))
            {
                string[] split = line.Split('/');
                if (split.Length == 5)
                {
                    int tmpID = int.Parse(split[0]);
                    string tmpName = split[1];
                    string tmpLName = split[2];
                    string tmpCountry = split[3];
                    int tmpWeight = int.Parse(split[4]);
                    //creating new Athlete athD in order to stock all athlete's info then store it in dictionary
                    athD = new Athlete(tmpName, tmpLName, tmpCountry, tmpWeight);
                    dictionaryAthletes[tmpID] = athD;
                }
            }

            Console.WriteLine("\nWEIGHT LIFTED BY DESCENDING ORDER, THEN BY ALPHABETICAL ORDER: \n");

            foreach (KeyValuePair<int, Athlete> entry in dictionaryAthletes.OrderByDescending(newOrder => newOrder.Value.Weight).ThenBy(newOrder => newOrder.Value.Fname))
            {
                //Console.WriteLine("Id: " + entry.Key + "\nWeight: " + entry.Value.Weight + "\nFname: " + entry.Value.Fname + "\nLname: " + entry.Value.Lname + "\nCountry: " + entry.Value.Country);
                Console.WriteLine("Id: " + entry.Key + "\t\tWeight: " + entry.Value.Weight + "\t\tName: " + entry.Value.Fname.First().ToString().ToUpper() + entry.Value.Fname.Substring(1) + " " + entry.Value.Lname.First().ToString().ToUpper() 
                    + entry.Value.Lname.Substring(1) + "\t\tCountry: " + entry.Value.Country.First().ToString().ToUpper() + entry.Value.Country.Substring(1));
            }
        }

        public static void displayTitle()
        {
            //Console.Clear();
            Console.WriteLine("******************************************************************************************************************");
            Console.WriteLine("  Alaidine Ben Ayed                                                                              KRIZA LACSAMANA  ");
            Console.WriteLine("  420-B05-RO                                                                                             0771310  ");
            Console.WriteLine("                                                                                                         gr. 427  ");
            Console.WriteLine("******************************************************************************************************************\n\n");
            Console.WriteLine("                         == Techniques de développement dans un environnement graphique ==                        \n\n");
            Console.WriteLine("******************************************************************************************************************\n");
            Console.WriteLine("========================================    WEIGHTLIFTING REGISTRATION    ========================================\n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static int menu()
        {
            Console.WriteLine("*****                                        Please choose an option                                         *****\n");
            Console.WriteLine("*****                                    1. Add athlete                                                      *****");
            Console.WriteLine("*****                                    2. Delete athlete                                                   *****");
            Console.WriteLine("*****                                    3. Modify weight lifted                                             *****");
            Console.WriteLine("*****                                    4. Display list of athletes                                         *****");
            Console.WriteLine("*****                                    0. Exit                                                             *****\n");
            Console.WriteLine("******************************************************************************************************************\n");
            Console.Write("Chosen option: ");

            string result = Console.ReadLine();
            result = isValid(result);
            int option = int.Parse(result);
            return option;
        }

        public static string isValid(string option)
        {
            if (string.IsNullOrEmpty(option) || Int32.Parse(option) < 0 || Int32.Parse(option) > 4)
            {
                Console.Clear();
                displayTitle();
                Console.WriteLine("                        <<<<<<<<<<<<<<   INVALID OPTION, PLEASE TRY AGAIN   >>>>>>>>>>>>>>>                       \n");
                Console.WriteLine("******************************************************************************************************************\n");
            }
            return option;
        }

        public static void idInvalid()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                               <<<<<<<<<<<<<<   ID DOES NOT EXIST   >>>>>>>>>>>>>>>                       \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static void add()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                          <<<<<<<<<<<<<<<<<<<<<<<   ADD ATHLETE   >>>>>>>>>>>>>>>>>>>>>>                          \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static void delete()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                          <<<<<<<<<<<<<<<<<<<<<   DELETE ATHLETE   >>>>>>>>>>>>>>>>>>>                            \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static void modify()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                         <<<<<<<<<<<<<<<<<<<<<<   MODIFY ATHLETE   >>>>>>>>>>>>>>>>>>>>>>>                        \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static void display()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                         <<<<<<<<<<<<<<<<<<<<<<   DISPLAY ATHLETE   >>>>>>>>>>>>>>>>>>>>>>                         \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

        public static void exit()
        {
            Console.Clear();
            displayTitle();
            Console.WriteLine("                     <<<<<<<<<<<<<<<<<<   THANK YOU, HAVE A GREAT DAY :)   >>>>>>>>>>>>>>>>                     \n");
            Console.WriteLine("******************************************************************************************************************\n");
        }

    }
}

