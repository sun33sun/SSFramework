using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace ProjectBase.Timeline
{
	public class TextMixerBehaviour : PlayableBehaviour
	{
		public List<double> timeList = new List<double>();
		List<TextBehaviour> behaviours = new List<TextBehaviour>();
		private PlayableDirector director;
		private TMP_Text tmp;
		string originText;

		public override void OnPlayableCreate(Playable playable)
		{
			director = (playable.GetGraph().GetResolver() as PlayableDirector);
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (tmp == null)
			{
				tmp = playerData as TMP_Text;
				originText = tmp.text;
			}

			TextBehaviour behaviour = null;

			if (behaviours.Count < 1)
			{
				int inputCount = playable.GetInputCount();
				for (int i = 0; i < inputCount; i++)
				{
					ScriptPlayable<TextBehaviour> input = (ScriptPlayable<TextBehaviour>)playable.GetInput(i);
					behaviour = input.GetBehaviour();
					behaviours.Add(behaviour);
				}
			}

			for (int i = 0; i < behaviours.Count; i++)
			{
				behaviour = behaviours[i];
				if (director.time < timeList[i])
				{
					behaviour.IsUsed = false;
					continue;
				}

				if (behaviour.IsUsed)
					continue;
				behaviour.IsUsed = true;
				tmp.text = behaviour.newContent;
			}
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			if (tmp != null)
				tmp.text = originText;
			base.OnPlayableDestroy(playable);
		}
	}
}

