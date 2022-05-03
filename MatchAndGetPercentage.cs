using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodMatchTester
{
    public class MatchAndGetPercentage
    {
       public static int GetPercentage(string firstName, string secondName)
        {
            firstName = firstName.ToLower();
            secondName = secondName.ToLower();

            StringBuilder sentence = new StringBuilder(firstName + "matches" + secondName);

            StringBuilder percentageNumber = new StringBuilder();

            // loop to count matching character and give appended count
            int count = 0;
            while (sentence.Length > 0)
            {

                count = sentence.ToString().Count(f => (f == sentence[0]));
                string temporarySentence = sentence.ToString().Replace(sentence[0].ToString(), string.Empty);
                sentence.Clear();
                sentence.Append(temporarySentence);
                percentageNumber.Append(count);
                count = 0;
            }

            string stringNumber = percentageNumber.ToString();
            long number = Int64.Parse(stringNumber);

            // getting percentage to two digits. outer loop keeps looping till percentage is two digits and inner keeps adding the first and last digit
            long sum = 0;
            StringBuilder percentage = new StringBuilder(number.ToString());
            StringBuilder temporaryPercentage = new StringBuilder();

            while (percentage.Length > 2)
            {
                temporaryPercentage.Clear();
                temporaryPercentage.Append(percentage);
                percentage.Clear();
                while (temporaryPercentage.Length > 0)
                {
                    string temporaryPercentageAsNumber = temporaryPercentage.ToString();
                    long Number = Int64.Parse(temporaryPercentageAsNumber);
                    if (temporaryPercentage.Length == 1)
                    {
                        sum = Number;
                        temporaryPercentage.Clear();
                    }
                    else
                    {   // get the first digit and last digit, add them then remove before appending to percentage
                        long digits = (long)Math.Log10(Number);
                        long firstDigit = (long)(Number / Math.Pow(10, digits));

                        long lastDigit = Number % 10;

                        sum = firstDigit + lastDigit;

                        temporaryPercentage.Remove(temporaryPercentage.Length - 1, 1);
                        temporaryPercentage.Remove(0, 1);
                    }

                    percentage.Append(sum);

                }

            }
            // converting final percentage to integer
            string finalPercentage = percentage.ToString();
            int intPercentage = Int32.Parse(finalPercentage);
            return intPercentage;


        }
    }
}
