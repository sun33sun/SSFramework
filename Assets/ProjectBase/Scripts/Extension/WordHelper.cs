using NPOI.Util;
using NPOI.XWPF.UserModel;
using ProjectBase;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Enviro
{
	public class WordHelper : Singleton<WordHelper>
	{
		public string WordTemplateName => "Report.docx";
		public string WordGeneratedName => "GeneratedReport.docx";
		private WordHelper() { }

		public void CreateWord(List<string> pictureNames)
		{
			FileStream fsWord = Read(WordTemplateName);
			XWPFDocument doc = new XWPFDocument(fsWord);

			foreach (string pictureName in pictureNames)
			{
				string key = $"${pictureName.Replace("_默认图", string.Empty)}$";

				foreach (XWPFParagraph gp in doc.Paragraphs)
				{
					if (!gp.ParagraphText.Contains(key))
						continue;

					while (gp.Runs.Count > 0)
					{
						gp.RemoveRun(0);
					}

					XWPFRun gr = gp.CreateRun();
					gr.SetText(string.Empty);
					FileStream fsPicture = ReadJpg(pictureName);
					gr.AddPicture(fsPicture, (int)PictureType.JPEG, pictureName, Units.ToEMU(192 * 2), Units.ToEMU(108 * 2));
					fsPicture.Close();
					fsPicture.Dispose();
				}
			}

			WriteAndClose(WordGeneratedName, doc);
			fsWord.Close();
			fsWord.Dispose();
		}

		private FileStream Read(string name)
		{
			string path = $"{Application.streamingAssetsPath}/{name}";
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

			return fs;
		}

		private FileStream ReadJpg(string name)
		{
			name = $"/Screenshot/{name}.jpg";

			return Read(name);
		}

		private void WriteAndClose(string pictureName, XWPFDocument doc)
		{
			string name = pictureName.Split('.')[0];
			string path = $"{Application.streamingAssetsPath}/{name}.docx";

			FileStream fs = null;
			if (File.Exists(path))
				fs = new FileStream(path, FileMode.Open, FileAccess.Write);
			else
				fs = new FileStream(path, FileMode.Create);

			doc.Write(fs);
			fs.Close();
			fs.Dispose();
			Debug.Log($"在{path}生成报告");
		}

	}
}

