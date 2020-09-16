using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
 - Read Mission & Object tables
 */

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };
	
	public static List<Dictionary<string, string>> Read(string file)
	{
		var list = new List<Dictionary<string, string>>();
		TextAsset data = Resources.Load ("files/"+file) as TextAsset;

		var lines = Regex.Split (data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;
		
		var header = Regex.Split(lines[1], SPLIT_RE);
		for(var i=2; i < lines.Length; i++) {
			
			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;
			
			var entry = new Dictionary<string, string>();
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                string finalvalue = value;
				entry[header[j]] = finalvalue;
			}
			list.Add (entry);
		}
		return list;
	}
}

/*
 * https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/#comment-7111
 */
