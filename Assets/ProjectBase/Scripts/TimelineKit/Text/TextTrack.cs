using ProjectBase.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ProjectBase.Timeline
{
	[TrackColor(0.7366781f, 0.3261246f, 0.8529412f)]
	[TrackClipType(typeof(TextClip))]
	[TrackBindingType(typeof(TMP_Text))]
	public class TextTrack : TrackAsset
	{
		protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
		{
			return base.CreatePlayable(graph, gameObject, clip);
		}

		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			var scriptPlayable = ScriptPlayable<TextMixerBehaviour>.Create(graph, inputCount);

			TextMixerBehaviour b = scriptPlayable.GetBehaviour();

			foreach (var item in GetClips())
			{
				item.displayName = ((int)(item.start * 60)).ToString();
				item.duration = ProjectBaseSettings.Frame5;
				b.timeList.Add(item.start);
			}

			return scriptPlayable;
		}
	}
}

