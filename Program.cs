using GoodMatchTester.Services;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodMatchTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose one option:\n 1. Manually input names of the players to match\n 2. Read from your CSV file\n");
            string inputType = Console.ReadLine();

            if (inputType == "1")
            {
                //Get and log process start time
                var startTime = DateTime.Now;
                Logger.Log("Starting matching process of user inputed names " + startTime);

                GetUserInput();

                // log execution time
                Logger.Log("End of matching process");
                Logger.Log("Execution Time: " + (DateTime.Now - startTime));
                Console.ReadLine();
            }
            else if (inputType == "2")
            {
                var startTime = DateTime.Now;
                Logger.Log("Starting matching process of CSV file names " + startTime);

                ReadCSVFile();

                Logger.Log("End of matching process");
                Logger.Log("Execution Time: " + (DateTime.Now - startTime));
                Console.ReadLine();
            }
            else { Console.WriteLine("Invalid Input, Please try again, choose 1 or 2"); Logger.Log("Invalid option entered:" + inputType + " | please enter 1 for manual input or 2 to read from CSV file"); Main(args); }
            Console.ReadLine();
        }

        static void GetUserInput()
        {
            Console.WriteLine("Please Enter the First Name");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrEmpty(firstName))
            {
                firstName = firstName.Trim();

                if (Regex.IsMatch(firstName, @"^[a-zA-Z]+$"))
                {
                    Console.WriteLine("Please Enter the Second Name");
                    string secondName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(secondName))
                    {
                        secondName = secondName.Trim();

                        if (Regex.IsMatch(secondName, @"^[a-zA-Z]+$"))
                        {
                            int percentage = MatchAndGetPercentage.GetPercentage(firstName, secondName);

                            if (percentage >= 80)
                            {
                                Console.WriteLine(firstName + " " + "matches" + " " + secondName + " " + percentage + "%" + ", " + "good match");
                            }
                            else
                            {
                                Console.WriteLine(firstName + " " + "matches" + " " + secondName + " " + percentage + "%");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Only use alphabetical characters and no spaces between characters please");
                            Logger.Log("Invalid SecondName entered by user: " + secondName + " | Only use alphabetical characters and no spaces between characters please");
                            GetUserInput();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Empty input, please try again to enter names");
                        Logger.Log("Empty SecondName input by user: " + secondName + " | no empty values allowed, please enter name");
                        GetUserInput();
                    }
                }
                else
                {
                    Console.WriteLine("Only use alphabetical characters and no spaces between characters please");
                    Logger.Log("Invalid FirstName entered by user: " + firstName + " | Only use alphabetical characters and no spaces between characters please");
                    GetUserInput();
                }

            }
            else
            {
                Console.WriteLine("Empty input, please try again to enter names");
                Logger.Log("Empty FirstName input by user: " + firstName + " | no empty values allowed, please enter name");
                GetUserInput();
            }

        }
        static void ReadCSVFile()
        {
       
            Console.WriteLine("please enter CSV file full path or full name with extension, eg: players.csv :");
            var filepath = Console.ReadLine();

            var fullpath = AppContext.BaseDirectory;
            var finalPath = fullpath.Remove(fullpath.IndexOf("bin"));

            var path = finalPath + @"Resources\\"+filepath;
            var filepaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv", SearchOption.AllDirectories);
            if (File.Exists(path))
            {     
                    var lines = File.ReadAllLines(path);
                    
                    var list = new List<Player>();
                    
                    var malelist = new List<Player>();
                    
                    var femalelist = new List<Player>();
                    
                    foreach (var line in lines)
                    {
                        var values = line.Split(",");
                        var player = new Player() { Name = values[0], Gender = values[1] };

                        if (!string.IsNullOrEmpty(player.Name))
                        {
                            player.Name = player.Name.Trim();

                            if (Regex.IsMatch(player.Name, @"^[a-zA-Z]+$"))
                            {
                                if (!string.IsNullOrEmpty(player.Gender))
                                {
                                    player.Gender = player.Gender.Trim();

                                    if (Regex.IsMatch(player.Gender, @"^[mfMF]+$"))
                                    {
                                        list.Add(player);
                                    }
                                    else
                                    {
                                        Logger.Log("Invalid Gender from CSV file: " + player.Gender + " | correct gender should be f/m or F/M");
                                    }
                                }
                                else
                                {
                                    Logger.Log("Empty Gender Input from CSV file: " + player.Gender + " | no empty values allowed, please enter gender");
                                }
                            }
                            else
                            {
                                Logger.Log("Invalid Name from CSV file: " + player.Name + " | Only use alphabetical characters and no spaces between characters please");
                            }

                        }
                        else { Logger.Log("Empty Name Input from CSV file: " + player.Name + " | no empty values allowed, please enter name"); }


                    }
                    
                    Console.WriteLine(".............................................................");
                    Console.WriteLine("      CSV List of Players   ");
                    list.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
                    
                    malelist = list.Where(x => x.Gender == "m" || x.Gender == "M").ToList();
                    
                    malelist = malelist.GroupBy(x => x.Name).Select(x => x.First()).ToList();
                    
                    femalelist = list.Where(x => x.Gender == "f" || x.Gender == "F").ToList();
                    
                    femalelist = femalelist.GroupBy(x => x.Name).Select(x => x.First()).ToList();
                    Console.WriteLine("..............................Two Groups of Players................................");
                    
                    Console.WriteLine("   Male Players ");
                    malelist.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
                    Console.WriteLine("   Female Players ");
                    femalelist.ForEach(x => Console.WriteLine($"{x.Name}\t{x.Gender}"));
                    
                    int outterloop;
                    int innerloop;
                    if (malelist.Count() >= femalelist.Count())
                    {
                        outterloop = malelist.Count();
                        innerloop = femalelist.Count();
                    }
                    else
                    {
                        outterloop = femalelist.Count();
                        innerloop = malelist.Count();
                    }
                    Console.WriteLine("...........................Matching Results............................");
                    
                    List<CsvResult> csvResults = new List<CsvResult>();
                    var percentageList = new List<int>();
                    var percentageListReversed = new List<int>();
                    var macthNames = new List<string>();

                    File.WriteAllText("output.txt", "");
                    for (int i = 0; i < outterloop; i++)
                    {
                        for (int j = 0; j < innerloop; j++)
                        {
                            int percentage = MatchAndGetPercentage.GetPercentage(malelist[i].Name, femalelist[j].Name);
                            string macthStringResult = "";

                            if (percentage > 80)
                            {
                                macthNames.Add($"{malelist[i].Name} matches {femalelist[j].Name} combination average: ");
                                macthStringResult = malelist[i].Name + " " + "matches" + " " + femalelist[j].Name + " " + percentage + "%" + ", good match";
                                percentageList.Add(percentage);
                              
                            }
                            else
                            {
                                macthNames.Add($"{malelist[i].Name} matches {femalelist[j].Name} combination average: ");
                                macthStringResult = malelist[i].Name + " " + "matches" + " " + femalelist[j].Name + " " + percentage + "%";
                                percentageList.Add(percentage);
                               
                            }
                            csvResults.Add(new CsvResult { CsvStringResult = macthStringResult, ResultPercentage = percentage });
                            
                        }
                    }
                    csvResults = ResultOrdering.OrderMatchResults(csvResults);
                    if (csvResults.Any())
                    {
                        csvResults.ForEach(result => { Console.WriteLine(result.CsvStringResult); });
                        csvResults.ForEach(result => { File.AppendAllText("output.txt", result.CsvStringResult + Environment.NewLine); });
                        File.AppendAllText("output.txt", "///////////////////////////////////////" + Environment.NewLine);
                    }
                    File.AppendAllText("output.txt", "Reversed Results\n" + Environment.NewLine);
                    Console.WriteLine("...........................Reversed Result............................");
                    List<CsvResult> csvResultsReverse = new List<CsvResult>();
                    for (int i = 0; i < outterloop; i++)
                    {
                        for (int j = 0; j < innerloop; j++)
                        {
                            int percentage = MatchAndGetPercentage.GetPercentage(femalelist[j].Name, malelist[i].Name);
                            string macthStringResult = "";

                            if (percentage > 80)
                            {
                                macthStringResult = femalelist[j].Name + " " + "matches" + " " +  malelist[i].Name + " " + percentage + "%" + ", good match";
                                percentageListReversed.Add(percentage);
                            }
                            else
                            {
                                macthStringResult = femalelist[j].Name + " " + "matches" + " " +  malelist[i].Name + " " + percentage + "%";
                                percentageListReversed.Add(percentage);
                            }
                            csvResultsReverse.Add(new CsvResult { CsvStringResult = macthStringResult, ResultPercentage = percentage });
                            
                        }
                    }
                    csvResultsReverse = ResultOrdering.OrderMatchResults(csvResultsReverse);
                    if (csvResultsReverse.Any())
                    {
                        csvResultsReverse.ForEach(result => { Console.WriteLine(result.CsvStringResult); });
                        csvResultsReverse.ForEach(result => { File.AppendAllText("output.txt", result.CsvStringResult + Environment.NewLine); });
                        File.AppendAllText("output.txt", "///////////////////////////////////////" + Environment.NewLine);
                    }
                    File.AppendAllText("output.txt", "Average Results of each combination\n" + Environment.NewLine);
                    Console.WriteLine("...........................Average Result of each combination............................");
                    if (percentageListReversed != null && percentageList != null)
                    {
                        for (int i = 0; i < percentageList.Count; i++)
                        {
                            Console.WriteLine($"{macthNames[i]} {(percentageList[i] + percentageListReversed[i])/2}");
                            File.AppendAllText("output.txt", $"{macthNames[i]} {(percentageList[i] + percentageListReversed[i]) / 2}" + Environment.NewLine);
                        }
                    }
            }
            else
            {
                Console.WriteLine("FILE NOT FOUND \n did you add the csv file on our Resource folder/directory yet?\n if you did, make sure to type the name of the file including its extension\n please check the way you wrote it, it should contain the csv extension. you should type it like this for example: players.csv\n");
                //ReadCSVFile();
                Console.WriteLine("enter 1 to try entering CSV file name again or\n enter 2 to choose between manual input or read from csv ");
                string input = Console.ReadLine();
                if(input == "1")
                {
                    ReadCSVFile();
                }
                else if(input == "2")
                {
                    Main(filepaths);
                }
                
                Logger.Log("filepath: " + filepath + " Does Not Exist"); 
            }
        }
    }
}

