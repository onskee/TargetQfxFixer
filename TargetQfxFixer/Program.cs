using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TargetQfxFixer {
	class Program {
		static void Main(string[] args) {

			if (args != null && args.Length == 1) {

				Console.WriteLine("Looking for " + args[0]);

				if (File.Exists(args[0])) {

					List<string> headers = new List<string>() {
						"OFXHEADER:",
						"DATA:",
						"VERSION:",
						"SECURITY:",
						"ENCODING:",
						"CHARSET:",
						"COMPRESSION:",
						"OLDFILEUID:",
						"NEWFILEUID:"  
					};

					List<string> tagsToRemove = new List<string>()
					{
						"DTACCTUP",
						"</CODE>",
						"</SEVERITY>", 
						"</MESSAGE>",
						"</DTSERVER>",
						"</LANGUAGE>",
						"</ORG>",
						"</FID>",
						"</INTU.BID>",
						"<INTU.USERID>",
						"</INTU.USERID>",
						"</TRNUID>",
						"</CODE>",
						"</SEVERITY>",
						"</CURDEF>",
						"</ACCTID>",
						"</DTSTART>",
						"</DTEND>",
						"</TRNTYPE>",
						"</DTPOSTED>",
						"<DTUSER>",
						"</DTUSER>",
						"</TRNAMT>",
						"</FITID>",
						"<CORRECTFITID>",
						"</CORRECTFITID>",
						"<REFNUM>",
						"</REFNUM>",
						"</NAME>",
						"<SIC>",
						"</SIC>",
						"</BALAMT>", 
						"</DTASOF>",

					};

					using (var rdr = new StreamReader(args[0])) {
						string txt = rdr.ReadToEnd();

						Console.WriteLine(txt);

						var split = txt.Split(new string[] { "<" }, StringSplitOptions.None);
						var header = split[0];
						var xml = txt.Substring(header.Length - 1, txt.Length - header.Length);

						header = header.Replace(" ", "");
						xml = xml.Replace("<", Environment.NewLine + "<");

						var xmlSplit = xml.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

						var newXml = string.Empty;
						foreach (var line in xmlSplit) {
							
							var query = (from x in tagsToRemove where line.Contains(x) select x);
							if (!query.Any()) {
								newXml += line.Replace("  ", " ") + Environment.NewLine;
							}

							
						}

						Console.WriteLine(newXml);

						using (var wtr = new StreamWriter(Path.GetFileNameWithoutExtension(args[0]) + "-fix" + Path.GetExtension(args[0]))) {

							wtr.Write(header + Environment.NewLine + newXml);
							wtr.Flush();
						}

					}

				}
				else {
					Console.WriteLine("Unable to find file " + args[0]);
				}
			}

		}
	}
}
