using System;
using System.Linq;

namespace  Gaming.Quiz.Library.Utility
{
    public class Editing
    {
        public static String Name(String playerName)
        {
            String name = "";
            try
            {
                if (playerName == playerName.ToUpper())
                    name = ScrapeDisplayName(playerName);
                else
                    name = terStegen(playerName);
            }
            catch (Exception ex)
            {
            }

            return name;
        }

        private static String ScrapeDisplayName(string str)
        {
            String name = "";

            if (str.IndexOf(" ") > -1)
            {
                String[] arrays = str.Split(' ');

                String lastName = arrays[arrays.Count() - 1];
                String initial = arrays[0].Substring(0, 1);

                name = initial + ". " + lastName;
            }
            else
                name = str;

            return name.Trim();
        }

        private static String terStegen(string str)
        {
            String name = "";

            if (str.IndexOf(" ") > -1)
            {
                String[] arrays = str.Split(' ');
                String lastName = "";

                for (int i = 0; i < arrays.Count(); i++)
                {
                    if (arrays[i] == arrays[i].ToUpper())
                        lastName += arrays[i] + " ";
                    else
                    {
                        if (i == (arrays.Count() - 1))
                            lastName = arrays[i];
                    }
                }

                String initial = arrays[0].Substring(0, 1);

                name = initial + ". " + lastName;
            }
            else
                name = str;

            return name.Trim();
        }

        private static String ScrapFirsNameLastName(string playerName, Int32 isFirstName)
        {
            String name = "";

            if (playerName.IndexOf(" ") > -1)
            {
                String[] arrays = playerName.Split(' ');

                String lastName = "";// arrays[arrays.Count() - 1];
                String firstName = arrays[0];

                if (arrays.Count() > 1)
                {
                    for (int i = 1; i < arrays.Count(); i++)
                    {
                        lastName += arrays[i] + " ";
                    }
                }


                name = isFirstName == 1 ? firstName : lastName;
            }
            else
                name = isFirstName == 1 ? playerName : "";

            return (name.Trim()).ToUpper();
        }
    }
}
