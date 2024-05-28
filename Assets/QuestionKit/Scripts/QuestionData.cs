using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace ProjectBase.Exam
{
	public class QuestionData
	{
		public enum ChoiceType
		{
			Single = 1,
			Multiple
		}

		public enum ChoiceIndex
		{
			A = 1,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			K,
			L,
			M,
			N,
			O,
			P,
			Q,
			R,
			S,
			T,
			U,
			V,
			W,
			X,
			Y,
			Z
		}

		public ChoiceType choiceType;
		public string strHead;
		public Dictionary<ChoiceIndex, string> optionDic = new Dictionary<ChoiceIndex, string>();
		public List<ChoiceIndex> multipleRights = new List<ChoiceIndex>();
		public ChoiceIndex singleRight;

		public float score;
		public string strAnalysis = "";
	}
}