using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

    public static string[] SplitOutsideBlocks(this string str, char separator, char blockStart, char blockEnd)
    {
        if (str.Contains(blockStart) && str.Contains(blockEnd))
        {
            List<string> result = new List<string>();
            int blockLevel = 0;
            int lastSubstringEndIndex = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == blockStart)
                {
                    blockLevel++;
                }
                else
                {
                    if (str[i] == blockEnd)
                    {
                        blockLevel--;
                    }
                }
                if (blockLevel == 0)
                {
                    if (str[i] == separator || i == str.Length - 1)
                    {
                        var substringLength = i - lastSubstringEndIndex;
                        if (i == str.Length - 1)
                        {
                            substringLength++;
                        }
                        var substring = str.Substring(lastSubstringEndIndex, substringLength);
                        result.Add(substring);
                        lastSubstringEndIndex = i + 1;
                    }
                }
            }

            return result.ToArray();
        }
        else
        {
            return str.Split(separator);
        }
    }

    public static bool GetSubstringUntillChar(this string str, char ch, out string result)
    {
        var index = str.IndexOf(ch);
        result = "";
        if (index > -1)
        {
            result = str.Substring(0, index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool GetSubstringAfterChar(this string str, char ch, out string result)
    {
        var index = str.IndexOf(ch);
        result = "";
        if (index > -1)
        {
            result = str.Substring(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool GetSubstringBetveen(this string str, char firstChar, char secondChar, out string result)
    {
        result = "";
        var startIndex = str.IndexOf(firstChar);
        var endIndex = str.LastIndexOf(secondChar) - 1;
        if (endIndex - startIndex == 1)
        {
            return false;
        }

        if (startIndex >= 0 && endIndex >= 0)
        {
            result = str.Substring(startIndex + 1, endIndex - startIndex);
            return true;
        }
        return false;
    }

    public static int ConcatToInt(this string str)
    {
        var bytes = System.Text.Encoding.UTF32.GetBytes(str);
        var concatedString = "";

        for (int i = 0; i < str.Length; i++)
        {
            concatedString += (int)str[i] % 32;
        }
        //for (int i = 0; i < bytes.Length; i++)
        //{
        //    Debug.Log(bytes[i]);
        //}

        return int.Parse(concatedString);
    }

    public static T[] DecodeCharSeparatedEnums<T>(this string charSeparatedEnum, char separator = ',')
    {
        var enums = new List<T>();
        if (charSeparatedEnum != "" && !string.IsNullOrEmpty(charSeparatedEnum))
        {
            if (!String.IsNullOrEmpty(charSeparatedEnum) && charSeparatedEnum.Length > 0)
            {
                foreach (string str in charSeparatedEnum.Split(separator))
                {
                    if (str == "")
                    {
                        continue;
                    }
                    var strLower = str.ToLower();
                    bool enumFound = false;
                    foreach (T e in Enum.GetValues(typeof(T)))
                    {
                        var enumLower = e.ToString().ToLower();
                        if (enumLower.Length == strLower.Length)
                        {
                            if (enumLower == strLower)
                            {
                                //Debug.Log(e.ToString());
                                enums.Add(e);
                                enumFound = true;
                                break;
                            }
                        }
                    }

                    if (!enumFound) throw new NullReferenceException("There is no such name " + str + " inside " + typeof(T).ToString() + " enum");
                }
            }
        }
        return enums.ToArray();
    }

    public static T DecodeCharSeparatedEnumsAndGetFirst<T>(this string charSeparatedEnum, char separator = ',')
    {
        var enums = charSeparatedEnum.DecodeCharSeparatedEnums<T>();

        if (enums.Length > 0) return enums[0];
        else
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(0);
        }
    }

    public static bool DecodeCharSeparatedEnumsAndGetFirst<T>(this string charSeparatedEnum, out T result, char separator = ',')
    {
        List<T> enums = new List<T>();
        if (charSeparatedEnum.Length > 0)
        {
            foreach (string str in charSeparatedEnum.Split(separator))
            {
                bool enumFound = false;
                foreach (T e in Enum.GetValues(typeof(T)))
                {
                    if (e.ToString().ToLower() == str.ToLower())
                    {
                        //Debug.Log(e.ToString());
                        enums.Add(e);
                        enumFound = true;
                        break;
                    }
                }

                //if (!enumFound) throw new NullReferenceException("There is no such name " + str + " inside " + typeof(T).ToString() + " enum");
            }
        }

        if (enums.Count > 0)
        {
            result = enums[0];
            return true;
        }
        else
        {
            result = (T)Enum.GetValues(typeof(T)).GetValue(0);
            return false;
        }
    }
}